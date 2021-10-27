using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphBuilder : MonoBehaviour
{
   
    [SerializeField] private bool displayNodes;
    [SerializeField] private Vector2Int offset;
    public Vector2Int gridSize;
    private static Node[,] nodes;
    public static bool[,] obstaclesMap;

    public static GraphBuilder current;

    private void Awake()
    {
        current = this;
    }
    void Start()
    {
        gridSize.x = Mathf.Clamp(gridSize.x, 2, 2000);
        gridSize.y = Mathf.Clamp(gridSize.y, 2, 2000);
        obstaclesMap = new bool[gridSize.x, gridSize.y];
        GraphBuild();
    }
    void GraphBuild()
    {
        nodes = new Node[gridSize.x, gridSize.y];
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                nodes[i, j] = new Node(offset.x * i, offset.y * j);
            }
        }
    }

    public List<Node> GetNeighboors(int i, int j, bool[,] visitedMap)
    {
        List<Node> neighboors = new List<Node>();


        AddAdjNode(neighboors, i + 1, j,visitedMap);
        AddAdjNode(neighboors, i - 1, j, visitedMap);
        AddAdjNode(neighboors, i, j + 1, visitedMap);
        AddAdjNode(neighboors, i, j - 1, visitedMap);

        if (i > 1 && i < gridSize.x - 1 && j > 1 && j < gridSize.y - 1)
        {
            if (!obstaclesMap[i + 1, j] && !obstaclesMap[i, j + 1]) AddAdjNode(neighboors, i + 1, j + 1, visitedMap);
            if (!obstaclesMap[i, j + 1] && !obstaclesMap[i - 1, j]) AddAdjNode(neighboors, i - 1, j + 1, visitedMap);
            if (!obstaclesMap[i - 1, j] && !obstaclesMap[i, j - 1]) AddAdjNode(neighboors, i - 1, j - 1, visitedMap);
            if (!obstaclesMap[i, j - 1] && !obstaclesMap[i + 1, j]) AddAdjNode(neighboors, i + 1, j - 1, visitedMap);
        }

        //AddAdjNode(neighboors, i + 1, j + 1, visitedMap);
        //AddAdjNode(neighboors, i - 1, j + 1, visitedMap);
        //AddAdjNode(neighboors, i - 1, j - 1, visitedMap);
        //AddAdjNode(neighboors, i + 1, j - 1, visitedMap);

        foreach (Node node in neighboors)
        {
            node.cameFrom = new Node(i, j);
        }
        return neighboors;
    }
    void AddAdjNode(List<Node> neighboors, int i, int j, bool[,] visitedMap)
    {
        if (i < 0 || i > gridSize.x - 1 || j < 0 || j > gridSize.y - 1) return;
        if (obstaclesMap[i, j]) return;
        if (visitedMap[i, j]) return;
        neighboors.Add(new Node(i,j));
        visitedMap[i,j] = true;
    }
    private void OnDrawGizmos()
    {
        try
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    if (obstaclesMap[i, j])
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(nodes[i, j].position, Vector3.one * 0.2f);
                    }
                    if (displayNodes)
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawCube(nodes[i, j].position, Vector3.one * 0.1f);
                    }
                }
            }
        }
        catch
        {
            return;
        }
    }
}

[System.Serializable]
public class Node
{
    [SerializeField] private int xComponent, yComponent;
    public int x
    {
        get
        {
            return xComponent;
        }
        set
        {
            xComponent = value;
        }
    }
    public int y
    {
        get
        {
            return yComponent;
        }
        set
        {
            yComponent = value;
        }
    }
    public Node cameFrom;
    public Node(int x, int y)
    {
        this.xComponent = x;
        this.yComponent = y;
    }
    public Node(Vector3 position)
    {
        this.xComponent = Mathf.RoundToInt(position.x);
        this.yComponent = Mathf.RoundToInt(position.z);
    }
    public Node(int x, int y, Node cameFrom)
    {
        this.xComponent = x;
        this.yComponent = y;
        this.cameFrom = cameFrom;
    }
    public Vector3 position
    {
        get
        {
            return new Vector3(x, 0, y);
        }
    }
    public static bool operator !=(Node lhs, Node rhs)
    {
        if (lhs.x != rhs.x || lhs.y != rhs.y) return true;
        return false;
    }
    public static bool operator ==(Node lhs, Node rhs)
    {  
        if (lhs.x == rhs.x && lhs.y == rhs.y) return true;
        return false;
    }
    public override bool Equals(object obj)
    {
        Node node = (Node)obj;
        if (this == node) return true;
        return false;

    }
    public override int GetHashCode()
    {
        return GetHashCode();
    }
}
[System.Serializable]
public class Point
{
    [SerializeField] private int xComponent, yComponent;
    public int x
    {
        get
        {
            return xComponent;
        }
        set
        {
            xComponent = value;
        }
    }
    public int y
    {
        get
        {
            return yComponent;
        }
        set
        {
            yComponent = value;
        }
    }
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public static bool operator !=(Point lhs, Node rhs)
    {
        if (lhs.x != rhs.x || lhs.y != rhs.y) return true;
        return false;
    }
    public static bool operator ==(Point lhs, Node rhs)
    {
        if (lhs.x == rhs.x && lhs.y == rhs.y) return true;
        return false;
    }
    public override bool Equals(object obj)
    {
        Point node = (Point)obj;
        if (this == node) return true;
        return false;

    }
    public override int GetHashCode()
    {
        return GetHashCode();
    }
}
