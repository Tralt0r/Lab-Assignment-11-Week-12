using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    public GameObject meteorPrefab;
    public GameObject bigMeteorPrefab;
    public CinemachineVirtualCamera cinemachineCamera;
    public bool gameOver = false;

    public int meteorCount = 0;

    private GameObject bigMeteorObject;

    public bool BigMeteorActive
    {
        get 
        { 
            return bigMeteorObject != null; 
        }
    }

    void Awake()
    {
        Instance = this;
    }

    //Start spawning meteors
    void Start()
    {
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        InvokeRepeating("SpawnMeteor", 1f, 2f);
        InvokeRepeating("SpawnEnemy", 2f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            CancelInvoke();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame && gameOver)
        {
            SceneManager.LoadScene("Week5Lab");
        }

        if (meteorCount == 5)
        {
            BigMeteor();
        }
    }

    //Spawn obstacles and enemies
    void SpawnMeteor()
    {
        Instantiate(meteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
    }

    void BigMeteor()
    {
        meteorCount = 0;
        bigMeteorObject = Instantiate(bigMeteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
    }
}