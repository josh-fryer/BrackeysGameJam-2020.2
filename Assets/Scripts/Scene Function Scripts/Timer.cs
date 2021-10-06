using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI clockText;
    [SerializeField] float maxTime = 100;
    public static float startTime = 100;
    public bool canCount = false;
    public bool testing = false;

    GameMaster gameMaster;

    // Start is called before the first frame update
    void Start()
    {
        if(testing)
        {
            startTime = maxTime;
        }

        clockText.text = startTime.ToString();
        gameMaster = FindObjectOfType<GameMaster>();

        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "House Scene") // Resets timer for new game
        {
            startTime = maxTime;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        if(canCount)
        {
            startTime -= Time.deltaTime;
            clockText.text = Mathf.Round(startTime).ToString();
        }

        if (startTime <= 0)
        {
            canCount = false;
            startTime = maxTime;
            gameMaster.OutOfTime();      
        }
    }
}
