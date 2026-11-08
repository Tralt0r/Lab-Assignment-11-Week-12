public interface ISaveable
{
    string SaveID { get; }
    string SavedData { get; }
    void LoadFromData(string json);
}