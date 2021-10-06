using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcAreaScript : MonoBehaviour
{
    OrcController[] orcs;

    void Start() 
    {
        orcs = FindObjectsOfType<OrcController>();
    }

   private void OnTriggerExit2D(Collider2D other)
   {
     if(other.CompareTag("Player"))
     {
        for(int i = 0; i < orcs.Length; i++)
        {
            orcs[i].playerExitArea = true;
        }
     }
   }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            for(int i = 0; i < orcs.Length; i++)
            {
                orcs[i].playerExitArea = false;
            }
        }
    }


}
