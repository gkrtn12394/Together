using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public static class GameNumberManager
{
    public const int NUMBER_OF_GAMES = 7;
    public const int MAX_GAMES = 3;
}

public static class GameInfo
{
    public static string game1Name = "경쟁 슈팅 게임";
    public static string game2Name = "협력 슈팅 게임";
    public static string game3Name = "보드 게임";
    public static string game4Name = "파지 줍기";
    public static string game5Name = "랜덤 화살 피하기";
    public static string game6Name = "자리 뺏기";
    public static string game7Name = "타워 어택";

    public const int GAME1_MAX_PLAYERS = 4;
    public const int GAME2_MAX_PLAYERS = 2;
    public const int GAME3_MAX_PLAYERS = 4;
    public const int GAME4_MAX_PLAYERS = 2;
    public const int GAME5_MAX_PLAYERS = 2;
    public const int GAME6_MAX_PLAYERS = 2;
    public const int GAME7_MAX_PLAYERS = 2;

    //public const int GAME1_MAX_PLAYERS = 1;
    //public const int GAME2_MAX_PLAYERS = 1;
    //public const int GAME3_MAX_PLAYERS = 1;
    //public const int GAME4_MAX_PLAYERS = 1;
    //public const int GAME5_MAX_PLAYERS = 1;
    //public const int GAME6_MAX_PLAYERS = 1;
    //public const int GAME7_MAX_PLAYERS = 1;
}

public class SelectGame : Photon.PunBehaviour
{
    public Image[] games = new Image[GameNumberManager.NUMBER_OF_GAMES];
    public Sprite[] gameImages = new Sprite[GameNumberManager.NUMBER_OF_GAMES];

    public Text createOrJoin;

    private string roomName = "";
    private string roomVersion = "1.0";

    private bool isPopup = false;

    void Start()
    {
        if (JoinOrCreate.selectingJoinRoom)
        {
            createOrJoin.text = "Join Game!";
        }
        else
        {
            createOrJoin.text = "Create Game!";
        }
    }

    void Update()
    {
        OnCancelKeyPress();
    }

