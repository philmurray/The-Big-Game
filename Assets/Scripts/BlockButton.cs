using UnityEngine;
using System.Collections;

public class BlockButton : MonoBehaviour {

    public GameObject Block;
    public Transform Parent;

    private GameObject DraggingBlock;
    public void StartDrag() {
        if (BuildingSceneController.instance.AvailableBlocks(Block.GetComponent<BlockBehavior>().Block.Type) > 0)
        {
            DraggingBlock = Instantiate(Block) as GameObject;
            DraggingBlock.transform.SetParent(Parent);
            var blockBehavior = DraggingBlock.GetComponent<BlockBehavior>();

            if (BuildingSceneController.instance.DefaultOrientation.ContainsKey(blockBehavior.Block.Type)) {
                blockBehavior.Block.Orientation = BuildingSceneController.instance.DefaultOrientation[blockBehavior.Block.Type];
            }
            blockBehavior.Block.Position = new Vector3(0, 1, 0);
            blockBehavior.transform.position = blockBehavior.Block.TransformPosition;
            blockBehavior.transform.rotation = blockBehavior.Block.TransformRotation;
            blockBehavior.OnMouseDown();
        }
    }
    public void EndDrag()
    {
        if (DraggingBlock != null) {
            DraggingBlock.GetComponent<BlockBehavior>().OnMouseUp();
            DraggingBlock = null;
        }
    }
}
