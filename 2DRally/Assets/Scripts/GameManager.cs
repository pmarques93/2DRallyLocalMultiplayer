using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private Player[] players;
    private Camera[] mainCameras;

    public static int PlayersInGame;

    void Awake()
    {
        /*
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i])
            {
                Player pl = Instantiate(players[i], spawnPositions[i].position, spawnPositions[i].rotation);
            }
        }
        */

        mainCameras = new Camera[4];
        mainCameras[0] = GameObject.FindGameObjectWithTag("p1MainCamera").GetComponent<Camera>();
        mainCameras[1] = GameObject.FindGameObjectWithTag("p2MainCamera").GetComponent<Camera>();
        mainCameras[2] = GameObject.FindGameObjectWithTag("p3MainCamera").GetComponent<Camera>();
        mainCameras[3] = GameObject.FindGameObjectWithTag("p4MainCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        


    }

    void FixedUpdate()
    {
        PlayersInGame = FindObjectsOfType<Player>().Length;

        if (PlayersInGame == 0)
        {
            mainCameras[0].gameObject.SetActive(false);
            mainCameras[1].gameObject.SetActive(false);
            mainCameras[2].gameObject.SetActive(false);
            mainCameras[3].gameObject.SetActive(false);
        }
        if (PlayersInGame == 1)
        {
            mainCameras[0].gameObject.SetActive(true);
            mainCameras[1].gameObject.SetActive(false);
            mainCameras[2].gameObject.SetActive(false);
            mainCameras[3].gameObject.SetActive(false);
        }
        if (PlayersInGame == 2)
        {
            mainCameras[0].gameObject.SetActive(true);
            mainCameras[1].gameObject.SetActive(true);
            mainCameras[2].gameObject.SetActive(false);
            mainCameras[3].gameObject.SetActive(false);
        }
        if (PlayersInGame == 3)
        {
            mainCameras[0].gameObject.SetActive(true);
            mainCameras[1].gameObject.SetActive(true);
            mainCameras[2].gameObject.SetActive(true);
            mainCameras[3].gameObject.SetActive(false);
        }
        if (PlayersInGame == 4)
        {
            mainCameras[0].gameObject.SetActive(true);
            mainCameras[1].gameObject.SetActive(true);
            mainCameras[2].gameObject.SetActive(true);
            mainCameras[3].gameObject.SetActive(true);
        }


        if (PlayersInGame == 2)
        {
            mainCameras[0].rect = new Rect(new Vector2(0, 0.505f), new Vector2(1, 0.5f));
            mainCameras[1].rect = new Rect(new Vector2(0, -0.005f), new Vector2(1, 0.5f));
        }
        else if (PlayersInGame == 3)
        {
            mainCameras[0].rect = new Rect(new Vector2(0, 0.505f), new Vector2(1, 0.5f));
            mainCameras[1].rect = new Rect(new Vector2(0f, -0.005f), new Vector2(0.495f, 0.5f));
            mainCameras[2].rect = new Rect(new Vector2(0.502f, -0.005f), new Vector2(0.498f, 0.5f));
        }
        else if (PlayersInGame == 4)
        {
            mainCameras[0].rect = new Rect(new Vector2(0, 0.505f), new Vector2(0.495f, 0.5f));
            mainCameras[1].rect = new Rect(new Vector2(0.502f, -0.005f), new Vector2(0.495f, 0.5f));
            mainCameras[2].rect = new Rect(new Vector2(0f, -0.005f), new Vector2(0.495f, 0.5f));
            mainCameras[3].rect = new Rect(new Vector2(0.502f, 0.505f), new Vector2(0.498f, 0.5f));
        }
    }
}
