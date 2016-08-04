using UnityEngine;
using System.Collections;

public class PlayerBasedMaterial : MonoBehaviour {

    public Material PlayerOne;
    public Material PlayerTwo;

    // Use this for initialization
    void Start()
    {
        if (GameController.instance.ActivePlayer == GameController.Player.One)
        {
            GetComponent<Renderer>().material = PlayerOne;
        }
        else
        {
            GetComponent<Renderer>().material = PlayerTwo;
        }
    }
}
