using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animalButton : Photon.PunBehaviour
{
    public GameObject checkImage;
    private PhotonView pv;
    private string name;

    // Start is called before the first frame update
    void Start()
    {
        pv = GameObject.Find("Canvas").GetComponent<PhotonView>();

        name = gameObject.name;
        checkImage.SetActive(false);
    }

    public void OnClickAnimalButton()
    {
        if (!checkImage.GetActive())
        {
            checkImage.SetActive(true);
            if (name == "CatButton")
            {
                PlayerCharcter.myCharcter = "cat";
            }
            else if (name == "DogButton")
            {
                PlayerCharcter.myCharcter = "dog";
            }
            else if (name == "SquirrelButton")
            {
                PlayerCharcter.myCharcter = "squirrel";
            }
            else if (name == "LionButton")
            {
                PlayerCharcter.myCharcter = "lion";
            }
        }
        else
        {
            checkImage.SetActive(false);
            PlayerCharcter.myCharcter = "";
        }

        pv.RPC("notifyClick", PhotonTargets.Others, name);
        GameObject.Find(name).GetComponent<switchState>().chageState();
    }
}
