using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    [SerializeField] private GraphBuilder graph;
    [SerializeField] private List<Node> closed;
    [SerializeField] private List<Node> open;
    [SerializeField] private List<Node> path;

    private bool[,] visitedMap;
    

    void Start()
    {
        graph = GraphBuilder.current;
    }
    public bool BestFirstSearch(Node root, Node goal)
    {
        visitedMap = new bool[graph.gridSize.x, graph.gridSize.y];
        List<Node> nghb;
        closed = new List<Node>();
        open = new List<Node>();
        closed.Add(root);
        nghb = graph.GetNeighboors(root.x, root.y,visitedMap);
        open.AddRange(nghb);

        while (open.Count > 0)
        {
            int h = ChebyshevDistance(open[0], goal);
            int minHindex = 0;
            for (int i = 0; i < open.Count; i++)
            {
                if (ChebyshevDistance(open[i], goal) < h)
                {
                    h = ChebyshevDistance(open[i], goal);
                    minHindex = i;
                }
            }
            closed.Add(open[minHindex]);
            if (open[minHindex] == goal)
            {
                path = closed;
                return true;
            }
            nghb = graph.GetNeighboors(open[minHindex].x, open[minHindex].y, visitedMap);
            open.RemoveAt(minHindex);
            open.AddRange(nghb);

        }
        return false;
    }
    public List<Node> PathRestore(List<Node> closed)
    {
        path = new List<Node>();
        Node curNode = closed[closed.Count - 1];
        path.Add(curNode);
        while (curNode != closed[0])
        {
            for (int i = 0; i < closed.Count; i++)
            {
                if (curNode.cameFrom == closed[i])
                {
                    curNode = closed[i];
                    path.Add(curNode);
                    break;
                }
            }

        }
        path.Reverse();
        return path;
    }

    public List<Node> GetPath()
    {
        return path;
    }
    public void SimplifyPath()
    {
        int c = 0;
        while (path.Count - 2 > c)
        {

            if (path.Count <= 2) return;
            Vector2 direction1 = new Vector2();
            direction1.x = path[c + 1].x - path[c].x;
            direction1.y = path[c + 1].y - path[c].y;

            Vector2 direction2 = new Vector2();
            direction2.x = path[c + 2].x - path[c + 1].x;
            direction2.y = path[c + 2].y - path[c + 1].y;

            direction1.Normalize();
            direction2.Normalize();

            if (Vector2.Dot(direction1, direction2) > 0.99f)
            {
                path.Remove(path[c + 1]);
            }
            else
            {
                c++;
            }
        }
    }
    public void FindPath(Node start, Node end)
    {
        if(BestFirstSearch(start, end))
        {
            path = PathRestore(closed);
            SimplifyPath();
        }
        else
        {
            path = new List<Node>();
        }
    }
    public static int ChebyshevDistance(Node root, Node goal)
    {
        return Mathf.Max(Mathf.Abs(root.x - goal.x), Mathf.Abs(root.y - goal.y));
    }
}
