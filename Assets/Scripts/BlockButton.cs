using UnityEngine;
using System.Collections;

public class BlockButton : MonoBehaviour {

    public GameObject Block;
    public Transform Parent;

    private GameObject DraggingBlock;
    public void StartDrag() {
        if (BuildingSceneController.instance.AvailableBlocks(Block.GetComponent<BuildingBlockBehavior>().Block.Type) > 0)
        {
            DraggingBlock = Instantiate(Block) as GameObject;
            DraggingBlock.transform.SetParent(Parent);
            var blockBehavior = DraggingBlock.GetComponent<BuildingBlockBehavior>();

            if (BuildingSceneController.instance.DefaultOrientation.ContainsKey(blockBehavior.Block.Type)) {
                blockBehavior.Block.Orientation = BuildingSceneController.instance.DefaultOrientation[blockBehavior.Block.Type];
            }
            blockBehavior.Block.Position = new Vector3(100, 1, 100);
            blockBehavior.transform.position = blockBehavior.Block.TransformPosition;
            blockBehavior.transform.rotation = blockBehavior.Block.TransformRotation;
            blockBehavior.OnMouseDown();
        }
    }
    public void EndDrag()
    {
        if (DraggingBlock != null) {
            DraggingBlock.GetComponent<BuildingBlockBehavior>().OnMouseUp();
            DraggingBlock = null;
        }
    }
}
