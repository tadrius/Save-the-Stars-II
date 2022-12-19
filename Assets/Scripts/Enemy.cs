using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Tooltip("VFX for enemy death.")]
    [SerializeField] GameObject deathParticles;
    [Tooltip("Parent object under which to spawn runtime objects such as collision particles.")]
    [SerializeField] Transform runtimeSpawnsParent;
    [Tooltip("Enemy score value. When enemy is hit the player's score will increase by this amount.")]
    [SerializeField] int scoreValue = 1;
    [Tooltip("How much damage the enemy can receive before being destroyed.")]
    [SerializeField] int hitPoints = 1;

    Scoreboard scoreboard;

    private void Start() {
        scoreboard = FindObjectOfType<Scoreboard>();
    }

    private void OnParticleCollision(GameObject other) {
        scoreboard.IncreaseScore(scoreValue);
        Weapon weapon = other.GetComponent<Weapon>();
        ReceiveDamage(weapon.GetDamage());
    }

    public void ReceiveDamage(int damageAmount) {
        hitPoints -= damageAmount;
        if (hitPoints <= 0) {
            Die();
        }
    }

    private void Die() {
        SpawnDeathFX();
        Destroy(this.gameObject);
    }
    
    private void SpawnDeathFX() {
         // spawn death particles and child under runtime spawn parent
        GameObject particlesInstance = Instantiate(deathParticles, transform.position, Quaternion.identity);
        particlesInstance.transform.parent = runtimeSpawnsParent;       
    }
}
