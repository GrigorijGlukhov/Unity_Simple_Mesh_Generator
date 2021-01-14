/*
 * Copyright (C) Grigorij Glukhov. 2020. All rigths reserved
 * Generate procedural cube mesh on current gameObject
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMeshCube : MonoBehaviour
{
  public int m_Heigth = 1;
  public int m_Width = 1;
  public int m_Length = 1;

  // Start is called before the first frame update
  void Start()
  {

    MeshBuilder meshBuilder = new MeshBuilder();
    
    Vector3 upDir      = Vector3.up      * m_Heigth;
    Vector3 rightDir   = Vector3.right   * m_Width;
    Vector3 forwardDir = Vector3.forward * m_Length;

    Vector3 farCorner  = (upDir + rightDir + forwardDir) / 2;
    Vector3 nearCorner = -farCorner;

    BuildQuadV2(meshBuilder, nearCorner, forwardDir, rightDir);
    BuildQuadV2(meshBuilder, nearCorner, rightDir, upDir);
    BuildQuadV2(meshBuilder, nearCorner, upDir, forwardDir);

    BuildQuadV2(meshBuilder, farCorner, -rightDir, -forwardDir);
    BuildQuadV2(meshBuilder, farCorner, -upDir, -rightDir);
    BuildQuadV2(meshBuilder, farCorner, -forwardDir, -upDir);

    Mesh mesh = meshBuilder.CreateMesh();

    // Get meshFilter we working with. Create mesh in this object
    MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

    if(meshFilter != null)
    {
      // Finalize mesh and its creation
      meshFilter.mesh = meshBuilder.CreateMesh();
      // Temporary auto-normals generator Unity function
    }
  }

  void BuildQuadV2(MeshBuilder meshBuilder, Vector3 offset, Vector3 widthDir, Vector3 lengthDir)
  {
    Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;

    meshBuilder.Vertices.Add(offset);
    meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
    meshBuilder.Normals.Add(normal);

    meshBuilder.Vertices.Add(offset + lengthDir);
    meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
    meshBuilder.Normals.Add(normal);

    meshBuilder.Vertices.Add(offset + lengthDir + widthDir);
    meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
    meshBuilder.Normals.Add(normal);

    meshBuilder.Vertices.Add(offset + widthDir);
    meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
    meshBuilder.Normals.Add(normal);

    int baseIndex = meshBuilder.Vertices.Count - 4;

    meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
    meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
  }
}
