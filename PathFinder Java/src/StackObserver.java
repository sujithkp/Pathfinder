import java.util.*;

public class StackObserver implements Observer {
	
	private Plotter plotter;
	
	public StackObserver(Plotter plotter){
		this.plotter = plotter;
	}
	
	public void update(Observable observable, Object obj){
		SubscribedMessage msg = (SubscribedMessage) obj;
		
		if (msg.getAction() == Action.PathFound)
			plotter.showPathFound();
		
		if (msg.getAction() == Action.Added){
			plotter.move(msg.getFromNode(), msg.getToNodde());
		}
		
		if (msg.getAction() == Action.Deleted){
			plotter.returnFrom(msg.getFromNode(), msg.getToNodde());
		}
	}
}
