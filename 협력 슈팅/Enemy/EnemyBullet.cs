//<copyright file="EnemyBullet.cs">
//Copyright (c) 2019 All Rights Reserved
//</copyright>
//<author>Kang Daeun</author>
//<date>03/20/2019 11:44:44 AM </date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float speed;
    Vector2 _direction; //총알 방향
    bool isReady;   //총알 방향이 설정되어 있는지

    void Awake()
    {
        speed = 5f;
        isReady = false;
    }

    void Start()
    {
        
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized; //direction normalized, 유닛벡터 얻기위해

        isReady = true;
    }

    void Update()
    {
        if (isReady)
        {
            Vector2 position = transform.position;  //총알 현재 위치

            position += _direction * speed * Time.deltaTime; //총알 위치 계산

            transform.position = position;  //총알 위치 업데이트

            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            if ((transform.position.x < min.x) || (transform.position.x > max.x)||
                (transform.position.y < min.y) || (transform.position.y > max.y))
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerTag")
        {
            Destroy(gameObject);
        }
    }

}
