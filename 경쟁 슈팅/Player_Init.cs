using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System;

// 카메라 할당 수정필요
// 

public class Player_Init : Photon.MonoBehaviour
{
    public static GameObject myPlayer;

    [SerializeField]
    public Vector3[] spawnPositions;

    string[] playerNames;
    string playerName;

    public int position;
    public int direction = 0; // default: 0, up: 1, down: -1

    [SerializeField]
    public Sprite[] playerImages;

    private PhotonView pv;

    private bool isSet = false; // remote용 변수

    void initDir()
    {
        position = CrossSceneInfo.pos + 1;
        direction = CrossSceneInfo.getMyDirection();

        Debug.Log("pos:" + position + ", dir: " + direction);
    }

    void rotatePlayer()
    {
        if (direction == -1)
        {
            transform.Rotate(180, 0, 0);

            Debug.Log("rotated! // pos: " + position);
        }
    }

    void setColor()
    {
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = playerImages[position];
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext("init.position:" + position);
            stream.SendNext("init.direction:" + direction);

            //Debug.Log("init: writing");
        }
        else
        {
            string s = stream.ReceiveNext().ToString(); // pos

            if (s.Contains("init.position:"))
            {
                string strPos = s.Substring(s.IndexOf(":") + 1);

                //Debug.Log(s + " strPos:" + Convert.ToInt32(strPos));

                position = Convert.ToInt32(strPos);
            }

            s = stream.ReceiveNext().ToString(); // dir

            if (s.Contains("init.direction:"))
            {
                string strDir = s.Substring(s.IndexOf(":") + 1);

                //Debug.Log(s + " strDir:" + Convert.ToInt32(strDir));

                direction = Convert.ToInt32(strDir);
            }

            if(!isSet)
            {
                rotatePlayer();
                setColor();

                isSet = true;
            }

            //Debug.Log("init: reading");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
        playerNames = CrossSceneInfo.playerNames;

        if (pv.isMine)
        {
            initDir();

            playerName = playerNames[position - 1]; // playerNames는 index 0부터 시작

            rotatePlayer();
            setColor();

            myPlayer = Player.player;
        }
    }

    void Update()
    {
    }
}
