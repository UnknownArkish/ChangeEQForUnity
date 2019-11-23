using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestDNA))]
public class TestDNAEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TestDNA testDNA = target as TestDNA;

        if (GUILayout.Button("应用DNA"))
        {
            ApplyDNA(testDNA);
        }
    }

    private void ApplyDNA(TestDNA testDNA)
    {
        SkinnedMeshRenderer renderer = testDNA.GetComponent<SkinnedMeshRenderer>();
        Transform[] boneTransforms = renderer.bones;
        Transform transform = renderer.transform;


        string boneName = testDNA.DNABoneName;
        Vector3 dna = testDNA.DNA;
        DNAType dnaType = testDNA.DNAType;


        int boneIndex = FindBoneIndex(boneTransforms, boneName);
        if (boneIndex == -1)
        {
            Debug.LogFormat("骨骼名字{0}不存在",boneName);
            return;
        }

        Mesh mesh = renderer.sharedMesh;
        Matrix4x4[] bindposes = mesh.bindposes;

        // 在原来的bindposes上，再乘以dna的结果
        Matrix4x4 dnaMatrix = MakeDNAMatrix(dna, dnaType);
        bindposes[boneIndex] = dnaMatrix * testDNA.OriginBindposes[boneIndex];

        mesh.bindposes = bindposes;
    }

    private int FindBoneIndex(Transform[] srcs, string boneName)
    {
        int result = -1;
        for (int i = 0; i < srcs.Length; i++)
        {
            if (srcs[i].name == boneName)
            {
                result = i;
                break;
            }
        }
        return result;
    }
    private Matrix4x4 MakeDNAMatrix(Vector3 dna, DNAType dnaType)
    {
        switch (dnaType)
        {
            case DNAType.Position:
                return Matrix4x4.Translate(dna);
                break;
            case DNAType.Rotation:
                return Matrix4x4.Rotate(Quaternion.Euler(dna.x, dna.y, dna.z));
                break;
            case DNAType.Scale:
                return Matrix4x4.Scale(dna);
                break;
        }
        return Matrix4x4.identity;
    }
}
