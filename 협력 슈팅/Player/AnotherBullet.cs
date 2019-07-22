//<copyright file="AnotherBullet.cs">
//Copyright (c) 2019 All Rights Reserved
//</copyright>
//<author>Kang Daeun</author>
//<date>05/12/2019 10:50:02 PM </date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherBullet : MonoBehaviour
{
    float speed;

    void Start()
    {
        speed = 8f;
    }

    void Update() 
    {
        Vector2 position = transform.position;  //총알의 현재 위치

        //총알의 새 위치 계산
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);

        transform.position = position; //총알 위치 업데이트

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "EnemyTag")
        {
            Destroy(gameObject);
        }
    }

}
