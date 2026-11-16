using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int itemCount = 20;

    List<InventoryItem> inventory = new List<InventoryItem>();
    string[] names = { "Sword", "Shield", "Potion", "Bow" };

    void Start()
    {
        MakeInventory();
        Debug.Log("Original inventory");
        ShowInventory();

        //linear search
        string nameToFind = inventory[Random.Range(0, inventory.Count)].Name;
        InventoryItem nameResult = LinearSearchByName(nameToFind);
        if (nameResult != null)
            Debug.Log("Linear search found: " + nameResult);
        else
            Debug.Log("Linear search couldn't find " + nameToFind);

        if (LinearSearchByName("RandomItem") == null)
            Debug.Log("RandomItem not found");

        //binary search
        int idToFind = inventory[Random.Range(0, inventory.Count)].ID;
        InventoryItem idResult = BinarySearchByID(idToFind);
        if (idResult != null)
            Debug.Log("Binary search found: " + idResult);

        //quicksort
        QuickSortByValue();
        Debug.Log("Inventory sorted by value");
        ShowInventory();
    }

    void MakeInventory()
    {
        inventory.Clear();
        List<int> usedIDs = new List<int>();

        for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
        {
            //make sure no two items share an ID
            int id = Random.Range(1000, 9999);
            while (usedIDs.Contains(id))
            {
                id = Random.Range(1000, 9999);
            }
            usedIDs.Add(id);

            string name = names[itemIndex % names.Length];
            int value = Random.Range(1, 501);
            inventory.Add(new InventoryItem(id, name, value));
        }
    }

    void ShowInventory(){foreach (InventoryItem item in inventory){Debug.Log(item);}}

    //Task 1: just loop through until found
    public InventoryItem LinearSearchByName(string itemName)
    {
        for (int itemIndex = 0; itemIndex < inventory.Count; itemIndex++)
        {if (inventory[itemIndex].Name.ToLower() == itemName.ToLower()){return inventory[itemIndex];}}
        return null;
    }

    //Task 2: sorted by ID first
    public InventoryItem BinarySearchByID(int targetID)
    {
        inventory.Sort((firstItem, secondItem) => firstItem.ID.CompareTo(secondItem.ID));
        int low = 0;
        int high = inventory.Count - 1;

        while (low <= high)
        {
            int middle = (low + high) / 2;
            if (inventory[middle].ID == targetID){return inventory[middle];}
            else if (inventory[middle].ID < targetID){low = middle + 1;}
            else{high = middle - 1;}
        }
        return null;
    }

    //Task 3: sort by value
    public void QuickSortByValue(){QuickSort(0, inventory.Count - 1);}

    void QuickSort(int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(low, high);
            QuickSort(low, pivotIndex - 1);
            QuickSort(pivotIndex + 1, high);
        }
    }

    int Partition(int low, int high)
    {
        int pivotValue = inventory[high].Value;
        int smallerIndex = low - 1;

        for (int currentIndex = low; currentIndex < high; currentIndex++)
        {
            if (inventory[currentIndex].Value <= pivotValue)
            {
                smallerIndex++;
                Swap(smallerIndex, currentIndex);
            }
        }

        Swap(smallerIndex + 1, high);
        return smallerIndex + 1;
    }

    void Swap(int firstIndex, int secondIndex)
    {
        InventoryItem temp = inventory[firstIndex];
        inventory[firstIndex] = inventory[secondIndex];
        inventory[secondIndex] = temp;
    }
}