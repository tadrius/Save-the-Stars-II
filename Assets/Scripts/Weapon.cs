using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Tooltip("Hit FX for weapon collisions.")]
    [SerializeField] GameObject hitFX;
    [Tooltip("How much damage a single hit of this weapon will deal.")]
    [SerializeField] int damage = 1;

    static GameObject RuntimeSpawnsParent;
    static string RuntimeSpawns = "RuntimeSpawns";
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        RuntimeSpawnsParent = GameObject.FindGameObjectWithTag(RuntimeSpawns);
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();        
    }

    public int GetDamage() {
        return this.damage;
    }

    private void OnParticleCollision(GameObject other) {
        part.GetCollisionEvents(other, collisionEvents);

        foreach(ParticleCollisionEvent collision in collisionEvents) {
            // spawn hit FX and child under runtime spawn parent
            GameObject particlesInstance = Instantiate(hitFX, collision.intersection, Quaternion.identity);
            particlesInstance.transform.parent = RuntimeSpawnsParent.transform;
        }            
    }
}
