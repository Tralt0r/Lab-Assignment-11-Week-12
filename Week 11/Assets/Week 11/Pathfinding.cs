using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    //Start and Goal
    [SerializeField] private Vector2Int start = new Vector2Int(0, 1);
    [SerializeField] private Vector2Int goal = new Vector2Int(4, 4);

    //Grid settings
    [SerializeField] private int randomWidth = 10;
    [SerializeField] private int randomHeight = 10;
    [Range(0f, 1f)]
    [SerializeField] private float obstacleProbability = 0.25f;
    private const float cellSize = 1f;
    private static readonly Vector2Int[] directions =
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    private int[,] grid =
    {
        { 0, 1, 0, 0, 0 },
        { 0, 1, 0, 1, 0 },
        { 0, 0, 0, 1, 0 },
        { 0, 1, 1, 1, 0 },
        { 0, 0, 0, 0, 0 }
    };

    //Above is part of the orginal script
    private readonly List<Vector2Int> path = new List<Vector2Int>();
    private readonly Queue<Vector2Int> frontier = new Queue<Vector2Int>();
    private readonly Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

    private int Cols => grid.GetLength(1);
    private int Rows => grid.GetLength(0);

    private void Start(){FrameCamera();FindPath();}

    //If press G make new grid and shift to click to add obstacle
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)){GenerateRandomGrid(randomWidth, randomHeight, obstacleProbability); return;}
        bool left = Input.GetMouseButtonDown(0);
        bool right = Input.GetMouseButtonDown(1);
        if (!left && !right) return;

        if (!TryGetCellUnderMouse(out Vector2Int cell)) return;
        if (left && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))){AddObstacle(cell);}
        else if (left){SetStart(cell);}
        else{SetGoal(cell);}
    }

    private Vector3 CellToWorld(Vector2Int cell)
    {
        return transform.position + new Vector3(
            (cell.x - (Cols - 1) * 0.5f) * cellSize,
            0f,
            (cell.y - (Rows - 1) * 0.5f) * cellSize);
    }

    private bool TryGetCellUnderMouse(out Vector2Int cell)
    {
        cell = default;
        Camera cam = Camera.main;
        if (cam == null) return false;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, transform.position);
        if (!ground.Raycast(ray, out float distance)) return false;

        Vector3 local = ray.GetPoint(distance) - transform.position;
        cell = new Vector2Int(Mathf.RoundToInt(local.x / cellSize + (Cols - 1) * 0.5f), Mathf.RoundToInt(local.z / cellSize + (Rows - 1) * 0.5f));
        return IsInBounds(cell);
    }

    private void FrameCamera()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float halfW = Cols * cellSize * 0.5f + 1;
        float halfH = Rows * cellSize * 0.5f + 1;
        float height;
        if (cam.orthographic){cam.orthographicSize = Mathf.Max(halfH, halfW / cam.aspect); height = 10f;}
        else{float tan = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad); height = Mathf.Max(halfH / tan, halfW / (tan * cam.aspect));}
        cam.transform.SetPositionAndRotation(transform.position + Vector3.up * height, Quaternion.Euler(90f, 0f, 0f));
    }

    public void SetStart(Vector2Int position)
    {
        if (!IsWalkable(position) || position == start) return;
        start = position;
        FindPath();
    }

    public void SetGoal(Vector2Int position)
    {
        if (!IsWalkable(position) || position == goal) return;
        goal = position;
        FindPath();
    }

    public void AddObstacle(Vector2Int position)
    {
        if (!IsInBounds(position) || position == start || position == goal) return;
        if (grid[position.y, position.x] == 1) return;

        grid[position.y, position.x] = 1;
        FindPath();
    }

    [ContextMenu("Generate Random Grid")]
    private void GenerateRandomGridFromInspector(){GenerateRandomGrid(randomWidth, randomHeight, obstacleProbability);}
    //Make a random grid with random blockers
    public void GenerateRandomGrid(int width, int height, float obstacleProbability)
    {
        width = Mathf.Max(1, width);
        height = Mathf.Max(1, height);
        obstacleProbability = Mathf.Clamp01(obstacleProbability);

        grid = new int[height, width];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                grid[y, x] = Random.value < obstacleProbability ? 1 : 0;

        //Keep start/goal inside the grid
        start = new Vector2Int(Mathf.Clamp(start.x, 0, width - 1), Mathf.Clamp(start.y, 0, height - 1));
        goal = new Vector2Int(Mathf.Clamp(goal.x, 0, width - 1), Mathf.Clamp(goal.y, 0, height - 1));
        if (start == goal) goal = new Vector2Int(width - 1, height - 1);

        grid[start.y, start.x] = 0;
        grid[goal.y, goal.x] = 0;

        FrameCamera();
        FindPath();
    }

    //Draw gizmos for grid and blockers
    private void OnDrawGizmos()
    {
        Vector3 size = new Vector3(cellSize, 0.1f, cellSize);

        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Cols; x++)
            {
                Gizmos.color = grid[y, x] == 1 ? Color.black : Color.white;
                Gizmos.DrawCube(CellToWorld(new Vector2Int(x, y)), size);
            }
        }

        //Color path
        Gizmos.color = Color.blue;
        foreach (Vector2Int step in path){Gizmos.DrawCube(CellToWorld(step), size);}
        Gizmos.color = Color.green;
        Gizmos.DrawCube(CellToWorld(start), size);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(CellToWorld(goal), size);
    }

    private bool IsInBounds(Vector2Int position){return position.x >= 0 && position.x < Cols && position.y >= 0 && position.y < Rows;}
    private bool IsWalkable(Vector2Int position){return IsInBounds(position) && grid[position.y, position.x] == 0;}
    //Find path to goal from start
    private void FindPath()
    {
        path.Clear();

        if (!IsWalkable(start) || !IsWalkable(goal)) return;

        frontier.Clear();
        cameFrom.Clear();
        frontier.Enqueue(start);
        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();
            if (current == goal) break;

            foreach (Vector2Int dir in directions)
            {
                Vector2Int next = current + dir;
                if (IsWalkable(next) && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        for (Vector2Int step = goal; step != start; step = cameFrom[step])
            path.Add(step);
        path.Add(start);
        path.Reverse();
    }
}