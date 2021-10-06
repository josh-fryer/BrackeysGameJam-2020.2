using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int myHealth = 100;
    [SerializeField] bool godMode = false;  
    [SerializeField] Animator myAnimator;
    [Header("Audio")]
    [SerializeField] AudioClip swordHitFleshSFX;
    [SerializeField] [Range(0,1)]float hitFleshVol = 0.7f;
    [SerializeField] AudioClip[] orcScreamSFX;
    [SerializeField] [Range(0,1)]float orcScreamVol = 0.7f;


    [HideInInspector] public bool isHit = false;

    SwordController swordController;
    PlayerController playerController;

    private bool foundSwordCtrl = false;

    // Start is called before the first frame update
    void Start()
    {
        
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update() 
    {
        //Debug.Log("isHit = "+ isHit);
        
        if (playerController.swordIsEquipped && !foundSwordCtrl)
        {
            swordController = FindObjectOfType<SwordController>();
            foundSwordCtrl = true;
        }
        else if(swordController == null)
        {
            foundSwordCtrl = false;
        }

        if (swordController != null)
        { 
            if (!swordController.attacking)
            {
                isHit = false;
            }
        }
    }

    public void DealDamage(int dTG)
    {
        if (!isHit)
        {
            myHealth -= dTG;
            if (swordHitFleshSFX != null)
            {
                AudioSource.PlayClipAtPoint(swordHitFleshSFX, transform.position, hitFleshVol);
            } 
            if (orcScreamSFX.Length > 0)
            {
                AudioSource.PlayClipAtPoint(orcScreamSFX[Random.Range(0, orcScreamSFX.Length)], transform.position, orcScreamVol);
            }
            
            isHit = true;
        }
        if (myHealth <= 0 && !godMode)
        {
            myAnimator.SetBool("isDead", true);
        }
    }
}
