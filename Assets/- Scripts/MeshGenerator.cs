/*
 * Copyright (C) Grigorij Glukhov. 2020. All rights reserved
 * This script create polygon from 2 faces
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
  //[ExecuteAlways]
  //[ExecuteInEditMode]
  public int width = 1;
  public int height = 1;

  private Vector3[] vertices;
  private int[]     triangles;
  private Vector3[] normals;
  private Vector2[] uvs;

  void Start()
  {
    MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();// gets <MeshFilter> from gameObject
    Mesh mesh = new Mesh();
    
    CreateVertices  (ref vertices);
    CreateTriangles (ref triangles);
    CreateNormals   (ref normals);
    CreateUVs       (ref uvs);

    mesh.vertices   = vertices;
    mesh.triangles  = triangles;
    mesh.normals    = normals;
    mesh.uv         = uvs;

    meshFilter.mesh = mesh;
    mesh.RecalculateBounds();
    //mesh.Clear(); // Remove all mesh data from mesh
    
  }

  // Create 4 vertices
  void CreateVertices( ref Vector3[] _vertices) {
    _vertices = new Vector3[4]
    {
      new Vector3(0, 0, 0),
      new Vector3(width, 0, 0),
      new Vector3(0, height, 0),
      new Vector3(width, height, 0)
    };
  }

  // Assign order of triangles to vertices
  void CreateTriangles( ref int[] _triangles) {
    _triangles = new int[6]
    {
      // lower left triangle
      0, 2, 1,
      // upper right triangle
      2, 3, 1
    };
  }

  // Create direction of normals
  void CreateNormals( ref Vector3[] _normals) {
    _normals = new Vector3[4]
    {
      -Vector3.forward,
      -Vector3.forward,
      -Vector3.forward,
      -Vector3.forward
    };
  }

  // Create UVs set
  void CreateUVs( ref Vector2[] _uvs) {
    _uvs = new Vector2[4]
    {
      new Vector2(0, 0),
      new Vector2(1, 0),
      new Vector2(0, 1),
      new Vector2(1, 1)

    };
  }
}
