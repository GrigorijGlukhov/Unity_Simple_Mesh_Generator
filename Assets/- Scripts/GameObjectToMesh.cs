/*
 * Copyright (C) Grigorij Glukhov. 2020. All rigths reserved
 * Generate procedural mesh on current gameObject
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectToMesh : MonoBehaviour
{
  [Header("Size of each face")]
  public int m_Length = 1;
  public int m_Width  = 1;
  MeshBuilder meshBuilder = new MeshBuilder();
    // Start is called before the first frame update
    void Start()
    {
      // Mathod contain vertex, face, normal and uv reoutine
      // working with meshBuilder
      // _offset - positioning face
      BuildQuad(meshBuilder, new Vector3(1, 1, 1));

      // Get meshFilter we working with. Create mesh in this object
      MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

      if(meshFilter != null)
      {
        // Finalize mesh and its creation
        meshFilter.mesh = meshBuilder.CreateMesh();
      }
    }

    // Using MeshBuilder class adding information about current mesh we create
    void BuildQuad(MeshBuilder _meshBuilder, Vector3 _offset)
    {
      //Set up the verticies and triangles:
      meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, 0.0f) + _offset);
      meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
      meshBuilder.Normals.Add(Vector3.up);
        
      meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, m_Length) + _offset);
      meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
      meshBuilder.Normals.Add(Vector3.up);

      meshBuilder.Vertices.Add(new Vector3(m_Width, 0.0f, m_Length) + _offset);
      meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
      meshBuilder.Normals.Add(Vector3.up);

      meshBuilder.Vertices.Add(new Vector3(m_Width, 0.0f, 0.0f) + _offset);
      meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
      meshBuilder.Normals.Add(Vector3.up);

      int baseIndex = meshBuilder.Vertices.Count - 4;
      
      meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
      meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }
}
