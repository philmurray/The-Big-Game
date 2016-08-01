﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;

public class BuildingSceneController : MonoBehaviour {

    public static BuildingSceneController instance;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public enum States { Intro, Playing, Stopping };
    public States State = States.Intro;

    public int BaseX;
    public int BaseY;
    public int MaxHeight;

    public BlockContainer BlockContainer;
    public GameObject SelectionMenu;
    public GameObject RotateButton;

    public Dictionary<Block.BlockType, int> DefaultOrientation = new Dictionary<Block.BlockType, int>();

    private BlockBehavior _selectedBlock;
    public BlockBehavior SelectedBlock {
        set
        {
            if (_selectedBlock != null)
            {
                _selectedBlock.Deselect();
            }
            _selectedBlock = value;

            if (_selectedBlock != null)
            {
                _selectedBlock.Select();
            }
            SelectionMenu.SetActive(_selectedBlock != null);
            RotateButton.SetActive(_selectedBlock != null && _selectedBlock.Block.Orientations.Count > 1);
        }
        get {
            return _selectedBlock;
        }
    }

    private GameObject HUD;
    private HUDController HUD_Controller;

    public void Play()
    {
        if (State == States.Intro)
        {
            State = States.Playing;

            if (GameController.instance.Blocks.Count == 0) {
                for (int x = 0; x < BaseX; x++) {
                    for (int y = 0; y < BaseY; y++) {
                        GameController.instance.Blocks.Add(new Block()
                        {
                            Type = Block.BlockType.Base,
                            Position = new Vector3(x, 0, y)
                        });
                    }
                }
                BlockContainer.SetBlocks(GameController.instance.Blocks);
            }
            Destroy(GameObject.FindGameObjectWithTag("Modal"));
            HUD.SetActive(true);
        }
    }

    void Start()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD");
        HUD_Controller = HUD.GetComponent<HUDController>();
        HUD.SetActive(false);
    }

    public int AvailableBlocks(Block.BlockType block)
    {
        switch (block)
        {
            case Block.BlockType.Small:
                return GameController.instance.PlayerState.AvailableSmallBlocks;
            case Block.BlockType.Medium:
                return GameController.instance.PlayerState.AvailableMediumBlocks;
            case Block.BlockType.Large:
                return GameController.instance.PlayerState.AvailableLargeBlocks;
            case Block.BlockType.Huge:
                return GameController.instance.PlayerState.AvailableHugeBlocks;
            case Block.BlockType.Flag:
                return GameController.instance.PlayerState.AvailableFlags;
            case Block.BlockType.Crystal:
                return GameController.instance.PlayerState.AvailableCrystals;
            default:
                return 0;
        }
    }

    public void PlaceBlock(Block block)
    {
        GameController.instance.Blocks.Add(block);
        switch (block.Type)
        {
            case Block.BlockType.Small:
                GameController.instance.PlayerState.AvailableSmallBlocks--;
                break;
            case Block.BlockType.Medium:
                GameController.instance.PlayerState.AvailableMediumBlocks--;
                break;
            case Block.BlockType.Large:
                GameController.instance.PlayerState.AvailableLargeBlocks--;
                break;
            case Block.BlockType.Huge:
                GameController.instance.PlayerState.AvailableHugeBlocks--;
                break;
            case Block.BlockType.Flag:
                GameController.instance.PlayerState.AvailableFlags--;
                break;
            case Block.BlockType.Crystal:
                GameController.instance.PlayerState.AvailableCrystals--;
                break;
            default:
                break;
        }

        HUD_Controller.Refresh();
    }

    public void RemoveBlock(Block block)
    {
        if (GameController.instance.Blocks.Contains(block))
        {
            GameController.instance.Blocks.Remove(block);
            switch (block.Type)
            {
                case Block.BlockType.Small:
                    GameController.instance.PlayerState.AvailableSmallBlocks++;
                    break;
                case Block.BlockType.Medium:
                    GameController.instance.PlayerState.AvailableMediumBlocks++;
                    break;
                case Block.BlockType.Large:
                    GameController.instance.PlayerState.AvailableLargeBlocks++;
                    break;
                case Block.BlockType.Huge:
                    GameController.instance.PlayerState.AvailableHugeBlocks++;
                    break;
                case Block.BlockType.Flag:
                    GameController.instance.PlayerState.AvailableFlags++;
                    break;
                case Block.BlockType.Crystal:
                    GameController.instance.PlayerState.AvailableCrystals++;
                    break;
                default:
                    break;
            }

            HUD_Controller.Refresh();
        }
    }

    public void DeleteSelected() {
        Destroy(_selectedBlock.gameObject);
        RemoveBlock(_selectedBlock.Block);
    }

    public void RotateSelected()
    {
        _selectedBlock.Rotate();
        if (!DefaultOrientation.ContainsKey(_selectedBlock.Block.Type))
        {
            DefaultOrientation.Add(_selectedBlock.Block.Type, _selectedBlock.Block.Orientation);
        }
        else
        {
            DefaultOrientation[_selectedBlock.Block.Type] = _selectedBlock.Block.Orientation;
        }
    }

}
