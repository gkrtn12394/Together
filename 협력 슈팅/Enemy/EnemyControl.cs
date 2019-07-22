//<copyright file="EnemyControl.cs">
//Copyright (c) 2019 All Rights Reserved
//</copyright>
//<author>Kang Daeun</author>
//<date>03/15/2019 11:20:35 PM </date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    private PhotonView PhotonView;

    GameObject player;

    GameObject scoreUIText;
  

    public GameObject Explosion;

    float speed;

    int score;
    

    void Start()
    {
        speed = 1f;

        scoreUIText = GameObject.FindGameObjectWithTag("ScoreTextTag");

        player = GameObject.FindGameObjectWithTag("PlayerTag");

        PhotonView = PhotonView.Get(player);
    }

    void Update()
    {
        Vector2 position = transform.position;  //적의 현재 위치

        position = new Vector2(position.x, position.y + speed * Time.deltaTime);    //적의 새로운 위치 계산

        transform.position = position;  //적의 위치 업데이트

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        if(transform.position.y > max.y)
        {
            scoreUIText.GetComponent<GameScore>().Score -= 10;

            score = scoreUIText.GetComponent<GameScore>().Score;

            PhotonView.RPC("SyncTeamScore", PhotonTargets.All, -10);

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.tag == "PlayerTag") || (col.tag == "BulletTag"))
        {
            PlayExplosion();

            //50점 추가
            scoreUIText.GetComponent<GameScore>().Score += 50;

            score = scoreUIText.GetComponent<GameScore>().Score;

            PhotonView.RPC("SyncTeamScore", PhotonTargets.All, 50);

            Destroy(gameObject);
        }
    }

    /*[PunRPC]
    void SyncTeamScore(int score)
    {
        print("Sync Score");

        teamScoreUIText.GetComponent<TeamScore>().Score += sc;
        sc = teamScoreUIText.GetComponent<TeamScore>().Score;
    }*/

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(Explosion);

        explosion.transform.position = transform.position;
    }

}
