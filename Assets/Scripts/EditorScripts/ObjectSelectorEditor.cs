using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ObjectSelectorScript))]
public class ObjectSelectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectSelectorScript myScript = (ObjectSelectorScript)target;
        if (GUILayout.Button("Select Objects"))
        {
            myScript.SelectObjects();
        }
    }
}