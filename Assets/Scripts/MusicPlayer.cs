using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private void Awake() {
        // keep music player as a singleton (destroy this instance if there are 2 or more instances)
        if (2 <= FindObjectsOfType<MusicPlayer>().Length) {
            Destroy(this.gameObject);
        } else {
            // if this is an existing music player, keep it alive.
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
