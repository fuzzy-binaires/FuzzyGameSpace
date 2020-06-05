public class Int2 {
	public int x = 0;
	public int y = 0;

	public Int2(int x, int y){
		this.x = x;
		this.y = y;
	}

	public static Int2 zero(){
		return new Int2(0, 0);
	}

	public static Int2 one(){
		return new Int2(1, 1);
	}

	public int[] toArray(){
		return new int[]{x,y};
	}

	public string toString(){
		return "(" + x.ToString() + ", " + y.ToString() + ")";
	}
}