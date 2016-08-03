using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System;
using System.Collections.Generic;

public class BlockBehavior : MonoBehaviour {
    public Block Block;
    public Material BadMaterial;
    public Material GoodMaterial;
    public Material SelectedMaterial;
    public bool Draggable;

    private bool isDragging;
    private float dragHeight;
    private bool isGood;

    private List<Renderer> Renderers;

    void Start() {
        Renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
    }

    public void OnMouseDown()
    {
        if (Draggable)
        {
            foreach (var b in GameController.instance.Blocks)
            {
                if (Block.MinX < b.MaxX && Block.MaxX > b.MinX)
                {
                    if (Block.MinZ < b.MaxZ && Block.MaxZ > b.MinZ)
                    {
                        if (Block.MaxY == b.MinY)
                        {
                            return;
                        }
                    }
                }
            }

            isDragging = true;
            dragHeight = Block.TransformPosition.y;
            BuildingSceneController.instance.SelectedBlock = null;
            BuildingSceneController.instance.RemoveBlock(Block);
        }
    }

    public void Select()
    {
        SetMaterial(SelectedMaterial);
    }

    public void Deselect()
    {
        SetMaterial(GoodMaterial);
    }

    public void Rotate()
    {
        Block.Orientation = (Block.Orientation + 1) % Block.Orientations.Count;
        transform.rotation = Block.TransformRotation;
        UpdatePosition(Block.Position);
        if (!isGood) {
            Rotate();
        }
    }

    public void OnMouseUp() {
        if (isDragging) {
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
        List<Block> supportedBy = new List<Block>();

        isGood = true;
        foreach (var b in GameController.instance.Blocks) {
            if (b == Block)
            {
                continue;
            }
            if (Block.MinX < b.MaxX && Block.MaxX > b.MinX)
            {
                if (Block.MinZ < b.MaxZ && Block.MaxZ > b.MinZ)
                {
                    if (Block.MinY < b.MaxY && Block.MaxY > b.MinY)
                    {
                        if (b.IsSupport)
                        {
                            UpdatePosition(new Vector3(Block.Position.x, b.MaxY, Block.Position.z));
                            return;
                        }
                        else
                        {
                            isGood = false;
                        }
                    }
                    else if (Block.MinY == b.MaxY && b.IsSupport) {
                        supportedBy.Add(b);
                    }
                }
            }
        }
        if (supportedBy.Count == 0)
        {
            if (Block.MinY > 0)
            {
                UpdatePosition(new Vector3(Block.Position.x, 0, Block.Position.z));
                return;
            }
            else
            {
                isGood = false;
            }
        }
        if (Block.MaxY > BuildingSceneController.instance.MaxHeight)
        {
            isGood = false;
        }
        if (isGood)
        {
            SetMaterial(SelectedMaterial);
        }
        else
        {
            SetMaterial(BadMaterial);
        }
    }

    private void SetMaterial(Material material)
    {
        Renderers.ForEach(r => r.material = material);
    }

    private void UpdatePosition(Vector3 pos)
    {
        Block.Position = pos;
        transform.position = Block.TransformPosition;
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
                Vector3 gridPosition = new Vector3(Mathf.Round(newPosition.x), Block.Position.y, Mathf.Round(newPosition.z));
                if (gridPosition != transform.position)
                {
                    if (gridPosition.x < 0 ||
                        gridPosition.z < 0 ||
                        gridPosition.x + Block.Size.x > BuildingSceneController.instance.BaseX ||
                        gridPosition.z + Block.Size.z > BuildingSceneController.instance.BaseY)
                    {
                        transform.position = newPosition;
                        SetMaterial(BadMaterial);
                        isGood = false;
                    }
                    else
                    {
                        UpdatePosition(gridPosition);
                    }
                }
            }
        }
    }
}
