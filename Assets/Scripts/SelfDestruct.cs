using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{

    // TODO - the following does not reliably trigger.
    // private void OnEnable() {
    //     var main = GetComponent<ParticleSystem>().main;
    //     main.stopAction = ParticleSystemStopAction.Callback;
    // }

    // private void OnParticleSystemStopped() {
    //     Destroy(this.gameObject);
    // }

    private void Update() {
        bool particlesDone = false, audioDone = false;
        ParticleSystem ps = GetComponent<ParticleSystem>();
        AudioSource aus = GetComponent<AudioSource>();
        if ((null == ps) || (null != ps && ps.isStopped)) {
            particlesDone = true;
        }
        if ((null == aus) || (null != aus && !aus.isPlaying)) {
            audioDone = true;              
        }
        if (particlesDone && audioDone) {
            Destroy(this.gameObject);
        }
    }

}
