using System.Collections;
using System.Collections.Generic;
using UnityEngine;




using System;
using System.Reflection;

class MyClass
{
    private int myField = 42;
}

public class Program
{
    public static void Main()
    {
        Type type = typeof(MyClass);
        FieldInfo fieldInfo = type.GetField("myField", BindingFlags.NonPublic | BindingFlags.Instance);

        MyClass obj = new MyClass();
        Console.WriteLine("Field Value: " + fieldInfo.GetValue(obj));
    }
}
