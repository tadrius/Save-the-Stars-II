using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [Tooltip("Hit particles for weapon collisions.")]
    [SerializeField] GameObject hitParticles;
    [Tooltip("Parent object under which to spawn runtime objects such as collision particles.")]
    [SerializeField] Transform runtimeSpawnsParent;
    [Tooltip("How much damage a single hit of this weapon will deal.")]
    [SerializeField] int damage = 1;

    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();        
    }

    public int GetDamage() {
        return this.damage;
    }

    private void OnParticleCollision(GameObject other) {
        part.GetCollisionEvents(other, collisionEvents);

        foreach(ParticleCollisionEvent collision in collisionEvents) {
            // spawn hit particles and child under runtime spawn parent
            GameObject particlesInstance = Instantiate(hitParticles, collision.intersection, Quaternion.identity);
            particlesInstance.transform.parent = runtimeSpawnsParent;   
        }            
    }
}
