using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBasedImage : MonoBehaviour {

    public Sprite PlayerOne;
    public Sprite PlayerTwo;

	// Use this for initialization
	void Start () {
        if (GameController.instance.ActivePlayer == GameController.Player.One)
        {
            GetComponent<Image>().sprite = PlayerOne;
        }
        else
        {
            GetComponent<Image>().sprite = PlayerTwo;
        }
	}
}
