using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    [SerializeField] Mesh mesh;
    [SerializeField] Mesh mesh01;
    [SerializeField] Mesh mesh02;

    void Start()
    {
      
        mesh.SetIndices(mesh.GetIndices(0), MeshTopology.Lines, 0);
        mesh01.SetIndices(mesh01.GetIndices(0), MeshTopology.Lines, 0);
        mesh02.SetIndices(mesh02.GetIndices(0), MeshTopology.Lines, 0);

    }
}