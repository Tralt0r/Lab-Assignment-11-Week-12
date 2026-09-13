using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyBehavior))]
public class EnemyBehaviorEditor : Editor
{
    public override void OnInspectorGUI()
    {   //Draw the default inspector
        DrawDefaultInspector();

        //Get a reference to the EnemyBehavior script
        EnemyBehavior enemy = (EnemyBehavior)target;

        //Display a warning message if the size of cubes or spheres is out of bounds
        if (!string.IsNullOrEmpty(enemy.sizeWarning))
            EditorGUILayout.HelpBox(enemy.sizeWarning, MessageType.Warning);
        //Buttons to generate shapes, select all cubes/spheres, clear selection, and toggle all cubes/spheres
        if (GUILayout.Button("Generate Shapes"))
            //Actual buttons tos call the methods
            enemy.GenerateShapes();

        if (GUILayout.Button("Select all cubes/spheres"))
            enemy.selectObjects();

        if (GUILayout.Button("Clear selection"))
            enemy.ClearSelection();

        bool enabled = enemy.AllEnabled();
        GUI.backgroundColor = enabled ? Color.green : Color.red;
        if (GUILayout.Button(enabled ? "Disable all cubes/spheres" : "Enable all cubes/spheres"))
            enemy.ToggleAll();
        GUI.backgroundColor = Color.white;
    }
}