/*
 * Copyright (C) Grigorij Glukhov. 2020. All rigths reserved
 * Generate procedural cube mesh on current gameObject
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMeshHouse : MonoBehaviour
{
  [Header("Global size")]
  [Tooltip("Set Height of object")]
  public int m_Heigth              = 1;
  [Tooltip("Set Width of object")]
  public int m_Width               = 1;
  [Tooltip("Set Length of object")]
  public int m_Length              = 1;
  [Tooltip("Set Height of roof")]
  public float m_RoofHeight        = .5f;
  [Tooltip("How much roof offsets to sides")]
  public float m_RoofOverhangSide  = .2f;
  [Tooltip("How much roof offsets to direction")]
  public float m_RoofOverhangFront = .3f;
  [Tooltip("Roof bias anti-z-fighting")]
  public float m_RoofBias = .1f;

  void Start()
  {
    MeshBuilder meshBuilder = new MeshBuilder();
    
    Vector3 upDir      = Vector3.up      * m_Heigth;
    Vector3 rightDir   = Vector3.right   * m_Width;
    Vector3 forwardDir = Vector3.forward * m_Length;

    Vector3 farCorner  = upDir + rightDir + forwardDir;
    Vector3 nearCorner = Vector3.zero;

    // Shift pivot to centre-bottom
    Vector3 pivotOffSet = (rightDir + forwardDir) * .5f;
    farCorner  -= pivotOffSet;
    nearCorner -= pivotOffSet;

    // Directional quad function(takes an offset and 2 directions)
    //BuildQuadV2(meshBuilder, nearCorner, forwardDir, rightDir);
    BuildQuadV2(meshBuilder, nearCorner, rightDir, upDir);
    BuildQuadV2(meshBuilder, nearCorner, upDir, forwardDir);

    //BuildQuadV2(meshBuilder, farCorner, -rightDir, -forwardDir);
    BuildQuadV2(meshBuilder, farCorner, -upDir, -rightDir);
    BuildQuadV2(meshBuilder, farCorner, -forwardDir, -upDir);

    // Roof peak
    Vector3 roofPeak = Vector3.up * (m_Heigth + m_RoofHeight) + rightDir * .5f - pivotOffSet;

    Vector3 wallTopLeft = upDir - pivotOffSet;
    Vector3 wallTopRight = upDir + rightDir - pivotOffSet;

    // Second quad reversed
    BuildTriangle(meshBuilder, wallTopLeft, roofPeak, wallTopRight);
    BuildTriangle(meshBuilder, wallTopLeft + forwardDir, wallTopRight + forwardDir, roofPeak + forwardDir);

    // Roof cover
    // Calculate direction of roof sides
    Vector3 dirFromPeakLeft  = wallTopLeft  - roofPeak;
    Vector3 dirFromPeakRight = wallTopRight - roofPeak;

    // Add amount from m_RoofOverhangSide to extend roof cover to sides
    dirFromPeakLeft  += dirFromPeakLeft.normalized  * m_RoofOverhangSide;
    dirFromPeakRight += dirFromPeakRight.normalized * m_RoofOverhangSide;

    // and we extends roof some along length
    roofPeak -= Vector3.forward * m_RoofOverhangFront;
    forwardDir += Vector3.forward * m_RoofOverhangFront * 2.0f;

    // Small off to reduce z-fighting
    roofPeak += Vector3.up * m_RoofBias;

    // Second quad reversed
    BuildQuadV2(meshBuilder, roofPeak, forwardDir, dirFromPeakLeft);
    BuildQuadV2(meshBuilder, roofPeak, dirFromPeakRight, forwardDir);
    // Flipped quads for hide culling - doubled quads cheat
    BuildQuadV2(meshBuilder, roofPeak, dirFromPeakLeft, forwardDir);
    BuildQuadV2(meshBuilder, roofPeak, forwardDir, dirFromPeakRight);

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

  void BuildTriangle(MeshBuilder meshBuilder, Vector3 corner0, Vector3 corner1, Vector3 corner2)
  {
    Vector3 normal = Vector3.Cross((corner1 - corner0), (corner2 - corner0)).normalized;

    meshBuilder.Vertices.Add(corner0);
    meshBuilder.UVs.Add(new Vector2(0f, 0f));
    meshBuilder.Normals.Add(normal);

    meshBuilder.Vertices.Add(corner1);
    meshBuilder.UVs.Add(new Vector2(0f, 1f));
    meshBuilder.Normals.Add(normal);

    meshBuilder.Vertices.Add(corner2);
    meshBuilder.UVs.Add(new Vector2(1f, 1f));
    meshBuilder.Normals.Add(normal);

    int baseIndex = meshBuilder.Vertices.Count - 3;

    meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
  }

}
