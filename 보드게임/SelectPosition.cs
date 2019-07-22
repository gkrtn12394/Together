using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPosition : MonoBehaviour
{
    public GameObject[] allCamera = new GameObject[5];

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Display.displays.Length);

        if (PlayerCharcter.myCharcter == "cat")
        {
            // first
            for(int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    allCamera[i].SetActive(true);
                }
                else
                {
                    allCamera[i].SetActive(false);
                }
            }

        }
        else if(PlayerCharcter.myCharcter == "squirrel")
        {
            //second
            for (int i = 0; i < 5; i++)
            {
                if (i == 1)
                {
                    allCamera[i].SetActive(true);
                }
                else
                {
                    allCamera[i].SetActive(false);
                }
            }
        }
        else if(PlayerCharcter.myCharcter == "dog")
        {
            //third
            for (int i = 0; i < 5; i++)
            {
                if (i == 2)
                {
                    allCamera[i].SetActive(true);
                }
                else
                {
                    allCamera[i].SetActive(false);
                }
            }
        }
        else if(PlayerCharcter.myCharcter == "lion")
        {
            //fourth
            for (int i = 0; i < 5; i++)
            {
                if (i == 3)
                {
                    allCamera[i].SetActive(true);
                }
                else
                {
                    allCamera[i].SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
