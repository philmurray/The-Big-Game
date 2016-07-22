using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuSceneController : MonoBehaviour {

    // Use this for initialization
    public void OnePlayer() {
        SceneManager.LoadScene("Upgrades");
    }
    public void TwoPlayers()
    {
        SceneManager.LoadScene("Upgrades");
    }
    public void Settings()
    {

    }
}
