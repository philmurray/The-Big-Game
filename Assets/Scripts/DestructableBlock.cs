using UnityEngine;
using System.Collections;
using System;

public class DestructableBlock : Destructable {

    public RealBlockBehavior SmallBlockPiece;


    private RealBlockBehavior _rbb;

    void Start()
    {
        _rbb = GetComponent<RealBlockBehavior>();
    }

    public override void OnDamaged(Collision collision)
    {
        var initialHealth = _rbb.Block.Health(_rbb.Player);
        if (Health / initialHealth < 0.33f)
        {
            _rbb.SetModel("VeryDamaged");
        }
        else if (Health / initialHealth < 0.66f)
        {
            _rbb.SetModel("Damaged");
        }
    }

    public override void OnDestroyed(Collision collision)
    {
        base.OnDestroyed(collision);

        if (_rbb.Block.Size == Vector3.one)
        {
            CreateSmallPieces(Vector3.zero);
        }
        else
        {

        }
    }

    private void CreateSmallPieces(Vector3 pos)
    {
        var go = new GameObject();
        go.transform.SetParent(transform.parent, false);
        go.transform.localPosition = pos;

        for (float x = -0.5f; x <= 0.5; x++)
        {
            for (float y = -0.5f; y <= 0.5; y++)
            {
                for (float z = -0.5f; z <= 0.5; z++)
                {
                    CreateBlockPiece(go.transform, new Vector3(x,y,z));
                }
            }
        }

    }

    private void CreateBlockPiece(Transform parent, Vector3 position)
    {
        var b = Instantiate(SmallBlockPiece);
        var rb = b.GetComponent<RealBlockBehavior>();

        b.transform.SetParent(parent);
        b.transform.localPosition = new Vector3(
            Mathf.Approximately(position.x, 0.0f) ? position.x : position.x > 0 ? position.x - rb.Block.Size.x / 2 : position.x + rb.Block.Size.x / 2,
            Mathf.Approximately(position.y, 0.0f) ? position.y : position.y > 0 ? position.y - rb.Block.Size.y / 2 : position.y + rb.Block.Size.y / 2,
            Mathf.Approximately(position.z, 0.0f) ? position.z : position.z > 0 ? position.z - rb.Block.Size.z / 2 : position.z + rb.Block.Size.z / 2
        );
    }
}
