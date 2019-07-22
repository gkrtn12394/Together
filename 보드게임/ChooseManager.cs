using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseManager : Photon.PunBehaviour
{
    public Button CatButton;
    public Button DogButton;
    public Button SquirrelButton;
    public Button LionButton;
    public Button NextButton;

    public GameObject[] checkImages = new GameObject[4];

    private bool isPopup = false;

    private PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        NextButton.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool isAllChecked = checkCheckImages();

        if (isAllChecked)
        {
            NextButton.enabled = true;
        }

        OnCancelKeyPress();
    }

    bool checkCheckImages()
    {
        int i = 0;

        for(i = 0; i < 4; i++)
        {
            if (checkImages[i].activeSelf == false) break;
        }

        if (i == 4) return true;
        else return false;
    }

    [PunRPC]
    void notifyClick(string name)
    {
        Debug.Log("RPC 받음");

        GameObject foundButton = GameObject.Find(name);

        if (!foundButton.GetComponent<animalButton>().checkImage.GetActive())
        {
            Debug.Log("RPC로 받고나서 만약 그 체크가 액티브가 안되어있다면 활성화시키고 상호작용 없앰");
            foundButton.GetComponent<animalButton>().checkImage.SetActive(true);
            foundButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            Debug.Log("RPC로 받고나서 만약 그 체크가 활성화되어있다면 체크 활성화를 없애고");
            foundButton.GetComponent<animalButton>().checkImage.SetActive(false);
            if (foundButton.GetComponent<switchState>().chageStateOther() == false)
            {
                foundButton.GetComponent<Button>().interactable = true;
            }
        }
    }

    [PunRPC]
    void changeSceneOther(string name)
    {
        Debug.Log("나는 로그야");
        //GameObject.Find("Main Camera").GetComponent<PlayerCharcter>().chooseCharcter();
        Application.LoadLevel(name);
    }

    void OnCancelKeyPress()
    {
        //if (Application.platform == RuntimePlatform.Android)
        //{
        if (Input.GetKeyDown(KeyCode.Escape) && isPopup == false)
        {
            isPopup = true;

            QuitPopup.show(new QuitPopupOptions
            {
                cancelButtonDelegate = () =>
                {
                    Debug.Log("cancel");
                    isPopup = false;
                },
                okButtonDelegate = () =>
                {
                    Debug.Log("ok");

                    PhotonNetwork.Disconnect();

                    SceneManager.LoadScene("join_create");

                    isPopup = false;
                }
            });
        }
        //}
    }
}
