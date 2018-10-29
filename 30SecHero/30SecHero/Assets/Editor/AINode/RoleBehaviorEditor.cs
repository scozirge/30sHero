using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(RoleBehavior))]
public class RoleBehaviorEditor : Editor
{
    RoleBehavior MyRoleBehavior;
    SerializedObject GetTarget;
    SerializedProperty NodeList;
    GUIStyle ButtonStyle;
    GUIStyle FoldoutStyle;
    GUIStyle LabelStyle;
    ReorderableList NodeReorderableList;
    void OnEnable()
    {
        MyRoleBehavior = (RoleBehavior)target;
        GetTarget = new SerializedObject(MyRoleBehavior);
        NodeList = GetTarget.FindProperty("Nodes");
        NodeReorderableList = new ReorderableList(serializedObject, NodeList, true, true, true, true);
        NodeReorderableList.drawHeaderCallback = (Rect rect) =>
        {
            GUI.Label(rect, "Node List");
        };
        NodeReorderableList.drawElementCallback = DrawNameElement;
        NodeReorderableList.onRemoveCallback = RemoveNode;
    }
    void RemoveNode(ReorderableList _list)
    {
        if (EditorUtility.DisplayDialog("Remove Node", "Are you sure to remove this node?", "yes", "noooo!"))
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(_list);
        }
    }
    void RemoveNode(Node _node)
    {
        if (EditorUtility.DisplayDialog("Remove Node", "Are you sure to remove this node?", "yes", "noooo!"))
        {
            MyRoleBehavior.Nodes.Remove(_node);
        }
    }
    void Duplicate(Node _node)
    {
        if (EditorUtility.DisplayDialog("Duplicate Node", "Are you sure to duplicate this node?", "yes", "nooo!"))
        {
            MyRoleBehavior.Nodes.Add(_node.GetMemberwiseClone());
        }
    }
    void DrawNameElement(Rect _rect, int _index, bool _selected, bool _focused)
    {
        SerializedProperty nodeData = NodeReorderableList.serializedProperty.GetArrayElementAtIndex(_index);
        _rect.y += 2;
        _rect.height = EditorGUIUtility.singleLineHeight;
        SerializedProperty type = nodeData.FindPropertyRelative("Type");
        SerializedProperty nodeTag = nodeData.FindPropertyRelative("NodeTag");
        EditorGUI.LabelField(_rect, "Action" + _index + ":" + (ActionType)type.enumValueIndex + "(" + nodeTag.stringValue + ")");
    }
    public override void OnInspectorGUI()
    {
        GetTarget.Update();
        NodeReorderableList.DoLayoutList();
        FoldoutStyle = new GUIStyle(EditorStyles.foldout);
        FoldoutStyle.fontStyle = FontStyle.Bold;
        ButtonStyle = new GUIStyle(GUI.skin.button);
        //ListSize = NodeList.arraySize;

        //EditorGUILayout.LabelField("NodeCount : " + ListSize);
        /*
        if (GUILayout.Button("Add New Node(新增動作)", ButtonStyle))
        {
            MyRoleBehavior.Nodes.Add(new Node());
        }
        */
        for (int i = 0; i < NodeList.arraySize; i++)
        {
            SerializedProperty myListRef = NodeList.GetArrayElementAtIndex(i);
            SerializedProperty expand = myListRef.FindPropertyRelative("ExpandFolder");

            SerializedProperty type = myListRef.FindPropertyRelative("Type");
            SerializedProperty relativeToTarget = myListRef.FindPropertyRelative("RelativeToTarget");
            SerializedProperty nodeTag = myListRef.FindPropertyRelative("NodeTag");
            if (nodeTag.stringValue != "")
                expand.boolValue = EditorGUILayout.Foldout(expand.boolValue, "Action" + i + ":" + (ActionType)type.enumValueIndex + "(" + nodeTag.stringValue + ")", FoldoutStyle);
            else
                expand.boolValue = EditorGUILayout.Foldout(expand.boolValue, "Action" + i + ":" + (ActionType)type.enumValueIndex, FoldoutStyle);
            if (expand.boolValue)
            {
                EditorGUIUtility.labelWidth = 140;
                SerializedProperty activeOnlyByEvent = myListRef.FindPropertyRelative("ActiveOnlyByEvent");
                EditorGUILayout.PropertyField(activeOnlyByEvent);
                EditorGUIUtility.labelWidth = 100;
                EditorGUILayout.PropertyField(nodeTag);                
                SerializedProperty waitTime = myListRef.FindPropertyRelative("WaitSecond");
                EditorGUILayout.PropertyField(waitTime);
                EditorGUILayout.PropertyField(type, new GUIContent("ActionType"));
                SerializedProperty locoParticle = myListRef.FindPropertyRelative("LocoParticle");
                SerializedProperty worldPartilce = myListRef.FindPropertyRelative("WorldPartilce");

                switch (type.enumValueIndex)
                {
                    case (int)ActionType.Move:
                        SerializedProperty processTime = myListRef.FindPropertyRelative("MaxProcessingTime");
                        EditorGUILayout.PropertyField(processTime, new GUIContent("ProcessingTime"));
                        SerializedProperty moveSpeed = myListRef.FindPropertyRelative("MoveSpeed");
                        EditorGUILayout.PropertyField(moveSpeed);
                        EditorGUILayout.PropertyField(relativeToTarget, new GUIContent("RelativeTo"));
                        SerializedProperty MyDestination = myListRef.FindPropertyRelative("Destination");
                        EditorGUILayout.PropertyField(MyDestination);
                        break;
                    case (int)ActionType.Spell:
                        SerializedProperty skillList = myListRef.FindPropertyRelative("SkillList");
                        int skillSize = skillList.arraySize;
                        skillSize = EditorGUILayout.IntField("SkillCount", skillSize);
                        if (skillSize != skillList.arraySize)
                        {
                            while (skillSize > skillList.arraySize)
                            {
                                skillList.InsertArrayElementAtIndex(skillList.arraySize);
                            }
                            while (skillSize < skillList.arraySize)
                            {
                                skillList.DeleteArrayElementAtIndex(skillList.arraySize - 1);
                            }
                        }
                        for (int j = 0; j < skillList.arraySize; j++)
                        {
                            SerializedProperty skillRef = skillList.GetArrayElementAtIndex(j);
                            EditorGUILayout.PropertyField(skillRef);
                        }
                        break;
                    case (int)ActionType.Teleport:
                        EditorGUILayout.PropertyField(relativeToTarget, new GUIContent("RelativeTo"));
                        SerializedProperty teleportPos = myListRef.FindPropertyRelative("Destination");
                        EditorGUILayout.PropertyField(teleportPos);
                        EditorGUILayout.PropertyField(locoParticle);
                        EditorGUILayout.PropertyField(worldPartilce);
                        break;
                    case (int)ActionType.Perform:
                        EditorGUIUtility.labelWidth = 100;

                        SerializedProperty roleAniTriggerName = myListRef.FindPropertyRelative("RoleAniTriggerName");
                        SerializedProperty camAniTriggerName = myListRef.FindPropertyRelative("CamAniTriggerName");
                        SerializedProperty soundList = myListRef.FindPropertyRelative("SoundList");
                        EditorGUILayout.PropertyField(locoParticle);
                        EditorGUILayout.PropertyField(worldPartilce);
                        EditorGUILayout.PropertyField(roleAniTriggerName, new GUIContent("RoleAniName"));
                        EditorGUILayout.PropertyField(camAniTriggerName, new GUIContent("CamAniName"));
                        int soundSize = soundList.arraySize;
                        soundSize = EditorGUILayout.IntField("SoundCount", soundSize);
                        if (soundSize != soundList.arraySize)
                        {
                            while (soundSize > soundList.arraySize)
                            {
                                soundList.InsertArrayElementAtIndex(soundList.arraySize);
                            }
                            while (soundSize < soundList.arraySize)
                            {
                                soundList.DeleteArrayElementAtIndex(soundList.arraySize - 1);
                            }
                        }
                        for (int j = 0; j < soundList.arraySize; j++)
                        {
                            SerializedProperty soundRef = soundList.GetArrayElementAtIndex(j);
                            EditorGUILayout.PropertyField(soundRef);
                        }
                        break;
                    case (int)ActionType.RandomAction:
                        break;
                }
                SerializedProperty toRandomNode = myListRef.FindPropertyRelative("ToRandomNode");
                EditorGUILayout.PropertyField(toRandomNode, new GUIContent("GotoRandomNode"));
                if (toRandomNode.boolValue)
                {
                    SerializedProperty expandRandomNodes = myListRef.FindPropertyRelative("ExpandRandomNodes");
                    expandRandomNodes.boolValue = EditorGUILayout.Foldout(expandRandomNodes.boolValue, "RandomNodes");
                    if (expandRandomNodes.boolValue)
                    {
                        SerializedProperty goToNodes = myListRef.FindPropertyRelative("GoToNodes");
                        int randNodeDicSize = goToNodes.arraySize;
                        randNodeDicSize = EditorGUILayout.IntField("RandomNodesCount", randNodeDicSize);
                        if (randNodeDicSize != goToNodes.arraySize)
                        {
                            while (randNodeDicSize > goToNodes.arraySize)
                            {
                                goToNodes.InsertArrayElementAtIndex(goToNodes.arraySize);
                                goToNodes.GetArrayElementAtIndex(goToNodes.arraySize - 1).FindPropertyRelative("Key").stringValue = "";
                                goToNodes.GetArrayElementAtIndex(goToNodes.arraySize - 1).FindPropertyRelative("Weight").intValue = 0;
                            }
                            while (randNodeDicSize < goToNodes.arraySize)
                            {
                                goToNodes.DeleteArrayElementAtIndex(goToNodes.arraySize - 1);
                            }
                        }
                        for (int j = 0; j < goToNodes.arraySize; j++)
                        {
                            EditorGUILayout.BeginHorizontal(GUI.skin.label);
                            EditorGUIUtility.labelWidth = 40;
                            EditorGUILayout.PropertyField(goToNodes.GetArrayElementAtIndex(j).FindPropertyRelative("Key"), new GUIContent("Tag"));
                            EditorGUILayout.PropertyField(goToNodes.GetArrayElementAtIndex(j).FindPropertyRelative("Weight"),new GUIContent("Weit"));
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                ButtonStyle.normal.textColor = Color.black;
                if (GUILayout.Button("Duplicate Node(複製)", ButtonStyle))
                {
                    Duplicate(MyRoleBehavior.Nodes[i]);
                    //MyRoleBehavior.Nodes.Add(MyRoleBehavior.Nodes[i].GetMemberwiseClone());
                }
                ButtonStyle.normal.textColor = Color.red;
                if (GUILayout.Button("Remove Node(移除)", ButtonStyle))
                {
                    RemoveNode(MyRoleBehavior.Nodes[i]);
                    //MyRoleBehavior.Nodes.RemoveAt(i);
                }
            }
        }
        GetTarget.ApplyModifiedProperties();
    }
}
