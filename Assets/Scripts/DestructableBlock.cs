using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

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
        
        var collisionPoint = transform.InverseTransformVector(collision.contacts[0].point);

        List<Vector3> points = new List<Vector3>();
        for (int x = 0; x < _rbb.Block.Size.x; x++)
        {
            for (int y = 0; y < _rbb.Block.Size.y; y++)
            {
                for (int z = 0; z < _rbb.Block.Size.z; z++)
                {
                    points.Add(new Vector3(
                        0.5f - _rbb.Block.Size.x / 2 + x,
                        0.5f - _rbb.Block.Size.y / 2 + y,
                        0.5f - _rbb.Block.Size.z / 2 + z
                    ));
                }
            }
        }
        var quad = points.Select(p => new
        {
            point = p,
            distance = Vector3.Distance(p, collisionPoint)
        }).OrderBy(p => p.distance).First();


        CreateSmallPieces(quad.point);
    }

    private void CreateSmallPieces(Vector3 pos)
    {
        for (float x = -0.5f; x <= 0.5; x++)
        {
            for (float y = -0.5f; y <= 0.5; y++)
            {
                for (float z = -0.5f; z <= 0.5; z++)
                {
                    CreateBlockPiece(pos, new Vector3(x,y,z));
                }
            }
        }

    }

    private void CreateBlockPiece(Vector3 localCenter, Vector3 position)
    {
        var b = Instantiate(SmallBlockPiece, transform.parent) as RealBlockBehavior;
        
        
        b.transform.position = transform.TransformVector(new Vector3(
            localCenter.x + (Mathf.Approximately(position.x, 0.0f) ? position.x : position.x > 0 ? position.x - b.Block.Size.x / 2 : position.x + b.Block.Size.x / 2),
            localCenter.y + (Mathf.Approximately(position.y, 0.0f) ? position.y : position.y > 0 ? position.y - b.Block.Size.y / 2 : position.y + b.Block.Size.y / 2),
            localCenter.z + (Mathf.Approximately(position.z, 0.0f) ? position.z : position.z > 0 ? position.z - b.Block.Size.z / 2 : position.z + b.Block.Size.z / 2)
        ));
        b.transform.rotation = transform.rotation;
        b.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
    }
}
