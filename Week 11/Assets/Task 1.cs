using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Task1 : MonoBehaviour
{
    public Queue<string> loginQueue = new Queue<string>();

    public static string[] firstNames = {
        "Adam","Maria","John","Leila","Angel","George","Ethan","Noah","Liam","Maddie",
        "Olivia","Ava","Mason","Lucas","Joey","Manny","James","Amelia","Logan","Xavier"
    };

    public static char[] lastInitials = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    void Start()
    {
        for (int i = 0; i < Random.Range(4, 7); i++)
        {
            loginQueue.Enqueue(GetRandomPlayerName());
        }

        Debug.Log($"Initial login queue created. There are {loginQueue.Count} players in the queue: {string.Join(", ", loginQueue.ToList())}");

        StartCoroutine(AddPlayerRoutine());
        StartCoroutine(LoginPlayerRoutine());
    }

    string GetRandomPlayerName()
    {
        string first = firstNames[Random.Range(0, firstNames.Length)];
        char last = lastInitials[Random.Range(0, lastInitials.Length)];
        return $"{first} {last}.";
    }

    IEnumerator AddPlayerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            string newPlayer = GetRandomPlayerName();
            loginQueue.Enqueue(newPlayer);

            Debug.Log($"{newPlayer} is trying to login and added to the login queue.");
        }
    }

    IEnumerator LoginPlayerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            if (loginQueue.Count > 0)
            {
                string player = loginQueue.Dequeue();
                Debug.Log($"{player} is now inside the game.");
            }
            else
            {
                Debug.Log("Login server is idle. No players are waiting.");
            }
        }
    }
}