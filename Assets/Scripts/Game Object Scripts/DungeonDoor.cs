using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonDoor : MonoBehaviour
{

    GameMaster gameMaster;
    LevelLoader levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            if(gameMaster.playerHasKey == true)
            {
                StartCoroutine(levelLoader.LoadScene("Dungeon Scene"));
            }
        }
    }
}
