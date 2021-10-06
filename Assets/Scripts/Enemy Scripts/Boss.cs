using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform[] waypoints;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpawnSeconds = 1.5f;

    BoxCollider2D myCol;
    Animator myAnimator;
    GameMaster gameMaster;
    GameObject player;

    private int i = 0;

    private Vector3 shootDir;

    private bool teleportDelayRun = false;
    private bool isAlive = true;
    private bool isFiring = false;
    private bool fireLoopIsRunning = false;
    public bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        gameMaster = FindObjectOfType<GameMaster>();
        player = FindObjectOfType<PlayerController>().gameObject;
        myCol = GetComponent<BoxCollider2D>();
        StartCoroutine(SceneStartDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive) {return;}
        if (myAnimator.GetBool("isDead"))
        {
            gameMaster.youWin = true;
            isAlive = false;
        }
        SpawnBullets();

        if (isHit)
        {
            myAnimator.SetBool("isHit", true);
            myCol.enabled = false;
            isFiring = false;
            fireLoopIsRunning = false;
            if(!teleportDelayRun )
            {
                StartCoroutine(TeleportDelay());
                teleportDelayRun = true;
            }
            
        }
        shootDir = (player.transform.position - transform.position).normalized;
    }

    private IEnumerator TeleportDelay()
    {
        
        yield return new WaitForSeconds(1f);
        Teleport();
    }

    private void Teleport()
    {
        if (!myAnimator.GetBool("isDead"))
        {
            i = Random.Range(0, 3);
            //Debug.Log("i = "+ i);
            Transform nextWaypoint = waypoints[i].transform;
        
            if (nextWaypoint.transform.position == transform.position)
            {
                Teleport();
            }
            else
            {
                myAnimator.SetBool("isHit", false);
                transform.position = nextWaypoint.position;
                             
                isHit = false;
                teleportDelayRun = false;
                isFiring = true;
                fireLoopIsRunning = false;
                myCol.enabled = true;
            }
        }
    }

    private void SpawnBullets()
    {
        if (isFiring && !fireLoopIsRunning)
        {
            StartCoroutine(FireLoop());
            fireLoopIsRunning = true;
        }
    }

    private IEnumerator FireLoop()
    {
        while (isFiring)
        {
            Transform bulletTransform = Instantiate(bullet.transform, transform.position, Quaternion.identity);
            bulletTransform.GetComponent<Bullet>().Setup(shootDir);
            yield return new WaitForSeconds(bulletSpawnSeconds);
        }
    }

    private IEnumerator SceneStartDelay()
    {
        yield return new WaitForSeconds(0.8f);
        isFiring = true;
    }
}
