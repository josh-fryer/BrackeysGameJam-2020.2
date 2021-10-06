using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [SerializeField] GameObject key;
    [Header("UI")]
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject keyUI;
    [SerializeField] GameObject pausePanel;
    [Header("Audio")]
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioClip winMusic;
    [SerializeField] AudioClip earthquakeClip;
    
    OrcController lastOrc;
    PlayerController playerController;
    SwordController swordController;
    WorldEnd worldEndScript;
    LevelLoader sceneLoader;
    Timer timer;

    Animator playerAni;
    [Header("Other Variables")]
    public int deadOrcs = 0;
    private bool haveLastOrc = false;
    public bool worldEnd = false;
    public bool outOfTime = false;
    public bool playerHasKey = false;
    public bool keyIsSpawned = false;
    public bool youWin = false;
    private bool sceneDelayHasStarted = false;
    private bool youWinDelayHasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
       playerController = FindObjectOfType<PlayerController>();
       playerAni = playerController.gameObject.GetComponentInChildren<Animator>();
       //swordController = FindObjectOfType<SwordController>(); change this to trigger on sword pull
       worldEndScript = FindObjectOfType<WorldEnd>();
       sceneLoader = FindObjectOfType<LevelLoader>();
       timer = FindObjectOfType<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        SceneLoader();
        DeadOrcs();

        if (playerAni.GetBool("isDead"))
        {
            if(!sceneDelayHasStarted)
            {
                StartCoroutine(SceneDelay());
                sceneDelayHasStarted = true;
            }
        }

        if(playerHasKey)
        {
            keyUI.SetActive(true);
        }

        if(youWin)
        {
            if(!youWinDelayHasStarted)
            {     
                timer.canCount = false;
                musicPlayer.Pause();           
                StartCoroutine(WinScreenDelay());
                youWinDelayHasStarted = true;
            }
        }
    }

    private void LateUpdate() 
    {
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "Game Scene")  
        {
            if (deadOrcs == 3 && !keyIsSpawned)
            {
                Instantiate(key, new Vector3(-73.5f, 53.3f, 0f), Quaternion.identity);
                keyIsSpawned = true;
            }
        }
    }

    private IEnumerator WinScreenDelay()
    {
        yield return new WaitForSeconds(1f);
        musicPlayer.clip = winMusic;
        musicPlayer.Play();
        //playerHUD.SetActive(false);
        winUI.SetActive(true);
        yield return new WaitForSeconds(7f);
        //SceneManager.LoadScene("Menu");
        StartCoroutine(sceneLoader.LoadScene("Menu"));
    }

    private IEnumerator SceneDelay()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(sceneLoader.LoadScene("House Scene"));
    }

    public void OutOfTime()
    {
        Scene scene = SceneManager.GetActiveScene();

        outOfTime = true;
        
        //Stop player run and attack input;
        playerController.run = false;
        playerController.isAlive = false;
        if (playerController.swordIsEquipped)
        {
            swordController.getInput = false;
        }
        worldEndScript.canGrow = true;

        if(!(scene.name == "Dungeon Scene" || scene.name == "House Scene"))
        {
            Debug.Log("scene name is not house scene");
            worldEndScript.canMove = true;
            //SceneManager.LoadScene("House Scene");
            //StartCoroutine(sceneLoader.LoadScene("House Scene"));
        }
        AudioSource.PlayClipAtPoint(earthquakeClip, playerController.gameObject.transform.position);
        Debug.Log("OutOfTime");
    }

    private void DeadOrcs()
    {
        //Debug.Log("deadOrcs = "+ deadOrcs);
        if (deadOrcs == 2)
        {
            if (!haveLastOrc)
            {
                lastOrc = FindObjectOfType<OrcController>();
                haveLastOrc = true;
                //Debug.Log("lastOrc = "+ lastOrc.gameObject.name);
            }
            else
            {
                lastOrc.hasKey = true;
            }
        }
        
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "Dungeon Scene" || scene.name == "House Scene")
        {
            deadOrcs = 0;
        }
    }

    private void SceneLoader()
    {
        if (worldEnd == true && !youWin)
        {
            StartCoroutine(sceneLoader.LoadScene("House Scene"));
        }
    }

    public void FindSword()
    {
        swordController = FindObjectOfType<SwordController>();
    }

    public void PauseGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!(scene.name == "Menu"))
        {
            if (Time.timeScale == 1)
            {
                //pause game
                Time.timeScale = 0;
                if (musicPlayer != null)
                {
                    musicPlayer.Pause();
                }
                pausePanel.SetActive(true);
            }
            else
            {
                //continue game
                Time.timeScale = 1;
                if (musicPlayer != null)
                {
                    musicPlayer.UnPause();
                }
                pausePanel.SetActive(false);
            }
        }
    } 


}
