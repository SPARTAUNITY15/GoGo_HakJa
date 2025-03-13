using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (!EditorApplication.isPlaying)
        {
            DrawDefaultInspector();
            return;
        }

        MeshGenerator mapGen = (MeshGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.CreateShape();
                mapGen.UpdateMesh();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.CreateShape();
            mapGen.UpdateMesh();
        }
    }
}