    void OnCancelKeyPress()
    {
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

                    SceneManager.LoadScene("join_create");

                    isPopup = false;
                }
            });
        }
    }

    public void OnClickRightButton()
    {
        // image를 한 칸씩 왼쪽으로 밈
        // ( 1>4, 2>1, 3>2, 4>3 )

        // 1번째 게임 따로 저장
        Sprite spr = gameImages[0];
        string str = games[0].GetComponentInChildren<Text>().text;

        for (int i = 0; i < GameNumberManager.NUMBER_OF_GAMES; i++)
        {
            if (i == GameNumberManager.NUMBER_OF_GAMES - 1)
            {
                GameObject.Find("Image" + GameNumberManager.NUMBER_OF_GAMES).GetComponent<Image>().sprite = spr;
                gameImages[GameNumberManager.NUMBER_OF_GAMES - 1] = spr;

                games[GameNumberManager.NUMBER_OF_GAMES - 1].GetComponentInChildren<Text>().text = str;
            }
            else
            {
                GameObject.Find("Image" + (i + 1)).GetComponent<Image>().sprite = gameImages[i + 1];
                gameImages[i] = gameImages[i + 1];

                games[i].GetComponentInChildren<Text>().text = games[i + 1].GetComponentInChildren<Text>().text;
            }
        }
    }

    public void OnClickLeftButton()
    {
        // image를 한 칸씩 오른쪽으로 밈
        // ( 1>2, 2>3, 3>4, 4>1 )
        // ( 0>1, 1>2, 2>3, 3>0 )

        // 4번째 게임 따로 저장
        Sprite spr = gameImages[GameNumberManager.NUMBER_OF_GAMES - 1];
        string str = games[GameNumberManager.NUMBER_OF_GAMES - 1].GetComponentInChildren<Text>().text;

        for (int i = GameNumberManager.NUMBER_OF_GAMES - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                GameObject.Find("Image1").GetComponent<Image>().sprite = spr;
                gameImages[0] = spr;

                games[0].GetComponentInChildren<Text>().text = str;
            }
            else
            {
                GameObject.Find("Image" + (i + 1)).GetComponent<Image>().sprite = gameImages[i - 1];
                gameImages[i] = gameImages[i - 1];

                games[i].GetComponentInChildren<Text>().text = games[i - 1].GetComponentInChildren<Text>().text;
            }
        }
    }

    string getRoomName(string selectedGame, string inputString)
    {
        return selectedGame + "_" + inputString;
    }

    public void OnClickGameButton()
    {
        // 현재 선택된 게임 이름 get
        string selectedGame = games[1].GetComponentInChildren<Text>().text;

        Debug.Log("Selected Game: " + selectedGame);

        if (JoinOrCreate.selectingJoinRoom)
        {
            // 이름 입력 받음
            RoomNameInputPopup popup = RoomNameInputPopup.show(new RoomNameInputPopupOptions
            {
                okButtonDelegate = () =>
                {
                    Debug.Log("OK");

                    string input = GameObject.Find("Game Name text").GetComponent<Text>().text;

                    if (input != "")
                    {
                        roomName = getRoomName(selectedGame, input);

                        Debug.Log("Room Name: " + roomName);

                        CrossSceneInfo.selectGameName = selectedGame;
                        CrossSceneInfo.roomName = roomName;

                        // 서버에 connect
                        if (!PhotonNetwork.connected)
                        {
                            PhotonNetwork.ConnectUsingSettings(roomVersion);
                        }
                    }
                    else
                    {
                        GameObject.Find("Placeholder").GetComponent<Text>().text = "INPUT ROOM NAME!!";
                        GameObject.Find("Placeholder").GetComponent<Text>().color = Color.red;
                    }
                },
                infoString = "입장하고자 하는 방 이름?"
            });
        }
        else
        {
            // 이름 입력 받음
            RoomNameInputPopup popup = RoomNameInputPopup.show(new RoomNameInputPopupOptions
            {
                okButtonDelegate = () =>
                {
                    Debug.Log("OK");

                    string input = GameObject.Find("Game Name text").GetComponent<Text>().text;

                    if (input != "")
                    {
                        roomName = getRoomName(selectedGame, input);

                        Debug.Log("Room Name: " + roomName);

                        CrossSceneInfo.selectGameName = selectedGame;
                        CrossSceneInfo.roomName = roomName;

                        // 서버에 connect
                        if (!PhotonNetwork.connected)
                        {
                            PhotonNetwork.ConnectUsingSettings(roomVersion);
                        }
                    }
                    else
                    {
                        GameObject.Find("Placeholder").GetComponent<Text>().text = "INPUT ROOM NAME!!";
                        GameObject.Find("Placeholder").GetComponent<Text>().color = Color.red;
                    }
                },
                infoString = "생성하고자 하는 방 이름?"
            });
        }
    }

    // 포톤 클라우드와 연결되면 호출되는 콜백함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("로비 입장");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if (!JoinOrCreate.selectingJoinRoom)
        {
            // 접속 시도
            PhotonNetwork.CreateRoom(roomName);

            Debug.Log("Master 입장");
        }
        else
        {
            PhotonNetwork.JoinRoom(roomName);

            Debug.Log("Client 입장");
        }
    }

    public override void OnJoinedRoom()
    {
        if (!JoinOrCreate.selectingJoinRoom) return;

        Debug.Log("조인으로 로비입장");

        SceneManager.LoadScene("Lobby");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("create으로 로비입장");

        SceneManager.LoadScene("Lobby");
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("방 입장 실패");

        PhotonNetwork.Disconnect();

        ErrorPopup error = ErrorPopup.show(new ErrorPopupOptions
        {
            popupDelegate = () =>
            {
                Debug.Log("Error OK");
            },
            message = "방 입장에 실패하였습니다."
        });

        OnClickGameButton();
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("방 생성 실패");

        PhotonNetwork.Disconnect();

        ErrorPopup error = ErrorPopup.show(new ErrorPopupOptions
        {
            popupDelegate = () =>
            {
                Debug.Log("Error OK");
            },
            message = "방 입장에 실패하였습니다."
        });

        OnClickGameButton();
    }
}
