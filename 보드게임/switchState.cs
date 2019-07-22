using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class switchState : MonoBehaviour
{
    public Button[] otherButton = new Button[3];
    public GameObject myCheck;
    public GameObject[] otherCheck = new GameObject[3];

    public void chageState()
    {
        if (myCheck.activeSelf == true)
        {
            for(int i = 0; i<3; i++)
            {
                if(otherCheck[i].activeSelf == false)
                {
                    Debug.Log("내가 체크했을 때 다른게 false 라면 버튼 상호작용을 없애는 것 ");
                    otherButton[i].interactable = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (otherCheck[i].activeSelf == false && myCheck.activeSelf == false)
                {
                    Debug.Log("내가 체크 해제할 때 다른것도 해제되어 있다면 활성화시키는 것");
                    otherButton[i].interactable = true;
                    
                }
            }
        }
    }

    public bool chageStateOther()
    {
        int count = 0;

        for(int i = 0; i < 3; i++)
        {
            if(otherCheck[i].activeSelf == true && otherButton[i].interactable == true)
            {
                count++;
            }
            else if(otherCheck[i].activeSelf == false && otherButton[i].interactable == false)
            {
                continue;
            }
        }

        if (count == 0) return false;
        else return true;
    }
}
