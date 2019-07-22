using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : Photon.PunBehaviour
{
    private PhotonView pv;

    void Start()
    {
        pv = GameObject.Find("Canvas").GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    public void changeScene(string scene_name)
    {
        //GameObject.Find("Main Camera").GetComponent<PlayerCharcter>().chooseCharcter();
        pv.RPC("changeSceneOther", PhotonTargets.Others, scene_name);
        Application.LoadLevel(scene_name);

        Debug.Log("신 변경");
        Debug.Log("dd" + pv.isMine);
    }

    public void secondScenechange(string scene_name)
    {
        pv.RPC("changeSceneOther", PhotonTargets.Others, scene_name);
        Application.LoadLevel(scene_name);
    }
}
