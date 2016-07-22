using UnityEngine;
using System.Collections;

public class UpgradeSceneController : MonoBehaviour {

    public enum States { Intro, Playing, Stopping };
    public States SceneState = States.Intro;
    private float StateStart;

    public float PlayTime;

    public static UpgradeSceneController instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator SpawnUpgrades() {
        while (Time.fixedTime - StateStart < PlayTime) {
            //make some upgrades fall
            yield return new WaitForSeconds(1);
        }
        SceneState = States.Stopping;
        StateStart = Time.fixedTime;
    }

    public void StartGame()
    {
        Destroy(GameObject.FindGameObjectWithTag("Modal"));
        SceneState = States.Playing;
        StateStart = Time.fixedTime;
    }
}
