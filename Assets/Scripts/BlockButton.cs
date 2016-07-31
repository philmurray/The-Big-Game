using UnityEngine;
using System.Collections;

public class BlockButton : MonoBehaviour {

    public GameObject Block;

    private GameObject DraggingBlock;
    public void StartDrag() {
        if (BuildingSceneController.instance.AvailableBlocks(Block.GetComponent<BlockBehavior>().Block.Type) > 0)
        {
            DraggingBlock = Instantiate(Block) as GameObject;
            var blockBehavior = DraggingBlock.GetComponent<BlockBehavior>();
            
            blockBehavior.Block.Position = new Vector3(0, 1, 0);
            blockBehavior.transform.position = blockBehavior.Block.TransformPosition;
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
