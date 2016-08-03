using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System;
using System.Collections.Generic;

public class BlockContainer : MonoBehaviour {

    public List<Block> Blocks;
    public List<BlockTypeItem> BlockObjects;

    private Dictionary<Block.BlockType, GameObject> _blockObjects = new Dictionary<Block.BlockType, GameObject>();

    [Serializable]
    public class BlockTypeItem {
        public Block.BlockType BlockType;
        public GameObject BlockObject;
    }
    
	void Start () {
        BlockObjects.ForEach(b => _blockObjects.Add(b.BlockType, b.BlockObject));
	}

    public void SetBlocks(List<Block> blocks)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Blocks = blocks;
        Blocks.ForEach(b => AddBlock(b));
    }
    public void AddBlock(Block block) {
        var go = Instantiate(_blockObjects[block.Type], block.TransformPosition, block.TransformRotation) as GameObject;
        var bb = go.GetComponent<BlockBehavior>();
        if (bb != null)
        {
            bb.Block = block;
        }
        var rbb = go.GetComponent<RealBlockBehavior>();
        if (rbb != null)
        {
            rbb.Block = block;
        }
        go.transform.SetParent(transform);
    }
}
