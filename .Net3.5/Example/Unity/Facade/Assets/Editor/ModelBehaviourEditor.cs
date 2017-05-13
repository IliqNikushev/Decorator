using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[UnityEditor.CustomEditor(typeof(Facade.Unity.ModelBehaviour), editorForChildClasses: true)]
public class ModelBehaviourEditor : UnityEditor.Editor
{
    private ModelBehaviourFieldsDrawer fieldsDrawer = new ModelBehaviourFieldsDrawer();

    bool[] toggled = null;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        var behaviour = serializedObject.targetObject as Facade.Unity.ModelBehaviour;

        foreach (var item in behaviour.Model.PublicFields)
            item.Value = fieldsDrawer.ShowPropertyField(item.Name, item.Value, item.Type, "Model");

        if (toggled == null)
            toggled = new bool[behaviour.Model.Components.Length + 1];
        int current = 1;
        toggled[0] = EditorGUILayout.Foldout(toggled[0], "Components");
        if (toggled[0])
        {
            foreach (var component in behaviour.Model.Components)
            {
                EditorGUI.indentLevel += 1;
                toggled[current] = EditorGUILayout.Foldout(toggled[current], component.GetType().Name);

                if (toggled[current++])
                {
                    string path = "Model.ComponentsForEditor[" + (current - 1) + "]";
                    var prop = serializedObject.FindProperty("ModelOriginal");
                    EditorGUI.indentLevel += 1;
                    foreach (var item in component.PublicFields)
                        item.Value = fieldsDrawer.ShowPropertyField(item.Name, item.Value, item.Type, "Model.Component:" + component.GetType().Name);
                    EditorGUI.indentLevel -= 1;
                }
                EditorGUI.indentLevel -= 1;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}