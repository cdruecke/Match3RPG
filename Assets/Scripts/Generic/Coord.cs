using UnityEngine;

[System.Serializable]
public class Coord : System.Object {
	public int x;
	public int y;

	public Coord(int i, int j) {
		x = i;
		y = j;
	}

	public bool Equals(Coord other) {
		if (x == other.x && y == other.y) {
			return true;
		} else {
			return false;
		}
	}

	override public string ToString() {
		return "X: " + x + ", Y: " + y;
	}
}
