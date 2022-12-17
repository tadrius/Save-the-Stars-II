using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class CollisionHandler : MonoBehaviour
{

    [Tooltip("A delay between triggering loading the next scene and loading the scene.")]
    [SerializeField] float sceneLoadDelay = 2.75f;
    [Tooltip("Playable Director assigned with the master timeline.")]
    [SerializeField] PlayableDirector timeline;
    [Tooltip("Particles to play on failure, from first to play to last.")]
    [SerializeField] ParticleSystem[] failParticles;
    [Tooltip("Delay between starting each fail particles.")]
    [SerializeField] float failParticlesDelay = 0.2f;

    private void OnTriggerEnter(Collider other) {
        InitiateFailSequence();
    }

    private void InitiateFailSequence() {
        StartCoroutine(PlayFailFXWithDelay());
        DeactivateControls();
        DeactivateTimeline();
        ReloadScene();
    }

    IEnumerator PlayFailFXWithDelay() {
        for (int i = 0; i < failParticles.GetLength(0); i++) {
            failParticles[i].Play();
            // add a delay if the current index is the last available
            if (i + 1 < failParticles.GetLength(0)) {
                for (float time = failParticlesDelay; time >= 0; time -= Time.deltaTime)
                {
                    yield return null;
                }
            }
        }
        DeactivateShipVisuals();
    }

    private void DeactivateShipVisuals() {
        this.GetComponent<MeshRenderer>().enabled = false;
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void DeactivateControls() {
        this.GetComponentInParent<PlayerControls>().enabled = false;
    }

    private void DeactivateTimeline() {
        timeline.Pause();
    }

    private void ReloadScene() {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadScene(int sceneBuildIndex) {
        StartCoroutine(LoadSceneWithDelay(sceneBuildIndex));
    }

    IEnumerator LoadSceneWithDelay(int sceneBuildIndex) {
        // delay scene load
        for (float time = sceneLoadDelay; time >= 0; time -= Time.deltaTime)
        {
            yield return null;
        }
        // load next scene if scene count is greater than next scene index
        if (SceneManager.sceneCountInBuildSettings > sceneBuildIndex)
        {
            SceneManager.LoadScene(sceneBuildIndex);
        }
        else
        {
            Debug.Log("Scene build index is greater than scene count. Loading the first scene.");
            SceneManager.LoadScene(0);
        }
    }
}
