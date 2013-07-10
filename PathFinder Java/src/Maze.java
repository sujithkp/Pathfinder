public class Maze {
	
	private Node[][] nodes;

	IPathFinderStack<String> pathTraversed;
	
	private int numberOfPits = 0;
	
	private  int result = 0;

	public  Node[][] getVector(){
		return nodes;
	}

	public Maze(int[][] matrix ,IPathFinderStack<String> stack){
		pathTraversed = stack;
		addNodes(matrix);
	}	
	
	public void addNodes(int[][] matrix) 	{
		
		int rows = matrix.length;
		int cols = matrix[0].length;
		
		nodes = new Node[rows][cols];
		
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				Node node = nodes[i][j];
				
				if (node == null){
					node = new Node(matrix[i][j]);
					nodes[i][j] = node;
				}
				
				if (!(i - 1 < 0))
					nodes[i-1][j].setDownNode(node);
				
				if (!((i + 1) > rows - 1))
				{
					Node downNode = nodes[i+1][j];
					if (downNode == null)
						nodes[i + 1][j] = new Node(matrix[i+1][j]);
					nodes[i + 1][j].setUpNode(node);
				}
				
				if (!(j - 1  < 0))
					nodes[i][j-1].setRightNode(node);
				
				if (!(j + 1 > cols - 1)){
					Node leftNode = nodes[i][j+1];
					if (leftNode == null)
						nodes[i][j+1] = new Node(matrix[i][j+1]);
					nodes[i][j+1].setLeftNode(node);
				}
				
				String name =  new String(new char[] {(char) (65 + i), (char)(48 + j)});
				
				node.setName(name);
				
				if (node.isPit())
					this.numberOfPits ++;
			}
		}
		Logger.Log("number of pits = " + numberOfPits);
	}
	
	public void findPaths(){	
		
		Node startNode = nodes[0][0];
		
		pathTraversed.push(startNode.getName());
		
		Traverse(startNode.getRightNode());
		Traverse(startNode.getDownNode());
		Traverse(startNode.getLeftNode());
		Traverse(startNode.getUpNode());
	}	
		
	private long traverseCount = 0;
	
	private void Traverse(Node node){
		
	//	traverseCount++;
	//	Logger.Log("traverseCount = " + traverseCount);
		
		if (node == null)
			return ;
		
		if (node.isEntrance() || node.isVisited() || node.isPit())
			return ;
			
		if (node.isExit()){
		
		//	Logger.Log("pathTraversed.length = " + pathTraversed.getLength());
		//	Logger.Log(" " + ((nodes[0].length * nodes.length) - numberOfPits));
			
			pathTraversed.push(node.getName());
			if (pathTraversed.getLength() == (nodes[0].length * nodes.length) - numberOfPits){
				String msg = "Path found" + (++result);
				Logger.Log(msg);
				Logger.Log(pathTraversed.toString());
				pathTraversed.notifyPathFound();
			}
		 	
		 	pathTraversed.pop();
		 	return;				
		}
		
		pathTraversed.push(node.getName());
		node.setVisited(true);
		
		Traverse(node.getRightNode());
		Traverse(node.getDownNode());
		Traverse(node.getLeftNode());
		Traverse(node.getUpNode());
		
		node.setVisited(false);
		pathTraversed.pop();		
	}
}
