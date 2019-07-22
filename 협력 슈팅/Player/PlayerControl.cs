//<copyright file="PlayerControl.cs">
//Copyright (c) 2019 All Rights Reserved
//</copyright>
//<author>Kang Daeun</author>
//<date>03/19/2019 4:42:13 PM </date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerControl : Photon.MonoBehaviour
{
   private PhotonView PhotonView;

    GameObject GameManagerGO;

    public GameObject PlayerBullet;
    public GameObject BulletPosition;
    public GameObject Explosion;

    public GameObject OtherBullet;

    GameObject livesUIText;    //lives ui text 레퍼런스
    const int MaxLives = 3;
    int lives;  //현재 플레이어 생명
    bool isUnBeatTime = false;  //아팠을 때 잠깐 무적
    SpriteRenderer thisSR;

    public float speed;
    float accelStartY;   //처음 게임 시작할 때 accelator의 y값

    GameObject teamScoreUIText;
    int sc;

    void Start()
    {
        //Screen.SetResolution(1280, 720, false);
        //Screen.SetResolution(Screen.width, Screen.width * 16 / 9, false);

        PhotonView = GetComponent<PhotonView>();

        GameManagerGO = GameObject.FindGameObjectWithTag("GameManagerTag");

        livesUIText = GameObject.FindGameObjectWithTag("LiveTextTag");

        lives = MaxLives;

        livesUIText.GetComponent<LiveManager>().Lives = lives;

        accelStartY = Input.acceleration.y;

        thisSR = gameObject.GetComponent<SpriteRenderer>();

        teamScoreUIText = GameObject.FindGameObjectWithTag("TeamScoreTextTag");

    }

    void Update()
    {
        //3)
        //if (!PhotonView.isMine)
         // return;

        if (Input.GetMouseButtonDown(0))
        {
            print("shoot");

            gameObject.GetComponent<AudioSource>().Play();

            GameObject bullet = (GameObject)Instantiate(PlayerBullet);
            bullet.transform.position = BulletPosition.transform.position;
        }

       /* //윈도우 테스트용
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(x, y).normalized;*/

        //가속도 센서 이용(모바일)
        float x = Input.acceleration.x;
        float y = Input.acceleration.y - accelStartY;

        Vector2 direction = new Vector2(x, y);

        if (direction.sqrMagnitude > 1)
            direction.Normalize();
        //이 위까지 가속도 센서 (윈도우 테스트 시 주석처리 할 것)*/

        Move(direction);
    }

    [PunRPC]
    void FireOther(Vector3 position)
    {
        print("fire");

        GameObject bullet = (GameObject)Instantiate(OtherBullet);

        Vector3 temp = new Vector3(2 * position.x, 0, 0);
        position -= temp;

        bullet.transform.position = position;
    }

    void Move(Vector2 direction)
    {
        //상,하,좌,우 limit
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));  //왼쪽 아래
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));  //오른쪽 위

        max.x = max.x - 0.7f;
        min.x = min.x + 0.7f;

        max.y = max.y - 0.755f;
        min.y = min.y + 0.75f;

        Vector2 pos = transform.position;   //플레이어 현재 위치

        pos += direction * speed * Time.deltaTime;  //새 위치 계산

        //새로운 위치가 스크린밖으로 벗어나지 않게
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;   //플레이어 위치 업데이트
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (((col.tag == "EnemyTag") || (col.tag == "EnemyBulletTag")) && !isUnBeatTime)
        {
            lives--;
            livesUIText.GetComponent<LiveManager>().Lives = lives;

            if (lives == 0)
            {
                PhotonView.RPC("Die", PhotonTargets.All);
            }
            else
            {
                isUnBeatTime = true;
                StartCoroutine("UnBeatTime");   //깜빡이게 하면서 무적 코루틴
            }
        }
        else if (col.tag == "HeartTag" && lives < 3)
        {
            lives++;
            livesUIText.GetComponent<LiveManager>().Lives = lives;
        }
    }

    [PunRPC]
    void Die()
    {
        PlayExplosion();    //폭발

        //게임오버
        GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.Gameover);

        gameObject.SetActive(false);    //플레이어 숨기는 거
                                        //Destroy(gameObject);  //죽이기
    }

    IEnumerator UnBeatTime()
    {
        int countTime = 0;

        while (countTime < 10)
        {
            if (countTime % 2 == 0)
                thisSR.color = new Color32(255, 255, 255, 90);
            else
                thisSR.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(0.2f);  //딜레이를 주어야 함.

            countTime++;
        }

        thisSR.color = new Color32(255, 255, 255, 255);

        isUnBeatTime = false;

        yield return null;
    }

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(Explosion);

        explosion.transform.position = transform.position;
    }

    [PunRPC]
    void SyncTeamScore(int score)
    {
        print("Sync Score");

        teamScoreUIText.GetComponent<TeamScore>().Score += score;
        sc = teamScoreUIText.GetComponent<TeamScore>().Score;
    }

}