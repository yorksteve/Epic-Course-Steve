using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class ListCustomization : EditorWindow
{
    private Texture _mech1Tex;
    private Texture _mech2Tex;
    private GameObject _mech1;
    private GameObject _mech2;
    private List<GameObject> _mechList;

    [MenuItem("Window/List Customization")]
    static void Init()
    {
        ListCustomization window = (ListCustomization)EditorWindow.GetWindow(typeof(ListCustomization));
        window.Show();
    }

    private void OnEnable()
    {
        _mech1Tex = EditorGUIUtility.FindTexture("");
    }

    private void OnGUI()
    {
        // Display list from WaveCustomization

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(new GUIContent(_mech1Tex));
        if (GUILayout.Button("Add"))
        {
            _mechList.Add(_mech1);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(new GUIContent(_mech2Tex));
        if (GUILayout.Button("Add"))
        {
            _mechList.Add(_mech2);
        }
        EditorGUILayout.EndVertical();
    }
}
#endif
