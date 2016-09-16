using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;

public class BuildingSceneController : MonoBehaviour {

    public static BuildingSceneController instance;

    void Awake()
    {
        instance = this;
    }
    public enum States { Intro, Playing, Testing };
    public States State = States.Intro;

    public int BaseX;
    public int BaseY;
    public int MaxHeight;

    public BlockContainer BlockContainer;
    public BlockContainer RealBlockContainer;

    public GameObject HUD;

    public GameObject SelectionMenu;
    public GameObject RotateButton;
    public GameObject ReadyButton;
    public GameObject RetryButton;

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

    public void Play()
    {
        if (State == States.Intro)
        {
            State = States.Playing;

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

            Destroy(GameObject.FindGameObjectWithTag("Modal"));
        }
    }

    void Start()
    {
        BlockContainer.Player = GameController.instance.ActivePlayer;
        RealBlockContainer.Player = GameController.instance.ActivePlayer;
    }

    public void ReadyButtonClick()
    {
        State = States.Testing;
        BlockContainer.gameObject.SetActive(false);
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

        State = States.Playing;
        BlockContainer.gameObject.SetActive(true);
        RetryButton.SetActive(false);
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
