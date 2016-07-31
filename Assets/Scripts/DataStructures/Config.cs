using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
    [Serializable]
    public class Config
    {
        public List<BlockConfig> Blocks;
        public Dictionary<Block.BlockType, BlockConfig> BlocksDictionary = new Dictionary<Block.BlockType, BlockConfig>();

        [Serializable]
        public class BlockConfig {
            public Block.BlockType BlockType;
            public Vector3 Size;
            public List<Vector3> Orientations;
            public bool IsSupport;
        }

        public void Initialize()
        {
            Blocks.ForEach(b => BlocksDictionary.Add(b.BlockType, b));
        }
    }
}
