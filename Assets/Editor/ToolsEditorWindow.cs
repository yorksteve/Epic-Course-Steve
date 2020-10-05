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
    private string _tag = "";
    private float _attackRadius;
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

        EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutBool, "Mech");
        if (_foldoutBool)
        {
            EditorGUILayout.BeginHorizontal();
            _newColorMech = EditorGUILayout.ColorField("Mech Color", _newColorMech);
            if (GUILayout.Button("Change"))
                ChangeColor();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _newColorDissolve = EditorGUILayout.ColorField("Dissolve Color", _newColorDissolve);
            if (GUILayout.Button("Change"))
                ChangeColor();
            // Add slider to test shader
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _health = EditorGUILayout.FloatField("Health", _health);
            if (GUILayout.Button("Update"))
                ChangeHealth();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _damage = EditorGUILayout.FloatField("Damage", _damage);
            if (GUILayout.Button("Update"))
                ChangeDamage();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _speed = EditorGUILayout.FloatField("Speed", _speed);
            if (GUILayout.Button("Update"))
                ChangeSpeed();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _rotationPoint = EditorGUILayout.ObjectField("Rotation Point", _rotationPoint, typeof(Transform), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _tag = EditorGUILayout.TagField("Set Tag", _tag);
            if (GUILayout.Button("Set Tag"))
            {
                
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutBool, "Tower");
        if (_foldoutBool)
        {
            EditorGUILayout.BeginHorizontal();
            _health = EditorGUILayout.FloatField("Health", _health);
            if (GUILayout.Button("Update"))
                ChangeHealth();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _damage = EditorGUILayout.FloatField("Damage", _damage);
            if (GUILayout.Button("Update"))
                ChangeDamage();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _rotationPoint = EditorGUILayout.ObjectField("Rotation Point", _rotationPoint, typeof(Transform), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _tag = EditorGUILayout.TagField("Set Tag", _tag);
            if (GUILayout.Button("Set Tag"))
            {

            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _attackRadius = EditorGUILayout.FloatField("Attack Radius", _attackRadius);
            EditorGUILayout.EndHorizontal();
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