using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace practice
{
	public class MazePathTest
	{
		Stack<Cell> stack = new Stack<Cell>(47);
		List<int> result = new List<int>();
		Maze maze;
		public void RunTest()
		{
			maze = GetTestMaze1();
			var start = maze.StartCell;
			start.IsVisited = true;
			stack.Push(start);
			Traverse(start.TakeRight());
			
			var startTime = DateTime.Now;
			Console.WriteLine(startTime);
			//Traverse();
			var endTime = DateTime.Now;
			var overall = endTime.Subtract(startTime).TotalMinutes; ;

			//PrintResult();
			Console.WriteLine(string.Format("Overall {0} paths found.", result.Count));
			Console.WriteLine(overall);
			Console.ReadKey();
		}

		private Maze GetTestMaze1()
		{
			int[,] values = { 
								{ 2, 0, 0, 0, 0 }, 
								{ 0, 0, 0, 0, 0 }, 
								{ 0, 0, 0, 3, 1 } 
							};

			return new Maze(3, 5, values);
		}

		private Maze GetTestMaze2()
		{
			int[,] values = { 
								{2, 0, 0, 0, 0, 0, 0},
								{0, 0, 0, 0, 0, 0, 0},
								{0, 0, 0, 0, 0, 0, 0},
								{0, 0, 0, 0, 0, 0, 0},
								{0, 0, 0, 0, 0, 0, 0},
								{0, 0, 0, 0, 0, 0, 0},
								{0, 0, 0, 0, 3, 1, 1} 
							};
			return new Maze(7, 7, values);
		}

		private void PrintResult()
		{
			foreach (var path in result)
			{
				Console.WriteLine("{"+path+"}");
			}
			
		}

		private void Traverse(Cell cell)
		{
			int counter =1;
			do
			{
				counter++;
				if (cell == null || (!cell.CanStep()) || cell.IsVisited)
				{  }
				else if (cell.IsEnd())
				{
					if (stack.Count + 1 == maze.MaxPathLength)
					{
						result.Add(1);
						Console.WriteLine("{0}", counter);
						Console.WriteLine("Another path found, total count is {0}.", result.Count);
					}				
				}
				else
				{
					cell.IsVisited = true;
					stack.Push(cell);
				}
				
				cell = stack.Peek();

				if (!cell.RightVisited)
				{ cell = cell.TakeRight(); continue; }
				if (!cell.BottomVisited)
				{ cell = cell.TakeBottom(); continue; }
				if (!cell.LeftVisited)
				{ cell = cell.TakeLeft(); continue; }
				if (!cell.TopVisited)
				{ cell = cell.TakeTop(); continue; }
				
				cell = stack.Pop();
				if (cell != null) cell.ResetVisited();
				if(stack.Count>0)
					cell = stack.Peek();
			} while (stack.Count > 0);			
		}

		
	}

	

	class Cell
	{
		int _row;
		public int Row { get { return _row; } }
		int _col;
		public int Col { get { return _col; } }
		int _val;
		public int Value { get { return _val; } }

		public bool LeftVisited { get; set; }
		public bool RightVisited { get; set; }
		public bool TopVisited { get; set; }
		public bool BottomVisited { get; set; }

		public bool IsVisited { get; set; }

		public void ResetVisited()
		{
			LeftVisited = RightVisited = TopVisited = BottomVisited = IsVisited = false;
		}

		public bool CanStep() { return Value != 1; }
		public bool IsStart() { return Value == 2; }
		public bool IsEnd() { return Value == 3; }

		public Cell Left { get; set; }
		public Cell Right { get; set; }
		public Cell Top { get; set; }
		public Cell Bottom { get; set; }

		public Cell TakeLeft() { LeftVisited = true; return Left; }
		public Cell TakeRight() { RightVisited = true; return Right; }
		public Cell TakeTop() { TopVisited = true; return Top; }
		public Cell TakeBottom() { BottomVisited = true; return Bottom; }

		public Cell(int row, int col, int val)
		{
			_row = row;
			_col = col;
			_val = val;
		}

		public override string ToString()
		{
			return "{" + _row + ", " + _col + "}";
		}
		
	}

	class Maze
	{
		Cell[,] _cells;
		public Cell[,] Cells { get { return _cells; } }
		int _rowCnt;
		public int Rows { get { return _rowCnt; } }
		int _colCnt;
		public int Columns { get { return _colCnt; } }

		Cell _startCell;
		public Cell StartCell { get { return _startCell; } }
		Cell _endCell;
		public Cell EndCell { get { return _endCell; } }

		public bool IsBorderCell(Cell cell)
		{
			return cell.Row == 0 || cell.Col == 0 || cell.Row == _rowCnt - 1 || cell.Col == _colCnt - 1;
		}

		private bool IsFirstRow(Cell cell) { return cell.Row == 0; }
		private bool IsFirstCol(Cell cell) { return cell.Col == 0; }
		private bool IsLastRow(Cell cell) { return cell.Row == _rowCnt - 1; }
		private bool IsLastCol(Cell cell) { return cell.Col == _colCnt - 1; }
		int _maxPathLength;
		public int MaxPathLength { get { return _maxPathLength; } }

		public Maze(int rowCount, int colCount, int[,] values)
		{
			_cells = new Cell[rowCount, colCount];
			_rowCnt = rowCount;
			_colCnt = colCount;
			var pits = 0;
			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < colCount; j++)
				{
					var cell = _cells[i, j] = new Cell(i, j, values[i, j]);
					if (cell.IsStart()) _startCell = cell;
					if (cell.IsEnd()) _endCell = cell;
					if (!cell.CanStep()) pits++;					
				}
			}

			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < colCount; j++)
				{
					var cell = _cells[i, j];
					cell.Left = StepLeft(cell);
					cell.Right = StepRight(cell);
					cell.Top = StepUp(cell);
					cell.Bottom = StepDown(cell);
				}
			}
			_maxPathLength = (_rowCnt * _colCnt) - pits;
		}

		private Cell StepLeft(Cell cell)
		{
			if (IsFirstCol(cell)) return null;
			return _cells[cell.Row, cell.Col - 1];
		}

		private Cell StepRight(Cell cell)
		{
			if (IsLastCol(cell)) return null;
			return _cells[cell.Row, cell.Col + 1];
		}

		private Cell StepUp(Cell cell)
		{
			if (IsFirstRow(cell)) return null;
			return _cells[cell.Row - 1, cell.Col];
		}

		private Cell StepDown(Cell cell)
		{
			if (IsLastRow(cell)) return null;
			return _cells[cell.Row + 1, cell.Col];
		}
	}
}
