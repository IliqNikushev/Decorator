using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

class ModelBehaviourFieldsDrawer
{
    private bool GetIsFolded(string identifier, string visual)
    {
        bool isUnfolded = unfolded.ContainsKey(identifier) && unfolded[identifier];

        isUnfolded = EditorGUILayout.Foldout(isUnfolded, visual);
        if (unfolded.ContainsKey(identifier))
            unfolded[identifier] = isUnfolded;

        if (isUnfolded)
            if (!unfolded.ContainsKey(identifier))
                unfolded.Add(identifier, true);

        return isUnfolded;
    }

    private Facade.SerializableField[] PublicFieldsOf(object value)
    {
        return value.
            GetType().
            GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).
            Select(x => new Facade.SerializableField(value, x)).
            ToArray();
    }

    static System.Type[] NumericTypes = new System.Type[]
     {
        typeof(byte), typeof(sbyte),
        typeof(short), typeof(ushort),
        typeof(int), typeof(uint),
        typeof(long), typeof(ulong),
        typeof(float), typeof(double), typeof(decimal)
    };

    Dictionary<string, bool> unfolded = new Dictionary<string, bool>();

    public object ShowPropertyField(string name, object value, System.Type type, string nameChain = "")
    {
        nameChain += "." + name;
        if (type == typeof(bool))
        {
            return EditorGUILayout.Toggle(name, (bool)value);
        }
        else if (NumericTypes.Contains(type))
        {
            if (type == typeof(float))
                return EditorGUILayout.FloatField(name, (float)value);
            else if (type == typeof(double))
                return EditorGUILayout.DoubleField(name, (double)value);
            else if (type == typeof(long))
                return EditorGUILayout.LongField(name, (long)value);
            else
                return EditorGUILayout.IntField(name, (int)value);
        }
        else if (type.IsEnum)
        {
            // todo is enum showing all values?
            return EditorGUILayout.EnumPopup(name, (System.Enum)value);
        }
        else if (type.IsArray)
        {
            if (value == null)
                value = System.Array.CreateInstance(type.GetElementType(), 0);
            var array = value as System.Array;

            if (GetIsFolded(nameChain, name + ":" + array.Length + " " + type.GetElementType()))
            {
                EditorGUI.indentLevel += 1;
                int size = EditorGUILayout.IntField("Items", array.Length);

                if (size != array.Length)
                {
                    var destinationArray = System.Array.CreateInstance(type.GetElementType(), size);
                    System.Array.Copy(array, destinationArray, array.Length > size ? size : array.Length);

                    array = destinationArray;
                }

                for (int i = 0; i < array.Length; i++)
                {
                    if (type.GetElementType().IsClass && !type.GetElementType().IsSubclassOf(typeof(Object)))
                    {
                        // Custom class
                        if (GetIsFolded(nameChain + i, "[" + i + "]"))
                        {
                            EditorGUI.indentLevel += 1;

                            var newValue = ShowPropertyField("[" + i + "]", array.GetValue(i), type.GetElementType(), nameChain);
                            array.SetValue(newValue, i);

                            EditorGUI.indentLevel -= 1;
                        }
                    }
                    else
                    {
                        // Not custom class

                        var newValue = ShowPropertyField("[" + i + "]", array.GetValue(i), type.GetElementType(), nameChain);
                        array.SetValue(newValue, i);
                    }
                }
                EditorGUI.indentLevel -= 1;

                value = array;
            }

            return value;
        }
        else if (type == typeof(Vector2))
        {
            return EditorGUILayout.Vector2Field(name, (Vector2)value);
        }
        else if (type == typeof(Vector3))
        {
            return EditorGUILayout.Vector2Field(name, (Vector3)value);
        }
        else if (type == typeof(Vector4))
        {
            return EditorGUILayout.Vector2Field(name, (Vector4)value);
        }
        else if (type == typeof(Color) || type == typeof(Color32))
        {
            return EditorGUILayout.ColorField(name, (Color)value);
        }
        else if (type == typeof(Rect))
        {
            return EditorGUILayout.RectField(name, (Rect)value);
        }
        else if (type == typeof(string))
        {
            // todo Text attribute
            return EditorGUILayout.TextField(name, (string)value);
        }
        else
        {
            if (type.IsSubclassOf(typeof(Object)))
            {
                return EditorGUILayout.ObjectField(name, (Object)value, type, true);
            }
            if (type.IsSerializable)
            {
                if (value == null)
                {
                    EditorGUILayout.TextField(name, "Null");
                    return value;
                }
                else
                {
                    if (type.IsClass)
                    {
                        if (!name.Contains("["))
                        {
                            EditorGUI.indentLevel += 1;
                            if (GetIsFolded(nameChain, name))
                            {
                                EditorGUI.indentLevel += 1;
                                foreach (var field in PublicFieldsOf(value))
                                    field.Value = ShowPropertyField(field.Name, field.Value, field.Type, nameChain);
                                EditorGUI.indentLevel -= 1;
                            }
                            EditorGUI.indentLevel -= 1;
                        }
                        else
                        {
                            EditorGUILayout.LabelField(value == null ? type.ToString() : value.GetType().ToString());
                            EditorGUI.indentLevel += 1;
                            foreach (var field in PublicFieldsOf(value))
                                field.Value = ShowPropertyField(field.Name, field.Value, field.Type, nameChain);
                            EditorGUI.indentLevel -= 1;
                        }
                        return value;
                    }

                    Debug.Log(type + " is not expanding while looking in " + nameChain);
                    return value;
                }
            }
            else
            {
                Debug.Log(type + " is not serializable while looking in " + nameChain);
                return value;
            }
        }
    }
}