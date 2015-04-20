using UnityEngine;
using System.Collections;
	/// <summary>
	/// Funkcja publiczna niszcząca pionek
	/// </summary>
public class Player : MonoBehaviour {
	
	public int time;
	public int playerID;
	public string playerType;

	private string[] startPositions;
	private GameObject[] Pawns;
	
	
	/// <summary>
	/// Funkcja inicjująca początkowy układ pionków 
	/// </summary>
	void Start () {
		startPositions = new string[] {"A1", "A3", "A5", "A7", "B2", "B4", "B6", "B8", "C1", "C3", "C5", "C7",
									   "H2", "H4", "H6", "H8", "G1", "G3", "G5", "G7", "F2", "F4", "F6", "F8"};
		createPawns ();
	}
	
	/// <summary>
	/// Funkcja 
	/// </summary>
	void Update () {
	
	}
	/// <summary>
	/// Funkcja publiczna tworząca pionki dla gracza 
	/// </summary>
	public void createPawns() {
		Pawns = new GameObject[12];
		for (int a = 0; a < 12; a++) {
			// tworzymy obiekt pionka z prefaba
			Object pPrefab;
			if (playerID == 1) pPrefab = Resources.Load("pawn_prefab_black");
			else pPrefab = Resources.Load("pawn_prefab_white");
			Pawns[a] = (GameObject)Instantiate(pPrefab); 	

			// dodajemy skrypt dla pionka
			Pawns[a].AddComponent<Pawn>();
			// przypisujemy nazwę dla pionka
			Pawns[a].name = "Pawn" + a;
			// skalujemy obiekt
			Pawns[a].transform.localScale = new Vector3((float)0.1406877,(float)0.1406877,(float)0.1406877);

			// ustawiamy pionka na polu
			GameObject board = GameObject.Find("Board");
			Vector3 pawnPosCor;
			if (playerID == 1) pawnPosCor = board.GetComponent<Board>().getFieldCenterCoordinate(startPositions[a]);
			else pawnPosCor = board.GetComponent<Board>().getFieldCenterCoordinate(startPositions[a+12]);
			Pawns[a].transform.position = pawnPosCor;

			// dodajemy Box collider
			Pawns[a].AddComponent<BoxCollider>().size = new Vector3((float)3.4,(float)3.6,(float)1.56);
			// obracamy pionek o 45 stopni
			Pawns[a].transform.Rotate(0,0,45);

			// przypisujemy pionki dla playera oraz numery id dla pionków
			if (playerID == 1) {
				Pawns[a].transform.parent = GameObject.Find("Player1").gameObject.transform;
				Pawns[a].GetComponent<Pawn>().pawnID = 100 + a + 1;
				// zmieniamy stan pola na którym stawiamy pionka: 
				board.GetComponent<Board>().setFieldState(FieldState.NON_EMPTY, startPositions[a]);
			}
			if (playerID == 2) {
				Pawns[a].transform.parent = GameObject.Find("Player2").gameObject.transform;
				Pawns[a].GetComponent<Pawn>().pawnID = 200 + a + 1;
				// zmieniamy stan pola na którym stawiamy pionka: 
				board.GetComponent<Board>().setFieldState(FieldState.NON_EMPTY, startPositions[a+12]);
			}

			// dodajemy tag dla pionka
			Pawns[a].tag = "Pawn";

		}
	}
	/// <summary>
	/// Funkcja umożliwiająca wykonanie ruchu graczowi
	/// </summary>
	public void makeMove(){

	}
	/// <summary>
	/// Funkcja publiczna blokująca ruchy graczowi ktory oczekuje na swoją turę 
	/// </summary>
	public void waitForTurn(){
	
	}
}
