using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    NormalGun,
}

public class Gun : MonoBehaviour
{
    [Header("총 유형")]
    public WeaponType weaponType;

    [Header("총 연사속도 조정")]
    public float fireRate;

    [Header("총알 갯수")]
    public int bulletCount;
    public int maxBulletCount;

    [Header("총알 프리팹")]
    public GameObject bullet;

    [Header("총알 속도")]
    public float speed;


    // aaaaaaaaaaaaaaaaaa
}
