using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;

public abstract class BlockBehavior : MonoBehaviour {

    public GameController.Player Player;
    public Block Block;
    
    private ModelSwitcher Models;
    private bool TopCollider;

    public virtual void Start()
    {
        Models = GetComponent<ModelSwitcher>();
        SetModel("Normal");

        var collider = GetComponent<BoxCollider>();
        if (collider != null) {
            collider.size = Block.InitialSize;
            TopCollider = true;
        }
    }

    public void SetModel(string name)
    {
        if (Models.ModelsDictionary.ContainsKey(name))
        {
            Models.ModelName = name;
        }
        else
        {
            Models.ModelName = name + "_" + Player;
        }
        if (TopCollider)
        {
            Models.Model.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
