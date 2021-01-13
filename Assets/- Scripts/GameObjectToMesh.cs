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
  public int m_Height = 1;
  [Tooltip("Number of segments")]
  public int m_SegmentCount = 10;
  MeshBuilder meshBuilder = new MeshBuilder();
  
  // Start is called before the first frame update
  void Start()
  {
    // Number of X and Y segments will be produced
    for (int i = 0; i <= m_SegmentCount; i++) {

      float z = m_Length * i;
      float v = (1.0f / m_SegmentCount) * i; // uv

      for (int j = 0; j < m_SegmentCount; j++ ) {

        float x = m_Width * j;
        float u = (1.0f / m_SegmentCount) * j; // uv

        // Random range for height of mesh
        Vector3 offset = new Vector3(x, Random.Range(0.0f, m_Height), z);

        Vector2 uv = new Vector2(u, v);
        bool buildTriangles = i > 0 && j > 0;

        // Mathod contain vertex, face, normal and uv reoutine
        // working with meshBuilder
        // _offset - positioning face
        //BuildQuad(meshBuilder, offset);

        BuildQuadForGrid(meshBuilder, offset, uv, buildTriangles, m_SegmentCount);
      }
    }

    // Get meshFilter we working with. Create mesh in this object
    MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

    if(meshFilter != null)
    {
      // Finalize mesh and its creation
      meshFilter.mesh = meshBuilder.CreateMesh();
      // Temporary auto-normals generator Unity function
    }
  }

  // Builds quads based on previous one
  void BuildQuadForGrid(MeshBuilder _meshBuilder, 
                        Vector3 _position, 
                        Vector2 _uv, 
                        bool _buildTriangeles, 
                        int _vertsPerRow)
  {
    _meshBuilder.Vertices.Add(_position);
    _meshBuilder.UVs.Add(_uv);

    if (_buildTriangeles)
    {
      // setup face indicies (facing)
      int baseIndex = meshBuilder.Vertices.Count - 1;

      int index0 = baseIndex;
      int index1 = baseIndex - 1;
      int index2 = baseIndex - _vertsPerRow;
      int index3 = baseIndex - _vertsPerRow - 1;

      _meshBuilder.AddTriangle(index0, index2, index1);
      _meshBuilder.AddTriangle(index2, index3, index1);
    }
  } // BuildQuadForGrid mathod

  // Using MeshBuilder class adding information about current mesh we create
  void BuildQuad(MeshBuilder _meshBuilder, 
                 Vector3 _offset)
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
  } // BuildQuad method

}
