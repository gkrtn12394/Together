//<copyright file="GameManager.cs">
//Copyright (c) 2019 All Rights Reserved
//</copyright>
//<author>Kang Daeun</author>
//<date>03/20/2019 2:12:17 PM </date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   // public GameObject player;
    public GameObject enemySpawner;
    public GameObject heartSpawner;
    public GameObject gameOver;
    public GameObject scoreUIText;

    public GameObject backgroundSound;

    bool isPopup;

    public enum GameManagerState
    {
        //Opening,
        Gameplay,
        Gameover,
    }

    GameManagerState GMState;

    void Start()
    {
        // GMState = GameManagerState.Opening;
        GMState = GameManagerState.Gameplay;
        //----
        UpdateGameManagerState();

        /*//***변경
        if (player == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.player.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        }*/
    }

    void Update()
    {
        OnCancelKeyPress();
    }

    void UpdateGameManagerState()
    {
        switch (GMState)
        {
           // case GameManagerState.Opening:
            //    break;
            case GameManagerState.Gameplay:

                backgroundSound.GetComponent<AudioSource>().Play();

                scoreUIText.GetComponent<GameScore>().Score = 0;

                // player.GetComponent<PlayerControl>().Init();
            
                enemySpawner.GetComponent<EnemySpawn>().ScheduleEnemySpawner();
                heartSpawner.GetComponent<HeartSpawn>().ScheduleHeartSpawner();

                break;

            case GameManagerState.Gameover:
                //적 스폰 중지
                enemySpawner.GetComponent<EnemySpawn>().UnScheduleEnemySpawner();

                //하트 스폰 중지
                heartSpawner.GetComponent<HeartSpawn>().UnScheduleHeartSpawner();

                //배경음악 중지
                backgroundSound.GetComponent<AudioSource>().Stop();

                //게임오버 보여주기
                gameOver.SetActive(true);

                gameOver.GetComponent<AudioSource>().Play();

                //게임룸 돌아가기?
                //Invoke("ChangeToRoom", 8f); //8초 뒤에 방 돌아가기

                break;
        }
    }

    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

   /* public void StartGamePlay()
    {
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
    }*/

    public void ChangeToRoom()
    {
        PhotonNetwork.LoadLevel(1);
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
