using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

[CustomEditor(typeof(MyText))]
public class MyTextEditor : UnityEditor.UI.TextEditor
{
    public override void OnInspectorGUI()
    {
        MyText myText = target as MyText;
        //EditorGUI.BeginChangeCheck();
        myText.UIString = EditorGUILayout.TextField("UIString", myText.UIString);
        base.OnInspectorGUI();
        /*
        if(Application.isPlaying)
        {
            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("set");
                Undo.RecordObject(myText, "UIText Modification");
            }
        }
        */
    }
}