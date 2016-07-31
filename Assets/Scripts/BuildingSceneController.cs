using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;

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
    
    public int BaseX;
    public int BaseY;

    private HUDController HUD_Controller;

    void Start()
    {
        var hud = GameObject.FindGameObjectWithTag("HUD");
        HUD_Controller = hud.GetComponent<HUDController>();

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
        }
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

}
