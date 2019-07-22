using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;
using UnityEngine.UI;

// 게임 화면에 모든 플레이어 instantiate
// 게임 화면에 모든 bricks instatiate

public static class Player
{
    public static GameObject player;
}

public class Game : Photon.PunBehaviour
{
    private const int MAX_HP = 3;

    [SerializeField]
    public Vector3[] spawnPositions;

    [SerializeField]
    public GameObject[] cameras;

    [SerializeField]
    public GameObject brick;

    public GameObject[] gameOverImages;
    public bool[] isPlayerPlaying;
    public bool[] isReadyToClose;

    private int[] playerHp;
    private GameObject playerInfo;

    private int recvPos;

    PhotonView pv;
    string[] playerNames;
    int position;

    private bool isPopup = false;

    void Start()
    {
        pv = this.GetComponent<PhotonView>();

        playerNames = CrossSceneInfo.playerNames;
        position = CrossSceneInfo.pos + 1;

        initVal();

        setCamera();

        createPlayer();

        createBricks();

        playerInfoInit();
    }

    void Update()
    {
        OnCancelKeyPress();

        checkMyHp();
    }

    void initVal()
    {
        isPlayerPlaying = new bool[CrossSceneInfo.myGame_MAX_PLAYERS + 1];

        for (int i = 1; i <= CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            isPlayerPlaying[i] = true;
        }

        isReadyToClose = new bool[CrossSceneInfo.myGame_MAX_PLAYERS + 1];

        for (int i = 1; i <= CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            isReadyToClose[i] = false;
        }

        playerHp = new int[CrossSceneInfo.myGame_MAX_PLAYERS + 1];

        for (int i = 1; i <= CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            playerHp[i] = MAX_HP;
        }
    }

    void playerInfoInit()
    {
        GameObject prefab = Resources.Load("Player Info") as GameObject;

        playerInfo = Instantiate(prefab) as GameObject;
    }

    void checkMyHp()
    {
        Debug.Log("playerHp[position]: " + playerHp[position]);

        if (playerHp[position] <= 0)
        {
            gameOver();
            Debug.Log("게임 오버");
        }
    }

    void gameOver()
    {
        Destroy(Player.player);

        pv.RPC("playerGameOver", PhotonTargets.All, position);
    }

    //bool checkIsAllPlayerGameOver() // 게임 플레이 중인 사람이 2명 이상이면 false, 1명 이하면 true
    //{
    //    int count = 0;

    //    for(int i = 1; i <= CrossSceneInfo.myGame_MAX_PLAYERS; i++)
    //    {
    //        if (isPlayerPlaying[i] == true)
    //        {
    //            count++;
    //        }
    //    }

    //    if (count >= 2) return false;
    //    else return true;

    //}

    //bool checkisAllPlayerReadyToClose() // 모두가 true면 true 아니면 false
    //{
    //    int count = 0;

    //    for (int i = 1; i <= CrossSceneInfo.myGame_MAX_PLAYERS; i++)
    //    {
    //        if (isReadyToClose[i] == true)
    //        {
    //            count++;
    //        }
    //    }

    //    if (count == CrossSceneInfo.myGame_MAX_PLAYERS) return true;
    //    else return false;

    //}

    //void closeRoom()
    //{
    //    pv.RPC("notifyCloseRoom", PhotonTargets.All);
    //}

    //[PunRPC]
    //void notifyCloseRoom()
    //{
    //    pv.RPC("readyToCloseRoom", PhotonTargets.MasterClient, position);

    //    PhotonNetwork.LeaveRoom();
    //}

    [PunRPC]
    void shootPerson(int victimPos)
    {
        Debug.Log("victimPos: " + victimPos);

        playerHp[victimPos] = playerHp[victimPos] - 1;

        if (playerHp[victimPos] <= 0) playerHp[victimPos] = 0;

        updateHP();
    }

    void updateHP()
    {
        pv.RPC("setPlayerHp", PhotonTargets.Others, playerHp);
    }

