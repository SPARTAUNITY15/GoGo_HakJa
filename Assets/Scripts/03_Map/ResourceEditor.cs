using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Resources), true)]
public class ResourceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("����"))
        {
            ((Resources)target).ReceiveImpact(1);
        }
    }
}
