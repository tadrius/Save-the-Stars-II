using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Tooltip("VFX for enemy death.")]
    [SerializeField] GameObject deathParticles;
    [Tooltip("Parent object under which to spawn runtime objects such as deathParticles.")]
    [SerializeField] Transform runtimeSpawnsParent;
    [Tooltip("Enemy point value. When enemy is destroyed the player's score will increase by this amount.")]
    [SerializeField] int pointValue = 1;

    Scoreboard scoreboard;

    private void Start() {
        scoreboard = FindObjectOfType<Scoreboard>();
    }

    private void OnParticleCollision(GameObject other) {
        scoreboard.IncreaseScore(pointValue);
        SpawnDeathFX();
        Destroy(this.gameObject);
    }

    private void SpawnDeathFX() {
         // spawn death particles and child under runtime spawn parent
        GameObject particlesInstance = Instantiate(deathParticles, transform.position, Quaternion.identity);
        particlesInstance.transform.parent = runtimeSpawnsParent;       
    }
}
