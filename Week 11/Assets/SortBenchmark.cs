using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class FunctionTimer : MonoBehaviour
{
    [DllImport("Nativeplugin", EntryPoint = "TestSort")]
    public static extern void TestSort(int[] a, int length);

    public TextAsset numbersFile;
    int[] a;

    void Start()
    {
        MeasureFunctionExecutionTime();
    }

    void MeasureFunctionExecutionTime()
    {
        Stopwatch stopwatch = new Stopwatch();

        LoadArray();
        stopwatch.Start();
        TestSort(a, a.Length);                    // Method 1: native C++
        stopwatch.Stop();
        UnityEngine.Debug.Log($"Native C++ took {stopwatch.ElapsedMilliseconds} ms to execute.");

        LoadArray();
        stopwatch.Restart();
        Sorter.Sorter.Sort(a);                    // Method 2: managed C# DLL
        stopwatch.Stop();
        UnityEngine.Debug.Log($"Managed C# DLL took {stopwatch.ElapsedMilliseconds} ms to execute.");

        LoadArray();
        stopwatch.Restart();
        Array.Sort(a);                            // Method 3: MonoBehaviour
        stopwatch.Stop();
        UnityEngine.Debug.Log($"MonoBehaviour took {stopwatch.ElapsedMilliseconds} ms to execute.");
    }

    void LoadArray()
    {
        a = numbersFile.text
            .Split(new[] { ' ', '\n', '\r', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse).ToArray();
    }
}