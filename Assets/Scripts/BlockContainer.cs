using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System;
using System.Collections.Generic;

public class BlockContainer : MonoBehaviour {

    public GameController.Player Player;
    public List<Block> Blocks;
    public List<BlockTypeItem> BlockObjects;

    private Dictionary<Block.BlockType, GameObject> _blockObjects;
    public Dictionary<Block.BlockType, GameObject> BlockObjectsDictionary
    {
        get
        {
            if (_blockObjects == null)
            {
                _blockObjects = new Dictionary<Block.BlockType, GameObject>();
                BlockObjects.ForEach(b => _blockObjects.Add(b.BlockType, b.BlockObject));
            }
            return _blockObjects;
        }
    }

    [Serializable]
    public class BlockTypeItem {
        public Block.BlockType BlockType;
        public GameObject BlockObject;
    }

    public void SetBlocks()
    {
        var blocks = GameController.instance.GetPlayer(Player).Blocks;
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Blocks = blocks;
        Blocks.ForEach(b => AddBlock(b));
    }
    public void AddBlock(Block block)
    {
        var go = Instantiate(BlockObjectsDictionary[block.Type]) as GameObject;
        var bb = go.GetComponent<BlockBehavior>();
        if (bb != null)
        {
            bb.Block = block;
            bb.transform.position = bb.TransformPosition;
            bb.transform.rotation = bb.TransformRotation;
            bb.Player = Player;
        }
        go.transform.SetParent(transform,false);
    }
}
