using UnityEngine;
using System.Collections;

public class IntroCamera : MonoBehaviour {

    // Use this for initialization
    public void AllDone() {
        ShootingSceneController.instance.IntroDone();
    }
}
