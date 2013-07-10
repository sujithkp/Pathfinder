public class Node
{
	private String name;
	private int val;
	private boolean isPit;
	private boolean isVisited;
	private boolean	isEntrance;
	private boolean isExit;
	
	private Node downNode;
	private Node upNode;
	private Node leftNode;
	private Node rightNode;
	
	public Node(int val){
		this.val = val;
		this.isPit = (val == 1);
		this.isEntrance = (val == 2);
		this.isExit = (val == 3);
		this.isVisited = false;			
	}
	
	public String getName(){
		return this.name;
	}
	
	public void setName (String name) {
		this.name = name;
	}
	
	public boolean isPit(){
		return isPit;
	}
	
	public boolean isVisited(){
		return isVisited;
	}
	
	public void setVisited(boolean isVisited)	{
		this.isVisited = isVisited;
	}
	
	public boolean isEntrance(){
		return isEntrance;
	}
	
	public boolean isExit(){
		return isExit;
	}
	
	public Node getDownNode(){
		return downNode;
	}
	
	public void setDownNode(Node node){
		downNode = node;
	}
	
	public Node getUpNode(){
		return upNode;
	}
	
	public  void setUpNode(Node node){
		upNode = node;
	}
	
	public Node getLeftNode(){
		return leftNode;
	}
	
	public void setLeftNode(Node node){
		leftNode = node;
	}
	
	public Node getRightNode(){
		return rightNode;
	}	
		
	public void setRightNode(Node node)	{
		rightNode = node;
	}
}	