using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;
using System;

public class BuildingSceneController : MonoBehaviour {

    public static BuildingSceneController instance;

    void Awake()
    {
        instance = this;
    }

    public int BaseX;
    public int BaseY;
    public int MaxHeight;

    public BlockContainer BlockContainer;
    public BlockContainer RealBlockContainer;

    public UpgradesContainer UpgradesContainer;

    public GameObject HUD;

    public GameObject SelectionMenu;
    public GameObject RotateButton;
    public GameObject ReadyButton;
    public GameObject RetryButton;

    public ShopModal ShopModal;

    public Dictionary<Block.BlockType, int> DefaultOrientation = new Dictionary<Block.BlockType, int>();

    public float TestingTime;
    private float TestingStart;

    private BuildingBlockBehavior _selectedBlock;
    public BuildingBlockBehavior SelectedBlock {
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
            RotateButton.SetActive(_selectedBlock != null && _selectedBlock.Orientations.Count > 1);
        }
        get {
            return _selectedBlock;
        }
    }

    private void Setup()
    {
        if (GameController.instance.ActivePlayerBlocks.Count == 0)
        {
            for (int x = 0; x < BaseX; x++)
            {
                for (int y = 0; y < BaseY; y++)
                {
                    GameController.instance.ActivePlayerBlocks.Add(new Block()
                    {
                        Type = Block.BlockType.Base,
                        PositionX = x,
                        PositionY = 0,
                        PositionZ = y
                    });
                }
            }
        }

        BlockContainer.SetBlocks();
        UpdateButtons();
    }

    void Start()
    {
        BlockContainer.Player = GameController.instance.ActivePlayer;
        RealBlockContainer.Player = GameController.instance.ActivePlayer;

        Setup();
        ShowShop();
    }

    public void ShowShop()
    {
        ShopModal.gameObject.SetActive(true);
        ShopModal.Refresh();
        HUD.SetActive(false);
    }

    public void HideShop()
    {
        ShopModal.gameObject.SetActive(false);
        HUD.SetActive(true);
        UpdateButtons();
    }


    public void ReadyButtonClick()
    {
        BlockContainer.gameObject.SetActive(false);
        HUD.SetActive(false);
        foreach (var child in BlockContainer.gameObject.transform)
        {
            var go = child as GameObject;
            if (go != null)
            {
                go.SetActive(false);
            }
        }

        RealBlockContainer.SetBlocks();
        TestingStart = Time.fixedTime;

        StartCoroutine(AwaitPositionChange());
    }

    public void RetryButtonClick()
    {
        foreach (var child in RealBlockContainer.gameObject.transform)
        {
            Destroy(((Transform)child).gameObject);
        }
        
        BlockContainer.gameObject.SetActive(true);
        RetryButton.SetActive(false);
        HUD.SetActive(true);
        foreach (var child in BlockContainer.gameObject.transform)
        {
            var go = child as GameObject;
            if (go != null)
            {
                go.SetActive(true);
            }
        }

    }


    IEnumerator AwaitPositionChange()
    {
        var done = false;
        while (Time.fixedTime < TestingStart + TestingTime && !done) {
            foreach (var child in RealBlockContainer.gameObject.transform)
            {
                var trans = child as Transform;
                if (trans != null)
                {
                    var bp = trans.gameObject.GetComponent<RealBlockBehavior>().TransformPosition;
                    var dist = Vector3.Distance(trans.position, bp);
                    if (dist > 0.25f)
                    {
                        RetryButton.SetActive(true);
                        done = true;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        if (done == false)
        {
            GameController.instance.NextScene(true);
        }
    }

    public void PlaceBlock(Block block)
    {
        GameController.instance.ActivePlayerBlocks.Add(block);
        GameController.instance.ActivePlayerState.AddAvailableBlocks(block.Type, -1);
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        UpgradesContainer.Refresh();
        ReadyButton.SetActive(GameController.instance.ActivePlayerBlocks.Exists(b => b.Type == Block.BlockType.Crystal));
        foreach (var button in HUD.GetComponentsInChildren<BlockButton>())
        {
            button.RefreshAvailable();
        }
    }
    public void RemoveBlock(Block block)
    {
        if (GameController.instance.ActivePlayerBlocks.Contains(block))
        {
            GameController.instance.ActivePlayerBlocks.Remove(block);
            GameController.instance.ActivePlayerState.AddAvailableBlocks(block.Type, 1);
            UpdateButtons();
        }
    }
    public void RemoveAllBlocks()
    {
        foreach (var block in GameController.instance.ActivePlayerBlocks)
        {
            if (block.Type != Block.BlockType.Base)
            {
                GameController.instance.ActivePlayerBlocks.Remove(block);
                GameController.instance.ActivePlayerState.AddAvailableBlocks(block.Type, 1);
            }
        }
        UpdateButtons();
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
