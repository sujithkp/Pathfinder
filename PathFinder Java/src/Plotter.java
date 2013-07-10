import java.awt.*;
import java.util.*;

public class Plotter {
	
	private class NodeInfo {
		
		private String name;
		private Point textCordinate;
		private Point circleCordinate;
				
		public NodeInfo(String name, Point textCordinate, Point circleCordinate) {
			this.name = name;
			this.textCordinate = textCordinate;
			this.circleCordinate = circleCordinate;
		}
		
		public Point getCircleCordinate(){
			return this.circleCordinate;
		}
		
		public Point getTextCordinate(){
			return this.textCordinate;
		}
		
		public String getName()	{
			return this.name;
		}
		
	}
	
	private int DIAMETER = 25;	
	private int UPPER_BORDER = 50;	
	private int BOTTOM_BORDER = 50;	
	private int LEFT_BORDER = 50;	
	private int RIGHT_BORDER = 50;	
	private int X_DISTANCE = 100;	
	private int Y_DISTANCE = 100;
	
	private int pathCount = 0;
			
	private Graphics graphicsObj;
	private Dictionary<String,NodeInfo> nodeDetails;
	private String exitNode;
	
	public Plotter (Graphics g)	{
		this.graphicsObj = g;
		nodeDetails = new Hashtable<String,NodeInfo>();
	}
	
	private Point drawCircleFor(Point point)
	{
		int x1 = (int) point.getX() - 7;
		int y1 = (int) point.getY() - 17;
		int length = DIAMETER;
		int height = DIAMETER;
		
		graphicsObj.drawOval(x1,y1,length,height);
		return new Point (x1,y1);
	}
	
	private void fillCircle(String nodeName, Color fillColor) {
		
		NodeInfo nodeInfo = nodeDetails.get(nodeName);
		
		graphicsObj.setColor(fillColor);
		int x = (int) nodeInfo.getCircleCordinate().getX();
		int y = (int) nodeInfo.getCircleCordinate().getY();
		graphicsObj.fillOval(x,y,DIAMETER,DIAMETER);
		graphicsObj.setColor(Color.BLACK);
		Point textCordinates = nodeInfo.getTextCordinate();
		drawCircleFor(textCordinates);
		graphicsObj.drawString(nodeInfo.getName(), (int) textCordinates.getX(),(int) textCordinates.getY());	
	}
	
	private void showNodeAsActive(String nodeName){
		Logger.Log("make " + nodeName + " as active.");
		fillCircle(nodeName,Color.GREEN);
	}
	
	private void showNodeAsInActive(String nodeName){
		Logger.Log("make " + nodeName + " as active.");
		fillCircle(nodeName,Color.ORANGE);
	}
	
	private void showNodeAsPit(String nodeName) {
		fillCircle(nodeName,Color.RED);
	}
	
	private void showNodeAsExit (String nodeName){
		fillCircle(nodeName,Color.BLUE);
	}
	
	private void showNodeAsReleased(String nodeName){
		if (nodeName == exitNode)
			fillCircle(nodeName, Color.BLUE);
		else
			fillCircle(nodeName,Color.WHITE);
	}
	
	public void setVector(Node[][] vector) {
		
		X_DISTANCE = (int)(graphicsObj.getClipBounds().getWidth() - (LEFT_BORDER + RIGHT_BORDER)) / vector[0].length;
		Y_DISTANCE = (int)(graphicsObj.getClipBounds().getHeight() - (UPPER_BORDER + BOTTOM_BORDER)) / vector.length;
		
		for (int i = 0; i < vector.length; i++)
		{
			for (int j = 0; j < vector[0].length; j++)
			{
				Node node = vector[i][j];
				String nodeName = node.getName();
				
				Point textCordinate = new Point(LEFT_BORDER + (j * X_DISTANCE),UPPER_BORDER + (i * Y_DISTANCE));
				graphicsObj.drawString(nodeName,(int) textCordinate.getX(), (int) textCordinate.getY());
				Point circleStart = drawCircleFor(textCordinate);				
				nodeDetails.put(nodeName, new NodeInfo(nodeName, textCordinate, circleStart));
				
				if (node.isPit())
					showNodeAsPit(nodeName);
					
				if (node.isExit()){
					showNodeAsExit(nodeName);
					exitNode = nodeName;
				}
				
				if (node.isEntrance())
					graphicsObj.drawString("Start",(int) textCordinate.getX() - 4, (int) textCordinate.getY() - DIAMETER);
				else if (node.isExit())
					graphicsObj.drawString("Exit",(int) textCordinate.getX() - 4, (int) textCordinate.getY() - DIAMETER);
			}
		}
	}
	
	private void drawLine(String from , String to, Color color){
		
		Point p1 = ((NodeInfo)nodeDetails.get(from)).getCircleCordinate();
		Point p2 = ((NodeInfo)nodeDetails.get(to)).getCircleCordinate();
		
		int x1 = (int) p1.getX();
		int y1 = (int) p1.getY();
		
		int x2 = (int) p2.getX();
		int y2 = (int) p2.getY();
		
		Color defaultColor = graphicsObj.getColor();
		graphicsObj.setColor(color);

		if (y1 == y2)
		{
			y1 = y1 + DIAMETER / 2;
			y2 = y2 + DIAMETER / 2;
			
			if (x2 > x1){
				x1 = x1 + DIAMETER;
			}
			else
			{
				x2 = x2 + DIAMETER;
			}
			graphicsObj.drawLine(x1,y1,x2,y2);		
		}
		else
		{
			x1 = x1 + DIAMETER /2;
			x2 = x2 + DIAMETER /2;
			
			if (y2 > y1){
				y1 = y1 + DIAMETER;
			}
			else {
				y2 = y2 + DIAMETER;
			}
			graphicsObj.drawLine(x1,y1,x2,y2);		
		}
		
		graphicsObj.setColor(defaultColor);	
	}
	
	private void showLine(String from , String to){
		drawLine(from,to, Color.BLACK);		
	}
	
	private void removeLine(String from , String to){
		drawLine(from,to, Color.WHITE);
	}
	
	public void move(String from, String to)
	{
		if (from == null)
		{
			showNodeAsActive(to);
			return;
		}
			
		Logger.Log("Moved from " + from + " to " + to);
		showNodeAsInActive(from);
		showNodeAsActive(to);
		
		showLine(from,to);
		
		try{
			Thread.sleep(100);
		}catch (Exception ex) {}
	}
	
	public void showPathFound(){
		graphicsObj.drawString( "                           ", 50,(int) graphicsObj.getClipBounds().getHeight() - BOTTOM_BORDER + 10);
		graphicsObj.drawString( ++pathCount + " paths found.", 50,(int) graphicsObj.getClipBounds().getHeight() - BOTTOM_BORDER + 10);
		try{
			Thread.sleep(5000);
		}catch (Exception ex) {}
	}		
	
	public void returnFrom (String from, String to)
	{
		Logger.Log("Return from " + from + " to " + to);
		showNodeAsReleased(from);
		showNodeAsActive(to);
		removeLine(from,to);
		
		try{
			Thread.sleep(100);
		}catch (Exception ex) {}
		
	}		
}
