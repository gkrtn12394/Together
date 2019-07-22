using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChooseTrick : Photon.PunBehaviour
{
    public GameObject[] sameUserButton = new GameObject[3];
    public int myNum = 0;
    public int num = 1;
    private PhotonView pv;

    void Start()
    {
        pv = GameObject.Find("Canvas").GetComponent<PhotonView>();
    }

    public void choosePlace()
    {
        Debug.Log("하긴 하니?");

        if (num == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                sameUserButton[i].SetActive(false);
            }
            if(PlayerCharcter.myCharcter == "cat")
            {
                SelectTrick.boardNums[0] = myNum;
            }
            else if(PlayerCharcter.myCharcter == "dog")
            {
                SelectTrick.boardNums[1] = myNum;
            }
            else if(PlayerCharcter.myCharcter == "lion")
            {
                SelectTrick.boardNums[2] = myNum;
            }
            else if(PlayerCharcter.myCharcter == "squirrel")
            {
                SelectTrick.boardNums[3] = myNum;
            }
            pv.RPC("choosePlaceOther", PhotonTargets.Others, PlayerCharcter.myCharcter, myNum, num);
            num++;
        }
        else if (num == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                sameUserButton[i].SetActive(true);
            }
            if (PlayerCharcter.myCharcter == "cat")
            {
                SelectTrick.boardNums[0] = -1;
            }
            else if (PlayerCharcter.myCharcter == "dog")
            {
                SelectTrick.boardNums[1] = -1;
            }
            else if (PlayerCharcter.myCharcter == "lion")
            {
                SelectTrick.boardNums[2] = -1;
            }
            else if (PlayerCharcter.myCharcter == "squirrel")
            {
                SelectTrick.boardNums[3] = -1;
            }
            pv.RPC("choosePlaceOther", PhotonTargets.Others, PlayerCharcter.myCharcter, myNum, num);
            num--;
        }
    }
}
