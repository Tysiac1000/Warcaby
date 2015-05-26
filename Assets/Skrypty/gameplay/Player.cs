using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Klasa Gracza
/// </summary>
public class Player : MonoBehaviour {

	public int playerID;

	private string[] startPositions;
	public GameObject[] Pawns;

	public int pawnToMove, pawnToCapture;

	// Use this for initialization
	void Start () {
		startPositions = new string[] {"A1", "A3", "A5", "A7", "B2", "B4", "B6", "B8", "C1", "C3", "C5", "C7",
										"H2", "H4", "H6", "H8", "G1", "G3", "G5", "G7", "F2", "F4", "F6", "F8"};
		pawnToMove = 0;
		pawnToCapture = 0;
		createPawns ();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void createPawns() {
		Pawns = new GameObject[12];
		for (int a = 0; a < 12; a++) {
			/// <summary>
			/// Tworzymy obiekt pionka z prefaba
			/// </summary>
			Object pPrefab;
			if (playerID == 1) pPrefab = Resources.Load("pawn_prefab_black");
			else pPrefab = Resources.Load("pawn_prefab_white");
			Pawns[a] = (GameObject)Instantiate(pPrefab); 	

			/// <summary>
			/// Dodajemy skrypt dla pionka
			/// </summary>
			Pawns[a].AddComponent<Pawn>();
			/// <summary>
			/// Przypisujemy nazwę dla pionka
			/// </summary>
			Pawns[a].name = "Pawn" + a;
			/// <summary>
			/// Skalujemy obiekt
			/// </summary>
			Pawns[a].transform.localScale = new Vector3((float)0.1406877,(float)0.1406877,(float)0.1406877);

			/// <summary>
			/// Ustawiamy pionka na polu
			/// Przypisujemy ID Pionka dla pola
			/// </summary>
			GameObject board = GameObject.Find("Board");
			Vector3 pawnPosCor;
			if (playerID == 1) {
				pawnPosCor = board.GetComponent<Board>().getFieldCenterCoordinate(startPositions[a]);
				board.transform.FindChild(startPositions[a]).GetComponent<Field>().pawnId = 100 + a + 1;
			}
			else {
				pawnPosCor = board.GetComponent<Board>().getFieldCenterCoordinate(startPositions[a+12]);
				board.transform.FindChild(startPositions[a+12]).GetComponent<Field>().pawnId = 200 + a + 1;
			}
			Pawns[a].transform.position = pawnPosCor;


			/// <summary>
			/// Dodajemy Box collider
			/// </summary>
			Pawns[a].AddComponent<BoxCollider>().size = new Vector3((float)3.4,(float)3.6,(float)1.56);
			/// <param>
			/// Obracamy pionek o 45 stopni
			/// </param>
			Pawns[a].transform.Rotate(0,0,45);

			/// <param>
			/// Przypisujemy pionki dla playera oraz numery id dla pionków, oraz numery id pól na których stoją pionki
			/// </param>
			if (playerID == 1) {
				Pawns[a].transform.parent = GameObject.Find("Player1").gameObject.transform;
				Pawns[a].GetComponent<Pawn>().pawnID = 100 + a + 1;
				Pawns[a].GetComponent<Pawn>().fieldID = startPositions[a];
				/// <summary>
				/// Zmieniamy stan pola na którym stawiamy pionka: 
				/// </summary>
				board.GetComponent<Board>().setFieldState(FieldState.NON_EMPTY, startPositions[a]);
			}
			if (playerID == 2) {
				Pawns[a].transform.parent = GameObject.Find("Player2").gameObject.transform;
				Pawns[a].GetComponent<Pawn>().pawnID = 200 + a + 1;
				Pawns[a].GetComponent<Pawn>().fieldID = startPositions[a+12];
				/// <summary>
				// zmieniamy stan pola na którym stawiamy pionka: 
				/// </summary>
				board.GetComponent<Board>().setFieldState(FieldState.NON_EMPTY, startPositions[a+12]);
			}

			/// <summary>
			/// Dodajemy tag dla pionka
			/// </summary>
			Pawns[a].tag = "Pawn";

		}
		Debug.Log ("Utworzono pionki gracza");
	}

	/// <summary>
	/// Funkcja wykonująca fizyczny ruch
	/// </summary>
	public void makeMove(string idField){
		/// <summary>
		/// Ruszamy pionkiem i ustawiamy mu nowe id pola na które ruszył
		/// </summary>
		Pawn pawn2Move = Pawns [pawnToMove].GetComponent<Pawn> ();
		pawn2Move.move(idField);
		string fieldIDfromMove = pawn2Move.fieldID;
		pawn2Move.fieldID = idField;

		/// <summary>
		/// zmieniamy status pola na którego postawiliśmy pionka
		/// </summary>
		Field field2Move = GameObject.Find (idField).GetComponent<Field> ();
		field2Move.pawnId = pawn2Move.pawnID;
		field2Move.unsetHighlighted ();
		field2Move.fState = FieldState.NON_EMPTY;

		/// <summary>
		/// Zmieniamy status pola z którego ruszyliśmy
		/// </summary>
		Field fieldFromMove = GameObject.Find (fieldIDfromMove).GetComponent<Field> ();
		fieldFromMove.fState = FieldState.EMPTY;
		fieldFromMove.pawnId = 0;

		changeTurn (pawn2Move);
	}

	/// <summary>
	/// Funkcja wykonująca bicie
	/// </summary>
	public void makeCapture(string idOfDestField, int idOfCapturedPawn){
		/// <summary>
		/// Ruszamy pionkiem i ustawiamy mu nowe id pola na które ruszył
		/// </summary>
		Pawn pawn2Move = Pawns [pawnToMove].GetComponent<Pawn> ();
		pawn2Move.move(idOfDestField);
		string fieldIDfromMove = pawn2Move.fieldID;
		pawn2Move.fieldID = idOfDestField;

		/// <summary>
		/// Zmieniamy status pola z którego ruszyliśmy
		/// </summary>
		Field field2Move = GameObject.Find (idOfDestField).GetComponent<Field> ();
		field2Move.pawnId = pawn2Move.pawnID;
		field2Move.unsetHighlighted ();
		field2Move.fState = FieldState.NON_EMPTY;

		/// <summary>
		/// Zmieniamy status pola z którego ruszyliśmy
		/// </summary>
		Field fieldFromMove = GameObject.Find (fieldIDfromMove).GetComponent<Field> ();
		fieldFromMove.fState = FieldState.EMPTY;
		fieldFromMove.pawnId = 0;

		GameObject enemyPlayer = Camera.main.GetComponent<Gameplay>().getEnemyPlayer ();
		Player enemysPlayer = enemyPlayer.GetComponent<Player> ();
		Board bS = GameObject.Find ("Board").GetComponent<Board>();
		/// <summary>
		/// Trzeba zlikwidować pionka wroga
		/// podczas likwidacji trzeba także zmienić parametry pola z którego zbity jest pionek oraz parametry bitego pionka
		/// </summary>
		int pawnIndex;
		if (Camera.main.GetComponent<Gameplay>().whoseTurnID == 1) pawnIndex = idOfCapturedPawn - 201;
		else pawnIndex = idOfCapturedPawn - 101;

		Pawn enemyPlayerPawn = enemysPlayer.Pawns[pawnIndex].GetComponent<Pawn>();
		enemyPlayerPawn.pState = PawnState.CAPTURED;

		/// <summary>
		/// Zerujemy wartość id pionka który stał na polu
		/// </summary>
		FieldCords fID = bS.TranslateCords(enemyPlayerPawn.fieldID);
		bS.Fields[fID.Y][fID.X].GetComponent<Field>().pawnId = 0;
		bS.Fields[fID.Y][fID.X].GetComponent<Field>().fState = FieldState.EMPTY;
		/// <summary>
		/// Odkładamy pionek na bok
		/// </summary>
		enemyPlayerPawn.putOut ();

		enemyPlayerPawn.fieldID = "0";

	}

	public void changeTurn(Pawn pawn2Move){

		pawnToMove = 0;
		
		/// <summary>
		/// Trzeba jeszcze odznaczyć pionka po wykonanym ruchu
		/// </summary>
		pawn2Move.unHighlightPawn (pawn2Move.gameObject);
		
		/// <summary>
		/// Zmiana tury
		/// </summary>
		Gameplay gS = Camera.main.GetComponent<Gameplay> ();
		if (gS.whoseTurnID == 1) gS.whoseTurnID = 2; 
		else gS.whoseTurnID = 1;

		Debug.Log ("Zmieniono turę");

		gS.onTurnStart (gS.whoseTurnID);
		
		/// <summary>
		/// Reszte pól trzeba odznaczyć
		/// </summary>
		Board bS = GameObject.Find("Board").GetComponent<Board>();
		List<GameObject> darkField = bS.getDarkFieldsList();
		foreach (GameObject dField in darkField){
			if (dField.GetComponent<Field>().fState == FieldState.HIGHLIGHTED) dField.GetComponent<Field>().unsetHighlighted();
		}
	}

	public void waitForTurn(){
	
	}
}
