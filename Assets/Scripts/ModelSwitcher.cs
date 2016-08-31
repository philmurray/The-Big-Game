using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ModelSwitcher : MonoBehaviour {

    public List<ModelObject> ModelsList;

    private Dictionary<string, GameObject> _modelsDictionary;
    public Dictionary<string, GameObject> ModelsDictionary
    {
        get
        {
            if (_modelsDictionary == null)
            {
                _modelsDictionary = new Dictionary<string, GameObject>();
                ModelsList.ForEach(m => _modelsDictionary.Add(m.ModelName, m.Model));
            }
            return _modelsDictionary;
        }
    }

    [Serializable]
    public class ModelObject {
        public string ModelName;
        public GameObject Model;
    }

    private string _modelName;
    private GameObject _modelObject;
    public string ModelName {
        get {
            return _modelName;
        }
        set {
            if (_modelName != value)
            {
                if (_modelObject != null)
                {
                    Destroy(_modelObject);
                }
                _modelObject = Instantiate(ModelsDictionary[value]);
                _modelObject.transform.SetParent(transform, false);
                _modelName = value;
            }
        }
    }
    public GameObject Model {
        get
        {
            return _modelObject;
        }
    }
}
