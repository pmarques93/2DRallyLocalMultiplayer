using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] players;
    [SerializeField] private CinemachineVirtualCamera[] cameras;

    void Start()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i])
            {
                Instantiate(cameras[i], cameras[i].transform.position, cameras[i].transform.rotation);
            }
        }
        for (int i = 0; i < players.Length; i++)
        {  
            if (players[i] )
            {
                Instantiate(players[i], spawnPositions[i].position, spawnPositions[i].rotation);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
