using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Bush : MonoBehaviour
{
    [SerializeField] GameObject bushParticles;
    [SerializeField] AudioClip bushDestroySFX;

    private GameObject clone;

    public void bushIsHit()
    {
        AudioSource.PlayClipAtPoint(bushDestroySFX, transform.position, 1f);
        clone = (GameObject) Instantiate(bushParticles, transform.position, Quaternion.identity);
        Destroy(clone, bushParticles.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }
}
