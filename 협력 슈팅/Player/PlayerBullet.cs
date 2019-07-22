//<copyright file="PlayerBullet.cs">
//Copyright (c) 2019 All Rights Reserved
//</copyright>
//<author>Kang Daeun</author>
//<date>03/19/2019 4:52:29 PM </date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private PhotonView PhotonView;

    GameObject player;

    float speed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerTag");

        PhotonView = PhotonView.Get(player);

        speed = 8f;
    }

    void Update()
    {
        Vector2 position = transform.position;  //총알의 현재 위치

        //총알의 새 위치 계산
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);

        transform.position = position; //총알 위치 업데이트

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        if (transform.position.y > max.y)
        {
            PhotonView.RPC("FireOther", PhotonTargets.Others, transform.position);

            gameObject.SetActive(false); //Destroy(gameObject);
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
