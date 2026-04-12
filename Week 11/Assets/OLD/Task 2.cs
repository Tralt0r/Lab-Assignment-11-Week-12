using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Task2 : MonoBehaviour
{
    void Start()
    {
        List<string> nameArray = new List<string>();

        for (int i = 0; i < 15; i++)
        {
            string name = Task1.firstNames[Random.Range(0, Task1.firstNames.Length)];
            nameArray.Add(name);
        }

        Debug.Log("Created the name array: " + string.Join(", ", nameArray));

        HashSet<string> seen = new HashSet<string>();
        HashSet<string> duplicates = new HashSet<string>();

        foreach (string name in nameArray)
        {
            if (!seen.Add(name))
            {
                duplicates.Add(name);
            }
        }

        if (duplicates.Count > 0)
        {
            Debug.Log("The array has duplicate names: " + string.Join(", ", duplicates));
        }
        else
        {
            Debug.Log("The array has no duplicate names.");
        }
    }
}