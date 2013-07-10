import java.util.*;

public class ObservableStack<T> extends Observable implements IPathFinderStack<T>{
	
	private ArrayList<T> visitedNode;
	
	private int index = 0;
	
	public ObservableStack(){
		visitedNode = new ArrayList<T>();
	}
	
	public int getLength (){
		return index;
	}
	
	public  T peek(){	
		return visitedNode.get(index - 1);
	}
	
	public void notifyPathFound(){
		setChanged();
		notifyObservers(new SubscribedMessage(null, null,Action.PathFound));
	}
	
	public T pop(){
		T obj = visitedNode.get(--index);
		T toNode = (index == 0) ? null : peek();
		SubscribedMessage msg = new SubscribedMessage(obj, toNode, Action.Deleted);
		setChanged();
		notifyObservers(msg);
		return obj;
	}
	
	public void push (T obj){
		T fromNode = (index == 0) ? null : peek();
		visitedNode.add(index++, obj);
		SubscribedMessage msg = new SubscribedMessage(fromNode, obj, Action.Added);
		setChanged();		
		notifyObservers(msg);
	}
	
	public String toString(){
		String result = "";
		int i = index - 1;
		
		while (i >= 0)
		{
			result += visitedNode.get(i--);
			if (i != -1)
				result += " , ";
		}
			
		return result;
	}
	
}