    [PunRPC]
    void setPlayerHp(int[] playerHp)
    {
        for (int i = 1; i <= CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            this.playerHp[i] = playerHp[i];
            GameObject.Find("P" + i + " Count").GetComponent<Text>().text = playerHp[i].ToString();
        }
    }

    [PunRPC]
    void playerGameOver(int position)
    {
        gameOverImages[position].GetComponent<Image>().enabled = true;

        isPlayerPlaying[position] = false;
    }

    //[PunRPC]
    //void readyToCloseRoom(int position)
    //{
    //    isReadyToClose[position] = true;
    //}

    void setCamera()
    {
        if(position == 1)
        {
            cameras[0].GetComponent<Camera>().depth = 0;
            cameras[1].GetComponent<Camera>().depth = 1;
            cameras[2].GetComponent<Camera>().depth = 0;
            cameras[3].GetComponent<Camera>().depth = 0;
            cameras[4].GetComponent<Camera>().depth = 0;

            Debug.Log("카메라 1 ON!");
        }
        if (position == 2)
        {
            cameras[0].GetComponent<Camera>().depth = 0;
            cameras[1].GetComponent<Camera>().depth = 0;
            cameras[2].GetComponent<Camera>().depth = 1;
            cameras[3].GetComponent<Camera>().depth = 0;
            cameras[4].GetComponent<Camera>().depth = 0;

            Debug.Log("2 카메라 3 ON!");

        }
        if (position == 3)
        {
            cameras[0].GetComponent<Camera>().depth = 0;
            cameras[1].GetComponent<Camera>().depth = 0;
            cameras[2].GetComponent<Camera>().depth = 0;
            cameras[3].GetComponent<Camera>().depth = 1;
            cameras[4].GetComponent<Camera>().depth = 0;

            Debug.Log("3 카메라 3 ON!");

        }
        if (position == 4)
        {
            cameras[0].GetComponent<Camera>().depth = 0;
            cameras[1].GetComponent<Camera>().depth = 0;
            cameras[2].GetComponent<Camera>().depth = 0;
            cameras[3].GetComponent<Camera>().depth = 0;
            cameras[4].GetComponent<Camera>().depth = 1;

            Debug.Log("카메라 4 ON!");

        }
    }

    void createBricks()
    {
        for (int i = 0; i < MapCustomInfo.MAX_BRICKS; i++)
        {
            GameObject newBrickImage = Instantiate(brick, MapCustomInfo.bricksPosition[i], Quaternion.identity);

            Debug.Log("플레이어" + position + ": Bricks" + position + "에 Brick" + (i + 1) + " 생성");

            pv.RPC("createBrick", PhotonTargets.Others, position, MapCustomInfo.bricksPosition[i]);
        }
    }

    [PunRPC]
    void createBrick(int playerPosition, Vector3 brickPos)
    {
        GameObject newBrickImage = Instantiate(brick, brickPos, Quaternion.identity);
    }
        
    // 플레이어는 Game마다 하나.
    // 리모트는 따로 생성?
    // 생성한다면 move할 때 본인이 리모트인지 확인은 어떻게?
    void createPlayer() 
    {
        GameObject newPlayer = PhotonNetwork.Instantiate("PlayerA", spawnPositions[position], Quaternion.identity, 0);
        //newPlayer.GetComponent<Player_Init>().position = position;

        Player.player = newPlayer;

        Debug.Log("newPlayer" + position + " 생성");

        if(!newPlayer)
        {
            newPlayer = PhotonNetwork.Instantiate("PlayerA", spawnPositions[position], Quaternion.identity, 0);
            //newPlayer.GetComponent<Player_Init>().position = position;

            Player.player = newPlayer;

            Debug.Log("newPlayer" + position + " 재 생성");
        }
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

                    PhotonNetwork.Disconnect();

                    SceneManager.LoadScene("join_create");

                    isPopup = false;
                }
            });
        }
        //}
    }
}
