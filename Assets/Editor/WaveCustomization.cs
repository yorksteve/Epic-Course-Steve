using UnityEngine;
using UnityEditor;
using Scripts.Managers;


#if UNITY_EDITOR
public class WaveCustomization : EditorWindow
{
    private int _waveNumber;
    private int _numberOfMechs;
    private bool _mechsClicked = true;
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


        EditorGUILayout.BeginHorizontal();
        _waveNumber = (int)EditorGUILayout.IntField("Wave Number", _waveNumber);
        if (GUILayout.Button("Load"))
            LoadWave();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginFoldoutHeaderGroup(_mechsClicked, "Mechs");
        //for (int i = 0; i < _mechs.Length; i++)
        //{
        //    _mechs[i] = (GameObject)EditorGUILayout.ObjectField("Mech", _mechs[i], typeof(GameObject));
        //    _numberOfMechs = (int)EditorGUILayout.IntField("Amount in Wave", _numberOfMechs);
        //}

        _mech = (GameObject)EditorGUILayout.ObjectField("Mechs", _mech, typeof(GameObject));
        _numberOfMechs = (int)EditorGUILayout.IntField("Amount in Wave", _numberOfMechs);
        EditorGUILayout.EndFoldoutHeaderGroup();

    }

    private void LoadWave()
    {
        // Testing
        SpawnManager.Instance.LoadLevel(_waveNumber);
    }

}
#endif


