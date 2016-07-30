using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;

public class BuildingSceneController : MonoBehaviour {

    public static BuildingSceneController instance;

    void Awake()
    {
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
    
    public int BaseX;
    public int BaseY;

    void Start()
    {
        if (GameController.instance.Blocks.Count == 0) {
            for (int x = 0; x < BaseX; x++) {
                for (int y = 0; y < BaseY; y++) {
                    GameController.instance.Blocks.Add(new Block()
                    {
                        Type = Block.BlockType.Base,
                        X = x,
                        Y = y,
                        Z = 0
                    });
                }
            }
        }
    }

}
