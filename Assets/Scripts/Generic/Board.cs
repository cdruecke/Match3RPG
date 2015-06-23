using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;

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

	// TODO: Check to see if moving is allowed and if it creates a match-3.
	public bool CheckRules(int type, int xCoord, int yCoord) {
		return true;
	}

	// TODO: Change to find match-3
	int FindNeighbors(int type, int xCoord, int yCoord) {
		int numNeighbors = 0;
		for(int i = -2; i<=2; i++) {
			if(i+xCoord>=0&&i+xCoord<=Board.SIZE) {
				if(boardTiles[i+xCoord, yCoord].type==type && i!=0) {
					numNeighbors++;
				}
			}
			if (i+yCoord>=0&&i+yCoord<=Board.SIZE) {
				if(boardTiles[xCoord, i+yCoord].type==type && i!=0) {
					numNeighbors++;
				}
			}
		}
		return numNeighbors;
	}

	public void DisplayBoard() {
		// arrange tiles.
		for(int i = 0; i<Board.SIZE; i++) {
			for (int j = 0; j<Board.SIZE; j++) {
				boardTiles[i, j].gameObject.transform.position = boardTiles[i,j].gameObject.transform.parent.position + new Vector3(Tile.TILESIZE * i - (Board.SIZE*Tile.TILESIZE/2), -1 * Tile.TILESIZE * j + (Board.SIZE*Tile.TILESIZE/2), 0);
				switch(boardTiles[i,j].type) {
				case 0:
					boardTiles[i,j].gameObject.GetComponent<SpriteRenderer>().sprite = tiles[0];
					break;
				case 1:
					boardTiles[i,j].gameObject.GetComponent<SpriteRenderer>().sprite = tiles[1];
					break;
				case 2:
					boardTiles[i,j].gameObject.GetComponent<SpriteRenderer>().sprite = tiles[2];
					break;
				case 3:
					boardTiles[i,j].gameObject.GetComponent<SpriteRenderer>().sprite = tiles[3];
					break;
				case 4:
					boardTiles[i,j].gameObject.GetComponent<SpriteRenderer>().sprite = tiles[4];
					break;
				default:
					boardTiles[i,j].gameObject.GetComponent<SpriteRenderer>().sprite = emptyTile;
					break;
				}
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
