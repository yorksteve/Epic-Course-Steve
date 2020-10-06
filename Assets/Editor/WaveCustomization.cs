using UnityEngine;
using UnityEditor;
using Scripts.Managers;


#if UNITY_EDITOR
public class WaveCustomization : EditorWindow
{
    private int _waveNumber;
    private int _spawnDelay;
    private int _waveDuration;
    private int _numberOfMechs;
    private bool _waveSelected = true;
    private bool _mechsClicked = true;
    private string _waveStatus = "Select a Wave";
    //private GameObject[] _mechs;
    private GameObject _mech;

    [MenuItem("Window/Wave Customization")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        WaveCustomization window = (WaveCustomization)EditorWindow.GetWindow(typeof(WaveCustomization));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        _waveSelected = EditorGUILayout.Foldout(_waveSelected, _waveStatus);

        if (_waveSelected)
        {
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

            if (GUILayout.Button("Create New"))
                NewWave();

            _waveStatus = "Wave " + _waveNumber;
        }
    }

    private void LoadWave()
    {
        // Testing
        //SpawnManager.Instance.LoadLevel(_waveNumber);
    }

    private void NewWave()
    {

    }

}
#endif


