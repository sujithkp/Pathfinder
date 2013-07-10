/*
 * 
You are in a maze, and you need to find all possible path from an entrance to an exit. Here are the constraints:
    ● The maze is represented by a 2D grid.
    ● Spots that you can step on are represented by a 0.
    ● Pits that you will fall into (aka spots that you cannot step on) are represented by a 1.
    ● The entrance is represented by a 2.
    ● The exit is represented by a 3.
    ● Each path can only have two endpoints; entrance and exit. You cannot use the entrance or exit more than
      once for each path.
    ● You have to step on every spot exactly once.
    ● You can only move like a King in chess (horizontally or vertically but not diagonally)
*/

/*
 * Author : Sujith K.P
 * */

using System;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder
{
    public class Program
    {
        #region Inner Classes

        //Represents each point in the Maze.
        private class Node
        {
            private int val;

            public Node(int num)
            {
                val = num;

                switch (num)
                {
                    case 1: IsPit = true;
                        break;
                    case 2: IsEntranceNode = true;
                        break;
                    case 3:
                        IsExitNode = true;
                        break;
                }
            }
            public string Name { get; set; }

            public bool IsPit { get; private set; }
            public bool IsVisited { get; set; }
            public bool IsExitNode { get; private set; }
            public bool IsEntranceNode { get; private set; }

            public Node UpNode { get; set; }
            public Node DownNode { get; set; }
            public Node LeftNode { get; set; }
            public Node RightNode { get; set; }
        }

        #endregion

        //Number of pits in the maze.
        private int numberOfPits;

        //Stack traces the path traversed.
        private Stack<String> pathTraversed;

        //Represents the maze.All Nodes are wired to its adjacent nodes.
        private Node[,] Maze { get; set; }

        //Number of paths found.
        private int pathCount = 1;

        //displays a set.
        static void printSet(int[,] set)
        {
            Console.Write(" \t");
            for (var i = 0; i < set.GetLength(1); i++)
                Console.Write(i + "  ");

            Console.WriteLine("\n--------------------------");

            for (var i = 0; i < set.GetLength(0); i++)
            {
                Console.Write((char)(65 + i) + " | \t");
                for (var j = 0; j < set.GetLength(1); j++)
                    Console.Write(set[i, j] + "  ");

                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {

            int[,] set1 = new int[3, 5] {   { 2, 0, 0, 0, 0 },
                                            { 0, 0, 0, 0, 0 },
                                            { 0, 0, 0, 3, 1 }
                                        };

            int[,] set2 = new int[7, 7] {   { 2, 0, 0, 0, 0, 0, 0 }, 
                                            { 0, 0, 0, 0, 0, 0, 0 }, 
                                            { 0, 0, 0, 0, 0, 0, 0 }, 
                                            { 0, 0, 0, 0, 0, 0, 0 }, 
                                            { 0, 0, 0, 0, 0, 0, 0 }, 
                                            { 0, 0, 0, 0, 0, 0, 0 }, 
                                            { 0, 0, 0, 0, 3, 1, 1 } 
                                        };

            Console.WriteLine("Set 1");
            Console.WriteLine("=====");
            printSet(set1);
            Console.WriteLine("\nPath found");
            Console.WriteLine("==========");
            var startTime = DateTime.Now;
            new Program().AddPoints(set1).FindPaths();
            var endTime = DateTime.Now;
            var difference = endTime.Subtract(startTime).TotalSeconds;
            Console.WriteLine("\nTime taken in secs : " + difference);

            Console.WriteLine("\n\nSet 2");
            Console.WriteLine("=====");
            printSet(set2);
            Console.WriteLine("\nPath found");
            Console.WriteLine("==========");
            startTime = DateTime.Now;
            new Program().AddPoints(set2).FindPaths();
            endTime = DateTime.Now;
            difference = endTime.Subtract(startTime).TotalMinutes;
            Console.WriteLine("Time taken in mins : " + difference);
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }

        //Adds points to the maze.Transforms a matrix to 2 dimensional connected nodes.
        public Program AddPoints(int[,] matrix)
        {
            Maze = new Node[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    Node node = Maze[i, j];

                    if (node == null)
                    {
                        node = new Node(matrix[i, j]);
                        Maze[i, j] = node;
                    }
                    if (!(i - 1 < 0))
                        Maze[i - 1, j].DownNode = node;

                    if (!(i + 1 > matrix.GetLength(0) - 1))
                    {
                        var downNode = Maze[i + 1, j];
                        if (downNode == null)
                            Maze[i + 1, j] = new Node(matrix[i + 1, j]);
                        Maze[i + 1, j].UpNode = node;
                    }

                    if (!(j - 1 < 0))
                        Maze[i, j - 1].RightNode = node;

                    if (!(j + 1 > matrix.GetLength(1) - 1))
                    {
                        var leftNode = Maze[i, j + 1];
                        if (leftNode == null)
                            Maze[i, j + 1] = new Node(matrix[i, j + 1]);
                        Maze[i, j + 1].LeftNode = node;
                    }

                    node.Name = ((char)(65 + i)).ToString() + j.ToString();
                    if (node.IsPit)
                        numberOfPits++;
                }
            }
            return this;
        }

        /// <summary>
        /// Searches different paths possible between the start node and end node.
        /// </summary>
        public void FindPaths()
        {
            var startNode = Maze[0, 0];

            if (!startNode.IsEntranceNode)
                throw new Exception("Node is not starting node.");

            pathTraversed = new Stack<string>();

            pathTraversed.Push(startNode.Name);
            Traverse(startNode.RightNode);
            Traverse(startNode.DownNode);
            Traverse(startNode.LeftNode);
            Traverse(startNode.UpNode);
        }

        /// <summary>
        /// Traverses a node.
        /// </summary>
        /// <param name="node"></param>
        private void Traverse(Node node)
        {
            if (node == null)
                return;

            if (node.IsEntranceNode || node.IsPit || node.IsVisited)
                return;

            if (node.IsExitNode)
            {
                pathTraversed.Push(node.Name);
                if (pathTraversed.Count == Maze.Length - numberOfPits)
                {
                    var msg = "Path " + pathCount++ + " : " + string.Join("->", pathTraversed.Reverse());
                    Console.WriteLine(msg);
                }
                pathTraversed.Pop();
                return;
            }

            pathTraversed.Push(node.Name);
            node.IsVisited = true;

            Traverse(node.RightNode);   //
            Traverse(node.DownNode);    // Move to Next Node
            Traverse(node.LeftNode);    //  
            Traverse(node.UpNode);      //

            if (node.Name != pathTraversed.Peek())
                throw new Exception("Error in Logic.");

            node.IsVisited = false;
            pathTraversed.Pop();
        }
    }
}