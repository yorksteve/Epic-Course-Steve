using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Scripts.Managers;

#if UNITY_EDITOR
public class ListCustomization : EditorWindow
{
    private Texture2D _mech1Tex;
    private Texture2D _mech2Tex;
    private GameObject _mech1;
    private GameObject _mech2;
    private List<GameObject> _mechList;



    private void OnEnable()
    {
        _mech1 = GameObject.Find("Mech1");
        _mech2 = GameObject.Find("Mech2");
        _mech1Tex = Resources.Load<Texture2D>("Prefab Images/Mech1Pic.png");
        _mech2Tex = Resources.Load<Texture2D>("Prefab Images/Mech2Pic.png");
        if (_mech2Tex == null)
        {
            Debug.Log("Picture not found");
        }

        _mechList = SpawnManager.Instance.SetListCustomization();
    }

    private void OnGUI()
    {
        for (int i = 0; i < _mechList.Count; i++)
        {
            //_mechList[i] = (GameObject)EditorGUILayout.ObjectField(_mechList, typeof(GameObject));
        }

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
            //if (_addingMech == true)
            //{
            //    AddedMechs(_waveNumber, _mechList);
            //}
            //else
            //{
            //    WaveCustomization. NewWave();
            //}
        }

        if (GUILayout.Button("Close"))
        {
            //list.Close();
        }
    }
}
#endif
