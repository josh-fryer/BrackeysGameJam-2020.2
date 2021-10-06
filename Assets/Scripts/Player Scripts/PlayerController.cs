using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject mySword;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float swordDamage = 50f;
    [SerializeField] float kickForce = 8f;
    [SerializeField] float startKickTime = 1f;
    [SerializeField] Joystick joystick;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] damagedVoiceSFX;
    
    public int hits = 0;
    private int hitsChange = 1; 

    [Header("public bools")]
    [SerializeField] bool godMode = false;
    private bool beingKicked = false;
    public bool isTesting = false;
    public bool isAlive = true;
    [HideInInspector] public bool run = true;
    public bool swordIsEquipped = false;

    private float kickTime;
    float controlThrowX;
    float controlThrowY;
    
    public Animator myAnimator;
    Rigidbody2D rB;
    PolygonCollider2D myBodyCol;
    HeartsHealth hearts;
    GameMaster gameMaster;

    //Vectors
    Vector2 inputDirection;
    Vector2 clampedInput;
    public Vector2 lastDirection;
    private Vector3 collisionNormal;

    // Start is called before the first frame update
    void Start()
    {
        kickTime = startKickTime;
        gameMaster = FindObjectOfType<GameMaster>();
        hearts = GetComponent<HeartsHealth>();
        rB = GetComponent<Rigidbody2D>();
        myBodyCol = GetComponent<PolygonCollider2D>();   

        if(isTesting)
        {
            swordIsEquipped = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Pause();

        if (!isAlive) 
        { 
            rB.velocity = Vector2.zero;
            return; 
        }
        if (run == true)
        {
            Run();            
        }
        else
        {
            //rB.velocity = Vector2.zero;
            myAnimator.SetBool("Running", false);
        }
        PlayerInput();
        EquipSword();
        KickTime();
        if (hits >= hitsChange)
        {
            //play damaged SFX Here
            audioSource.clip = damagedVoiceSFX[UnityEngine.Random.Range(0, damagedVoiceSFX.Length)];
            audioSource.Play();
            hearts.healthShare--;
            hitsChange++;
        }
        if (hearts.healthShare == 0 && !godMode) // if you are dead
        {
            myAnimator.SetBool("isDead", true);
            if (mySword)
            {
                mySword.SetActive(false);
            }
            isAlive = false;
        }
        if (myBodyCol.IsTouchingLayers(LayerMask.GetMask("Walls")))
        {
            Debug.Log("Player is touching Walls");
            rB.velocity = Vector3.ProjectOnPlane(rB.velocity, collisionNormal); 
            /* This simply wipes out any movement toward normal but preserves everything else. It's what you might consider sliding collision.
            */
        }

    }

    private void PlayerInput()
    {
        //----joystick movement----
        if(joystick.Horizontal >= .2f)
        {
            //controlThrowX = 1f;
            controlThrowX = Mathf.Clamp(joystick.Horizontal * 2f, 0f, 1f); 
        }
        else if(joystick.Horizontal <= -.2f)
        {
            //controlThrowX = -1f;
            controlThrowX = Mathf.Clamp(joystick.Horizontal * 2f, -1f, 0f);
            //Debug.Log("joystick left * 2 = "+ joystick.Horizontal * 2f);
           // Debug.Log("controlThrowX "+ controlThrowX)

        }
        else
        {
            controlThrowX = 0f;
        }

        if(joystick.Vertical >= .2f)
        {
            ///controlThrowY = 1;
            controlThrowY = Mathf.Clamp(joystick.Vertical * 2f, 0f, 1f); 

        }
        else if(joystick.Vertical<= -.2f)
        {
            //controlThrowY = -1;
            controlThrowY = Mathf.Clamp(joystick.Vertical * 2f, -1f, 0f); 
        }
        else
        {
            controlThrowY = 0f;
        }
        //----joystick movement----

        //controlThrowY = joystick.Vertical; //Input.GetAxis("Vertical");
        //controlThrowX = joystick.Horizontal; //Input.GetAxis("Horizontal"); //value is between -1 to 1
        inputDirection = new Vector2(controlThrowX, controlThrowY);
        clampedInput = Vector2.ClampMagnitude(inputDirection, 1);
    }

    public void Run()
    {
        Vector2 movement = clampedInput * runSpeed;
        rB.velocity = movement;
        if (rB.velocity.magnitude > 0.1f)
        {
            myAnimator.SetBool("Running", true);
            lastDirection = inputDirection;
        }
        else
        {
            myAnimator.SetBool("Running", false);
            if (lastDirection.y == 0 && lastDirection.x >= 0.001) //is right
            {
                lastDirection = new Vector2(1, 0);
            }
            else if (lastDirection.y == 0 && lastDirection.x <= -0.001) //is left
            {
                lastDirection = new Vector2(-1, 0);         
            }
            else if (lastDirection.x == 0 && lastDirection.y <= -0.001) //is down
            {
                lastDirection = new Vector2(0, -1);
            }
            else if (lastDirection.x == 0 && lastDirection.y >= 0.001) //is up
            {
                lastDirection = new Vector2(0, 1);
            }
        }      
    }

    public void KickMe(Vector3 EnemyTransform)
    {
        if (!(myBodyCol.IsTouchingLayers(LayerMask.GetMask("Walls"))))
        {
            run = false;
            rB.velocity = Vector2.zero;
            Vector2 difference = (transform.position - EnemyTransform).normalized;
            rB.AddForce(difference * kickForce, ForceMode2D.Impulse);
            beingKicked = true;
            Debug.Log("player.KickMe called");
        }
    }

    private void KickTime()
    {
        if (beingKicked)
        {
            kickTime -= Time.deltaTime;
        }

        if (kickTime <= 0)
        {
            //Debug.Log("kickTime = 0");
            beingKicked = false;
            rB.velocity = Vector2.zero;
            run = true;
            kickTime = startKickTime;  
        }
    }


    private void EquipSword()
    {
        if (mySword != null)
        {
            if (swordIsEquipped)
            {
                mySword.SetActive(true);
                gameMaster.FindSword();
            }
            else
            {
                mySword.SetActive(false);
            }
        }
    }

    private void Pause()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            gameMaster.PauseGame();
        }
    }
}
