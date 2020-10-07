using UnityEngine;
using UnityEditor;
using Scripts.Managers;
using UnityEngine.AI;

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
    private string _tag;
    private float _attackRadius;

    private SerializedObject _mechEditor;
    private SerializedObject _towerEditor;
    private GameObject _objMech;
    private GameObject _objTower;

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

    private void OnEnable()
    {
        _towerEditor = new SerializedObject(TowerManager.Instance);
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        _objMech = (GameObject)EditorGUILayout.ObjectField("Mech", _objMech, typeof(GameObject));

        _mechEditor = new SerializedObject(_objMech.GetComponent<EnemyAI>());

        if (GUILayout.Button("Load Mech Data"))
        {
            EditorGUILayout.BeginHorizontal();
            _newColorMech = EditorGUILayout.ColorField("Mech Color", _newColorMech);
            var rends = _objMech.GetComponentsInChildren<Renderer>();
            foreach (var rend in rends)
            {
                rend.material.color = _newColorMech;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _newColorDissolve = EditorGUILayout.ColorField("Dissolve Color", _newColorDissolve);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _health = (float)EditorGUILayout.FloatField("Health", _health = _mechEditor.FindProperty("_maxHealth").floatValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _damage = EditorGUILayout.FloatField("Damage", _mechEditor.FindProperty("_damageAmount").floatValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _speed = EditorGUILayout.FloatField("Speed", _objMech.GetComponent<NavMeshAgent>().speed);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _tag = EditorGUILayout.TagField("Set Tag", _tag);
            EditorGUILayout.EndHorizontal();

            _mechEditor.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply to Prefab"))
                _mechEditor.Update();
            if (GUILayout.Button("Create New Prefab"))
                CreatePrefab(_objMech);
        }

        EditorGUILayout.Space(10);

        _objTower = (GameObject)EditorGUILayout.ObjectField("Tower", _objTower, typeof(GameObject));

        if (GUILayout.Button("Load Tower Data"))
        {

            EditorGUILayout.BeginHorizontal();
            _newColorTower = EditorGUILayout.ColorField("Tower Color", _newColorTower);
            var rends = _objTower.GetComponentsInChildren<Renderer>();
            foreach (var rend in rends)
            {
                rend.material.color = _newColorTower;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            //_health = EditorGUILayout.FloatField("Health", _towerEditor.FindProperty("_maxHealth").floatValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            //_damage = EditorGUILayout.FloatField("Damage", _tower.FindProperty("_damageAmount").floatValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _tag = EditorGUILayout.TagField("Set Tag", _tag);
            EditorGUILayout.EndHorizontal();

            _towerEditor.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply to Prefab"))
                _towerEditor.Update();
            if (GUILayout.Button("Create New Prefab"))
                CreatePrefab(_objTower);
        }
    }

    private void CreatePrefab(GameObject obj)
    {
        string path = "Assets/" + obj.name + ".prefab";
        path = AssetDatabase.GenerateUniqueAssetPath(path);
        PrefabUtility.SaveAsPrefabAssetAndConnect(obj, path, InteractionMode.UserAction);
    }
}
#endif