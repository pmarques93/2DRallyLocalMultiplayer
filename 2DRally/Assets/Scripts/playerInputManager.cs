using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputManager : MonoBehaviour
{
    private PlayerInputManager input;

    [SerializeField] private Sprite[] playerSprites;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] positions;


    void Start()
    {
        input = GetComponent<PlayerInputManager>();

        input.playerPrefab = playerPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.PlayersInGame == 0)
        {
            playerPrefab.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
            playerPrefab.GetComponent<Transform>().position = positions[0].position;
        }
        if (GameManager.PlayersInGame == 1)
        {
            playerPrefab.GetComponent<SpriteRenderer>().sprite = playerSprites[1];
            playerPrefab.GetComponent<Transform>().position = positions[1].position;
        }
        if (GameManager.PlayersInGame == 2)
        {
            playerPrefab.GetComponent<SpriteRenderer>().sprite = playerSprites[2];
            playerPrefab.GetComponent<Transform>().position = positions[2].position;
        }
        if (GameManager.PlayersInGame == 3)
        {
            playerPrefab.GetComponent<SpriteRenderer>().sprite = playerSprites[3];
            playerPrefab.GetComponent<Transform>().position = positions[3].position;
        }
    }
}
