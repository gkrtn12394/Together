using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    private static GameObject player1MoveText, player2MoveText, player3MoveText, player4MoveText;
    private static GameObject catWinsText, squirrelWinsText, dogWinsText, lionWinsText;
    private static GameObject dog, cat, lion, squirrel;

    public static bool catMove = true;
    public static bool dogMove = true;
    public static bool lionMove = true;
    public static bool squirrelMove = true;
    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;
    public static int player3StartWaypoint = 0;
    public static int player4StartWaypoint = 0;

    public static bool gameOver = false;

    private bool isPopup = false;

    // Start is called before the first frame update
    void Start()
    {
        catWinsText = GameObject.Find("catWinsText");
        squirrelWinsText = GameObject.Find("squirrelWinsText");
        dogWinsText = GameObject.Find("dogWinsText");
        lionWinsText = GameObject.Find("lionWinsText");

        player1MoveText = GameObject.Find("DogText");
        player2MoveText = GameObject.Find("CatText");
        player3MoveText = GameObject.Find("LionText");
        player4MoveText = GameObject.Find("SquirrelText");

        dog = GameObject.Find("dog"); // player1
        cat = GameObject.Find("cat"); // player2
        lion = GameObject.Find("lion");  // player3
        squirrel = GameObject.Find("squirrel"); // player4

        dog.GetComponent<FollowThePath>().moveAllowed = false;
        cat.GetComponent<FollowThePath>().moveAllowed = false;
        lion.GetComponent<FollowThePath>().moveAllowed = false;
        squirrel.GetComponent<FollowThePath>().moveAllowed = false;

        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);
        player3MoveText.gameObject.SetActive(false);
        player4MoveText.gameObject.SetActive(false);

        catWinsText.gameObject.SetActive(false);
        squirrelWinsText.gameObject.SetActive(false);
        dogWinsText.gameObject.SetActive(false);
        lionWinsText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        OnCancelKeyPress();

        if (dog.GetComponent<FollowThePath>().waypointIndex > player1StartWaypoint + diceSideThrown)
        {
            dog.GetComponent<FollowThePath>().moveAllowed = false;
            if((dog.GetComponent<FollowThePath>().waypointIndex == SelectTrick.boardNums[0]) || (dog.GetComponent<FollowThePath>().waypointIndex == SelectTrick.boardNums[2]) || (dog.GetComponent<FollowThePath>().waypointIndex == SelectTrick.boardNums[3]))
            {
                dogMove = false;
            }
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
            player1StartWaypoint = dog.GetComponent<FollowThePath>().waypointIndex - 1; 
        }

        if (cat.GetComponent<FollowThePath>().waypointIndex > player2StartWaypoint + diceSideThrown)
        {
            cat.GetComponent<FollowThePath>().moveAllowed = false;
            if ((cat.GetComponent<FollowThePath>().waypointIndex == SelectTrick.boardNums[1]) || (cat.GetComponent<FollowThePath>().waypointIndex == SelectTrick.boardNums[2]) || (cat.GetComponent<FollowThePath>().waypointIndex == SelectTrick.boardNums[3]))
            {
                catMove = false;
            }
            player2MoveText.gameObject.SetActive(false);
            player3MoveText.gameObject.SetActive(true);
            player2StartWaypoint = cat.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if (lion.GetComponent<FollowThePath>().waypointIndex > player3StartWaypoint + diceSideThrown)
        {
            lion.GetComponent<FollowThePath>().moveAllowed = false;
            if ((lion.GetComponent<FollowThePath>().waypointIndex == SelectTrick.boardNums[0]) || (lion.GetComponent<FollowThePath>().waypointIndex == SelectTrick.boardNums[1]) || (lion.GetComponent<FollowThePath>().waypointIndex == SelectTrick.boardNums[3]))
            {
                lionMove = false;
            }
            player3MoveText.gameObject.SetActive(false);
            player4MoveText.gameObject.SetActive(true);
            player3StartWaypoint = lion.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if (squirrel.GetComponent<FollowThePath>().waypointIndex > player4StartWaypoint + diceSideThrown)
        {
            squirrel.GetComponent<FollowThePath>().moveAllowed = false;
            if ((squirrel.GetComponent<FollowThePath>().waypointIndex == SelectTrick.boardNums[0]) || (squirrel.GetComponent<FollowThePath>().waypointIndex == SelectTrick.boardNums[1]) || (squirrel.GetComponent<FollowThePath>().waypointIndex == SelectTrick.boardNums[2]))
            {
                squirrelMove = false;
            }
            player4MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
            player4StartWaypoint = squirrel.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if(dog.GetComponent<FollowThePath>().waypointIndex == dog.GetComponent<FollowThePath>().waypoints.Length) // 강아지 승리
        {
            dogWinsText.gameObject.SetActive(true);
            catWinsText.gameObject.SetActive(true);
            squirrelWinsText.gameObject.SetActive(true);
            lionWinsText.gameObject.SetActive(true);

            catWinsText.GetComponent<Text>().text = "You Lose!";
            lionWinsText.GetComponent<Text>().text = "You Lose!";
            squirrelWinsText.GetComponent<Text>().text = "You Lose!";
            gameOver = true;
        }

        if (cat.GetComponent<FollowThePath>().waypointIndex == cat.GetComponent<FollowThePath>().waypoints.Length) // 고양이 승리
        {
            dogWinsText.gameObject.SetActive(true);
            catWinsText.gameObject.SetActive(true);
            squirrelWinsText.gameObject.SetActive(true);
            lionWinsText.gameObject.SetActive(true);

            dogWinsText.GetComponent<Text>().text = "You Lose!";
            squirrelWinsText.GetComponent<Text>().text = "You Lose!";
            lionWinsText.GetComponent<Text>().text = "You Lose!";
            gameOver = true;
        }

        if (lion.GetComponent<FollowThePath>().waypointIndex == lion.GetComponent<FollowThePath>().waypoints.Length) // 사자 승리
        {
            dogWinsText.gameObject.SetActive(true);
            catWinsText.gameObject.SetActive(true);
            squirrelWinsText.gameObject.SetActive(true);
            lionWinsText.gameObject.SetActive(true);

            dogWinsText.GetComponent<Text>().text = "You Lose!";
            squirrelWinsText.GetComponent<Text>().text = "You Lose!";
            catWinsText.GetComponent<Text>().text = "You Lose!";
            gameOver = true;
        }

        if (squirrel.GetComponent<FollowThePath>().waypointIndex == squirrel.GetComponent<FollowThePath>().waypoints.Length) // 다람쥐 승리
        {
            dogWinsText.gameObject.SetActive(true);
            catWinsText.gameObject.SetActive(true);
            squirrelWinsText.gameObject.SetActive(true);
            lionWinsText.gameObject.SetActive(true);

            dogWinsText.GetComponent<Text>().text = "You Lose!";
            lionWinsText.GetComponent<Text>().text = "You Lose!";
            catWinsText.GetComponent<Text>().text = "You Lose!";
            gameOver = true;
        }
    }

    public static void MovePlayer(int playerToMove)
    {
        switch(playerToMove)
        {
            case 1:
                dog.GetComponent<FollowThePath>().moveAllowed = true;
                break;

            case 2:
                cat.GetComponent<FollowThePath>().moveAllowed = true;
                break;

            case 3:
                lion.GetComponent<FollowThePath>().moveAllowed = true;
                break;

            case 4:
                squirrel.GetComponent<FollowThePath>().moveAllowed = true;
                break;
        }
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
