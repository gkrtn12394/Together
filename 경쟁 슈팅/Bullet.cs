using UnityEngine;
using UnityEngine.Networking;

public class Bullet : Photon.PunBehaviour
{
    private const int MAX_PLAYER = 4;
    private PhotonView pv;

    public int shooterPos;
    public int direction;
    public int speed;

    void start()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 collPoint = collision.contacts[0].point;

        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);

            Debug.Log("벽충돌");

            Debug.Log("(" + collPoint.x + ", " + collPoint.y + ", " + collPoint.z + "), positon: " + shooterPos);
        }

        if (collision.gameObject.tag == "PlayerA")
        {
            minusHp(collision.gameObject.GetComponent<Player_Init>().position);

            Destroy(gameObject);

            Debug.Log("총알 맞음 (맞은 사람: " + collision.gameObject.GetComponent<Player_Init>().position + ")");
        }

        if (collision.gameObject.tag == "Brick")
        {
            // 총알 방향 역방향으로
            if(direction == 1) // 3, 4
            {
                Debug.Log("curPos: " + shooterPos);
                this.GetComponent<MeshRenderer>().material = GameObject.Find("PlayerA(Clone)").GetComponent<Player_Fire>().materials[shooterPos - 2];
                this.GetComponent<Rigidbody>().velocity = this.transform.up * -1 * speed;
            }
            else if (direction == -1) // 1, 2
            {
                Debug.Log("curPos: " + shooterPos);
                this.GetComponent<MeshRenderer>().material = GameObject.Find("PlayerA(Clone)").GetComponent<Player_Fire>().materials[shooterPos + 2];
                this.GetComponent<Rigidbody>().velocity = this.transform.up * speed;
            }
        
            Debug.Log("장애물 맞음");
        }
    }

    void minusHp(int victimPos)
    {
        pv = GameObject.Find("NetworkManager").GetComponent<PhotonView>();

        pv.RPC("shootPerson", PhotonTargets.MasterClient, victimPos);
    }
}
