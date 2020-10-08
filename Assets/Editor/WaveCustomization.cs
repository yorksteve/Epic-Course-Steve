using UnityEngine;
using UnityEditor;
using Scripts.Managers;
using System.Collections.Generic;
using Scripts.ScriptableObjects;


#if UNITY_EDITOR
public class WaveCustomization : EditorWindow
{
    private int _waveNumber;
    private int _spawnDelay;
    private int _waveDuration;
    private int _numberOfMechs;
    [SerializeField] private GameObject[] _mechs;

 
    private List<GameObject> _mechList;
    private SerializedObject _mechEditor;

    private bool _addingWave;
    private bool _addingMech;
    private bool _dataLoaded;

    [MenuItem("Window/Wave Customization")]
    static void Init()
    {
        WaveCustomization window = (WaveCustomization)EditorWindow.GetWindow(typeof(WaveCustomization));
        window.Show();
    }

    private void OnEnable()
    {
        _mechEditor = new SerializedObject(this);

    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        _waveNumber = EditorGUILayout.IntField("Wave ", _waveNumber);
        if (GUILayout.Button("Load Data"))
        {
            // Get current wave info
            _spawnDelay = SpawnManager.Instance.RequestSpawnDelay(_waveNumber);
            _waveDuration = SpawnManager.Instance.RequestWaveDuration(_waveNumber);
            _numberOfMechs = SpawnManager.Instance.RequestAmountOfMechs(_waveNumber);
            _mechs = SpawnManager.Instance.RequestMechs(_waveNumber);

            _dataLoaded = true;
        }

        if (_dataLoaded == true)
        {
            // Display info and allow changes
            _spawnDelay = (int)EditorGUILayout.IntField("Spawn Delay", _spawnDelay);
            _waveDuration = (int)EditorGUILayout.IntField("Wave Duration", _waveDuration);
            _numberOfMechs = (int)EditorGUILayout.IntField("Amount Of Mechs in Wave", _numberOfMechs);

            // Display current prefabs in wave
            var property = _mechEditor.FindProperty("_mechs");
            EditorGUILayout.PropertyField(property, true);
            _mechEditor.ApplyModifiedProperties();

            // Add a new prefab to current wave
            if (GUILayout.Button("Add New Mech"))
            {
                AddMech(_waveNumber);
                ListCustomization listEditor = EditorWindow.GetWindow<ListCustomization>("List Customization");
                listEditor.Show();
            }

            // Load the selected wave
            if (GUILayout.Button("Load"))
                LoadWave();

            // Update the selected wave with above changes
            if (GUILayout.Button("Update Wave"))
                UpdateWave();

            // Create a new wave with the above changes
            if (GUILayout.Button("Create New"))
            {
                ListCustomization listEditor = EditorWindow.GetWindow<ListCustomization>("List Customization");
                listEditor.Show();
            }

            // Insert Wave
            if (GUILayout.Button("Insert Wave"))
                InsertWave();
        }
    }

    private void LoadWave()
    {
        SpawnManager.Instance.LoadLevel(_waveNumber);
    }

    private void UpdateWave()
    {
        SpawnManager.Instance.UpdateWaveSystem(_waveNumber, _spawnDelay, _waveDuration, _numberOfMechs);
    }

    private void NewWave()
    {
        SpawnManager.Instance.NewWave(_waveNumber, _spawnDelay, _waveDuration, _mechList);
    }

    private void InsertWave()
    {
        SpawnManager.Instance.InsertWave(_waveNumber);
        NewWave();
    }

    private void AddMech(int waveNumber)
    {
        SpawnManager.Instance.RequestSequence(waveNumber);
    }

    private void AddedMechs(int waveNumber, List<GameObject> mechs)
    {
        SpawnManager.Instance.AddedMechs(waveNumber, mechs);
    }
}
#endif


