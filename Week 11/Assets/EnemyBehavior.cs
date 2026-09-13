using System.Linq;
using UnityEngine;
using UnityEditor;
using System;

public class EnemyBehavior : MonoBehaviour
{
    private GameObject[] spheres;
    private GameObject[] cubes;
    //View selected objects in inspector
    private GameObject[] allObjects;
    //Size of cubes and spheres
    public float cubeSize = 1f;
    public float sphereRadius = 1f;

    [HideInInspector] public string sizeWarning = "";
    //Size limits for cubes and spheres
    private const float MinCubeSize = 0.1f;
    private const float MaxCubeSize = 2f;
    private const float MinSphereRadius = 1f;
    private const float MaxSphereRadius = 5f;
    //Generate shapes when the script is added to a GameObject
    private void OnValidate()
    {
        sizeWarning = "";

        if (cubeSize > MaxCubeSize)
        {
            sizeWarning = $"The cubes' sizes cannot be bigger than {MaxCubeSize}!";
            cubeSize = MaxCubeSize;
        }
        else if (cubeSize < MinCubeSize)
        {
            sizeWarning = $"The cubes' sizes cannot be smaller than {MinCubeSize}!";
            cubeSize = MinCubeSize;
        }

        if (sphereRadius < MinSphereRadius)
        {
            sizeWarning = $"The spheres' radius cannot be smaller than {MinSphereRadius}!";
            sphereRadius = MinSphereRadius;
        }
        else if (sphereRadius > MaxSphereRadius)
        {
            sizeWarning = $"The spheres' radius cannot be bigger than {MaxSphereRadius}!";
            sphereRadius = MaxSphereRadius;
        }

        RefreshObjectReferences();
        //Update the sizes of the cubes and spheres
        foreach (var cube in cubes)
            if (cube != null) cube.transform.localScale = Vector3.one * cubeSize;

        foreach (var sphere in spheres)
            if (sphere != null) sphere.transform.localScale = Vector3.one * sphereRadius * 2f;
    }
    //Refresh the references to the cubes and spheres in the scene
    private void RefreshObjectReferences()
    {
        GameObject[] sceneObjects = UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        //Add objects either sphere or cube to the respective arrays
        spheres = sceneObjects.Where(obj => obj.name.Contains("Sphere")).ToArray();
        cubes = sceneObjects.Where(obj => obj.name.Contains("Cube")).ToArray();
    }

    //Generate 5 cubes and 5 spheres
    public void GenerateShapes()
    {
        //Destroy existing cubes and spheres
        foreach (var cube in cubes ?? Array.Empty<GameObject>())
            if (cube != null) DestroyImmediate(cube);
        foreach (var sphere in spheres ?? Array.Empty<GameObject>())
            if (sphere != null) DestroyImmediate(sphere);

        //Create new cubes and spheres
        cubes = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = $"Cube{i + 1}";
            cube.transform.SetParent(transform);
            cube.transform.position = new Vector3(i * 2f, 0f, 0f);
            cube.transform.localScale = Vector3.one * cubeSize;
            cubes[i] = cube;
        }

        spheres = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = $"Sphere{i + 1}";
            sphere.transform.SetParent(transform);
            sphere.transform.position = new Vector3(i * 2f, 2f, 0f);
            sphere.transform.localScale = Vector3.one * sphereRadius * 2f;
            spheres[i] = sphere;
        }
    }

    //Select all cubes and spheres
    public void selectObjects()
    {
        RefreshObjectReferences();

        allObjects = spheres
            .Concat(cubes)
            .Where(obj => obj != null)
            .ToArray();

        //Ensures this object is still selected along with the cubes and spheres
        Selection.objects = allObjects.Append(this.gameObject).ToArray();
    }

    //Clear selection but keep this object selected
    public void ClearSelection()
    {
        allObjects = Array.Empty<GameObject>();
        //Ensures this object is still selected
        Selection.objects = new UnityEngine.Object[] { this.gameObject };
    }

    public bool AllEnabled() => allObjects != null && allObjects.Any(o => o != null && o.activeSelf);

    //Toggle all cubes and spheres on/off together
    public void ToggleAll()
    {   //Hide/Show all cubes and spheres based on the boolean
        bool enable = !AllEnabled();
        foreach (var obj in spheres.Concat(cubes))
            if (obj != null) obj.SetActive(enable);
    }
}