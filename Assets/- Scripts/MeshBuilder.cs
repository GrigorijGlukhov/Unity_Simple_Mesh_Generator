using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder 
{
  private List <Vector3> m_Vertices = new List<Vector3>();
  public List <Vector3> Vertices {get { return m_Vertices; }}

  private List <int> m_Triangles = new List<int>();
  public List <int> Triangles {get { return m_Triangles; }}

  private List <Vector3> m_Normals = new List<Vector3>();
  public List <Vector3> Normals {get { return m_Normals; }}

  private List <Vector2> m_UVs = new List<Vector2>();
  public List <Vector2> UVs {get { return m_UVs; }}

  public void AddTriangle(int _index0, int _index1, int _index2)
  {
    m_Triangles.Add(_index0);
    m_Triangles.Add(_index1);
    m_Triangles.Add(_index2);
  }

  public Mesh CreateMesh()
  {
    Mesh mesh = new Mesh();
    
    mesh.vertices = m_Vertices.ToArray();
    mesh.triangles = m_Triangles.ToArray();
    if (m_Normals.Count == m_Vertices.Count)
      mesh.normals = m_Normals.ToArray();
    if (m_UVs.Count == m_Vertices.Count)
      mesh.uv = m_UVs.ToArray();

    mesh.RecalculateBounds();

    return mesh;
  }
}
