using UnityEngine;
using UnityEditor;
using Scripts.Managers;


#if UNITY_EDITOR
public class WaveCustomization : EditorWindow
{
    private int _waveNumber;
    private int _spawnDelay;
    private int _waveDuration;
    private int _waveID;
    private int _numberOfMechs;
    private bool _waveSelected = true;
    private bool _mechsClicked = true;
    private string _waveStatus = "Select a Wave";
    private GameObject[] _mechs;
    private GameObject _mech;

    [MenuItem("Window/Wave Customization")]
    static void Init()
    {
        WaveCustomization window = (WaveCustomization)EditorWindow.GetWindow(typeof(WaveCustomization));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        _waveNumber = EditorGUILayout.IntField("Wave ", _waveNumber);
        if (GUILayout.Button("Load Data"))
        {
            _spawnDelay = SpawnManager.Instance.RequestSpawnDelay(_waveNumber);
            _waveDuration = SpawnManager.Instance.RequestWaveDuration(_waveNumber);
            //_mechs = 
            _waveID = SpawnManager.Instance.RequestWaveID(_waveNumber);


            _spawnDelay = (int)EditorGUILayout.IntField("Spawn Delay", _spawnDelay);
            _waveDuration = (int)EditorGUILayout.IntField("Wave Duration", _waveDuration);

            _numberOfMechs = (int)EditorGUILayout.IntField("Amount Of Mechs in Wave", _numberOfMechs);

            _mechsClicked = EditorGUILayout.Foldout(_mechsClicked, "Mechs");
            if (_mechsClicked)
            {
                _mech = (GameObject)EditorGUILayout.ObjectField("Mechs", _mech, typeof(GameObject));
                //for (int i = 0; i < _mechs.Length; i++)
                //{
                //    _mechs[i] = (GameObject)EditorGUILayout.ObjectField("Mech", _mechs[i], typeof(GameObject[]));
                //    _numberOfMechs = (int)EditorGUILayout.IntField("Amount in Wave", _numberOfMechs);
                //}
            }

            if (GUILayout.Button("Load"))
                LoadWave();

            if (GUILayout.Button("Update Wave"))
                UpdateWave();

            if (GUILayout.Button("Create New"))
                NewWave();
        }
    }

    private void LoadWave()
    {
        // Testing
        //SpawnManager.Instance.LoadLevel(_waveNumber);
    }

    private void UpdateWave()
    {

    }

    private void NewWave()
    {

    }
}
#endif


