using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    [SerializeField] OrcController myController;
    [SerializeField] Animator myAni;
    [SerializeField] AudioClip[] attackSFX;
    [SerializeField] [Range(0, 1)]float attackSFXVol = 1f;  

    private bool onPlayer = false;
    private bool playerIsTouchingWalls = false;
    private bool canDealDamage = true;

    PlayerController playerController;

    //PolygonCollider2D playerBodyCol;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        //playerBodyCol = playerController.gameObject.GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {           
            onPlayer = true;
            if (canDealDamage)       
            {
                StartCoroutine(DealDamage());
            }
        }
    }

    private IEnumerator DealDamage()
    {
        if(attackSFX != null)
        {
            AudioSource.PlayClipAtPoint(attackSFX[Random.Range(0, attackSFX.Length)], transform.position, attackSFXVol);
        }
        KickPlayer();
        while (onPlayer)
        {           
            playerController.hits++;
            StartCoroutine(ImmuneToDamage());
            yield return new WaitForSeconds(1f);
        }    
    }

    private IEnumerator ImmuneToDamage()
    {
        canDealDamage = false;
        yield return new WaitForSeconds(1f);
        canDealDamage = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onPlayer = false;
            StartCoroutine(myController.DelayAttack());
        }
    }

    public void KickPlayer()
    {
        
        Vector3 EnemyTransform = transform.position;
        playerController.KickMe(EnemyTransform);
        //Debug.Log("player kicked");
    }
}
