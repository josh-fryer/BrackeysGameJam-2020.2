using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] int swordDamage;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;
    public bool getInput = true;
    private bool attackButtonPressed = false;
    [Header("Audio")]
    [SerializeField] AudioClip swordSwing;
    
    [SerializeField] [Range(0,1)]float swordSwingVolume = 0.7f;

    [HideInInspector] public bool attacking = false;
    

    // Component Refs
    PlayerController player;
    Animator myAnimator;
    AudioSource myAudioSource;

    Transform myParent;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        myParent = transform.parent;
        getInput = true;
    }

    void Update()
    {
        //Debug.Log("getInput = "+ getInput);
        if(getInput)
        {
            MyInput();
        }
        if (attacking)
        {
            Attacking();
        }
        //Debug.Log("attacking bool = "+ attacking);
    }

    public void AttackButton()
    {
        if(player.swordIsEquipped == true)
        {
            attackButtonPressed = true;
        }     
    }

    public void MyInput()
    {
        //if (Input.GetMouseButtonDown(0))
        if(attackButtonPressed)
        {
            Debug.Log("Attack button is down");
            if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("1")) // If you are not already attacking
            {  
                if (player.lastDirection.y >= 0.1) // attack up
                {
                    myAnimator.SetTrigger("Attack up");
                }
                else if (player.lastDirection.x <= -0.1) // attack left
                {
                    myAnimator.SetTrigger("Attack Left");
                }
                else if (player.lastDirection.y <= -0.1) // attack down
                {
                    myAnimator.SetTrigger("Attack Down");
                }
                else if (player.lastDirection.x >= 0.1) // attack right
                {
                   myAnimator.SetTrigger("Attack right");
                }
                attacking = true;
                
                myAudioSource.clip = swordSwing;
                myAudioSource.volume = swordSwingVolume;
                myAudioSource.Play();
                attackButtonPressed = false;
            }
        }                    
        
    }

    private void Attacking()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.CompareTag("Bush"))
            {
                //Debug.Log("hit bush = "+ hitObject.gameObject.name);
                GameObject bush = hitObject.gameObject;
                bush.GetComponent<_Bush>().bushIsHit();

                /* bush.GetComponent<SpriteRenderer>().enabled = false;
                ParticleSystem bushPaticles = bush.GetComponentInChildren<ParticleSystem>();
                bushPaticles.Play();

                Destroy(bush, bushPaticles.main.duration); */
            }
            else if (hitObject.CompareTag("Orc"))
            {
                hitObject.gameObject.GetComponent<Health>().DealDamage(swordDamage);
                hitObject.gameObject.GetComponent<Health>().isHit = true;
                hitObject.gameObject.GetComponent<OrcController>().KickMe(transform.position);
            }
            else if (hitObject.CompareTag("Enemy"))
            {
                //Debug.Log("player hit boss");
                Health bossHealth = hitObject.gameObject.GetComponent<Health>();
                bossHealth.DealDamage(swordDamage);
                bossHealth.isHit = true;
                hitObject.gameObject.GetComponent<Boss>().isHit = true;
            }
        }
    }

    public void ResetSwordAngle() // Animation Event
    {
        myParent.localEulerAngles = new Vector3(0, 0, 0);
        attacking = false;
        //Debug.Log("Reset Sword Angle");
    }

    void OnDrawGizmosSelected() 
    {
        if (attackPoint == null)
        return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
