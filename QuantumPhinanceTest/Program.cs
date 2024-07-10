using System;
using System.Collections.Generic;

namespace QuantumPhinanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 5;
            var statements = new List<Tuple<int, int>>
                {
                    Tuple.Create(0, 1),
                    Tuple.Create(1, 2),
                    Tuple.Create(2, 3),
                    Tuple.Create(3, 4)
                };

            var result = CheckConsistencyAndOrder(n, statements);
            Console.WriteLine($"Is the information self-consistent? {(result.Item1 ? "Yes" : "No")}");
            Console.WriteLine($"Is the information enough to arrange the people in increasing order of height? {(result.Item2 ? "Yes" : "No")}");
            Console.WriteLine($"Who is the shortest person? {(result.Item3 != -1 ? result.Item3.ToString() : "Not enough information")}");
        }

        static Tuple<bool, bool, int> CheckConsistencyAndOrder(int n, List<Tuple<int, int>> statements)
        {
            var adjList = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                adjList[i] = new List<int>();
            }

            foreach (var statement in statements)
            {
                adjList[statement.Item1].Add(statement.Item2);
            }

            bool[] visited = new bool[n];
            bool[] recStack = new bool[n];
            List<int> topoSort = new List<int>();

            for (int i = 0; i < n; i++)
            {
                if (!visited[i] && IsCyclic(i, adjList, visited, recStack))
                {
                    return Tuple.Create(false, false, -1);
                }
            }

            visited = new bool[n];
            for (int i = 0; i < n; i++)
            {
                if (!visited[i])
                {
                    TopoSortDFS(i, adjList, visited, topoSort);
                }
            }

            topoSort.Reverse();

            bool isFullyConnected = topoSort.Count == n;

            int shortestPerson = isFullyConnected ? topoSort[0] : -1;

            return Tuple.Create(true, isFullyConnected, shortestPerson);
        }

        static bool IsCyclic(int v, List<int>[] adjList, bool[] visited, bool[] recStack)
        {
            if (!visited[v])
            {
                visited[v] = true;
                recStack[v] = true;

                foreach (int neighbor in adjList[v])
                {
                    if (!visited[neighbor] && IsCyclic(neighbor, adjList, visited, recStack))
                    {
                        return true;
                    }
                    else if (recStack[neighbor])
                    {
                        return true;
                    }
                }
            }

            recStack[v] = false;
            return false;
        }

        static void TopoSortDFS(int v, List<int>[] adjList, bool[] visited, List<int> topoSort)
        {
            visited[v] = true;
            foreach (int neighbor in adjList[v])
            {
                if (!visited[neighbor])
                {
                    TopoSortDFS(neighbor, adjList, visited, topoSort);
                }
            }
            topoSort.Add(v);
        }
    }
}
