using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public static class SettingScreenInfo
{
    public static Vector3 screenPos;
    public static float screenWidth;
    public static float screenHeight;
}

public class SettingScreen : Photon.PunBehaviour
{
    private PhotonView pv;

    private int position;
    public Image[] highlights = new Image[4];
    private bool[] isReady = new bool[CrossSceneInfo.myGame_MAX_PLAYERS];

    private Color32 bgColor = new Color32(214, 122, 202, 255);

    private UnityEvent allPlayersReady = new UnityEvent();

    private bool isClicked = false;
    private bool isPopup = false;
    private Vector3 downPos;
    private Vector3 upPos;

    public Text okText;

    public Image range;
    public Vector3 initPos;

    private float width;
    private float height;
    private float centerX;
    private float centerY;

    // Start is called before the first frame update
    void Start()
    {
        pv = this.GetComponent<PhotonView>();

        position = CrossSceneInfo.pos + 1;

        Debug.Log("SettingScreen: Positon: " + position);

        allPlayersReady.AddListener(OnAllPlayersReady);

        setHighlight();

        // range 초기화 (position이 1인 경우)
        // 위치 초기화 (initPos)
        rangePosInit();

        range.transform.Translate(initPos);
        range.rectTransform.sizeDelta = new Vector2(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            downPos = Input.mousePosition;

            if (position == 1)
            {
                if (downPos.x > 2500 && downPos.y < 400)
                {
                    isClicked = true;
                    Debug.Log("Down: " + downPos);
                }
            }
            else if (position == 2)
            {
                if (downPos.x < 480 && downPos.y < 400)
                {
                    isClicked = true;
                    Debug.Log("Down: " + downPos);
                }
            }
            else if (position == 3)
            {
                if (downPos.x > 2500 && downPos.y > 1100)
                {
                    isClicked = true;
                    Debug.Log("Down: " + downPos);
                }
            }
            else if (position == 4)
            {
                if (downPos.x < 480 && downPos.y > 1100)
                {
                    isClicked = true;
                    Debug.Log("Down: " + downPos);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            upPos = Input.mousePosition;

            // checkArea(); // 체크해서 몇 이하의 면적이면 사각형 취소, OK 비활성화

            Debug.Log("Up: " + upPos);
            isClicked = false;
        }

        if (isClicked)
        {
            drawRange();
            Debug.Log("그리는 중....");
        }

        if (PhotonNetwork.isMasterClient)
        {
            checkIsReady();
        }
    }

    void rangePosInit()
    {
        if (position == 1)
        {
            initPos = new Vector3(1480, -720);
            Debug.Log("포1");
        }
        else if (position == 2)
        {
            initPos = new Vector3(-1480, -720);
            Debug.Log("포2");

        }
        else if (position == 3)
        {
            initPos = new Vector3(-1480, 720);
            Debug.Log("포3");

        }
        else if (position == 4)
        {
            initPos = new Vector3(1480, 720);
            Debug.Log("포4");

        }
    }

    void drawRange()
    {
        // 현재 마우스 위치 받아와서
        Vector3 curPos = Input.mousePosition;

        Debug.Log("curPos: " + curPos);

        if (position == 1)
        {
            // 실 좌표계에서 canvas 좌표계로 변환
            initPos.x = 2960;
            initPos.y = 0;

            width = initPos.x - curPos.x;
            height = curPos.y - initPos.y;

            Debug.Log("width: " + width + " y: " + height);

            // 현재 x, y 구함
            centerX = (initPos.x - (width / 2));
            centerY = (initPos.y + (height / 2));
        }
        else if (position == 2)
        {
            // 실 좌표계에서 canvas 좌표계로 변환
            initPos.x = 0;
            initPos.y = 0;

            width = curPos.x - initPos.x;
            height = curPos.y - initPos.y;

            Debug.Log("width: " + width + " y: " + height);

            // 현재 x, y 구함
            centerX = (initPos.x + (width / 2));
            centerY = (initPos.y + (height / 2));
        }
        else if (position == 3)
        {
            // 실 좌표계에서 canvas 좌표계로 변환
            initPos.x = 2960;
            initPos.y = 1440;

            width = initPos.x - curPos.x;
            height = initPos.y - curPos.y;

            Debug.Log("width: " + width + " y: " + height);

            // 현재 x, y 구함
            centerX = (initPos.x - (width / 2));
            centerY = (initPos.y - (height / 2));
        }
        else if (position == 4)
        {
            // 실 좌표계에서 canvas 좌표계로 변환
            initPos.x = 0;
            initPos.y = 1440;

            width = curPos.x - initPos.x;
            height = initPos.y - curPos.y;

            Debug.Log("width: " + width + " y: " + height);

            // 현재 x, y 구함
            centerX = (initPos.x + (width / 2));
            centerY = (initPos.y - (height / 2));
        }

        // 위치 설정
        range.transform.position = new Vector3(centerX, centerY);
        Debug.Log("centerX: " + centerX + " curY: " + centerY);

        // 크기 설정
        range.rectTransform.sizeDelta = new Vector2(width, height);
    }

    public void OnClickOKButton()
    {
        SettingScreenInfo.screenPos.x = centerX;
        SettingScreenInfo.screenPos.y = centerY;
        SettingScreenInfo.screenHeight = height;
        SettingScreenInfo.screenWidth = width;

        if (!PhotonNetwork.isMasterClient)
        {
            pv.RPC("screenSettingEnd", PhotonTargets.MasterClient, CrossSceneInfo.pos);
        }
        else
        {
            isReady[CrossSceneInfo.pos] = true;
        }

        okText.text = "READY!!";
    }

    // 배열에 모두 스크린 셋팅이 끝났는지 저장해서 다 완료되면 방장이 ok
    [PunRPC]
    void screenSettingEnd(int position)
    {
        // 배열[position]에 true 저장
        isReady[position] = true;
    }

    void OnAllPlayersReady()
    {
        SceneManager.LoadScene("Map Customizing");
    }

    void checkIsReady()
    {
        int i;

        for(i = 0; i < CrossSceneInfo.myGame_MAX_PLAYERS; i++)
        {
            if (isReady[i] == false) break;
        }

        if(i == CrossSceneInfo.myGame_MAX_PLAYERS)
        {
            Debug.Log("모두 준비 끝");

            allPlayersReady.Invoke();
        }
    }


    void setHighlight()
    {
        if (position == 1)
        {
            highlights[0].color = bgColor;
            highlights[2].color = bgColor;
        }
        else if (position == 2)
        {
            highlights[0].color = bgColor;
            highlights[3].color = bgColor;
        }
        else if (position == 3)
        {
            highlights[1].color = bgColor;
            highlights[2].color = bgColor;
        }
        else if (position == 4)
        {
            highlights[1].color = bgColor;
            highlights[3].color = bgColor;
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
}
