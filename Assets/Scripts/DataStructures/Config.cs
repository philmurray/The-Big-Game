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

        public List<UpgradeConfig> Upgrades;

        private Dictionary<PlayerState.Upgrade, Dictionary<string,string>> _upgradesDictionary;
        public Dictionary<PlayerState.Upgrade, Dictionary<string, string>> UpgradesDictionary
        {
            get
            {
                if (_upgradesDictionary == null)
                {
                    _upgradesDictionary = new Dictionary<PlayerState.Upgrade, Dictionary<string, string>>();
                    Upgrades.ForEach(b => _upgradesDictionary.Add(b.UpgradeType, b.Options));
                }
                return _upgradesDictionary;
            }
        }


        public List<WeaponConfig> Weapons;

        private Dictionary<Weapon.WeaponType, WeaponConfig> _weaponsDictionary;
        public Dictionary<Weapon.WeaponType, WeaponConfig> WeaponsDictionary
        {
            get
            {
                if (_weaponsDictionary == null)
                {
                    _weaponsDictionary = new Dictionary<Weapon.WeaponType, WeaponConfig>();
                    Weapons.ForEach(b => _weaponsDictionary.Add(b.WeaponType, b));
                }
                return _weaponsDictionary;
            }
        }


        [Serializable]
        public class BlockConfig {
            public Block.BlockType BlockType;
            public Vector3 Size;
            public List<Vector3> Orientations;
            public bool IsSupport;
            public float Mass;
            public float Health;
        }

        [Serializable]
        public class UpgradeConfig {
            public PlayerState.Upgrade UpgradeType;
            public List<KeyValue> OptionsList;


            private Dictionary<string, string> _options;
            public Dictionary<string, string> Options
            {
                get
                {
                    if (_options == null)
                    {
                        _options = new Dictionary<string, string>();
                        OptionsList.ForEach(b => _options.Add(b.Key, b.Value));
                    }
                    return _options;
                }
            }
        }
        [Serializable]
        public class KeyValue {
            public string Key;
            public string Value;
        }

        [Serializable]
        public class WeaponConfig
        {
            public Weapon.WeaponType WeaponType;
            public float ProjectileMass;
            public float ProjectileDamage;
        }
    }
}
