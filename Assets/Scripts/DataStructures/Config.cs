﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
    [Serializable]
    public class Config
    {
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
    }
}
