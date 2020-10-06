using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class PrefabCreator : EditorWindow
{
    private Color _newColorMech;
    private Color _newColorTower;
    private Color _newColorDissolve;

    private float _health;
    private float _damage;
    private float _speed;
    private float _dissolve;
    private string _tag = "";
    private float _attackRadius;

    private SerializedObject _mech;
    private SerializedObject _tower;
    private GameObject _objMech;
    private GameObject _objTower;

    private Editor mechEditor;
    private Editor towerEditor;

    private bool _showMech = true;
    private bool _showTower = true;
    private string _statusMech = "Select a Mech";
    private string _statusTower = "Select a Tower";

    [MenuItem("Window/Prefab Creator")]
    static void Init()
    {
        PrefabCreator window = (PrefabCreator)EditorWindow.GetWindow(typeof(PrefabCreator));
        window.Show();
    }

    void OnGUI()
    {
        mechEditor = Editor.CreateEditor(_objMech);
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        _objMech = (GameObject)EditorGUILayout.ObjectField("Mech", _objMech, typeof(GameObject));

        if (EditorGUI.EndChangeCheck())
        {
            DestroyImmediate(mechEditor);
        }

        if (_objMech != null)
        {
            _mech = new SerializedObject(_objMech);
        }

        _showMech = EditorGUILayout.Foldout(_showMech, _statusMech);

        if (_showMech)
        {
            if (_mech != null)
            {
                EditorGUILayout.BeginHorizontal();
                _newColorMech = EditorGUILayout.ColorField("Mech Color", _newColorMech);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                _newColorDissolve = EditorGUILayout.ColorField("Dissolve Color", _newColorDissolve);
                EditorGUILayout.Slider(_dissolve, 0, 1);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                //_health = EditorGUILayout.FloatField("Health", _mech.FindProperty("_maxHealth").floatValue);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                //_damage = EditorGUILayout.FloatField("Damage", _mech.FindProperty("_damageAmount").floatValue);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                //_speed = EditorGUILayout.FloatField("Speed", _mech.FindProperty("Speed").floatValue);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                _tag = EditorGUILayout.TagField("Set Tag", _tag);
                EditorGUILayout.EndHorizontal();

                _mech.ApplyModifiedProperties();

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Apply to Prefab"))
                    _mech.Update();
                if (GUILayout.Button("Create New Prefab"))
                    CreatePrefab(_mech);

                _statusMech = _objMech.name;
            }
        }

        if (_objMech = null)
        {
            _showMech = false;
            _statusMech = "Select a Mech";
        }
      

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        towerEditor = Editor.CreateEditor(_objTower);

        EditorGUI.BeginChangeCheck();
        _objTower = (GameObject)EditorGUILayout.ObjectField("Tower", _objTower, typeof(GameObject));

        if (EditorGUI.EndChangeCheck())
        {
            DestroyImmediate(towerEditor);
        }

        if (_objTower != null)
        {
            _tower = new SerializedObject(_objTower);
        }

        _showTower = EditorGUILayout.Foldout(_showTower, _statusTower);
        if (_showTower)
        {
            if (_tower != null)
            {
                EditorGUILayout.BeginHorizontal();
                _newColorMech = EditorGUILayout.ColorField("Tower Color", _newColorTower);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                //_health = EditorGUILayout.FloatField("Health", _tower.FindProperty("_maxHealth").floatValue);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                //_damage = EditorGUILayout.FloatField("Damage", _tower.FindProperty("_damageAmount").floatValue);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                _tag = EditorGUILayout.TagField("Set Tag", _tag);
                EditorGUILayout.EndHorizontal();

                _mech.ApplyModifiedProperties();

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Apply to Prefab"))
                    _mech.Update();
                if (GUILayout.Button("Create New Prefab"))
                    CreatePrefab(_tower);

                _statusTower = _objTower.name;
            }
        }
       
        if (_objTower = null)
        {
            _showTower = false;
            _statusTower = "Select a Tower";
        }
    }

    private void CreatePrefab(SerializedObject objSer)
    {
        GameObject obj = (GameObject)objSer.targetObject;
        string path = "Assets/" + obj.name + ".prefab";
        path = AssetDatabase.GenerateUniqueAssetPath(path);
        PrefabUtility.SaveAsPrefabAssetAndConnect(obj, path, InteractionMode.UserAction);
    }
}
#endif