using UnityEngine;
using System.Collections;

public class BlockButton : MonoBehaviour {

    public GameObject Block;

    private GameObject DraggingBlock;
    public void StartDrag() {
        DraggingBlock = Instantiate(Block, new Vector3(0,1,0), Quaternion.identity) as GameObject;
        DraggingBlock.GetComponent<BlockBehavior>().OnMouseDown();
    }
    public void EndDrag()
    {
        if (DraggingBlock != null) {
            DraggingBlock.GetComponent<BlockBehavior>().OnMouseUp();
            DraggingBlock = null;
        }
    }
}
