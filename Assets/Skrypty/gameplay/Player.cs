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
	/// Funkcja aktualiacji - odświeżania
	/// </summary>
	void Update () {
	
	}
	/// <summary>
	/// Funkcja publiczna tworząca pionki dla gracza 
	/// </summary>
	public void createPawns() {
		Pawns = new GameObject[12];
		for (int a = 0; a < 12; a++) {
			/// <summary>
			/// Funkcja publiczna tworząca pionki dla gracza 
			/// </summary>
			// tworzymy obiekt pionka z prefaba
			Object pPrefab;
			if (playerID == 1) pPrefab = Resources.Load("pawn_prefab_black");
			else pPrefab = Resources.Load("pawn_prefab_white");
			Pawns[a] = (GameObject)Instantiate(pPrefab); 	

			// dodajemy skrypt dla pionka
			/// <summary>
			/// Funkcja publiczna tworząca pionki dla gracza 
			/// </summary>
			Pawns[a].AddComponent<Pawn>();
			// przypisujemy nazwę dla pionka
			/// <summary>
			/// Funkcja publiczna tworząca pionki dla gracza 
			/// </summary>
			Pawns[a].name = "Pawn" + a;
			/// <summary>
			/// Funkcja publiczna tworząca pionki dla gracza 
			/// </summary>
			// skalujemy obiekt
			Pawns[a].transform.localScale = new Vector3((float)0.1406877,(float)0.1406877,(float)0.1406877);

			/// <summary>
			// ustawiamy pionka na polu
			/// </summary>
			GameObject board = GameObject.Find("Board");
			Vector3 pawnPosCor;
			if (playerID == 1) pawnPosCor = board.GetComponent<Board>().getFieldCenterCoordinate(startPositions[a]);
			else pawnPosCor = board.GetComponent<Board>().getFieldCenterCoordinate(startPositions[a+12]);
			Pawns[a].transform.position = pawnPosCor;

			/// <summary>
			// dodajemy Box collider
			/// </summary>
			Pawns[a].AddComponent<BoxCollider>().size = new Vector3((float)3.4,(float)3.6,(float)1.56);

			/// <summary>
			/// obracamy pionek o 45 stopni
			/// </summary>// 
			Pawns[a].transform.Rotate(0,0,45);

			/// <summary>
			// przypisujemy pionki dla playera oraz numery id dla pionków
			/// </summary>
			if (playerID == 1) {
				Pawns[a].transform.parent = GameObject.Find("Player1").gameObject.transform;
				Pawns[a].GetComponent<Pawn>().pawnID = 100 + a + 1;

				/// <summary>
				/// zmieniamy stan pola na którym stawiamy pionka
				/// </summary>// 
				board.GetComponent<Board>().setFieldState(FieldState.NON_EMPTY, startPositions[a]);
			}
			if (playerID == 2) {
				Pawns[a].transform.parent = GameObject.Find("Player2").gameObject.transform;
				Pawns[a].GetComponent<Pawn>().pawnID = 200 + a + 1;
				/// <summary>
				/// // zmieniamy stan pola na którym stawiamy pionka 
				/// </summary>
				board.GetComponent<Board>().setFieldState(FieldState.NON_EMPTY, startPositions[a+12]);
			}
			/// <summary>
			// dodajemy tag dla pionka
			/// </summary>
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
