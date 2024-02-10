using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Scripts.Managers;
using System.IO;
using System;
using UnityEditorInternal;

#if UNITY_EDITOR
public class ListCustomization : EditorWindow
{
    private byte[] _fileData1;
    private byte[] _fileData2;

    private Texture2D _mech1Tex;
    private Texture2D _mech2Tex;
    private GameObject _mech1;
    private GameObject _mech2;
    //private GameObject _mech;
    //private Texture2D _mechTexture;
    //private GameObject[] _assetsAtPath;
    //private List<GameObject> _mechDisplayList = new List<GameObject>();
    //private Texture2D[] _mechTex;
    private List<GameObject> _mechList = new List<GameObject>();
    private ReorderableList _list;
    private Vector2 _scrollPos;
    private SerializedObject _listEditor;

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
        //EventManager.Listen("onAddToDisplayList", (Action<GameObject>)AddToDisplayList);

        _listEditor = new SerializedObject(this);

        _fileData1 = File.ReadAllBytes("Assets/Resources/Prefab Images/Mech1Pic.png");
        _fileData2 = File.ReadAllBytes("Assets/Resources/Prefab Images/Mech2Pic.png");
        _mech1Tex = new Texture2D(2, 2);
        _mech2Tex = new Texture2D(2, 2);
        _mech1Tex.LoadImage(_fileData1);
        _mech2Tex.LoadImage(_fileData2);

        //_mech1 = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/GameDevHQ/FileBase/Projects/Tutorials/Starter_Files/Epic_Tower_Defense/3D/Characters/Robots/Mech1/Prefab/Mech1.prefab", typeof(GameObject));
        //_mech2 = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/GameDevHQ/FileBase/3D/Characters/Robots/Mech_02/Prefab/Mech2.prefab", typeof(GameObject));

        //if (_mech1 == null)
        //{
        //    Debug.Log("Failed to get mech");
        //}

        _mechList = SpawnManager.Instance.SetListCustomization();
        if (_mechList == null)
        {
            _mechList = new List<GameObject>(5);
        }

        _list = new ReorderableList(_listEditor, _listEditor.FindProperty("_mechList"), true, true, true, true);
        _list.drawHeaderCallback = DrawHeader;
        _list.drawElementCallback = DrawElements;

        //_mechArray = new GameObject[] { _mech1, _mech2 };
        //_assetsAtPath = (GameObject[])AssetDatabase.LoadAllAssetsAtPath("Assets/Resources/Prefabs/Mechs");
        //foreach (var asset in _assetsAtPath)
        //{
        //    if (PrefabUtility.GetPrefabType(asset) == PrefabType.None)
        //    {
        //        _mechDisplayList.Add(asset);
        //    }
        //}

        //_mechTex = new Texture2D[] { _mech1Tex, _mech2Tex };
        //if (_mechDisplayList.Count == 0)
        //{
        //    Debug.Log("Failed to get mechs");
        //}
        //_mechTexture = _mechTex[0];
    }

    private void DrawElements(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.PropertyField(rect, _list.serializedProperty.GetArrayElementAtIndex(index));
    }

    private void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Wave Sequence");
    }

    private void OnGUI()
    {
        //Change to Reordablelist
        //_list.DoLayoutList();
        //_listEditor.ApplyModifiedProperties();

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
        EditorGUILayout.Space(200);
        EditorGUI.DrawPreviewTexture(_mech1Pos, _mech1Tex);
        //_mechTexture = (Texture2D)EditorGUILayout.ObjectField(_mechTexture, typeof(Texture2D), GUILayout.Width(250), GUILayout.Height(150));
        EditorGUILayout.BeginHorizontal();
        //if (GUILayout.Button("Previous"))
        //{
        //    CycleArray(false);
        //    EditorGUI.DrawPreviewTexture(_mech1Pos, _mechTexture);
        //}
        if (GUILayout.Button("Add"))
        {
            _mechList.Add(_mech1);
        }
        if (GUILayout.Button("Remove"))
        {
            var index = _mechList.LastIndexOf(_mech1);
            _mechList.RemoveAt(index);
        }
        //if (GUILayout.Button("Next"))
        //{
        //    CycleArray(true);
        //    EditorGUI.DrawPreviewTexture(_mech1Pos, _mechTexture);
        //}
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(180);

        EditorGUILayout.BeginVertical();
        EditorGUI.DrawPreviewTexture(_mech2Pos, _mech2Tex);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            _mechList.Add(_mech2);
        }
        if (GUILayout.Button("Remove"))
        {
            var index = _mechList.LastIndexOf(_mech2);
            _mechList.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();
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

    //private void AddToDisplayList(GameObject mech)
    //{
    //    _mechDisplayList.Add(mech);
    //}

    //private GameObject CycleArray(bool direction)
    //{
    //    for (int i = 0; i < _mechDisplayList.Count;)
    //    {
    //        if (direction == true)
    //        {
    //            if (i == _mechDisplayList.Count)
    //            {
    //                i = 0;
    //            }
    //            else
    //            {
    //                i++;
    //            }
    //        }
    //        else if (direction == false)
    //        {
    //            if (i == 0)
    //            {
    //                i = _mechDisplayList.Count;
    //            }
    //            else
    //            {
    //                i--;
    //            }
    //        }

    //        _mech = _mechDisplayList[i];
    //        _mechTexture = _mechTex[i];
    //        return _mech;
    //    }

    //    return null;
    //}

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent("onAddMechBool", AddingMech);
        EventManager.UnsubscribeEvent("onCreatingNewBool", CreatingWave);
        EventManager.UnsubscribeEvent("onInsertWaveBool", InsertingWave);
        //EventManager.UnsubscribeEvent("onAddToDisplayList", (Action<GameObject>)AddToDisplayList);
    }
}
#endif
