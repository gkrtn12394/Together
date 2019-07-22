using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class JoinOrCreate
{
    public static bool selectingJoinRoom = false;
}


public class CreateRoom : Photon.PunBehaviour
{
    private string roomName = "Shooting Game";
    private string version = "0.1";

    void Awake()
    {
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = true;

        // 해상도 적용
        Screen.SetResolution(2960, 1440, true);
    }

    public void OnClickJoinRoom()
    {
        JoinOrCreate.selectingJoinRoom = true;

        Debug.Log("selectingJoinRoom: " + JoinOrCreate.selectingJoinRoom);

        SceneManager.LoadScene("SelectGame");
    }

    public void OnClickCreateRoom()
    {
        JoinOrCreate.selectingJoinRoom = false;

        Debug.Log("selectingJoinRoom: " + JoinOrCreate.selectingJoinRoom);

        SceneManager.LoadScene("SelectGame");
    }

    public void OnClickQuit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
