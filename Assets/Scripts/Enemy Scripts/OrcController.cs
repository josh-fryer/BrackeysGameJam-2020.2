using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcController : MonoBehaviour
{
    [SerializeField] GameObject key;
    [SerializeField] Transform newWayPoint;
    [SerializeField] Animator myAnimator;
    [Header("Blood")]
    [SerializeField] GameObject bloodParticles;
    [SerializeField] AudioClip bloodSFX;
    private GameObject clone;
    
    [SerializeField] float requiredDistanceToP = 6f;
    [SerializeField] float enemyDistance = 3.0f;
    
    [Header("Speeds")]
    [SerializeField] float wanderSpeed = 5f;
    [SerializeField] float followSpeed = 5f;
    
    [Header("Knockback Settings")]
    [SerializeField] float kickForce = 9;
    [SerializeField] float startKickTime = 0.2f;

    private GameObject[] enemies;

    private float kickTime;
    private float distanceToP;
    private float aniStartSpeed;

    public bool hasKey = false;
    public bool playerExitArea = false;
    //public bool playerIsInside = false;

    [HideInInspector] public bool beingKicked = false;
    private bool isAlive = true;
    bool getPoint = true;
    [HideInInspector] public bool startedCoroutine = false;
    private bool distanceCheckCooldown = false;
    
    Vector3 newPosition;
    Vector3 circleCentre;
    

    PlayerController playerController;
    GameObject player;
    GameMaster gameMaster;
    Rigidbody2D rb;
    ParticleSystem bloodParticleSys;

    // Start is called before the first frame update
    void Start()
    {   
        kickTime = startKickTime;
        circleCentre = transform.position;
        playerController = FindObjectOfType<PlayerController>();
        gameMaster = FindObjectOfType<GameMaster>();
        player = playerController.gameObject;
        rb = GetComponent<Rigidbody2D>();
        myAnimator.SetBool("isWandering", true);
        aniStartSpeed = myAnimator.GetFloat("moveSpeed");
        bloodParticleSys = bloodParticles.GetComponent<ParticleSystem>();
        FindOrcs();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) 
        { 
            myAnimator.SetBool("isIdle", true);
            return; 
        }
        
        if (myAnimator.GetBool("isWandering"))
        { 
            //getPoint = true;
            wanderState();       
        }
        
        EnemyDistance();        
        BoundryClamp();

        if(gameMaster.outOfTime)
        {
            isAlive = false;
        }

        if (myAnimator.GetBool("isDead"))
        {
            if(hasKey)
            {
                Instantiate(key, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
                gameMaster.keyIsSpawned = true;
                //Debug.Log("Instantiate Key");
            }
            AudioSource.PlayClipAtPoint(bloodSFX, transform.position, 1f);
            clone = (GameObject) Instantiate(bloodParticles, transform.position, Quaternion.identity);
            Destroy(clone, bloodParticleSys.main.duration);
            Destroy(gameObject);
        }

        KickTime();
        if (playerExitArea && !myAnimator.GetBool("isWandering"))
        {
            //reset orc position and wander state
            myAnimator.SetBool("isFollowing", false);
            
            transform.position = Vector2.MoveTowards(transform.position, circleCentre, wanderSpeed * Time.deltaTime);
            //return to wander state on moving into original position
            if(transform.position == circleCentre)
            {
                //Debug.Log(gameObject.name+" returned to start pos");
                myAnimator.SetBool("isWandering", true);
                playerExitArea = false;
            }
        }
        if(!playerExitArea || myAnimator.GetBool("isWandering"))
        {
            if (!distanceCheckCooldown)
            {
                DistanceToPlayer();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Orc Collision");
        //&& player.GetComponent<PolygonCollider2D>().IsTouchingLayers(LayerMask.GetMask("Walls")
        if (col.gameObject.tag == "Player")
        {
            myAnimator.SetFloat("moveSpeed", 0f);
            Debug.Log("Orc Collided with player");
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            StartCoroutine(DelayAttack());
            myAnimator.SetFloat("moveSpeed", aniStartSpeed);
            Debug.Log("Orc EXIT Collision with player");

        }
    }

    private void OnDestroy() 
    {  
      gameMaster.deadOrcs++;
    }

    
    private void wanderState()
    {
        if (getPoint)
        {
            startedCoroutine = false;
            //newWayPoint.transform.position
            var xy = Random.insideUnitCircle * 2;
            newPosition = new Vector3(xy.x, xy.y, 0) + circleCentre;
            newWayPoint.transform.position = newPosition;
            getPoint = false;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, newWayPoint.transform.position, wanderSpeed * Time.deltaTime);
            if (transform.position == newWayPoint.transform.position)
            {
                if(!startedCoroutine)
                {
                    StartCoroutine(WaitBeforeMoving());
                    startedCoroutine = true;
                }  
            }  
        }       
    }

    private IEnumerator WaitBeforeMoving()
    {
        yield return new WaitForSeconds(2f);
        getPoint = true;
    }

    private void DistanceToPlayer()
    {
       distanceToP = Vector2.Distance(player.transform.position, transform.position);
       if (distanceToP <= requiredDistanceToP)
       {   
            myAnimator.SetBool("isWandering", false);
            myAnimator.SetBool("isFollowing", true);
       }
    }

    public IEnumerator DelayAttack()
    {
        myAnimator.SetBool("isWandering", false);
        myAnimator.SetBool("isFollowing", false);
        distanceCheckCooldown = true;
        yield return new WaitForSeconds(0.5f);
        distanceCheckCooldown = false;
        if (!playerExitArea)
        {
            myAnimator.SetBool("isFollowing", true);
        } 
    }

    public void KickMe(Vector3 playerTransform)
    {
        myAnimator.SetBool("isFollowing", false);
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;
        Vector2 difference = (transform.position - playerTransform).normalized;
        rb.AddForce(difference * kickForce, ForceMode2D.Impulse);
        beingKicked = true;
        Debug.Log("Orc KickMe called");
    }

    private void KickTime()
    {
        if (beingKicked)
        {
            kickTime -= Time.deltaTime;
            
        }

        if (kickTime <= 0)
        {
            Debug.Log("kickTime = 0");
            beingKicked = false;
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            kickTime = startKickTime;  
            StartCoroutine(DelayAttack());
        }
    }

    private void EnemyDistance()
    {
        if(!beingKicked)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    float currentDistance = Vector2.Distance(transform.position, enemy.transform.position);
                    if (currentDistance < enemyDistance)
                    {
                        Vector3 dist = transform.position - enemy.transform.position;
                        transform.position += dist * Time.deltaTime;
                    }
                }
            }
        }
    }

    private void FindOrcs()
    {
        enemies = GameObject.FindGameObjectsWithTag("Orc");
    }

    private void BoundryClamp()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -83.5f, -62.5f);
        pos.y = Mathf.Clamp(transform.position.y, 47.5f, 60.5f);
        transform.position = pos;
    }
}
