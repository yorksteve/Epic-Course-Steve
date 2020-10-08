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
    private Texture _mech1Tex;
    private Texture _mech2Tex;
    private GameObject _mech1;
    private GameObject _mech2;
    private List<GameObject> _mechList;
    private SerializedObject _mechEditor;
    private Rect _windowRect = new Rect(20, 20, 120, 50);

    private bool _addingWave;
    private bool _addingMech;
    private bool _dataLoaded;
    private bool _windowOpened;

    [MenuItem("Window/Wave Customization")]
    static void Init()
    {
        WaveCustomization window = (WaveCustomization)EditorWindow.GetWindow(typeof(WaveCustomization));
        window.Show();
    }

    private void OnEnable()
    {
        _mechEditor = new SerializedObject(this);
        _mech1 = GameObject.Find("Mech1");
        _mech2 = GameObject.Find("Mech2");
        _mech1Tex = Resources.Load<Texture>("Prefab Images/Mech1Pic.png");
        _mech2Tex = Resources.Load<Texture>("Prefab Images/Mech2Pic.png");
        if (_mech2Tex == null)
        {
            Debug.Log("Picture not found");
        }
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
                var list = _mechEditor.FindProperty("_waveMechs");
                _addingMech = true;
                //BeginWindows();
                //GUILayout.Window(0, _windowRect, ListCustomization, "List Customization");
                //EndWindows();
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
                _addingWave = true;
                //BeginWindows();
                //GUILayout.Window(0, _windowRect, ListCustomization, "List Customization");
                //EndWindows();
            }

            // Insert Wave
            if (GUILayout.Button("Insert Wave"))
                InsertWave();
        }


        if (_addingMech == true || _addingWave == true)
        {
            // Display list from WaveCustomization
            if (_windowOpened == false)
            {
                WaveCustomization list = EditorWindow.CreateWindow<WaveCustomization>("List Customization");
                list.Show();
                _windowOpened = true;
            }

            
            //for (EditorWindow.titleContent("List Customization"))
            //{

            //}


            EditorGUILayout.BeginVertical();
            EditorGUILayout.PrefixLabel(new GUIContent(_mech1Tex));
            if (GUILayout.Button("Add"))
            {
                _mechList.Add(_mech1);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.PrefixLabel(new GUIContent(_mech2Tex));
            if (GUILayout.Button("Add"))
            {
                _mechList.Add(_mech2);
            }
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Apply"))
            {
                if (_addingMech == true)
                {
                    AddedMechs(_waveNumber, _mechList);
                }
                else
                {
                    NewWave();
                }
            }

            if (GUILayout.Button("Close"))
            {
                //list.Close();
                _addingMech = false;
                _addingWave = false;
                _windowOpened = false;
            }
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
        _numberOfMechs++;
        _mechList = SpawnManager.Instance.RequestSequence(waveNumber);
        _mechList.Add(null);
    }

    private void AddedMechs(int waveNumber, List<GameObject> mechs)
    {
        SpawnManager.Instance.AddedMechs(waveNumber, mechs);
    }
}
#endif


