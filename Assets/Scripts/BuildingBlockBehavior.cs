using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System;
using System.Collections.Generic;

public class BuildingBlockBehavior : BlockBehavior {
    public bool Draggable;
    public bool IsSupport;

    private bool isDragging;
    private float dragHeight;
    private bool isGood;



    public void OnMouseDown()
    {
        if (Draggable && !BuildingSceneController.instance.ShopShowing())
        {
            foreach (var b in transform.parent.GetComponentsInChildren<BuildingBlockBehavior>())
            {
                if (b == this)
                {
                    continue;
                }
                if (MinX < b.MaxX && MaxX > b.MinX)
                {
                    if (MinZ < b.MaxZ && MaxZ > b.MinZ)
                    {
                        if (MaxY == b.MinY)
                        {
                            return;
                        }
                    }
                }
            }
            isDragging = true;
            dragHeight = TransformPosition.y;
            BuildingSceneController.instance.SelectedBlock = null;
            BuildingSceneController.instance.RemoveBlock(Block);
        }
    }

    public void Select()
    {
        SetModel("Selected");
    }

    public void Deselect()
    {
        SetModel("Normal");
    }

    public void Bad()
    {
        SetModel("Bad");
    }

    public void Rotate()
    {
        Block.Orientation = (Block.Orientation + 1) % Orientations.Count;
        transform.rotation = TransformRotation;
        UpdatePosition(Block.PositionX, Block.PositionY, Block.PositionZ);
        if (!isGood) {
            Rotate();
        }
    }

    public void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            if (isGood)
            {
                BuildingSceneController.instance.PlaceBlock(Block);
                BuildingSceneController.instance.SelectedBlock = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void RefreshCollisions()
    {
        List<BuildingBlockBehavior> supportedBy = new List<BuildingBlockBehavior>();

        isGood = true;
        foreach (var b in transform.parent.GetComponentsInChildren<BuildingBlockBehavior>()) {
            if (b == this)
            {
                continue;
            }
            if (MinX < b.MaxX && MaxX > b.MinX)
            {
                if (MinZ < b.MaxZ && MaxZ > b.MinZ)
                {
                    if (MinY < b.MaxY && MaxY > b.MinY)
                    {
                        if (b.IsSupport)
                        {
                            UpdatePosition(Block.PositionX, b.MaxY, Block.PositionZ);
                            return;
                        }
                        else
                        {
                            isGood = false;
                        }
                    }
                    else if (MinY == b.MaxY && b.IsSupport) {
                        supportedBy.Add(b);
                    }
                }
            }
        }
        if (supportedBy.Count == 0)
        {
            if (MinY > 0)
            {
                UpdatePosition(Block.PositionX, 0, Block.PositionZ);
                return;
            }
            else
            {
                isGood = false;
            }
        }
        if (MaxY > BuildingSceneController.instance.MaxHeight)
        {
            isGood = false;
        }
        if (isGood)
        {
            Select();
        }
        else
        {
            Bad();
        }
    }

    private void UpdatePosition(int x, int y, int z)
    {
        Block.PositionX = x;
        Block.PositionY = y;
        Block.PositionZ = z;

        transform.position = TransformPosition;
        RefreshCollisions();
    }

    void Update()
    {
        if (isDragging)
        {
            Plane plane = new Plane(Vector3.up, new Vector3(0, dragHeight, 0));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 newPosition = ray.GetPoint(distance);
                Vector3 gridPosition = new Vector3(Mathf.Round(newPosition.x), Block.PositionY, Mathf.Round(newPosition.z));
                if (gridPosition != transform.position)
                {
                    if (gridPosition.x < 0 ||
                        gridPosition.z < 0 ||
                        gridPosition.x + Size.x > BuildingSceneController.instance.BaseX ||
                        gridPosition.z + Size.z > BuildingSceneController.instance.BaseY)
                    {
                        transform.position = newPosition;
                        Bad();
                        isGood = false;
                    }
                    else
                    {
                        UpdatePosition((int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z);
                    }
                }
            }
        }
    }

    private int MinX
    {
        get
        {
            return Block.PositionX;
        }
    }
    private int MaxX
    {
        get
        {
            return (int)Math.Round(Block.PositionX + Size.x);
        }
    }

    private int MinY
    {
        get
        {
            return Block.PositionY;
        }
    }
    private int MaxY
    {
        get
        {
            return (int)Math.Round(Block.PositionY + Size.y);
        }
    }

    private int MinZ
    {
        get
        {
            return Block.PositionZ;
        }
    }
    private int MaxZ
    {
        get
        {
            return (int)Math.Round(Block.PositionZ + Size.z);
        }
    }
}
