using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class CollisionHandler : MonoBehaviour
{

    [Tooltip("Particles to play on success.")]
    [SerializeField] ParticleSystem successParticles;
    [Tooltip("A delay between success triggering loading the next scene and loading the scene.")]
    [SerializeField] float successSceneLoadDelay = 10.0f;
    [Tooltip("Particles to play on failure, from first to play to last.")]
    [SerializeField] ParticleSystem[] failParticles;
    [Tooltip("Delay between starting each fail particles.")]
    [SerializeField] float failParticlesDelay = 0.2f;
    [Tooltip("Audio to play on fail.")]
    [SerializeField] AudioSource[] failAudio;
    [Tooltip("A delay between failure triggering loading the next scene and loading the scene.")]
    [SerializeField] float failSceneLoadDelay = 2.75f;

    private PlayableDirector timeline;
    private string masterTimeline = "MasterTimeline";
    private string finish = "Finish";
    private bool actionOccuring = false;

    private void Start() {
        timeline = GameObject.FindGameObjectWithTag(masterTimeline).GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other) {
        // action occuring to prevent additional actions from triggering
        if (!actionOccuring) {
            actionOccuring = true;
            if (other.tag.Equals(finish) && !actionOccuring) {
                InitiateSuccessSequence();
            } else {
                InitiateFailSequence();
            }
        }
    }

    private void InitiateSuccessSequence() {
        successParticles.Play();
        LoadNextScene();
    }

    private void InitiateFailSequence() {
        StartCoroutine(PlayFailFXWithDelay());
        DeactivateControls();
        DeactivateTimeline();
        ReloadScene();
    }

    IEnumerator PlayFailFXWithDelay() {
        // play SFX
        foreach (AudioSource aus in failAudio) {
            aus.Play();
        }
        // play VFX
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
        PlayerControls pc = this.GetComponentInParent<PlayerControls>();
        pc.SetWeaponsActive(false);
        pc.enabled = false;
    }

    private void DeactivateTimeline() {
        timeline.Pause();
    }

    private void LoadNextScene() {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1, successSceneLoadDelay);
    }

    private void ReloadScene() {
        LoadScene(SceneManager.GetActiveScene().buildIndex, failSceneLoadDelay);
    }

    private void LoadScene(int sceneBuildIndex, float loadDelay) {
        StartCoroutine(LoadSceneWithDelay(sceneBuildIndex, loadDelay));
    }

    IEnumerator LoadSceneWithDelay(int sceneBuildIndex, float loadDelay) {
        // delay scene load
        for (float time = loadDelay; time >= 0; time -= Time.deltaTime)
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
