/*
 * Copyright (C) Grigorij Glukhov. 2021. All rights reserved
 * Class containing mesh data: ver, tri, nor, uvs
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderProc : MonoBehaviour
{
  public int m_RadialSegmentCount = 3;
  public int m_HeightSegmentCount = 6;
  public float m_Radius = 1;
  public float m_Height = 1;


  void Start() {

    MeshBuilder meshBuilder = new MeshBuilder();

    float heightInc = m_Height / m_HeightSegmentCount;
    
    for ( int i = 0; i <= m_HeightSegmentCount; i++ )
    {
      Vector3 centerPos = Vector3.up * heightInc * i;
      float v = (float)i / m_HeightSegmentCount;
      BuildRing(meshBuilder, m_RadialSegmentCount, centerPos, m_Radius, v, i > 0);
    }

    BuildCap( meshBuilder, Vector3.zero, true );
    BuildCap( meshBuilder, Vector3.up * m_Height, false );

    // make mehs visible
    FinalizeMesh( meshBuilder );
    
//    BuildRing(meshBuilder, 
//              m_RadialSegmentCount, 
//              Vector3.zero,
//              m_Radius,
//              0.0f,
//              false);
//    BuildRing(meshBuilder, 
//              m_RadialSegmentCount, 
//              Vector3.up * m_Height,
//              m_Radius,
//              1.0f,
//              true);
//
  }

  // make sides of cylinder
  void BuildRing ( MeshBuilder meshBuilder,
                   int segmentCount,
                   Vector3 centre,
                   float radius,
                   float v,
                   bool buildTriangles ) 
  {
    // Angle based on segments in ring
    float angleInc = (Mathf.PI * 2.0f) / segmentCount;

    for ( int i = 0; i <= segmentCount; i++ ) 
    {
      // Next angle 
      float angle = angleInc * i;   

      // calculate next position 
      Vector3 unitPosition = Vector3.zero;
      unitPosition.x = Mathf.Cos(angle);
      unitPosition.z = Mathf.Sin(angle);

      // Add vertex at position multiplied by radius
      meshBuilder.Vertices.Add(centre + unitPosition * radius);
      meshBuilder.Normals.Add(unitPosition);

      meshBuilder.UVs.Add(new Vector2( (float)i / segmentCount, v) );

      if ( i > 0 && buildTriangles )
      {
        int baseIndex = meshBuilder.Vertices.Count - 1;

        int vertsPerRow = segmentCount + 1;

        int index0 = baseIndex;
        int index1 = baseIndex - 1;
        int index2 = baseIndex - vertsPerRow;
        int index3 = baseIndex - vertsPerRow - 1;

        meshBuilder.AddTriangle(index0, index2, index1);
        meshBuilder.AddTriangle(index2, index3, index1);

      }
    }

  }

  // build cap for cylinder
  void BuildCap(MeshBuilder meshBuilder,
                Vector3 center,
                bool reverseDirection)
  {
    // Upper or bottom cap
    Vector3 normal = reverseDirection ? Vector3.down : Vector3.up;

    // place one vertex in center
    meshBuilder.Vertices.Add(center);
    meshBuilder.Normals.Add(normal);
    meshBuilder.UVs.Add(new Vector2( 0.5f, 0.5f ));

    int centerVertexIndex = meshBuilder.Vertices.Count - 1;

    // One angle unit for vertex around the edge:
    float angleInc = (Mathf.PI * 2.0f) / m_RadialSegmentCount;

    for ( int i = 0; i <= m_RadialSegmentCount; i++ )
    {
      // current angle
      float angle = angleInc * i;

      // calculate next position 
      Vector3 unitPosition = Vector3.zero;
      unitPosition.x = Mathf.Cos(angle);
      unitPosition.z = Mathf.Sin(angle);

      meshBuilder.Vertices.Add( center + unitPosition * m_Radius );
      meshBuilder.Normals.Add( normal );

      Vector2 uv = new Vector2( unitPosition.x + 1.0f, unitPosition.z + 1.0f ) * 0.5f;
      meshBuilder.UVs.Add( uv );

      // build a triangle
      if ( i > 0 )
      {
        int baseIndex = meshBuilder.Vertices.Count - 1;

        if ( reverseDirection )
          meshBuilder.AddTriangle( centerVertexIndex, baseIndex - 1, baseIndex );
        else
          meshBuilder.AddTriangle( centerVertexIndex, baseIndex, baseIndex - 1 );
      }
    }
  }

  void FinalizeMesh(MeshBuilder meshBuilder)
  {

    // Get meshFilter we working with. Create mesh in this object
    MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

    if(meshFilter != null)
    {
      // Finalize mesh and its creation
      meshFilter.mesh = meshBuilder.CreateMesh();
      // Temporary auto-normals generator Unity function
    }
  
  }

}
