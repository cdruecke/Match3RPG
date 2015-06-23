using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public static float TILESIZE = .75F;
	public Coord axes;
	private Board board;
	public int type = 0;
	// Use this for initialization

	void Start () {
		board = GameObject.Find ("Board").GetComponent<Board> ();
	}
	

	public void SetLocation(int a, int b) {
		axes = new Coord (a, b);
	}
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp() {
			if(Board.lastSelected!=null) {
				GameObject temp = GameObject.Find("Board");
				Board sandwich = temp.GetComponent<Board>();
				Tile selected = sandwich.GetTile(axes);
				if(Board.lastSelected==axes) {
					// check rules HERE (probably in Board)
					if(Board.pbNum>0 && sandwich.CheckRules(type, axes.x, axes.y)) {
						Board.pbNum--;
					} else {
						Debug.Log("Not enough peanut butters!");
						return;
					}
				} else {
					return;
				}
				type = selected.type;
				board.DisplayBoard();
			} else {
			Board.lastSelected = axes;
		}
	}

	public int RandomSprite() {
		int i = Random.Range(0, board.tiles.Length);
		type = i;
		return type;
	}
}
