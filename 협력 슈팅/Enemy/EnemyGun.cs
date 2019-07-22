//<copyright file="EnemyGun.cs">
//Copyright (c) 2019 All Rights Reserved
//</copyright>
//<author>Kang Daeun</author>
//<date>03/20/2019 12:00:51 PM </date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject EnemyBullet;

    void Start()
    {
        Invoke("FireEnemyBullet", 1f);
    }

    void Update()
    {
        
    }

    void FireEnemyBullet()
    {
        GameObject player = GameObject.Find("AirplaneY");   //**일단 이렇게 해놓음

        if(player != null)
        {
            GameObject bullet = (GameObject)Instantiate(EnemyBullet);

            bullet.transform.position = transform.position; //총알의 처음 위치 설정

            Vector2 direction = player.transform.position - bullet.transform.position;

            bullet.GetComponent<EnemyBullet>().SetDirection(direction);
        }
    }
}
