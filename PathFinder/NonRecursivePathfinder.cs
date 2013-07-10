using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PathFinder
{
    public class NonRecursivePathfinder
    {

        private class Node
        {
            private int val;
            private int numberOfBranchesAvailable;
            private int branchesConsumed;
            private List<Node> branches = new List<Node>(4);

            private Node leftNode;
            private Node rightNode;
            private Node upNode;
            private Node downNode;

            public Node(int val)
            {
                this.val = val;

                IsStartNode = (val == 2);
                IsEndNode = (val == 3);
                IsPit = (val == 1);
            }

            public string Name { get; set; }

            public bool IsVisited { get; set; }

            public bool IsStartNode { get; private set; }

            public bool IsEndNode { get; private set; }

            public bool IsPit { get; private set; }

            public Node LeftNode
            {
                set
                {
                    numberOfBranchesAvailable++;
                    leftNode = value;
                    branches.Add(leftNode);
                }
            }

            public Node RightNode
            {
                set
                {
                    numberOfBranchesAvailable++;
                    rightNode = value;
                    branches.Add(rightNode);
                }
            }

            public Node UpNode
            {
                set
                {
                    numberOfBranchesAvailable++;
                    upNode = value;
                    branches.Add(upNode);
                }
            }

            public Node DownNode
            {
                set
                {
                    numberOfBranchesAvailable++;
                    downNode = value;
                    branches.Add(downNode);
                }
            }

            public Node getNextBranch()
            {
                while (branchesConsumed != numberOfBranchesAvailable)
                {
                    var node = branches[branchesConsumed++];
                    if (!node.IsVisited)
                        return node;
                }

                branchesConsumed = 0;
                return null;
            }

            public override string ToString()
            {
                return this.Name;
            }
        }

        private int numberOfPits;
        private Node[,] Maze { get; set; }

        public NonRecursivePathfinder AddPoints(int[,] matrix)
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

        public int FindPaths()
        {
            return Start(Maze[0, 0]);
        }

        private int Start(Node startNode)
        {
            int pathsFound = 0;
            Stack<Node> stack = new Stack<Node>();
            Stack<Node> pathTraversed = new Stack<Node>();

            Node currNode = startNode;

            while (currNode != null)
            {
                if (currNode.IsVisited)
                {
                    currNode = stack.Pop();
                    continue;
                }

                pathTraversed.Push(currNode);
                stack.Push(currNode);

                if (currNode.IsEndNode)
                {
                    if (pathTraversed.Count == Maze.Length - numberOfPits)
                    {
                        pathsFound++;
                        pathTraversed.Pop();
                        var fs = new StreamWriter("d:\\pf.txt", true);
                        fs.WriteLine(string.Join("->", pathTraversed.Reverse()));
                        fs.Close();
                        currNode = stack.Pop();
                        currNode.IsVisited = false;
                        currNode = stack.Pop();
                        currNode.IsVisited = false;
                        while (null == (currNode = currNode.getNextBranch()))
                        {
                            currNode = stack.Pop();
                            currNode.IsVisited = false;
                        }
                        continue;
                    }
                }

                currNode.IsVisited = true;

                var nextBranchOfThisNode = currNode = currNode.getNextBranch();

                if (nextBranchOfThisNode == null)
                {
                    currNode.IsVisited = false;
                    currNode = stack.Pop();
                }
            }

            return pathsFound;
        }
    }
}
