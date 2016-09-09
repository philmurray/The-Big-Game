using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;
using System;

public abstract class BlockBehavior : MonoBehaviour {

    public GameController.Player Player;
    public Block Block;
    public List<Vector3> Orientations;
    public Vector3 InitialSize;

    private ModelSwitcher Models;
    private bool TopCollider;

    public virtual void Start()
    {
        Models = GetComponent<ModelSwitcher>();
        SetModel("Normal");

        var collider = GetComponent<BoxCollider>();
        if (collider != null) {
            collider.size = InitialSize;
            TopCollider = true;
        }
    }

    public void SetModel(string name)
    {
        if (Models.ModelsDictionary.ContainsKey(name))
        {
            Models.ModelName = name;
        }
        else if (Models.ModelsDictionary.ContainsKey(name + "_" + Player))
        {
            Models.ModelName = name + "_" + Player;
        }
        if (TopCollider)
        {
            Models.Model.GetComponent<BoxCollider>().enabled = false;
        }
        var expander = Models.Model.GetComponent<ExpanderModel>();
        if (expander != null)
        {
            expander.transform.localScale = InitialSize;
        }
    }

    public Vector3 TransformPosition
    {
        get
        {
            return Block.Position + (Size - Vector3.one) / 2;
        }
    }
    public Quaternion TransformRotation
    {
        get
        {
            return Quaternion.Euler(Orientations[Block.Orientation]);
        }
    }

    public Vector3 Size
    {
        get
        {
            var tmpSize = InitialSize;

            if (Orientations[Block.Orientation].x == 90)
            {
                float tmp = tmpSize.y;
                tmpSize.y = tmpSize.z;
                tmpSize.z = tmp;
            }
            if (Orientations[Block.Orientation].y == 90)
            {
                float tmp = tmpSize.x;
                tmpSize.x = tmpSize.z;
                tmpSize.z = tmp;
            }
            if (Orientations[Block.Orientation].z == 90)
            {
                float tmp = tmpSize.x;
                tmpSize.x = tmpSize.y;
                tmpSize.y = tmp;
            }
            return tmpSize;
        }
    }

    public BlockContainer ParentContainer
    {
        get
        {
            return transform.parent.GetComponent<BlockContainer>();
        }
    }
}
