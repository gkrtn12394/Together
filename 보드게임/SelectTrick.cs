using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectTrick : Photon.PunBehaviour
{
    private PhotonView pv;
    static public int[] boardNums = new int[4];
    public GameObject[] allbutton = new GameObject[16];
    public Button nextButton;

    public bool isPopup = false;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        //nextButton.enabled = false;

        //if(PlayerCharcter.myCharcter == "cat")
        //{
        //    for(int i = 4; i < 16; i++)
        //    {
        //        allbutton[i].SetActive(false);
        //    }
        //}
        //else if(PlayerCharcter.myCharcter == "squirrel")
        //{
        //    for(int i = 0; i < 4; i++)
        //    {
        //        allbutton[i].SetActive(false);
        //    }
        //    for (int i = 8; i < 16; i++)
        //    {
        //        allbutton[i].SetActive(false);
        //    }

        //}
        //else if(PlayerCharcter.myCharcter == "lion")
        //{
        //    for (int i = 0; i < 8; i++)
        //    {
        //        allbutton[i].SetActive(false);
        //    }
        //    for (int i = 12; i < 16; i++)
        //    {
        //        allbutton[i].SetActive(false);
        //    }
        //}
        //else if(PlayerCharcter.myCharcter == "dog")
        //{
        //    for (int i = 0; i < 12; i++)
        //    {
        //        allbutton[i].SetActive(false);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        bool isAllChecked = checkNumber();

        if (isAllChecked)
        {
            nextButton.enabled = true;
        }

        OnCancelKeyPress();
    }

    bool checkNumber()
    {
        int count = 0;
        for(int i = 0; i < 4; i++)
        {
            count++;
            if (boardNums[i] == -1) break;
        }

        if (count == 4) return true;
        else return false;
    }

    [PunRPC]
    void choosePlaceOther(string charcter, int mynum, int onoff)
    {
        Debug.Log("RPC 받음");

        if(charcter == "cat")
        {
            if(onoff == 1)
            {
                boardNums[0] = mynum;
            }
            else if(onoff == 2)
            {
                boardNums[0] = -1;
            }
        }
        else if(charcter == "dog")
        {
            if (onoff == 1)
            {
                boardNums[1] = mynum;
            }
            else if (onoff == 2)
            {
                boardNums[1] = -1;
            }
        }
        else if(charcter == "lion")
        {
            if (onoff == 1)
            {
                boardNums[2] = mynum;
            }
            else if (onoff == 2)
            {
                boardNums[2] = -1;
            }
        }
        else if(charcter == "squirrel")
        {
            if (onoff == 1)
            {
                boardNums[3] = mynum;
            }
            else if (onoff == 2)
            {
                boardNums[3] = -1;
            }
        }
    }

    [PunRPC]
    void changeSceneOther(string name)
    {
        Debug.Log("나는 로그야");
        Application.LoadLevel(name);
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
