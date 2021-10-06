using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    GameMaster gameMaster;

    Animator myAni;

    void Start()
    {
        Debug.Log("Key is Spawned");
        gameMaster = FindObjectOfType<GameMaster>();
        myAni = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("Key trigger entered");
        if (other.CompareTag("Player"))
        {
            gameMaster.playerHasKey = true;
            myAni.SetTrigger("gotKey");
            
        }
    }

    private void DestroyMe() // Animation Event
    {
        Destroy(gameObject);
    }
}
