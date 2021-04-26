/*
 * Copyright (C) Grigorij Glukhov. 2020. All rights reserved
 * Generate procedural cube mesh on current gameObject
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMeshFence : MonoBehaviour
{
  [Header("Post size")]
  public float m_PostHeight       = 1f;
  public float m_PostWidth        = 0.2f;
  [Header("Posts placement")]
  public float m_SectionCount     = 10f;
  public float m_DistBetweenPosts = 3f;
  public float m_PostTiltAngle    = 10f;
  public float m_PostDistVar      = 0.5f;
  [Header("Cross placement")]
  public float m_CrossPieceWidth  = 0.1f;
  public float m_CrossPieceHeight = 0.05f;
  public float m_CrossPieceY      = 0.5f;
  public float m_PostHeightVar    = 0.2f;
  public float m_CrossPieceYVar   = 0.2f;

  void Start()
  {
    MeshBuilder meshBuilder = new MeshBuilder();

    Vector3 prevCrossPosition = Vector3.zero;
    Quaternion prevRotation   = Quaternion.identity;

    for(int i = 0; i <= m_SectionCount; i++)
    {
      float distOffset = Random.Range(-m_PostDistVar, m_PostDistVar);
      // Calculate placement for posts and its direction
      Vector3 offset = Vector3.forward * m_DistBetweenPosts * i + (Vector3.forward * distOffset);

      float xAngle = Random.Range(-m_PostTiltAngle, m_PostTiltAngle);
      float zAngle = Random.Range(-m_PostTiltAngle, m_PostTiltAngle);
      Quaternion rotation = Quaternion.Euler(xAngle, 0.0f, zAngle);
      
      BuildPosts(meshBuilder, offset, rotation);
      
      // Crosspiece:
      Vector3 crossPosition = offset;
      // offset to the back of the post:
      crossPosition += rotation * (Vector3.back * m_PostWidth * 0.5f);
      // offset the height:
      //crossPosition += Vector3.up * m_CrossPieceY;

      // Cross position randomization
      float randomYStart = Random.Range(-m_CrossPieceYVar, m_CrossPieceYVar);
      float randomYEnd = Random.Range(-m_CrossPieceYVar, m_CrossPieceYVar);

      Vector3 crossYOffsetStart = prevRotation *  Vector3.up * (m_CrossPieceY + randomYStart);
      Vector3 crossYOffsetEnd = rotation * Vector3.up * (m_CrossPieceY + randomYEnd);

      if (i != 0) 
        BuildCrossPiece(meshBuilder, prevCrossPosition + crossYOffsetStart, crossPosition + crossYOffsetEnd);

      prevCrossPosition = crossPosition;
      prevRotation = rotation;
    }
  }

  void BuildPosts(MeshBuilder meshBuilder, Vector3 position, Quaternion rotation) 
  {
    // Some randomizarion
    float postHeight = m_PostHeight + Random.Range(-m_PostHeightVar, m_PostHeightVar);
    //Vector3 upDir      = Vector3.up      * m_PostHeight;
    Vector3 upDir      = rotation * Vector3.up      * postHeight;
    Vector3 rightDir   = rotation * Vector3.right   * m_PostWidth;
    Vector3 forwardDir = rotation * Vector3.forward * m_PostWidth;

    Vector3 farCorner  = upDir + rightDir + forwardDir + position;
    Vector3 nearCorner = position;
    
    // shift pivoit to centre-bottom:
    Vector3 pivotOffSet = (rightDir + forwardDir) * 0.5f;
    farCorner  -= pivotOffSet;
    nearCorner -= pivotOffSet;

    BuildQuadV2(meshBuilder, nearCorner, rightDir, upDir);
    BuildQuadV2(meshBuilder, nearCorner, upDir,    forwardDir);

    BuildQuadV2(meshBuilder, farCorner, -rightDir,   -forwardDir);
    BuildQuadV2(meshBuilder, farCorner, -upDir,      -rightDir);
    BuildQuadV2(meshBuilder, farCorner, -forwardDir, -upDir);

    // Get meshFilter we working with. Create mesh in this object
    MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

    if(meshFilter != null)
    {
      // Finalize mesh and its creation
      meshFilter.mesh = meshBuilder.CreateMesh();
      // Temporary auto-normals generator Unity function
    }
  }

  void BuildCrossPiece(MeshBuilder meshBuilder, Vector3 start, Vector3 end) 
  {
    Vector3 dir = end - start;

    Quaternion rotation = Quaternion.LookRotation(dir);

    Vector3 upDir      = rotation * Vector3.up      * m_CrossPieceHeight;
    Vector3 rightDir   = rotation * Vector3.right   * m_CrossPieceWidth;
    Vector3 forwardDir = rotation * Vector3.forward * dir.magnitude;

    Vector3 farCorner  = upDir + rightDir + forwardDir + start;
    Vector3 nearCorner = start;

    BuildQuadV2(meshBuilder, nearCorner, forwardDir, rightDir);
    BuildQuadV2(meshBuilder, nearCorner, rightDir,   upDir);
    BuildQuadV2(meshBuilder, nearCorner, upDir,      forwardDir);

    BuildQuadV2(meshBuilder, farCorner, -rightDir,   -forwardDir);
    BuildQuadV2(meshBuilder, farCorner, -upDir,      -rightDir);
    BuildQuadV2(meshBuilder, farCorner, -forwardDir, -upDir);

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




















