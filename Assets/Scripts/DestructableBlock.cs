using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DataStructures;

public class DestructableBlock : Destructable {

    public Block.BlockType SmallBlockPiece;
    public Block.BlockType LargeBlockPiece;

    private RealBlockBehavior _rbb;

    public override void Start()
    {
        base.Start();
        _rbb = GetComponent<RealBlockBehavior>();
    }

    public override void OnDamaged(Collision collision)
    {
        var initialHealth = _rbb.InitialHealth;
        if (Health / initialHealth < 0.33f)
        {
            _rbb.SetModel("VeryDamaged");
        }
        else if (Health / initialHealth < 0.66f)
        {
            _rbb.SetModel("Damaged");
        }
    }

    struct BlockUnit
    {
        public Vector3 Point;
        public float Distance;
        public Vector3 Index;
    }
    public override void OnDestroyed(Collision collision)
    {
        base.OnDestroyed(collision);
        
        var collisionPoint = transform.InverseTransformPoint(collision.contacts[0].point);


        var blockSize = _rbb.InitialSize;
        List<BlockUnit> points = new List<BlockUnit>();
        for (int x = 0; x < blockSize.x; x++)
        {
            for (int y = 0; y < blockSize.y; y++)
            {
                for (int z = 0; z < blockSize.z; z++)
                {
                    Vector3 point = new Vector3(
                        0.5f - blockSize.x / 2 + x,
                        0.5f - blockSize.y / 2 + y,
                        0.5f - blockSize.z / 2 + z
                    );
                    points.Add(new BlockUnit {
                        Point = point,
                        Distance = Vector3.Distance(point, collisionPoint),
                        Index = new Vector3(x,y,z)
                    });
                }
            }
        }
        var quad = points.OrderBy(p => p.Distance).First();
        CreateSmallPieces(quad.Point);

        List<float> position = new List<float> { 0.0f, 0.0f, 0.0f};
        List<float> size = new List<float> { blockSize.x, blockSize.y, blockSize.z };
        List<float> index = new List<float> { quad.Index.x, quad.Index.y, quad.Index.z };

        List<int> dimensions = new List<int> { 0, 1, 2 };
        dimensions.OrderBy(x => UnityEngine.Random.value).ToList().ForEach(d => SliceDimension(d, position, size, index));
    }

    private void SliceDimension(int dimension, List<float> position, List<float> size, List<float> index)
    {
        if (size[dimension] > 1)
        {
            if (index[dimension] > 0.0f) {

                List<float> pieceSize = new List<float>(size);
                pieceSize[dimension] = index[dimension];

                List<float> piecePos = new List<float>(position);
                piecePos[dimension] = position[dimension] - (size[dimension] - pieceSize[dimension]) / 2;

                CreateLargePiece(piecePos, pieceSize);
            }

            if (index[dimension] < size[dimension] - 1)
            {
                List<float> pieceSize = new List<float>(size);
                pieceSize[dimension] = (size[dimension] - 1) - index[dimension];

                List<float> piecePos = new List<float>(position);
                piecePos[dimension] = position[dimension] + (size[dimension] - pieceSize[dimension]) / 2;

                CreateLargePiece(piecePos, pieceSize);
            }

            position[dimension] = index[dimension] - size[dimension] / 2 + 0.5f;
            size[dimension] = 1.0f;
        }
    }

    private void CreateLargePiece(List<float> pos, List<float> size)
    {
        var b = _rbb.ParentContainer.AddBlock(LargeBlockPiece).GetComponent<RealBlockBehavior>();
        b.InitialSize = new Vector3(size[0], size[1], size[2]);
        b.Multiplier = size[0] * size[1] * size[2];

        var localPosition = new Vector3(pos[0], pos[1], pos[2]);

        b.transform.position = transform.TransformPoint(localPosition);
        b.transform.rotation = transform.rotation;
        b.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * 0.75f;
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
        var b = _rbb.ParentContainer.AddBlock(SmallBlockPiece).GetComponent<RealBlockBehavior>();
        var blockSize = b.Size;
        var localPosition = new Vector3(
            localCenter.x + (Mathf.Approximately(position.x, 0.0f) ? position.x : position.x > 0 ? position.x - blockSize.x / 2 : position.x + blockSize.x / 2),
            localCenter.y + (Mathf.Approximately(position.y, 0.0f) ? position.y : position.y > 0 ? position.y - blockSize.y / 2 : position.y + blockSize.y / 2),
            localCenter.z + (Mathf.Approximately(position.z, 0.0f) ? position.z : position.z > 0 ? position.z - blockSize.z / 2 : position.z + blockSize.z / 2)
        );

        b.transform.position = transform.TransformPoint(localPosition);
        b.transform.rotation = transform.rotation;
        b.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * 0.75f;
    }
}
