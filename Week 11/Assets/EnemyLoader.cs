using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EnemyLoader : MonoBehaviour
{
    public Dictionary<string, int> enemies = new Dictionary<string, int>();

    void Start()
    {
        LoadEnemies();
    }

    void LoadEnemies()
    {
        string path = Application.streamingAssetsPath + "/enemies.xml";

        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        XmlNodeList enemyNodes = doc.SelectNodes("Enemies/Enemy");

        foreach (XmlNode node in enemyNodes)
        {
            string name = node["Name"].InnerText;
            int hp = int.Parse(node["HP"].InnerText);

            enemies[name] = hp;
        }

        Debug.Log("Enemies loaded: " + enemies.Count);
    }
}