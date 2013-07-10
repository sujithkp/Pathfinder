import java.awt.*;
import java.applet.*;
import java.util.*;

public class PathFinder extends Applet {
	
	public void init() {
	}
	
	public static void main(String[] args){
		
		int[][]	set1 = new int[][] {{ 2, 0, 0, 0, 0, 0, 0 }, 
							        { 0, 0, 0, 0, 0, 0, 0 }, 
							        { 0, 0, 0, 0, 0, 0, 0 }, 
							        { 0, 0, 0, 0, 0, 0, 0 }, 
							        { 0, 0, 0, 0, 0, 0, 0 }, 
							        { 0, 0, 0, 0, 0, 0, 0 }, 
							        { 0, 0, 0, 0, 3, 1, 1 }};
							    
		int [][] set2 = new int[][]{{ 2, 0, 0, 0, 0 },
                                    { 0, 0, 0, 0, 0 },
                                    { 0, 0, 0, 3, 1 }};
                                    
                                    
		ObservableStack<String> observableStack = new ObservableStack<String>();
	 	Plotter plotter = new Plotter(null);
       	StackObserver stackObserver = new StackObserver(plotter);
       	//observableStack.addObserver(stackObserver);
       	
       	Maze maze = new Maze(set1,(IPathFinderStack<String>)observableStack);
		
		maze.findPaths();					
	}

	public void paint(Graphics g) {
		
		String setNumber = getParameter("Set");
		
		System.out.println("SetNumber : " + setNumber);
		
        int[][] set1 = new int[][]{	{ 2, 0, 0, 0, 0 },
                                    { 0, 0, 0, 0, 0 },
                                    { 0, 0, 0, 3, 1 } 	};
        if (!setNumber.equals("1"))                      
        {
        	System.out.println("selecting set2");
	        set1 = new int[][] {    { 2, 0, 0, 0, 0, 0, 0 }, 
							        { 0, 0, 0, 0, 0, 0, 0 }, 
							        { 0, 0, 0, 0, 0, 0, 0 }, 
							        { 0, 0, 0, 0, 0, 0, 0 }, 
							        { 0, 0, 0, 0, 0, 0, 0 }, 
							        { 0, 0, 0, 0, 0, 0, 0 }, 
							        { 0, 0, 0, 0, 3, 1, 1 } 
							    };
        }
                                            
        Plotter plotter = new Plotter(g);
        
       	ObservableStack<String> observableStack = new ObservableStack<String>();
       	StackObserver stackObserver = new StackObserver(plotter);
       	observableStack.addObserver(stackObserver);
       	
       	Maze maze = new Maze(set1,(IPathFinderStack<String>)observableStack);
       	plotter.setVector(maze.getVector());
       	
       	maze.findPaths();
	}
}
