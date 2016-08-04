using UnityEngine;
using System.Collections;

public class MainMenuSceneController : MonoBehaviour {

    // Use this for initialization
    public void OnePlayer() {
        GameController.instance.NextScene(true);
    }
    public void TwoPlayers()
    {
        GameController.instance.PlayerTwo.IsHuman = true;
        GameController.instance.NextScene(true);
    }
    public void Settings()
    {

    }
}
