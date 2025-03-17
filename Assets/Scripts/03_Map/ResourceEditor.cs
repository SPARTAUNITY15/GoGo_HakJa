using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Resources))]
public class ResourceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("½ÇÇà"))
        {
            ((Resources)target).ReceiveImpact(1);
        }
    }
}
