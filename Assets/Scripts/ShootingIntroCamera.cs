using UnityEngine;
using System.Collections;

public class ShootingIntroCamera : MonoBehaviour {

    // Use this for initialization
    public void AllDone() {
        ShootingSceneController.instance.IntroDone();
    }
}
