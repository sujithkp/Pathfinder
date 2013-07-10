interface IStack<T> {
	public T peek();
	public T pop();
	public void push (T obj);
	public int getLength ();
}
