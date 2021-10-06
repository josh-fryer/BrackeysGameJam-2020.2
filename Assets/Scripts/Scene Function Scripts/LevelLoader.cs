using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Animator transition;

    private Timer myTimer;
    private MusicPlayer musicPlayer;

    // Start is called before the first frame update
    void Start()
    {
        myTimer = FindObjectOfType<Timer>();
        musicPlayer = FindObjectOfType<MusicPlayer>();
    }

    public IEnumerator LoadScene(string scene)
    {
        if (musicPlayer != null)
        {
            musicPlayer.fadeOut = true;
        }
        else{ Debug.Log("No musicPlayer in this scene = "+ SceneManager.GetActiveScene().name ); }
        transition.SetTrigger("Start");
        if (player != null)
        {
            player.SetActive(false);
        }
        if (myTimer != null)
        {
            myTimer.canCount = false;
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }

    public void LoadMenuToGame() //For menu scene button
    {
        //SceneManager.LoadScene("House Scene");
        StartCoroutine(LoadScene("House Scene"));
    }
}
