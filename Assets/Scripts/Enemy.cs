using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] GameObject deathParticles;
    [SerializeField] Transform runtimeSpawnsParent;


    private void OnParticleCollision(GameObject other) {
        // spawn death particles and child under runtime spawn parent
        GameObject particlesInstance = Instantiate(deathParticles, transform.position, Quaternion.identity);
        particlesInstance.transform.parent = runtimeSpawnsParent;

        // destroy the enemy
        Destroy(this.gameObject);
    }
}
