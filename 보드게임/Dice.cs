using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : Photon.PunBehaviour
{
    private PhotonView pv;
    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private int whosTurn = 1;
    private bool coroutineAllowed = true;

    // Start is called before the first frame update
    void Start()
    {
        //pv = GameObject.Find("Canvas").GetComponent<PhotonView>();
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rend.sprite = diceSides[5];
    }

    private void OnMouseDown()
    {
        pv = GameObject.Find("BoardCanvas").GetComponent<PhotonView>();
        if (!GameControl.gameOver && coroutineAllowed)
            StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        GameControl.diceSideThrown = randomDiceSide + 1;
        if (whosTurn == 1)
        {
            if(GameControl.dogMove == false)
            {
                GameControl.dogMove = true;
            }
            else
            {
                GameControl.MovePlayer(1);
            }
        }
        else if ( whosTurn == 2 )
        {
            if (GameControl.catMove == false)
            {
                GameControl.catMove = true;
            }
            else
            {
                GameControl.MovePlayer(2);
            } 
        }
        else if (whosTurn == 3)
        {
            if (GameControl.lionMove == false)
            {
                GameControl.lionMove = true;
            }
            else
            {
                GameControl.MovePlayer(3);
            }
        }
        else if (whosTurn == 4)
        {
            if (GameControl.squirrelMove == false)
            {
                GameControl.squirrelMove = true;
            }
            else
            {
                GameControl.MovePlayer(4);
            }
        }

        Debug.Log("whosTurn : " + whosTurn);
        Debug.Log("diceside : " + GameControl.diceSideThrown);
        Debug.Log("pv:" + pv.isMine);
        pv.RPC("moveFriend", PhotonTargets.Others, whosTurn, GameControl.diceSideThrown);

        whosTurn++;

        if(whosTurn == 5)
        {
            whosTurn = 1;
        }

        coroutineAllowed = true;
    }

    //[PunRPC]
    //public void moveFriend(int turn, int sideNum)
    //{
    //    Debug.Log("옮겨보자 ");
    //    GameControl.MovePlayer(turn);
    //    GameControl.diceSideThrown = sideNum;
    //}
}
