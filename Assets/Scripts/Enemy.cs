using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("FX for enemy death.")]
    [SerializeField] GameObject deathFX;  
    [Tooltip("Enemy hit value. When enemy is hit the player's score will increase by this amount.")]
    [SerializeField] int hitValue = 1;
    [Tooltip("Enemy kill value. When enemy is destroyed the player's score will increase by this amount.")]
    [SerializeField] int killValue = 2;
    [Tooltip("How much damage the enemy can receive before being destroyed.")]
    [SerializeField] int hitPoints = 1;

    static string RuntimeSpawns = "RuntimeSpawns";
    static GameObject RuntimeSpawnsParent;
    static Scoreboard scoreboard;


    private void Start() {
        RuntimeSpawnsParent = GameObject.FindGameObjectWithTag(RuntimeSpawns);
        scoreboard = FindObjectOfType<Scoreboard>();

        // add a rigid body to use all child colliders
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void OnParticleCollision(GameObject other) {
        scoreboard.IncreaseScore(hitValue);
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
        scoreboard.IncreaseScore(killValue);
        SpawnDeathFX();
        Destroy(this.gameObject);
    }
    
    private void SpawnDeathFX() {
         // spawn death FX and child under runtime spawn parent
        GameObject particlesInstance = Instantiate(deathFX, transform.position, Quaternion.identity);
        particlesInstance.transform.parent = RuntimeSpawnsParent.transform;
    }
}
