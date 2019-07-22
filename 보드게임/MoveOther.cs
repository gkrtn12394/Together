using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOther : Photon.PunBehaviour
{
    private PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("cat : " + SelectTrick.boardNums[0]);
        Debug.Log("squirrel :" + SelectTrick.boardNums[3]);
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void moveFriend(int turn, int sideNum)
    {
        Debug.Log("옮겨보자 ");
        GameControl.MovePlayer(turn);
        GameControl.diceSideThrown = sideNum;
    }
}
