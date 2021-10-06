using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    //[SerializeField] AudioClip bulletSFX;
    //[SerializeField] [Range(0,1)] float SFXVol = 0.7f;

    PlayerController playerController;

    private Vector3 shootDir;

    
    private void Start() 
    {
       playerController = FindObjectOfType<PlayerController>();
       //AudioSource.PlayClipAtPoint(bulletSFX, transform.position, SFXVol);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            playerController.hits++;
        }
    }
public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir;
    }
}
