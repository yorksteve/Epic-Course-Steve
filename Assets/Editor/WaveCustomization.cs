using UnityEngine;
using UnityEditor;
using Scripts.Managers;
using System.Collections.Generic;
using Scripts.ScriptableObjects;
using System;


#if UNITY_EDITOR
public class WaveCustomization : EditorWindow
{
    private int _waveNumber;
    private int _spawnDelay;
    private int _waveDuration;
    private int _numberOfMechs;
    [SerializeField] private GameObject[] _mechs;

    private List<GameObject> _mechList;

    private bool _addingWave;
    private bool _addingMech;
    private bool _dataLoaded;

    [MenuItem("Tools/Wave Customization")]
    static void Init()
    {
        WaveCustomization window = (WaveCustomization)EditorWindow.GetWindow(typeof(WaveCustomization));
        window.Show();
    }

    private void OnEnable()
    {
        EventManager.Listen("onCreateNewWave", (Action<List<GameObject>>)NewWave);
        EventManager.Listen("onInsertWave", (Action<List<GameObject>>)InsertWave);
        EventManager.Listen("onAddedMech", (Action<List<GameObject>>)AddedMechs);
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
            EditorGUILayout.Foldout(true, "Mechs in Wave");
            for (int i = 0; i < _mechs.Length; i++)
            {
                _mechs[i] = (GameObject)EditorGUILayout.ObjectField(_mechs[i], typeof(GameObject));
            }

            // Add a new prefab to current wave
            if (GUILayout.Button("Add New Mech"))
            {
                AddMech(_waveNumber);
                ListCustomization listEditor = EditorWindow.GetWindow<ListCustomization>("List Customization");
                listEditor.position = new Rect(600, 200, 600, 600);
                listEditor.Show();
                EventManager.Fire("onAddMechBool");
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
                listEditor.position = new Rect(600, 200, 600, 600);
                listEditor.Show();
                EventManager.Fire("onCreateNewBool");
            }

            // Insert Wave
            if (GUILayout.Button("Insert Wave"))
            {
                ListCustomization listEditor = EditorWindow.GetWindow<ListCustomization>("List Customization");
                listEditor.position = new Rect(600, 200, 600, 600);
                listEditor.Show();
                EventManager.Fire("onInsertWaveBool");
            }

            // Close Window
            if (GUILayout.Button("Close"))
                this.Close();
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

    private void NewWave(List<GameObject> mechList)
    {
        SpawnManager.Instance.NewWave(_waveNumber, _spawnDelay, _waveDuration, mechList);
    }

    private void InsertWave(List<GameObject> mechList)
    {
        SpawnManager.Instance.InsertWave(_waveNumber);
        NewWave(mechList);
    }

    private void AddMech(int waveNumber)
    {
        SpawnManager.Instance.RequestSequence(waveNumber);
    }

    private void AddedMechs(List<GameObject> mechs)
    {
        SpawnManager.Instance.AddedMechs(_waveNumber, mechs);
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent("onCreateNewWave", (Action<List<GameObject>>)NewWave);
        EventManager.UnsubscribeEvent("onInsertWave", (Action<List<GameObject>>)InsertWave);
        EventManager.UnsubscribeEvent("onAddedMech", (Action<List<GameObject>>)AddedMechs);
    }
}
#endif


