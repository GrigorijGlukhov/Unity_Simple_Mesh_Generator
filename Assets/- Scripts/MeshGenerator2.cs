using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator2 : MonoBehaviour
{
  public int width = 1;
  public int height = 1;
  public int xCount = 10;
  public int yCount = 10;


  // Start is called before the first frame update
  void Start()
  {
    MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

    Mesh mesh = new Mesh();
    for ( int i = 0; i < xCount; i++ )
    {
    Vector3[] vertices = new Vector3[4]
    {
      new Vector3(i, 0, 0),
      new Vector3(i+width, 0, 0),
      new Vector3(i, height, 0),
      new Vector3(i+width, height, 0)
    };
    mesh.vertices = vertices;

    int[] triangles = new int[6]
    {
      // lower left triangle
      0, 2, 1,
      // upper right triangle
      2, 3, 1
    };
    mesh.triangles = triangles;

    Vector3[] normals = new Vector3[4]
    {
      -Vector3.forward,
      -Vector3.forward,
      -Vector3.forward,
      -Vector3.forward
    };
    mesh.normals = normals;

    Vector2[] uv = new Vector2[4]
    {
      new Vector2(0, 0),
      new Vector2(1, 0),
      new Vector2(0, 1),
      new Vector2(1, 1)

    };
    mesh.uv = uv;
    };

    meshFilter.mesh = mesh;
    
  }

}
