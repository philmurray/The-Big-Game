﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System;
using System.Collections.Generic;

public class BlockBehavior : MonoBehaviour {
    public Block Block;
    public Material BadMaterial;
    public Material GoodMaterial;
    public bool Draggable;

    private bool isDragging;
    private float dragHeight;
    private bool isGood;

    private Renderer Renderer;

    void Start() {
        Renderer = GetComponent<Renderer>();
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
            BuildingSceneController.instance.RemoveBlock(Block);
        }
    }

    public void OnMouseUp() {
        if (isDragging) {
            isDragging = false;
            if (isGood)
            {
                BuildingSceneController.instance.PlaceBlock(Block);
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
            if (Block.MinX < b.MaxX && Block.MaxX > b.MinX)
            {
                if (Block.MinZ < b.MaxZ && Block.MaxZ > b.MinZ)
                {
                    if (Block.MinY < b.MaxY && Block.MaxY > b.MinY)
                    {
                        if (isDragging)
                        {
                            UpdatePosition(new Vector3(Block.Position.x, Block.Position.y + b.Size.y, Block.Position.z));
                            return;
                        }
                    }
                    else if (Block.MinY == b.MaxY) {
                        supportedBy.Add(b);
                    }
                }
            }
        }
        if (isDragging && supportedBy.Count == 0)
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
        if (isGood)
        {
            Renderer.material = GoodMaterial;
        }
        else
        {
            Renderer.material = BadMaterial;
        }
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
                        Renderer.material = BadMaterial;
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
