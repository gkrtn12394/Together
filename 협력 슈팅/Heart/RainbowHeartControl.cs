//<copyright file="RainbowHeartControl.cs">
//Copyright (c) 2019 All Rights Reserved
//</copyright>
//<author>Kang Daeun</author>
//<date>03/31/2019 12:49:58 AM </date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowHeartControl : MonoBehaviour
{

    float speed;

    void Start()
    {
        speed = 2f;
    }

    void Update()
    {
        Vector2 position = transform.position;  //하트의 현재 위치

        position = new Vector2(position.x, position.y + speed * Time.deltaTime);    // 새로운 위치 계산

        transform.position = position;  //위치 업데이트

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        if (transform.position.y > max.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerTag")
        {
            gameObject.GetComponent<AudioSource>().Play();

            Destroy(gameObject, 0.2f);
        }
    }
}
