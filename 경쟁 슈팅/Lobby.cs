using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// popup 이름 없이 Ok하는거 예외처리 필요
// 방장 입장 -> client 입장 -> client 이름 설정 -> 방장 이름 설정 순이면 에러

// 2. 이름 팝업에서 이름 설정을 빠르게 할 경우(아직 네트워크에 진입 안된 듯) : 에러떠서 이벤트 확인 필요
    // 네트워크 진입 문제 x : count는 1로 표시됨
    // isMasterPlayer는 start에서 false <- **이 문제 인걸로 확인
    // popupInit 확인 **********************
// 6. popup에 이름없을 때 나가기버튼 누르면 안나가짐
// ------------------------------------- 해결 --------------------------------
// 1. 방장(isMasterClient)이 나갔을 경우: 다른 플레이어가 1번 자리(position) (아니면 티아라 위치를 이동)
    // 방장이 rpc를 전달하기 전에 disconnect돼서 rpc 메시지가 사라짐
// 3. 나갔을 때 나간 플레이어의 정보 지워지는거 확인 필요 // 적용되지 않음 -> RPC가 다른 플레이어에게 적용되기 전에 disconnected 되는 듯
    // 이름이 지워지는 것은 확인 -> 나간 플레이어의 화면에서 다시 로비가 생성됨
    // RPC 메시지를 보내던 중 Scene이 바뀌면서 새로 Lobby를 생성하는게 아닌가..?
    // 로비가 다시 시작되면서 position도 -1로 초기화됨
// 4. 중복 방 생성할 경우: 경고 문구 뜨도록
// 5. 방 참가할 때 방이 없는 경우 : 경고 문구

public static class CrossSceneInfo
{
    public static int myGame_MAX_PLAYERS;

    public static string selectGameName;
    public static string roomName;

    public static string[] playerNames = new string[4];
    public static int[] IDs = new int[4];
    public static int pos;

    public static int getMyDirection()
    {
        int direction = 0;

        if (pos <= 1) direction = -1;
        else direction = 1;

        return direction;
    }
}

public class Lobby : Photon.PunBehaviour
{
    PhotonView pv;

    public TextMesh[] playerNames; // 공유

    public Image managerImage;

    public Image[] readyImages;
    public GameObject gameStartButton;

    public Text gameName;
    public Text roomName;

    private bool[] isPlayerReady; // 공유
    private UnityEvent allPlayersReady = new UnityEvent();
    private UnityEvent allPlayersNotReady = new UnityEvent();

    private int playerPosition = -1;
    private string playerName = "";

    private bool isPopup = false;
    private bool isFirst = true;

    private bool[] isPlayerInLobby; // 공유
    private bool[] isMasterUpdated;

