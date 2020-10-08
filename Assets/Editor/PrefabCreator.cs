using UnityEngine;
using UnityEditor;
using Scripts.Managers;
using UnityEngine.AI;
using Scripts.Interfaces;

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
    [SerializeField] private GameObject _objMech;
    [SerializeField] private GameObject _objTower;

    private bool _showMech = false;
    private bool _showTower = false;
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

        if (_objMech != null)
        {
            //_mechEditor = _objMech.GetComponent<EnemyAI>();
        }

        if (GUILayout.Button("Load Mech Data"))
        {
            _showMech = true;
            _showTower = false;
            //_health = _objMech.GetComponent<EnemyAI>().
            //_damage = _objMech.GetComponent<EnemyAI>().Damag;
            _speed = _objMech.GetComponent<NavMeshAgent>().speed;
            _tag = _objMech.tag;
            var rends = _objMech.GetComponentsInChildren<Renderer>();
            foreach (var rend in rends)
            {
                rend.sharedMaterial.color = _newColorMech;
            }
        }

        if (_showMech == true)
        {
            EditorGUILayout.BeginHorizontal();
            _newColorMech = EditorGUILayout.ColorField("Mech Color", _newColorMech);
            var rends = _objMech.GetComponentsInChildren<Renderer>();
            foreach (var rend in rends)
            {
                rend.sharedMaterial.color = _newColorMech;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _newColorDissolve = EditorGUILayout.ColorField("Dissolve Color", _newColorDissolve);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _health = (float)EditorGUILayout.FloatField("Health", _health);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _damage = EditorGUILayout.FloatField("Damage", _damage);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _speed = EditorGUILayout.FloatField("Speed", _speed);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _tag = EditorGUILayout.TagField("Set Tag", _tag);
            EditorGUILayout.EndHorizontal();

            //_mechEditor.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply to Prefab"))
            {
                _mechEditor.ApplyModifiedProperties();
                //_mechEditor.Update();
            }
            if (GUILayout.Button("Create New Prefab"))
                CreatePrefab(_objMech);
            if (GUILayout.Button("Back"))
            {
                _showMech = false;
                _objMech = null;
                Undo.PerformUndo();
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(10);

        _objTower = (GameObject)EditorGUILayout.ObjectField("Tower", _objTower, typeof(GameObject));

        if (GUILayout.Button("Load Tower Data") && _objTower != null)
        {
            _showTower = true;
            _showMech = false;
            var rends = _objTower.GetComponentsInChildren<Renderer>();
            foreach (var rend in rends)
            {
                rend.sharedMaterial.color = _newColorTower;
            }
        }

        if (_showTower == true)
        {
            EditorGUILayout.BeginHorizontal();
            _newColorTower = EditorGUILayout.ColorField("Tower Color", _newColorTower);
            var rends = _objTower.GetComponentsInChildren<Renderer>();
            foreach (var rend in rends)
            {
                rend.sharedMaterial.color = _newColorTower;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _health = EditorGUILayout.FloatField("Health", _health);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _damage = EditorGUILayout.FloatField("Damage", _damage);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _tag = EditorGUILayout.TagField("Set Tag", _tag);
            EditorGUILayout.EndHorizontal();

            //_towerEditor.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply to Prefab"))
            {
                //_towerEditor.ApplyModifiedProperties();
                //_towerEditor.Update();
            }
            if (GUILayout.Button("Create New Prefab"))
                CreatePrefab(_objTower);
            if (GUILayout.Button("Back"))
            {
                _showTower = false;
                _objTower = null;
                Undo.PerformUndo();
            }
            EditorGUILayout.EndHorizontal();
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