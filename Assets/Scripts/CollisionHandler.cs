using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class CollisionHandler : MonoBehaviour
{

    [Tooltip("A delay between triggering loading the next scene and loading the scene.")]
    [SerializeField] float sceneLoadDelay = 1.0f;
    [Tooltip("Object with Playable Director assigned with the master timeline.")]
    [SerializeField] GameObject timeline;

    private void OnTriggerEnter(Collider other) {
        InitiateFailSequence();
    }

    private void InitiateFailSequence() {
        DeactivateShipVisuals();
        DeactivateControls();
        DeactivateTimeline();
        ReloadScene();        
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
        timeline.GetComponent<PlayableDirector>().Pause();
    }

    private void ReloadScene() {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadScene(int sceneBuildIndex) {
        StartCoroutine(LoadSceneWithDelay(sceneBuildIndex));
    }

    IEnumerator LoadSceneWithDelay(int sceneBuildIndex)
    {
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
