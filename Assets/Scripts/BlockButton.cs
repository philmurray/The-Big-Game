using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using UnityEngine.UI;

public class BlockButton : MonoBehaviour {

    public Block.BlockType BlockType;
    public BlockContainer Parent;
    public Text AvailableText;

    private GameObject DraggingBlock;
    public void StartDrag() {
        Debug.Log("Start the Dragon");
        if (GameController.instance.ActivePlayerState.GetAvailableBlocks(BlockType) > 0)
        {
            DraggingBlock = Instantiate(Parent.BlockObjectsDictionary[BlockType]) as GameObject;
            DraggingBlock.transform.SetParent(Parent.transform);

            var blockBehavior = DraggingBlock.GetComponent<BuildingBlockBehavior>();

            if (BuildingSceneController.instance.DefaultOrientation.ContainsKey(blockBehavior.Block.Type)) {
                blockBehavior.Block.Orientation = BuildingSceneController.instance.DefaultOrientation[blockBehavior.Block.Type];
            }
            blockBehavior.Block.PositionX = 100;
            blockBehavior.Block.PositionY = 1;
            blockBehavior.Block.PositionZ = 100;

            blockBehavior.Player = GameController.instance.ActivePlayer;
            blockBehavior.transform.position = blockBehavior.TransformPosition;
            blockBehavior.transform.rotation = blockBehavior.TransformRotation;
            blockBehavior.OnMouseDown();
        }
    }
    public void EndDrag()
    {
        Debug.Log("End the Dragon");
        if (DraggingBlock != null) {
            DraggingBlock.GetComponent<BuildingBlockBehavior>().OnMouseUp();
            DraggingBlock = null;
        }
    }
    public void RefreshAvailable()
    {
        AvailableText.text = GameController.instance.ActivePlayerState.GetAvailableBlocks(BlockType).ToString();
    }
}
