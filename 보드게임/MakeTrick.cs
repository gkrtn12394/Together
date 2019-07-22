using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTrick : MonoBehaviour
{
    public GameObject[] allbutton = new GameObject[16];

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerCharcter.myCharcter == "cat")
        {
            for (int i = 4; i < 16; i++)
            {
                allbutton[i].SetActive(false);
            }
        }
        else if (PlayerCharcter.myCharcter == "squirrel")
        {
            for (int i = 0; i < 4; i++)
            {
                allbutton[i].SetActive(false);
            }
            for (int i = 8; i < 16; i++)
            {
                allbutton[i].SetActive(false);
            }

        }
        else if (PlayerCharcter.myCharcter == "lion")
        {
            for (int i = 0; i < 8; i++)
            {
                allbutton[i].SetActive(false);
            }
            for (int i = 12; i < 16; i++)
            {
                allbutton[i].SetActive(false);
            }
        }
        else if (PlayerCharcter.myCharcter == "dog")
        {
            for (int i = 0; i < 12; i++)
            {
                allbutton[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}