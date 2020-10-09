using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Scripts.Managers;
using System.IO;

#if UNITY_EDITOR
public class ListCustomization : EditorWindow
{
    private byte[] _fileData1;
    private byte[] _fileData2;

    private Texture2D _mech1Tex;
    private Texture2D _mech2Tex;
    private GameObject _mech1;
    private GameObject _mech2;
    private List<GameObject> _mechList;
    private Vector2 _scrollPos;

    private Rect _mech1Pos = new Rect(350, 40, 250, 150);
    private Rect _mech2Pos = new Rect(350, 240, 250, 150);

    private bool _addingMech;
    private bool _creatingWave;
    private bool _insertingWave;



    private void OnEnable()
    {
        EventManager.Listen("onAddMechBool", AddingMech);
        EventManager.Listen("onCreatingNewBool", CreatingWave);
        EventManager.Listen("onInsertWaveBool", InsertingWave);

        _fileData1 = File.ReadAllBytes("Assets/Resources/Prefab Images/Mech1Pic.png");
        _fileData2 = File.ReadAllBytes("Assets/Resources/Prefab Images/Mech2Pic.png");
        _mech1Tex = new Texture2D(2, 2);
        _mech2Tex = new Texture2D(2, 2);
        _mech1Tex.LoadImage(_fileData1);
        _mech2Tex.LoadImage(_fileData2);

        _mech1 = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/GameDevHQ/FileBase/Projects/Tutorials/Starter_Files/Epic_Tower_Defense/3D/Characters/Robots/Mech1/Prefab/Mech1.prefab", typeof(GameObject));
        _mech2 = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/GameDevHQ/FileBase/3D/Characters/Robots/Mech_02/Prefab/Mech2.prefab", typeof(GameObject));

        if (_mech1 == null)
        {
            Debug.Log("Failed to get mech");
        }

        _mechList = SpawnManager.Instance.SetListCustomization();
        if (_mechList == null)
        {
            _mechList = new List<GameObject>(5);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
        for (int i = 0; i < _mechList.Count; i++)
        {
            _mechList[i] = (GameObject)EditorGUILayout.ObjectField(_mechList[i], typeof(GameObject));
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginVertical();
        EditorGUI.DrawPreviewTexture(_mech1Pos, _mech1Tex);
        EditorGUILayout.Space(200);
        if (GUILayout.Button("Add"))
        {
            _mechList.Add(_mech1);
        }
        if (GUILayout.Button("Remove"))
        {
            var index = _mechList.LastIndexOf(_mech1);
            _mechList.RemoveAt(index);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(180);

        EditorGUILayout.BeginVertical();
        EditorGUI.DrawPreviewTexture(_mech2Pos, _mech2Tex);
        if (GUILayout.Button("Add"))
        {
            _mechList.Add(_mech2);
        }
        if (GUILayout.Button("Remove"))
        {
            var index = _mechList.LastIndexOf(_mech2);
            _mechList.RemoveAt(index);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Apply"))
        {
            if (_addingMech == true)
            {
                EventManager.Fire("onAddedMech", _mechList);
            }
            else if (_creatingWave == true)
            {
                EventManager.Fire("onCreateNewWave", _mechList);
            }
            else
            {
                EventManager.Fire("onInsertWave", _mechList);
            }

            this.Close();
        }

        if (GUILayout.Button("Close"))
        {
            this.Close();
        }
    }

    private void AddingMech()
    {
        _addingMech = true;
        _creatingWave = false;
        _insertingWave = false;
    }

    private void CreatingWave()
    {
        _addingMech = false;
        _creatingWave = true;
        _insertingWave = false;
    }

    private void InsertingWave()
    {
        _addingMech = false;
        _creatingWave = false;
        _insertingWave = true;
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent("onAddMechBool", AddingMech);
        EventManager.UnsubscribeEvent("onCreatingNewBool", CreatingWave);
        EventManager.UnsubscribeEvent("onInsertWaveBool", InsertingWave);
    }
}
#endif
