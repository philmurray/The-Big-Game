using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.DataStructures;

public class MultipleBlockUpgrade : MonoBehaviour {
    public Text NumberText;
    public Block.BlockType BlockType;

    void Start()
    {
        NumberText.text = GameController.instance.ActivePlayerBlocks.FindAll(b => b.Type == BlockType).Count.ToString() + " x";
    }
}
