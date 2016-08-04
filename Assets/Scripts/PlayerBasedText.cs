using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBasedText : MonoBehaviour {

    public string PlayerOne;
    public string PlayerTwo;

    // Use this for initialization
    void Start()
    {
        if (GameController.instance.ActivePlayer == GameController.Player.One)
        {
            GetComponent<Text>().text = PlayerOne;
        }
        else
        {
            GetComponent<Text>().text = PlayerTwo;
        }
    }
}
