public class Int3 {
	public int x = 0;
	public int y = 0;
	public int z = 0;

	public Int3(int x, int y, int z){
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public static Int3 zero(){
		return new Int3(0, 0, 0);
	}

	public static Int3 one(){
		return new Int3(1, 1, 1);
	}

	public int[] toArray(){
		return new int[]{x,y};
	}

	public string toString(){
		return "(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";
	}
}