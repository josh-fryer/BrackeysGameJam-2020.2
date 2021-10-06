using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordInStone : MonoBehaviour
{
    [SerializeField] Transform pullPos;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject mouseUI; //mouse promt
    [SerializeField] ParticleSystem swordParticles;
    [SerializeField] AudioClip successSFX;
    [SerializeField] float soundVol = 1f;
    [SerializeField] int neededSwordPulls = 7;


    CircleCollider2D myTriggerCol;
    GameObject player;
    PlayerController playerController;
    int pulled = 0;

    bool PlayerisTugging = false;

    // Start is called before the first frame update
    void Start()
    {
        myTriggerCol = GetComponent<CircleCollider2D>();
        playerController = FindObjectOfType<PlayerController>();
        player = playerController.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerisTugging && Input.GetMouseButtonDown(0))
        {
            pulled++;
            playerController.myAnimator.SetTrigger("pullSword");

        }

        if (pulled == neededSwordPulls)
        {
            PlayerisTugging = false;
            playerController.isAlive = true;
            Destroy(sword);
            swordParticles.Play();
            playerController.swordIsEquipped = true;
            myTriggerCol.enabled = false;
            mouseUI.SetActive(false);
            this.enabled = false;
            AudioSource.PlayClipAtPoint(successSFX, transform.position, soundVol);
        }      
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            playerController.isAlive = false;
            player.transform.position = pullPos.position; // player is in place
            PlayerisTugging = true;
            mouseUI.SetActive(true);
        }
    }
}
