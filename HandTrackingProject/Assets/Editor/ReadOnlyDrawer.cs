using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // Disable GUI interaction for read-only fields
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true; // Re-enable GUI interaction
    }
}