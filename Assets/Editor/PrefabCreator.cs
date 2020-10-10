using UnityEngine;
using UnityEditor;
using Scripts.Managers;
using UnityEngine.AI;
using Scripts.Interfaces;
using Scripts;

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
    private string _name;
    private float _attackRadius;

    private TowerManager _towerEditor;
    [SerializeField] private GameObject _objMech;
    [SerializeField] private GameObject _objTower;

    private bool _showMech = false;
    private bool _showTower = false;


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

        _objMech = (GameObject)EditorGUILayout.ObjectField("Mech", _objMech, typeof(GameObject));

        if (GUILayout.Button("Load Mech Data") && _objMech != null)
        {
            _showMech = true;
            _showTower = false;
            _health = _objMech.GetComponent<Scripts.EnemyAI>().EditorGetHealth();
            _damage = _objMech.GetComponent<Scripts.EnemyAI>().Damage();
            _speed = _objMech.GetComponent<NavMeshAgent>().speed;
            _tag = _objMech.tag;
            _name = _objMech.name;
            //var rends = _objMech.GetComponentsInChildren<Renderer>();
            //foreach (var rend in rends)
            //{
            //    _newColorMech = rend.materials[0].color;
            //}
        }

        if (_showMech == true)
        {
            //EditorGUILayout.BeginHorizontal();
            //_newColorMech = EditorGUILayout.ColorField("Mech Color", _newColorMech);
            //EditorGUILayout.EndHorizontal();

            //EditorGUILayout.BeginHorizontal();
            //_newColorDissolve = EditorGUILayout.ColorField("Dissolve Color", _newColorDissolve);
            //EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _name = EditorGUILayout.TextField("Prefab Name", _name);
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

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply to Prefab"))
            {
                ApplyToMechPrefab();
            }
            if (GUILayout.Button("Create New Prefab"))
                CreateMechPrefab();
            if (GUILayout.Button("Back"))
            {
                _showMech = false;
                _objMech = null;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(10);

        _objTower = (GameObject)EditorGUILayout.ObjectField("Tower", _objTower, typeof(GameObject));

        if (GUILayout.Button("Load Tower Data") && _objTower != null)
        {
            _showTower = true;
            _showMech = false;
            //var rends = _objTower.GetComponentsInChildren<Renderer>();
            //foreach (var rend in rends)
            //{
            //    _newColorTower = rend.sharedMaterial.color;
            //}
            
            var editor = TowerManager.Instance;
            _health = editor.EditorGetHealth(_objTower);
            _damage = editor.EditorGetDamage(_objTower);
            _tag = _objTower.tag;
            _name = _objTower.name;
        }

        if (_showTower == true)
        {
            //EditorGUILayout.BeginHorizontal();
            //_newColorTower = EditorGUILayout.ColorField("Tower Color", _newColorTower);
            //EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _name = EditorGUILayout.TextField("Prefab Name", _name);
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

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply to Prefab"))
            {
                ApplyToTowerPrefab();
            }
            if (GUILayout.Button("Create New Prefab"))
                CreateTowerPrefab();
            if (GUILayout.Button("Back"))
            {
                _showTower = false;
                _objTower = null;
                Undo.PerformUndo();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void CreateMechPrefab()
    {
        string path = "Assets/Prefabs/Mechs" + _objMech.name + ".prefab";
        path = AssetDatabase.GenerateUniqueAssetPath(path);
        PrefabUtility.SaveAsPrefabAssetAndConnect(_objMech, path, InteractionMode.UserAction);
    }

    private void CreateTowerPrefab()
    {
        string path = "Assets/Prefabs/Towers" + _objTower.name + ".prefab";
        path = AssetDatabase.GenerateUniqueAssetPath(path);
        PrefabUtility.SaveAsPrefabAssetAndConnect(_objTower, path, InteractionMode.UserAction);
    }

    private void ApplyToMechPrefab()
    {
        var script = _objMech.GetComponent<Scripts.EnemyAI>();
        var agent = _objMech.GetComponent<NavMeshAgent>();
        script.ChangeDamageEditor(_damage);
        script.ChangeHealthEditor(_health);
        _objMech.tag = _tag;
        agent.speed = _speed;
        //var rends = _objMech.GetComponentsInChildren<Renderer>();
        //foreach (var rend in rends)
        //{
        //    rend.sharedMaterial.color = _newColorMech;
        //}
    }

    private void ApplyToTowerPrefab()
    {
        //var rends = _objTower.GetComponentsInChildren<Renderer>();
        //foreach (var rend in rends)
        //{
        //    rend.sharedMaterial.color = _newColorTower;
        //}
        _objTower.tag = _tag;
        TowerManager.Instance.EditorSetHealth(_objTower, _health);
        TowerManager.Instance.EditorSetDamage(_objTower, _damage);
    }
}
#endif