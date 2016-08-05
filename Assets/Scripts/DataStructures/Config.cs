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

        private Dictionary<Block.BlockType, BlockConfig> _blocksDictionary;
        public Dictionary<Block.BlockType, BlockConfig> BlocksDictionary {
            get
            {
                if (_blocksDictionary == null) {
                    _blocksDictionary = new Dictionary<Block.BlockType, BlockConfig>();
                    Blocks.ForEach(b => _blocksDictionary.Add(b.BlockType, b));
                }
                return _blocksDictionary;
            }
        }

        [Serializable]
        public class BlockConfig {
            public Block.BlockType BlockType;
            public Vector3 Size;
            public List<Vector3> Orientations;
            public bool IsSupport;
        }
    }
}
