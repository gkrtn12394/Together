using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharcter : MonoBehaviour
{
    public Button[] allButtons = new Button[4];
    public GameObject[] allMarks = new GameObject[4];

    static public string myCharcter;

    public void chooseCharcter()
    {
        for(int i = 0; i < 4; i++)
        {
            if(allButtons[i].interactable == true)
            {
                Debug.Log("allButtons[i].name: " + allButtons[i].name);

                if(allButtons[i].name == "CatButton")
                {
                    myCharcter = "cat";
                }
                else if (allButtons[i].name == "DogButton")
                {
                    myCharcter = "dog";
                }
                else if (allButtons[i].name == "LionButton")
                {
                    myCharcter = "lion";
                }
                else if (allButtons[i].name == "SquirrelButton")
                {
                    myCharcter = "squirrel";
                }

            }
        }
    }
}
