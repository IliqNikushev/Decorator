using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomSerializableClass
{
    public int IField;
    public string SField;
    public int[] ArrayField = new int[] { 1, 2, 3 };
    public CustomSerializableClass NullField;
    public CustomSerializableNestedClass NestedSerializableClass = new CustomSerializableNestedClass();
}

[System.Serializable]
public class CustomSerializableNestedClass
{
    public int IField = 3;
    public string SField = "5";
    public int[] ArrayField = new int[] { 1, 2, 3 };
}