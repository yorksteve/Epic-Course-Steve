using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class ToolsEditorWindow : EditorWindow
{
    string editorName = "Prefab Manipulator";
    private bool _foldoutBool = true;
    private Color _newColorMech;
    private Color _newColorDissolve;
    private float _health;
    private float _damage;
    private float _speed;
    private GUIStyle style;
    private Object _rotationPoint;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ToolsEditorWindow window = (ToolsEditorWindow)EditorWindow.GetWindow(typeof(ToolsEditorWindow));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        editorName = EditorGUILayout.TextField("Text Field", editorName);

        // Ability to create and alter mechs and towers

        EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutBool, "Mech", style = EditorStyles.foldoutHeader);
        if (_foldoutBool)
        {
            _newColorMech = EditorGUILayout.ColorField("Mech Color", _newColorMech);
            if (GUILayout.Button("Change"))
                ChangeColor();

            _newColorDissolve = EditorGUILayout.ColorField("Dissolve Color", _newColorDissolve);
            if (GUILayout.Button("Change", style = EditorStyles.miniButton))
                ChangeColor();
            // Add slider to test shader

            _health = EditorGUILayout.FloatField("Health", _health);
            if (GUILayout.Button("Update"))
                ChangeHealth();

            _damage = EditorGUILayout.FloatField("Damage", _damage);
            if (GUILayout.Button("Update"))
                ChangeDamage();

            _speed = EditorGUILayout.FloatField("Speed", _speed);
            if (GUILayout.Button("Update"))
                ChangeSpeed();

            _rotationPoint = EditorGUILayout.ObjectField("Rotation Point", _rotationPoint, typeof(Transform), true);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();

        EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutBool, "Tower");
        if (_foldoutBool)
        {
            _health = EditorGUILayout.FloatField("Health", _health);
            if (GUILayout.Button("Update"))
                ChangeHealth();

            _damage = EditorGUILayout.FloatField("Damage", _damage);
            if (GUILayout.Button("Update"))
                ChangeDamage();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void ChangeColor()
    {

    }

    private void ChangeHealth()
    {

    }

    private void ChangeDamage()
    {

    }

    private void ChangeSpeed()
    {

    }

}
#endif