using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class CollisionHandler : MonoBehaviour
{

    [Tooltip("A delay between triggering loading the next scene and loading the scene.")]
    [SerializeField] float sceneLoadDelay = 1.0f;
    [Tooltip("Object with Playable Director assigned with the master timeline.")]
    [SerializeField] GameObject timeline;

    private void OnTriggerEnter(Collider other) {
        // Destroy(this.gameObject);
        timeline.GetComponent<PlayableDirector>().Pause();
        StartCoroutine(LoadSceneWithDelay(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadSceneWithDelay(int sceneBuildIndex)
    {
        for (float time = sceneLoadDelay; time >= 0; time -= Time.deltaTime)
        {
            yield return null;
            Debug.Log($"{time}");
        }
        LoadScene(sceneBuildIndex);
    }

    void LoadScene(int sceneBuildIndex)
    {
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
