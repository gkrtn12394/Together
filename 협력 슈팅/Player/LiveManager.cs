//<copyright file="LiveManager.cs">
//Copyright (c) 2019 All Rights Reserved
//</copyright>
//<author>Kang Daeun</author>
//<date>04/22/2019 1:26:47 PM </date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiveManager : MonoBehaviour
{
    public Text livesTextUI;

    int lives;

    public int Lives
    {
        get
        {
            return this.lives;
        }
        set
        {
            this.lives = value;
            UpdateLivesTextUI();
        }
    }

    void Start()
    {
        livesTextUI = GetComponent<Text>();
    }

    void UpdateLivesTextUI()
    {
        livesTextUI.text = lives.ToString();
    }
}
