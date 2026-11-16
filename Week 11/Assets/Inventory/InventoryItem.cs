using System;

[Serializable]
public class InventoryItem
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }

    public InventoryItem(int id, string name, int value){ID = id;Name = name;Value = value;}
    public override string ToString(){return $"[ID: {ID}] {Name} (Value: {Value})";}
}