    void Awake()
    {
        if (CrossSceneInfo.selectGameName == GameInfo.game1Name)
        {
            PhotonNetwork.room.MaxPlayers = GameInfo.GAME1_MAX_PLAYERS;
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game2Name)
        {
            PhotonNetwork.room.MaxPlayers = GameInfo.GAME2_MAX_PLAYERS;
        }
        else if(CrossSceneInfo.selectGameName == GameInfo.game3Name)
        {
            PhotonNetwork.room.MaxPlayers = GameInfo.GAME3_MAX_PLAYERS;
        }
        else if(CrossSceneInfo.selectGameName == GameInfo.game4Name)
        {
            PhotonNetwork.room.MaxPlayers = GameInfo.GAME4_MAX_PLAYERS;
        }
        else if(CrossSceneInfo.selectGameName == GameInfo.game5Name)
        {
            PhotonNetwork.room.MaxPlayers = GameInfo.GAME5_MAX_PLAYERS;
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game6Name)
        {
            PhotonNetwork.room.MaxPlayers = GameInfo.GAME6_MAX_PLAYERS;
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game7Name)
        {
            PhotonNetwork.room.MaxPlayers = GameInfo.GAME7_MAX_PLAYERS;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        // 변수 초기화
        valInit();

        // 모든 플레이어가 준비 완료 상태인지 확인
        allPlayersReady.AddListener(OnAllPlayersReady);
        allPlayersNotReady.AddListener(OnAllPlayersNotReady);

        // 로비 초기화
        lobbyInit();

        popupInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerName.Equals("") && playerPosition != -1 && isFirst)
        {
            playerNames[playerPosition].text = playerName;
            isPlayerReady[playerPosition] = true;

            isFirst = false;
        }

        readyImagesUpdate();

        OnAllPlayersReadyListener();

        OnCancelKeyPress();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////// 방 상태 관련 함수 ///////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void valInit()
    {
        gameName.text = CrossSceneInfo.selectGameName;
        roomName.text = CrossSceneInfo.roomName;

        if(CrossSceneInfo.selectGameName == GameInfo.game1Name)
        {
            CrossSceneInfo.myGame_MAX_PLAYERS = GameInfo.GAME1_MAX_PLAYERS;
        }
        else if(CrossSceneInfo.selectGameName == GameInfo.game2Name)
        {
            CrossSceneInfo.myGame_MAX_PLAYERS = GameInfo.GAME2_MAX_PLAYERS;
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game3Name)
        {
            CrossSceneInfo.myGame_MAX_PLAYERS = GameInfo.GAME3_MAX_PLAYERS;
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game4Name)
        {
            CrossSceneInfo.myGame_MAX_PLAYERS = GameInfo.GAME4_MAX_PLAYERS;
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game5Name)
        {
            CrossSceneInfo.myGame_MAX_PLAYERS = GameInfo.GAME5_MAX_PLAYERS;
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game6Name)
        {
            CrossSceneInfo.myGame_MAX_PLAYERS = GameInfo.GAME6_MAX_PLAYERS;
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game7Name)
        {
            CrossSceneInfo.myGame_MAX_PLAYERS = GameInfo.GAME7_MAX_PLAYERS;
        }

        isPlayerReady = new bool[CrossSceneInfo.myGame_MAX_PLAYERS]; // 공유
        isPlayerInLobby = new bool[CrossSceneInfo.myGame_MAX_PLAYERS]; // 공유
        isMasterUpdated = new bool[CrossSceneInfo.myGame_MAX_PLAYERS];

        for (int i = 0; i < CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            isPlayerReady[i] = false;
            isPlayerInLobby[i] = false;
            isMasterUpdated[i] = false;
        }
    }

    void lobbyInit()
    {
        // 해상도 적용
        Screen.SetResolution(2960, 1440, true);

        // 체크이미지 전부 visible false로
        readyImagesInit();
        // Game Start button visible false로
        gameStartButtonInit();

        Debug.Log("로비 초기화 완료");
    }

    void readyImagesInit()
    {
        Debug.Log("레디이미지 초기화");
        for (int i = 0; i < CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            readyImages[i].enabled = false;
        }
    }

    void gameStartButtonInit()
    {
        Debug.Log("게임스타트버튼 초기화");
        gameStartButton.SetActive(false);
    }

    void popupInit()
    {
        // 팝업 띄움
        Popup popup = Popup.show(new PopupOptions
        {
            okButtonDelegate = () =>
            {
                Debug.Log("OK");

                string input = GameObject.Find("Player Name text").GetComponent<Text>().text;

                if (input != "")
                {
                    playerName = input;

                    setMyPosition();

                    Debug.Log("Player Name: " + playerName);
                }
                else
                {
                    GameObject.Find("Placeholder").GetComponent<Text>().text = "INPUT NAME!!";
                    GameObject.Find("Placeholder").GetComponent<Text>().color = Color.red;
                }
            }
        });
    }

    void readyImagesUpdate()
    {
        for (int i = 0; i < CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            if (isPlayerReady[i] == true)
            {
                readyImages[i].enabled = true;
            }
            else
            {
                readyImages[i].enabled = false;

            }
        }
    }

    void OnAllPlayersReadyListener()
    {
        int i;

        for (i = 0; i < CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            if (isPlayerReady[i] == false) break;
        }

        if (i == CrossSceneInfo.myGame_MAX_PLAYERS)
        {
            allPlayersReady.Invoke();
        }
        else
        {
            allPlayersNotReady.Invoke();
        }
    }

    void OnAllPlayersReady()
    {
        if (PhotonNetwork.isMasterClient)
        {
            gameStartButton.SetActive(true);
        }
    }

    void OnAllPlayersNotReady()
    {
        if (PhotonNetwork.isMasterClient)
        {
            gameStartButton.SetActive(false);
        }
    }

    public void OnClickGameStartButton()
    {
        pv.RPC("gameStart", PhotonTargets.All);
    }

    [PunRPC]
    void gameStart()
    {
        Debug.Log("게임 시작 전 데이터 로드");

        string[] strPlayerNames = new string[CrossSceneInfo.myGame_MAX_PLAYERS];

        for (int i = 0; i < CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            strPlayerNames[i] = playerNames[i].text;
        }

        CrossSceneInfo.playerNames = strPlayerNames;
        CrossSceneInfo.pos = playerPosition;

        CrossSceneInfo.IDs[playerPosition] = PhotonNetwork.player.ID;

        Debug.Log("로드 완료");

        if (CrossSceneInfo.selectGameName == GameInfo.game1Name)
        {
            SceneManager.LoadScene("Map Customizing");
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game2Name)
        {
            SceneManager.LoadScene("CoopShootingGame");
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game3Name)
        {
            SceneManager.LoadScene("ChooseCharcter");
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game4Name)
        {
            SceneManager.LoadScene("PickUp");
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game5Name)
        {
            SceneManager.LoadScene("AvoidArrow");
        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game6Name)
        {

        }
        else if (CrossSceneInfo.selectGameName == GameInfo.game7Name)
        {
            SceneManager.LoadScene("TowerAttack");
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////// 플레이어 참가 관련 함수 ///////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [PunRPC]
    void notifyNewPlayer(string newPlayerName)
    {
        // 새로운 플레이어의 포지션을 할당함
        int newPlayerPosition = getNewPlayerPosition();

        // 해당 포지션이 사용 중임을 기재
        isPlayerInLobby[newPlayerPosition] = true;

        // 해당 포지션에 새로운 플레이어의 정보를 입력
        playerNames[newPlayerPosition].text = newPlayerName;
        isPlayerReady[newPlayerPosition] = true;

        // 새로운 플레이어에게 포지션을 할당 (모두에게 전달하지만 newPlayerName을 통해서 본인인증 후 본인만 포지션을 할당받음)
        pv.RPC("assignPlayerPosition", PhotonTargets.All, newPlayerName, newPlayerPosition);

        // playerNames의 text를 복사하기 위한 배열
        string[] newPlayerNames = new string[CrossSceneInfo.myGame_MAX_PLAYERS];

        for (int i = 0; i < CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            newPlayerNames[i] = playerNames[i].text;
        }

        // 새로운 playerNames와 isPlayerReady, isPlayerInLobby를 클라이언트들에게 전달
        pv.RPC("setPlayerInfo", PhotonTargets.All, newPlayerNames, isPlayerReady, isPlayerInLobby);
    }

    [PunRPC]
    void assignPlayerPosition(string newPlayerName, int newPlayerPosition)
    {
        Debug.Log("newPlayerName: " + newPlayerName);
        Debug.Log("myName: " + playerName);

        if (playerName.Equals(newPlayerName))
        {
            playerPosition = newPlayerPosition;

            Debug.Log("포지션 할당받음");
        }
    }

    [PunRPC]
    void setPlayerInfo(string[] newPlayerNames, bool[] newIsPlayerReady, bool[] newIsPlayerInLobby)
    {
        for(int i = 0; i < CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            playerNames[i].text = newPlayerNames[i];
            isPlayerReady[i] = newIsPlayerReady[i];
            isPlayerInLobby[i] = newIsPlayerInLobby[i];
        }

        Debug.Log("플레이어 정보 저장 완료");
    }

    void setMyPosition()
    {
        if (PhotonNetwork.isMasterClient)
        {
            playerPosition = getNewPlayerPosition();
            isPlayerInLobby[playerPosition] = true;
            Debug.Log("난 마스터");
        }
        else
        {
            pv.RPC("notifyNewPlayer", PhotonTargets.MasterClient, playerName);
            Debug.Log("난 일반클라이언트");
        }
    }

    int getNewPlayerPosition()
    {
        for(int i = 0; i < CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            if(isPlayerInLobby[i] == false)
            {
                return i;
            }
        }

        return -1;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////// 플레이어 퇴장 관련 함수들 //////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnCancelKeyPress()
    {
        //if (Application.platform == RuntimePlatform.Android)
        //{
        if (Input.GetKeyDown(KeyCode.Escape) && isPopup == false)
        {
            isPopup = true;

            QuitPopup.show(new QuitPopupOptions
            {
                cancelButtonDelegate = () =>
                {
                    Debug.Log("cancel");
                    isPopup = false;
                },
                okButtonDelegate = () =>
                {
                    Debug.Log("ok");

                    if (playerPosition != -1) // playerPosition이 설정 되지 않은 상태에선 그냥 창만 닫음 
                    {
                        pv.RPC("playerQuit", PhotonTargets.MasterClient, playerPosition);
                    }
                    else
                    {
                        PhotonNetwork.Disconnect();

                        SceneManager.LoadScene("join_create");
                    }

                    isPopup = false;
                }
            });
        }
        //}
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.Log("disconnected!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    void leaveRoom()
    {
        PhotonNetwork.Disconnect(); // 혼자 있으면 그냥 disconnect

        SceneManager.LoadScene("join_create");
    }

    [PunRPC]
    void changeMaster()
    {
        if (PhotonNetwork.isMasterClient) // 서버는 밑 단계 수행 할 필요x
        {
            return;
        }

        int pos = getNextMaster();

        //if (pos == MAX_PLAYER)
        //{
        //    Debug.Log("바꿀 플레이어 없음");

        //    leaveRoom();

        //    return;
        //}

        Debug.Log("selected player: " + pos);

        // 현재 기록 0번으로 복사
        playerNames[0].text = playerNames[pos].text;
        isPlayerInLobby[0] = isPlayerInLobby[pos];
        isPlayerReady[0] = isPlayerReady[pos];

        // 해당 플레이어의 원래 기록 삭제
        deletePlayer(pos);

        // 플레이어들의 업데이트 완료 메시지를 서버로 보냄
        pv.RPC("masterUpdated", PhotonTargets.MasterClient, playerPosition);

        // 방장이 될 플레이어의 playerPositon 수정
        if (pos == playerPosition)
        {
            playerPosition = 0;
        }

        Debug.Log("방장 변경");
    }

    int getNextMaster()
    {
        // positon 1 이후 제일 첫 플레이어를 찾음
        int i;

        for (i = 1; i < CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            if (isPlayerInLobby[i] == true)
            {
                break;
            }
        }

        return i;
    }

    void deletePlayer(int playerPosition)
    {
        playerNames[playerPosition].text = "No player";
        isPlayerInLobby[playerPosition] = false;
        isPlayerReady[playerPosition] = false;

        Debug.Log("player delete complete!!");
    }

    [PunRPC]
    void masterUpdated(int playerPosition)
    {
        // 게임의 플레이어 수 카운트
        int totalPlayerCount = getCountOfPlayers();

        // 1명이면 서버 혼자 남아있기 때문에 그냥 disconnect
        //if (totalPlayerCount == 1)
        //{
        //    leaveRoom();
        //}

        isMasterUpdated[playerPosition] = true;

        int updatedPlayerCount = 0;

        // 업데이트를 마무리한 플레이어의 수 체크
        for (int ipcIndex = 0; ipcIndex < CrossSceneInfo.myGame_MAX_PLAYERS; ipcIndex++)
        {
            if (isMasterUpdated[ipcIndex] == true)
            {
                updatedPlayerCount++;
            }
        }

        Debug.Log("updatedPlayerCount: " + updatedPlayerCount);
        Debug.Log("totalPlayerCount: " + totalPlayerCount);

        // 본인 제외(-1)한 플레이어 수와 비교, 같으면 (모두 업데이트를 완료했으면)
        if (updatedPlayerCount == totalPlayerCount - 1)
        {
            // 어차피 삭제되기 때문에 isMasterUpdated 초기화 불필요
            leaveRoom();
        }
    }


    // 퇴장한 플레이어가 서버에게 자신의 포지션을 알려주는 함수
    [PunRPC]
    void playerQuit(int quitPlayerPosition)
    {
        Debug.Log("서버(플레이어" + playerPosition + "): 플레이어" + quitPlayerPosition + " 기록 삭제");

        if (playerPosition != quitPlayerPosition) // 퇴장한 플레이어가 서버가 아니면
        {
            pv.RPC("notifyQuitPlayer", PhotonTargets.All, quitPlayerPosition); // 다른 클라이언트에게 알림
        }
        else // 퇴장한 플레이어가 서버면
        {
            int totalPlayerCount = getCountOfPlayers();

            if (totalPlayerCount == 1) // 현재 플레이어가 1명 뿐이면
            {
                leaveRoom();
            }
            else
            {
                pv.RPC("changeMaster", PhotonTargets.All);

            }
        }
    }

    public int getCountOfPlayers()
    {
        int totalPlayerCount = 0;

        // 로비에 있는 총 인원 카운트
        for (int tpcIndex = 0; tpcIndex < CrossSceneInfo.myGame_MAX_PLAYERS; tpcIndex++)
        {
            if (isPlayerInLobby[tpcIndex] == true)
            {
                totalPlayerCount++;
            }
        }

        return totalPlayerCount;
    }

    // 서버가 클라이언트들에게 퇴장한 플레이어의 포지션을 알려주는 함수
    [PunRPC]
    void notifyQuitPlayer(int quitPlayerPosition)
    {
        if (playerPosition == quitPlayerPosition)
        {
            Debug.Log("플레이어" + playerPosition + " 기록 삭제 완료");

            leaveRoom();

            return;
        }

        deletePlayer(quitPlayerPosition);

        Debug.Log("클라이언트" + playerPosition + ": 플레이어" + quitPlayerPosition + "  기록 삭제");
    }

    // 방을 떠났을 경우 호출되는 콜백함수
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("join_create");

        PhotonNetwork.Disconnect();
    }

    public override void OnLeftLobby()
    {
        SceneManager.LoadScene("join_create");

        PhotonNetwork.Disconnect();
    }
}
