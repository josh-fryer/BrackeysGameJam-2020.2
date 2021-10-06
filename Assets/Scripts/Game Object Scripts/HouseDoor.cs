using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDoor : MonoBehaviour
{
    LevelLoader levelLoader;

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        StartCoroutine(levelLoader.LoadScene("Game Scene"));
    }

}
