using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Fire : Photon.MonoBehaviour
{
    private PhotonView pv;

    private int position;
    private int direction = 0; // default: 0, up: 1, down: -1

    [Header("장착된 총")]
    [SerializeField]
    public Gun currentGun;

    [SerializeField]
    public Material[] materials;

    void fire()
    {
        GameObject bullet;
        Vector3 playerPos;

        if (pv.isMine)
        {
            if (direction == -1)
            {
                playerPos = new Vector3(currentGun.transform.position.x, currentGun.transform.position.y - 30.0f, currentGun.transform.position.z);
                bullet = Instantiate(currentGun.bullet, playerPos, Quaternion.identity);
                bullet.GetComponent<MeshRenderer>().material = materials[position];
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * -1 * currentGun.speed;
                bullet.GetComponent<Bullet>().direction = direction;
                bullet.GetComponent<Bullet>().shooterPos = position;
            }
            else
            {
                playerPos = new Vector3(currentGun.transform.position.x, currentGun.transform.position.y + 30.0f, currentGun.transform.position.z);
                bullet = Instantiate(currentGun.bullet, playerPos, Quaternion.identity);
                bullet.GetComponent<MeshRenderer>().material = materials[position];
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * currentGun.speed;
                bullet.GetComponent<Bullet>().direction = direction;
                bullet.GetComponent<Bullet>().shooterPos = position;
            }

            pv.RPC("fireOther", PhotonTargets.Others, playerPos, position);
        }
    }

    [PunRPC]
    void fireOther(Vector3 playerPos, int position)
    {
        GameObject bullet;

        if (direction == -1)
        {
            bullet = Instantiate(currentGun.bullet, playerPos, Quaternion.identity);
            bullet.GetComponent<MeshRenderer>().material = materials[position];
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * -1 * currentGun.speed;
            bullet.GetComponent<Bullet>().direction = direction;
            bullet.GetComponent<Bullet>().shooterPos = position;
        }
        else
        {
            bullet = Instantiate(currentGun.bullet, playerPos, Quaternion.identity);
            bullet.GetComponent<MeshRenderer>().material = materials[position];
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * currentGun.speed;
            bullet.GetComponent<Bullet>().direction = direction;
            bullet.GetComponent<Bullet>().shooterPos = position;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext("fire.position:" + position);
            stream.SendNext("fire.direction:" + direction);

            //Debug.Log("fire: writing");
        }
        else
        {
            string s = stream.ReceiveNext().ToString(); // pos

            if (s.Contains("fire.position:"))
            {
                string strPos = s.Substring(s.IndexOf(":") + 1);

                //Debug.Log(s + " strPos:" + strPos);

                position = Convert.ToInt32(strPos);
            }

            s = stream.ReceiveNext().ToString(); // dir

            if (s.Contains("fire.direction:"))
            {
                string strDir = s.Substring(s.IndexOf(":") + 1);

                //Debug.Log(s + " strDir:" + strDir);

                direction = Convert.ToInt32(strDir);
            }

            //Debug.Log("fire: reading");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        //pv.ObservedComponents[0] = this;

        direction = this.gameObject.GetComponent<Player_Init>().direction;
        position = this.gameObject.GetComponent<Player_Init>().position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fire();
        }
    }
}
