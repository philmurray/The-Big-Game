using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;

public class GameController : MonoBehaviour {

    public PlayerState PlayerState;

    public static GameController instance;

    void Awake() {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

}
