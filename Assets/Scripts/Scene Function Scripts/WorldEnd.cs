using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEnd : MonoBehaviour
{
    [SerializeField] Vector2 endScale;
    [SerializeField] Transform endPos;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float scaleSpeed = 2f;

    private Vector2 startPos;
    private Vector2 startScale;

    private SpriteRenderer spriteRen;

    public bool canGrow = false;
    public bool canMove = false;

    private GameMaster gameMaster;

    // Start is called before the first frame update
    void Start()
    {
        
        gameMaster = FindObjectOfType<GameMaster>();
        spriteRen = GetComponent<SpriteRenderer>();
        spriteRen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        startPos = transform.position;
        startScale = transform.localScale;
        if (canGrow)
        {
            spriteRen.enabled = true;
            if(canMove)
            {
                transform.position = Vector2.MoveTowards(startPos, endPos.position, moveSpeed * Time.deltaTime);      
            }
            
            transform.localScale = Vector2.MoveTowards(startScale, endScale, scaleSpeed * Time.deltaTime);
           
            if (transform.localScale.x == endScale.x && transform.localScale.y == endScale.y)
            {
                //transform.position == endPos.position;
                if (!canMove )
                {
                    Debug.Log("World ender complete");
                    gameMaster.worldEnd = true; 
                }
                else if (transform.position == endPos.position)
                {
                    Debug.Log("World ender complete");
                    gameMaster.worldEnd = true; 
                }
                
            }

        }
    }
}
