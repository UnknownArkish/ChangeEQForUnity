using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// DNA影响的类型
public enum DNAType
{
    Position,
    Rotation,
    Scale,
}

public class TestDNA : MonoBehaviour
{
    public DNAType DNAType;
    public string DNABoneName;
    public Vector3 DNA;

    public Matrix4x4[] OriginBindposes { get; private set; }

    private void Start()
    {
        SkinnedMeshRenderer renderer = GetComponent<SkinnedMeshRenderer>();
        Mesh mesh = renderer.sharedMesh;

        OriginBindposes = new Matrix4x4[mesh.bindposes.Length];
        for( int i = 0; i < mesh.bindposes.Length; i++)
        {
            OriginBindposes[i] = mesh.bindposes[i];
        }
    }
}
