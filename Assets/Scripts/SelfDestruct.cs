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
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps.isStopped) {
            Destroy(this.gameObject);   
        }
    }

}
