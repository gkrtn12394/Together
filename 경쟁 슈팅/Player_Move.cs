using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class Player_Move : Photon.MonoBehaviour
{
    [SerializeField]
    public float moveSpeed;

    private Transform tr;
    private PhotonView pv;

    private float firstH;
    private float firstV;

    private int direction = 0; // default: 0, up: 1, down: -1

    private Vector3 curPos;

    void move()
    {
        if (pv.isMine)
        {
            if (direction == -1) // position 1, 2
            {
                // 키보드 조작
                float keyH = Input.GetAxis("Horizontal") * moveSpeed;
                float keyV = Input.GetAxis("Vertical") * moveSpeed;

                if (tr.position.y + keyV > 800) // 이동 제한 (비행기 새로 길이 포함)
                {
                    tr.position = new Vector3(tr.position.x + keyH, tr.position.y + keyV, 0);
                }
                else
                {
                    tr.position = new Vector3(tr.position.x + keyH, tr.position.y, 0);
                }

                // 가속계 조작
                float h = Input.acceleration.x * moveSpeed;
                float v = Input.acceleration.z * -1 * moveSpeed;

                if (tr.position.y + v > 800) // 이동 제한
                {
                    tr.position = new Vector3(tr.position.x + h, tr.position.y + v, 0);
                }
                else
                {
                    tr.position = new Vector3(tr.position.x + h, tr.position.y, 0);
                }
            }
            else // position 3, 4
            {
                // 키보드 조작
                float keyH = Input.GetAxis("Horizontal") * moveSpeed;
                float keyV = Input.GetAxis("Vertical") * moveSpeed;

                if (tr.position.y + keyV < 625) // 이동 제한
                {
                    tr.position = new Vector3(tr.position.x + keyH, tr.position.y + keyV, 0);
                }
                else
                {
                    tr.position = new Vector3(tr.position.x + keyH, tr.position.y, 0);
                }

                // 가속계 조작
                float h = Input.acceleration.x * moveSpeed;
                float v = Input.acceleration.z * moveSpeed;

                if (tr.position.y + v < 625) // 이동 제한
                {
                    tr.position = new Vector3(tr.position.x + h, tr.position.y + v, 0);
                }
                else
                {
                    tr.position = new Vector3(tr.position.x + h, tr.position.y, 0);
                }
            }
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position, curPos, 0.5f);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            if (tr)
            {
                stream.SendNext("move.position:" + tr.position);
            }
            else
            {
                Debug.Log("Transform is Null");
                return;
            }

            //Debug.Log("move: writing");
        }
        if (stream.isReading)
        {
            string s = stream.ReceiveNext().ToString();

            if(s.Contains("move.position:"))
            {
                string strPos = s.Substring(s.IndexOf(":") + 1);

                string x = strPos.Substring(1, strPos.IndexOf(',') - 1);
                string y = strPos.Substring(strPos.IndexOf(',') + 2, strPos.IndexOf(',', strPos.IndexOf(',') - 2));

                if (float.TryParse(x, out float fx) && float.TryParse(y, out float fy))
                {
                    curPos = new Vector3(fx, fy, 0.0f);
                }
            }

            //Debug.Log("move: reading"); 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        direction = CrossSceneInfo.getMyDirection();
    }

    void FixedUpdate()
    {
        move();
    }
}
