using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public static float TILESIZE = .75F;
	public Coord axes;
	private Board board;
	public bool settled;
	public int type = 0;
	// Use this for initialization

	void Start () {
		board = GameObject.Find ("Board").GetComponent<Board> ();
		settled = true;
	}
	

	public void SetLocation(int a, int b) {
		axes = new Coord (a, b);
	}
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp() {
			print(board.TestMatch(type, axes));
	}

	public bool Match() {
		type = 0;
		settled = false;
		return true;
	}

	public int RandomSprite() {
		int i = Random.Range(1, board.tiles.Length);
		type = i;
		return type;
	}
}
