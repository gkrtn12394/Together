using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

// checkPosOverRange, changePosToPlayerView 범위 조정

public static class MapCustomInfo
{
    public static Vector3[] bricksPosition = new Vector3[3];
    public const int MAX_BRICKS = 1;
}

public class MapCustomizing : Photon.PunBehaviour
{
    private PhotonView pv;

    public GameObject brick;
    public GameObject OkButton;
    public Text okText;

    private int position;

    private UnityEvent allBricksInstantiated = new UnityEvent();
    private UnityEvent allPlayersReady = new UnityEvent();

    private bool[] isReady = new bool[CrossSceneInfo.myGame_MAX_PLAYERS];

    private int count = 0;

    void Start()
    {
        pv = this.GetComponent<PhotonView>();

        position = CrossSceneInfo.pos + 1;

        OkButton.SetActive(false);

        allPlayersReady.AddListener(OnAllPlayersReady);
        allBricksInstantiated.AddListener(OnAllBricksInstantiated);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (count < MapCustomInfo.MAX_BRICKS)
            {
                Vector3 checkedPos = checkPosOverRange(Input.mousePosition);

                GameObject newBrick = Instantiate(brick, checkedPos, Quaternion.identity);

                Debug.Log("장애물 생성");

                Vector3 changedPos = changePosToPlayerView(checkedPos);

                MapCustomInfo.bricksPosition[count] = changedPos;

                newBrick.transform.parent = GameObject.Find("Background").GetComponent<Transform>();

                count++;
            }
        }

        if (count == MapCustomInfo.MAX_BRICKS)
        {
            allBricksInstantiated.Invoke();
        }

        if (PhotonNetwork.isMasterClient)
        {
            checkIsReady();
        }
    }

    Vector3 checkPosOverRange(Vector3 pos)
    {
        Vector3 checkedPos = new Vector3(0, 0, 0);

        // 스테이지 범위 체크
        if (pos.x > 2850) checkedPos.x = 2850;
        else if (pos.x < 115) checkedPos.x = 115;
        else checkedPos.x = pos.x;

        if (pos.y > 1390) checkedPos.y = 1390;
        else if (pos.y < 60) checkedPos.y = 60;
        else checkedPos.y = pos.y;

        return checkedPos;
    }

    Vector3 changePosToPlayerView(Vector3 pos)
    {
        // 플레이어 시야로 변화
        if (position == 1)
        {
            pos.x = pos.x / 2;
            pos.y = pos.y / 2 + (1310 / 2.0f);

            Debug.Log("플레이어" + position + " 장애물 설치");
        }
        else if (position == 2)
        {
            pos.x = pos.x / 2 + (2735 / 2.0f);
            pos.y = pos.y / 2 + (1310 / 2.0f);

            Debug.Log("플레이어" + position + " 장애물 설치");
        }
        else if (position == 3)
        {
            pos.x = pos.x / 2;
            pos.y = pos.y / 2;

            Debug.Log("플레이어" + position + " 장애물 설치");
        }
        else if (position == 4)
        {
            pos.x = pos.x / 2 + (2735 / 2.0f);
            pos.y = pos.y / 2;

            Debug.Log("플레이어" + position + " 장애물 설치");
        }

        return pos;
    }

    void checkIsReady()
    {
        int i;

        for (i = 0; i < CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            if (isReady[i] == false) break;
        }

        if (i == CrossSceneInfo.myGame_MAX_PLAYERS)
        {
            Debug.Log("모두 준비 끝");

            allPlayersReady.Invoke();
        }
    }

    void OnAllPlayersReady()
    {
        SceneManager.LoadScene("CompeteShootingGame");
    }

    void OnAllBricksInstantiated()
    {
        OkButton.SetActive(true);
    }

    public void OnClickOkButton()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            pv.RPC("mapCustomEnd", PhotonTargets.MasterClient, CrossSceneInfo.pos);
        }
        else
        {
            isReady[CrossSceneInfo.pos] = true;
        }

        okText.text = "READY!!";
    }

    [PunRPC]
    void mapCustomEnd(int position)
    {
        // 배열[position]에 true 저장
        isReady[position] = true;
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
