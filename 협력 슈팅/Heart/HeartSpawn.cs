//<copyright file="HeartSpawn.cs">
//Copyright (c) 2019 All Rights Reserved
//</copyright>
//<author>Kang Daeun</author>
//<date>03/31/2019 1:01:53 AM </date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSpawn : MonoBehaviour
{
    public GameObject Heart;

    float spawnRateInSeconds = 10f;   //하트충전 생성 속도

    void Start()
    {
        
    }

    void Update()
    {

    }

    void SpawnHeart()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        GameObject aHeart = (GameObject)Instantiate(Heart);
        aHeart.transform.position = new Vector2(Random.Range(min.x, max.x), min.y);

        ScheduleNextHeartSpawn();
    }

    void ScheduleNextHeartSpawn()
    {
        Invoke("SpawnHeart", spawnRateInSeconds);
    }

    public void ScheduleHeartSpawner()
    {
        Invoke("SpawnHeart", spawnRateInSeconds);
    }

    public void UnScheduleHeartSpawner()
    {
        CancelInvoke("SpawnHeart");
    }

}
