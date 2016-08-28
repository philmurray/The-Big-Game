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
            RotateButton.SetActive(_selectedBlock != null && _selectedBlock.Block.Orientations.Count > 1);
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

            if (GameController.instance.ActivePlayerBlocks.Count == 0) {
                for (int x = 0; x < BaseX; x++) {
                    for (int y = 0; y < BaseY; y++) {
                        GameController.instance.ActivePlayerBlocks.Add(new Block()
                        {
                            Type = Block.BlockType.Base,
                            Position = new Vector3(x, 0, y)
                        });
                    }
                }
                BlockContainer.SetBlocks();
            }
            Destroy(GameObject.FindGameObjectWithTag("Modal"));
            HUDController.instance.gameObject.SetActive(true);
        }
    }

    void Start()
    {
        BlockContainer.Player = GameController.instance.ActivePlayer;
        RealBlockContainer.Player = GameController.instance.ActivePlayer;
        HUDController.instance.gameObject.SetActive(false);
    }

    public void ReadyButtonClick()
    {
        State = States.Testing;
        HUDController.instance.gameObject.SetActive(false);
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
        HUDController.instance.gameObject.SetActive(true);
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
                    var bp = trans.gameObject.GetComponent<RealBlockBehavior>().Block.TransformPosition;
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

    public int AvailableBlocks(Block.BlockType block)
    {
        switch (block)
        {
            case Block.BlockType.Small:
                return GameController.instance.ActivePlayerState.AvailableSmallBlocks;
            case Block.BlockType.Medium:
                return GameController.instance.ActivePlayerState.AvailableMediumBlocks;
            case Block.BlockType.Large:
                return GameController.instance.ActivePlayerState.AvailableLargeBlocks;
            case Block.BlockType.Huge:
                return GameController.instance.ActivePlayerState.AvailableHugeBlocks;
            case Block.BlockType.Flag:
                return GameController.instance.ActivePlayerState.AvailableFlags;
            case Block.BlockType.Crystal:
                return GameController.instance.ActivePlayerState.AvailableCrystals;
            default:
                return 0;
        }
    }

    public void PlaceBlock(Block block)
    {
        GameController.instance.ActivePlayerBlocks.Add(block);
        switch (block.Type)
        {
            case Block.BlockType.Small:
                GameController.instance.ActivePlayerState.AvailableSmallBlocks--;
                break;
            case Block.BlockType.Medium:
                GameController.instance.ActivePlayerState.AvailableMediumBlocks--;
                break;
            case Block.BlockType.Large:
                GameController.instance.ActivePlayerState.AvailableLargeBlocks--;
                break;
            case Block.BlockType.Huge:
                GameController.instance.ActivePlayerState.AvailableHugeBlocks--;
                break;
            case Block.BlockType.Flag:
                GameController.instance.ActivePlayerState.AvailableFlags--;
                break;
            case Block.BlockType.Crystal:
                GameController.instance.ActivePlayerState.AvailableCrystals--;
                ReadyButton.SetActive(true);
                break;
            default:
                break;
        }

        HUDController.instance.Refresh();
    }

    public void RemoveBlock(Block block)
    {
        if (GameController.instance.ActivePlayerBlocks.Contains(block))
        {
            GameController.instance.ActivePlayerBlocks.Remove(block);
            switch (block.Type)
            {
                case Block.BlockType.Small:
                    GameController.instance.ActivePlayerState.AvailableSmallBlocks++;
                    break;
                case Block.BlockType.Medium:
                    GameController.instance.ActivePlayerState.AvailableMediumBlocks++;
                    break;
                case Block.BlockType.Large:
                    GameController.instance.ActivePlayerState.AvailableLargeBlocks++;
                    break;
                case Block.BlockType.Huge:
                    GameController.instance.ActivePlayerState.AvailableHugeBlocks++;
                    break;
                case Block.BlockType.Flag:
                    GameController.instance.ActivePlayerState.AvailableFlags++;
                    break;
                case Block.BlockType.Crystal:
                    GameController.instance.ActivePlayerState.AvailableCrystals++;
                    if (!GameController.instance.ActivePlayerBlocks.Exists(b => b.Type == Block.BlockType.Crystal))
                    {
                        ReadyButton.SetActive(false);
                    }
                    break;
                default:
                    break;
            }

            HUDController.instance.Refresh();
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
