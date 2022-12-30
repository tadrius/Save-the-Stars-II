using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Tooltip("Shoot FX for weapon shooting.")]
    [SerializeField] public GameObject shootFX;
    [Tooltip("Hit FX for weapon collisions.")]
    [SerializeField] GameObject hitFX;
    [Tooltip("How much damage a single hit of this weapon will deal.")]
    [SerializeField] int damage = 1;

    static GameObject RuntimeSpawnsParent;
    static string RuntimeSpawns = "RuntimeSpawns";
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;
    private List<ParticleSystem.Particle> triggerParticles;


    // Start is called before the first frame update
    void Start() {
        RuntimeSpawnsParent = GameObject.FindGameObjectWithTag(RuntimeSpawns);
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        triggerParticles = new List<ParticleSystem.Particle>();
    }

    // get the amount of damage this weapon deals
    public int GetDamage() {
        return this.damage;
    }

    // enable emissions to shoot weapon
    public void SetWeaponsActive(bool isActive) {
        var em = part.emission;
        em.enabled = isActive;
    }

    // when a weapon's particle collides with a game object
    private void OnParticleCollision(GameObject other) {
        part.GetCollisionEvents(other, collisionEvents);

        foreach(ParticleCollisionEvent collision in collisionEvents) {
            // spawn hit FX and child under runtime spawn parent
            GameObject hitFXInstance = Instantiate(hitFX, collision.intersection, Quaternion.identity);
            hitFXInstance.transform.parent = RuntimeSpawnsParent.transform;
        }            
    }

    // when the weapon particle hits the shoot trigger, indicating a shot has been fired
    private void OnParticleTrigger() {
        part.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, triggerParticles);

        foreach(ParticleSystem.Particle particle in triggerParticles) {
            // spawn shoot FX and child under runtime spawn parent
            GameObject shootFXInstance = Instantiate(shootFX, transform.position, Quaternion.identity);
            shootFXInstance.transform.parent = RuntimeSpawnsParent.transform;
        }
    }
}
