using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class PrefabCreator : EditorWindow
{
    private bool _foldoutBool = true;
    private Color _newColorMech;
    private Color _newColorDissolve;
    private float _health;
    private float _damage;
    private float _speed;
    private string _tag = "";
    private float _attackRadius;
    private Object _rotationPoint;

    [MenuItem("Window/Prefab Creator")]
    static void Init()
    {
        PrefabCreator window = (PrefabCreator)EditorWindow.GetWindow(typeof(PrefabCreator));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        // Ability to create and alter mechs and towers

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutBool, "Mech", null);
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

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Changes"))
                UpdatePrefab();
            if (GUILayout.Button("Create New Prefab"))
                CreatePrefab();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
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
            if (GUILayout.Button("Assign"))
                //Assign the rotation point
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

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Changes"))
                UpdatePrefab();
            if (GUILayout.Button("Create New Prefab"))
                CreatePrefab();
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUI.EndChangeCheck();
    }

    private void ChangeColor()
    {

    }

    private void ChangeHealth()
    {
        Debug.Log("ChangeHealth()");
    }

    private void ChangeDamage()
    {

    }

    private void ChangeSpeed()
    {

    }

    private void CreatePrefab()
    {

    }

    private void UpdatePrefab()
    {

    }

}
#endif