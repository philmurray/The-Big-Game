﻿using UnityEngine;
using System.Collections;

public class Shatter : MonoBehaviour {

    public bool Active;
    public float ShardLifeTime;
    public float ShardLifeTimeVariance;
    public float ShardExplosionForce;

    private bool isQuitting;
    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    void OnDestroy()
    {
        if (Active && !isQuitting)
        {
            MeshFilter MF = GetComponent<MeshFilter>();
            MeshRenderer MR = GetComponent<MeshRenderer>();
            Rigidbody RB = GetComponent<Rigidbody>();
            Mesh M = MF.mesh;
            Vector3[] verts = M.vertices;
            Vector3[] normals = M.normals;
            Vector2[] uvs = M.uv;
            for (int submesh = 0; submesh < M.subMeshCount; submesh++)
            {
                int[] indices = M.GetTriangles(submesh);
                for (int i = 0; i < indices.Length; i += 3)
                {
                    Vector3[] newVerts = new Vector3[3];
                    Vector3[] newNormals = new Vector3[3];
                    Vector2[] newUvs = new Vector2[3];
                    for (int n = 0; n < 3; n++)
                    {
                        int index = indices[i + n];
                        newVerts[n] = verts[index];
                        newUvs[n] = uvs[index];
                        newNormals[n] = normals[index];
                    }
                    Mesh mesh = new Mesh();
                    mesh.vertices = newVerts;
                    mesh.normals = newNormals;
                    mesh.uv = newUvs;

                    mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

                    GameObject GO = new GameObject("Triangle " + (i / 3));
                    GO.transform.SetParent(transform.parent);
                    GO.transform.position = transform.position;
                    GO.transform.rotation = transform.rotation;
                    GO.transform.localScale = transform.localScale;
                    GO.AddComponent<MeshRenderer>().material = MR.materials[submesh];
                    GO.AddComponent<MeshFilter>().mesh = mesh;
                    GO.AddComponent<BoxCollider>();
                    GO.AddComponent<PositionTracker>();
                    var rb = GO.AddComponent<Rigidbody>();
                    rb.velocity = RB.velocity;
                    rb.angularVelocity = RB.angularVelocity;
                    rb.AddExplosionForce(ShardExplosionForce, transform.position, 30);

                    Destroy(GO, ShardLifeTime + Random.Range(0.0f, ShardLifeTimeVariance));
                }
            }
        }
    }
}