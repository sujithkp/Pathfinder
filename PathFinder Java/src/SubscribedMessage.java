
public class SubscribedMessage {
	
	private String fromNode;
	
	private String toNode;
	
	private Action action;
	
	public SubscribedMessage(Object fromNode,Object toNode, Action action)	{
		this.fromNode = (String) fromNode;
		this.toNode = (String) toNode;
		this.action = action;
	}
	
	public String getFromNode () {
		return this.fromNode;
	}
	
	public String getToNodde() {
		return this.toNode;
	}
	
	public Action getAction() {
		return this.action;
	}
}
