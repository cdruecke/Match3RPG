using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {
	bool initialized = false;
	TextAsset loadedLevel;
	public static int SIZE = 10;
	public static Coord lastSelected;
	public static int pbNum;
	public static int baNum;
	public static int sjNum;
	public static int gjNum;
	public GameObject[] counters;
	public Sprite[] tiles;
	public Sprite emptyTile;
	Tile[,] boardTiles;
	public GameObject tile;

	// Use this for initialization
	void Start () {
		// initialize board
		StartBoard();
	}

	void StartBoard() {
		initialized = true;
		boardTiles = new Tile[Board.SIZE, Board.SIZE];
		for(int i = 0; i<Board.SIZE; i++) {
			for (int j = 0; j<Board.SIZE; j++) {
				GameObject tempTile = GameObject.Instantiate(tile);
				tempTile.transform.parent = gameObject.transform;
				tempTile.AddComponent<SpriteRenderer>();
				boardTiles[i, j] = tempTile.AddComponent<Tile>();
				tempTile.AddComponent<CircleCollider2D>();
				tempTile.GetComponent<CircleCollider2D>().radius = .35F;
				boardTiles[i, j].SetLocation(i, j);
				// boardTiles[i, j].RandomSprite();
			}
		}
	}

	public Tile GetTile(Coord i) {
		return boardTiles[i.x, i.y];
	}


	void RefreshGUIText() {
		counters[0].GetComponent<UnityEngine.UI.Text>().text = ": " + pbNum;
		counters[1].GetComponent<UnityEngine.UI.Text>().text = ": " + baNum;
		counters[2].GetComponent<UnityEngine.UI.Text>().text = ": " + sjNum;
		counters[3].GetComponent<UnityEngine.UI.Text>().text = ": " + gjNum;
	}

	private bool isSettled() {
		for(int i = 0; i<Board.SIZE; i++) {
			for (int j = 0; j<Board.SIZE; j++) {
				if(!boardTiles[i,j].settled) return false;
			}
		}
		return true;
	}

	// Check to see if moving creates a match-3.
	public bool CheckMove(Coord first, Coord second) {
		if(((first.x-second.x==1 || first.x-second.x==-1) && first.y == second.y) || ((first.y-second.y==1 || first.y-second.y==-1) && first.x == second.x)) {
			Coord[] possible = FindMatch(boardTiles[first.x, first.y].type, first.x, first.y);
			if(possible.Length>0) {
				return true;
			} else if (FindMatch(boardTiles[second.x, second.y].type, second.x, second.y).Length>0) {
				return true;
			} else {
				return false;
			}
		} else 
			return false;
	}

	public bool TestMatch(int type, Coord cd) {
		Coord[] cool = FindMatch(type, cd.x, cd.y);
		return MakeMatch(cool);
	}

	public Coord[] FindMatch(int type, int xCoord, int yCoord) {
		List<Coord> matches = new List<Coord>();
		bool xSetup = false;
		bool ySetup = false;
		for(int i = -2; i<=2; i++) {
			if(i+xCoord>=0&&i+xCoord<Board.SIZE && i!=0) {
				if(boardTiles[i+xCoord, yCoord].type==type && !xSetup) {
					xSetup = true;
				} else if(boardTiles[i+xCoord, yCoord].type==type) {
					if(i!=1)
						matches.Add(new Coord(i-1+xCoord, yCoord));
					else
						matches.Add(new Coord(i-2+xCoord, yCoord));
					matches.Add(new Coord(i+xCoord, yCoord));
				} else xSetup = false;
			}
			if (i+yCoord>=0&&i+yCoord<Board.SIZE && i!=0) {
				if(boardTiles[xCoord, i+yCoord].type==type && i!=0 && !ySetup) {
					ySetup = true;
				} else if(boardTiles[xCoord, i+yCoord].type==type && i!=0) {
					if(i!=1)
						matches.Add(new Coord(xCoord, i-1+yCoord));
					else
						matches.Add(new Coord(xCoord, i-2+yCoord));
					matches.Add(new Coord(xCoord, i+yCoord));
				} else ySetup = false;
			}
			if(matches.Count>0) matches.Add(new Coord(xCoord, yCoord));
		}
		return matches.ToArray();
	}

	private bool MakeMatch(Coord[] matches) {
		for(int i = 0; i<matches.Length; i++) {
			if(!boardTiles[matches[i].x, matches[i].y].Match()) return false;
			DisplayBoard();
		}
		return true;
	}

	public void FindMatchButton() {
		for (int i = 0; i<Board.SIZE; i++) {
			for (int j = 0; j<Board.SIZE; j++) {
				Coord[] cool = FindMatch(boardTiles[i,j].type, i, j);
				print("At " + i + ", " + j + ": " + FindMatch(boardTiles[i,j].type, i, j).Length);
				MakeMatch(cool);
			}
		}
	}

	public void DisplayBoard() {
		// arrange tiles.
		for(int i = 0; i<Board.SIZE; i++) {
			for (int j = 0; j<Board.SIZE; j++) {
				boardTiles[i, j].gameObject.transform.position = boardTiles[i,j].gameObject.transform.parent.position + new Vector3(Tile.TILESIZE * i - (Board.SIZE*Tile.TILESIZE/2), -1 * Tile.TILESIZE * j + (Board.SIZE*Tile.TILESIZE/2), 0);
				boardTiles[i,j].gameObject.GetComponent<SpriteRenderer>().sprite = tiles[boardTiles[i,j].type];
			}
		}
	}

	//TODO:  bookkeeping variables for tiles used, if necessary.
	void SetTileAllocations(int type, int i) {

	}

	// Sets up one line of the board from text editor.
	void SetLineBoard(string[] tiles, int lineNum) {
		if(lineNum==Board.SIZE+1) {
			for(int i = 0; i<Board.SIZE; i++) {
				SetTileAllocations(i, int.Parse(tiles[i]));
			}
		} else {
			for(int i = 0; i<Board.SIZE+1; i++) {
				//This was used for sandwich board, but not needed for randomly generated board.
				//boardTiles[i, lineNum].type = (int) (tiles[i].Substring(0, 2));
			}
		}
	}


		public bool LoadLevel() {
			/*
		     // Handle any problems that might arise when reading the text
		    	TextAsset textFile = Resources.Load<TextAsset>(fileName);
		    	string[] lines = textFile.text.Split('\n');
		    	for(int i = 0; i<Board.SIZE+1; i++) {
		    		string[] tempLine = lines[i].Split(',');
		    		SetLineBoard(tempLine, i);
		    	}
		    	string[] numStuff = lines[Board.SIZE+1].Split(',');
		    	for(int i = 0; i<Board.SIZE; i++) {
		    		SetTileAllocations(i, int.Parse(numStuff[i]));
		    	}
		    	DisplayBoard();

		    	return true; 
		    */
		for (int i = 0; i<Board.SIZE; i++) {
			for(int j = 0; j<Board.SIZE; j++) {
				boardTiles[i,j].RandomSprite(); 
			}
		}
		DisplayBoard();
		return true;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
