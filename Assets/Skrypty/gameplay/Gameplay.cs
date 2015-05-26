using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Klasa Gameplay
/// </summary>
public class Gameplay : MonoBehaviour {

	/// <summary>
	/// Elementy funkcjonalne
	/// </summary>
	public int whoseTurnID;
	public bool isThereCapture;
	public bool isThereMove;

	/// <summary>
	/// Elementy GUI 
	/// </summary>
	public GameObject panelGameEnd;
	public GameObject panelGameStart;
	public Text textWinner;
	public Image wPawnS,bPawnS;

	public GameObject Field_bright;
	public GameObject Field_dark;
	public Mesh boardMesh;
	public Material boardMaterial;

	/// <summary>
	/// Obiekty gry
	/// </summary>
	private GameObject Board;
	private List<GameObject> players;
	/// <summary>
	/// Komponenty
	/// </summary>
	private Board BoardScript;

	// Use this for initialization
	void Start () {
		/// <summary>
		/// Tworzymy plansze
		/// </summary>
		createBoard ();

		/// <summary>
		/// Tworzymy graczy
		/// </summary>
		players = new List<GameObject>();
		createPlayer ("Player1", 1);
		createPlayer ("Player2", 2);

		/// <summary>
		/// Ustalamy kto rozpoczyna
		/// </summary>
		whoseTurnID = 1;
		isThereCapture = false;
		isThereMove = true;

		/// <summary>
		/// Ustawiamy komponent skryptu dla planszy
		/// </summary>
		BoardScript = Board.transform.GetComponent<Board> ();

		/*
		switch (GameObject.Find ("Options").GetComponent<Options> ().gMode) {
			case GameMode.ONE_DEVICE:
				createPlayer ("Player1", 1);
				createPlayer ("Player2", 2);
				break;
			case GameMode.HOST:
				break;
			case GameMode.CLIENT:
				break;
		};
		*/

	}

	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Funkcja tworzy obiekt planszy
	/// </summary>
	private void createBoard() {
		/// <summary>
		/// Tworzymy obiekt planszy
		/// </summary>
		Board = new GameObject();
		Board.name = "Board";
		/// <summary>
		/// Dodajemy skrypt do planszy
		/// </summary>
		Board.AddComponent<Board>();
		Board BoardScript = Board.GetComponent<Board>();
		/// <summary>
		/// Przypisujemy materiały
		/// </summary>
		BoardScript.Field_bright = Field_bright;
		BoardScript.Field_dark = Field_dark;
		/// <summary>
		/// Dodajemy siatkę obiektu planszy
		/// </summary>
		Board.AddComponent<MeshFilter>().mesh = boardMesh;
		Board.GetComponent<MeshFilter> ().transform.localScale = new Vector3 ((float)3.782616,(float)3.782616,(float)3.782616);
		Board.GetComponent<MeshFilter> ().transform.Rotate(new Vector3 (270, 90, 0));
		Board.GetComponent<MeshFilter> ().transform.position = new Vector3 (0, (float)0.01, 0);
		Board.AddComponent<MeshRenderer> ().castShadows = false;
		Board.GetComponent<MeshRenderer> ().receiveShadows = true;
		/// <summary>
		/// ustawiamy materiał dla siatki planszy
		/// </summary>
		Board.GetComponent<MeshRenderer> ().material = boardMaterial;
		Debug.Log ("Utworzono planszę");
	}

	/// <summary>
	/// Funckja tworzy gracza
	/// </summary>
	private void createPlayer(string playerName, int ID)
	{
		GameObject player = new GameObject ();
		player.AddComponent<Player> ();
		player.name = playerName;
		player.GetComponent<Player> ().playerID = ID;

		players.Add (player);

		Debug.Log ("Utworzono gracza");
	}

	/// <summary>
	/// Funkcja musi być wywoływana na początku każdUj tury
	/// aby sprawdzić czy są ruchy i bicia dla danego gracza
	/// </summary>
	public void onTurnStart(int pID){

		if (pID == 1) {
			wPawnS.enabled = false;
			bPawnS.enabled = true;
		} else {
			wPawnS.enabled = true;
			bPawnS.enabled = false;
		}

		/// <summary>
		/// Ustawiamy wartość określającą czy jest jakieś bicie na fałsz
		/// </summary>
		isThereCapture = false;
		/// <summary>
		/// Ustawiamy wartość określającą czy jest jakiś ruch na fałsz
		/// </summary>
		isThereMove = false;
		
		/// <summary>
		/// Pobieramy listę pionków gracza
		/// </summary>
		List<GameObject> PlayerPawns = new List<GameObject>();
		for (int q = 0; q < players[pID-1].transform.childCount; q++) { 
			PlayerPawns.Add(players[pID-1].transform.GetChild(q).gameObject);
		}
		
		/// <summary>
		/// Trzeba sprawdzić każdy pionek pod względem możliwości wykonania ruchu bądź bicia
		/// </summary>
		for (int a = 0; a < PlayerPawns.Count; a++) {
			/// <summary>
			/// Pobieramy komponent skryptu dla aktualnie sprawdzanego pionka
			/// </summary>
			Pawn CurrentPawnScript = PlayerPawns[a].GetComponent<Pawn>();
			CurrentPawnScript.pState = PawnState.CANT_MOVE;

			/// <summary>
			/// Sprawdzamy czy pionek nie został ubity
			/// </summary>
			if (CurrentPawnScript.pState != PawnState.CAPTURED){

				/// <summary>
				/// Trzeba pobrać id pola na którym jest pionek
				/// </summary>
				string FieldId = CurrentPawnScript.fieldID;
				/// <summary>
				/// Tworzymy listę która będzie przechowywać id-ki sąsiadujących pól
				/// </summary>
				if (FieldId != "0") {
					List<string> closestFields = BoardScript.getSurroundingFields(FieldId);

					/// <summary>
					/// W tej pętli sprawdzane będą wszystkie pola sąsiadujące, czy na którymś jest pionek wroga
					/// </summary>
					for  (int b = 0; b < closestFields.Count; b++) {

						/// <summary>
						// Pobieramy komponent skryptu dla aktualnie sprawdzanego pola
						/// </summary>
						Field CurrentFieldScript = GameObject.Find (closestFields[b]).transform.GetComponent<Field>();

						/// <summary>
						/// Jeśli pole jest nie puste
						/// </summary>
						FieldState BorderFieldState = CurrentFieldScript.fState;
						if (BorderFieldState == FieldState.NON_EMPTY){
							int PawnIDonField = BoardScript.getPawnIDonField(closestFields[b]);
							int PlayerIDonField = (int)PawnIDonField / 100;

							/// <summary>
							/// Jeśli pionek wroga
							/// </summary>
							if (pID != Player
							if (pID != PlayerIDonField) {
								/// <summary>
								/// Sprawdzamy czy w lini prostej za tym polem jest wolne pole
								/// </summary>
								string endFieldID = BoardScript.getFieldIDinLine(FieldId,closestFields[b]);

								if (endFieldID != "brak_pola"){
									/// <summary>
									/// Trzeba sprawdzić czy pole za jest wolne 
									/// dlatego pobieramy status pola końcowego
									/// </summary>
									FieldState endFieldStatus = GameObject.Find (endFieldID).transform.GetComponent<Field>().fState;
									/// <summary>
									/// sprawdzamy czy pole końcowe jest puste
									/// </summary>
									if (endFieldStatus != FieldState.NON_EMPTY){

										
										/// <summary>
										/// Jeśli pole za pionkiem wroga jest puste to mamy bicie
										/// </summary>
										GameObject.Find (endFieldID).transform.GetComponent<Field>().fState = FieldState.MOVE_ALLOWED;
										CurrentPawnScript.pState = PawnState.CAN_CAPTURE;
										//Debug.Log("Ustawiono dla pionka: "+CurrentPawnScript.name+", stan: "+CurrentPawnScript.pState);
										isThereCapture = true;

										Debug.Log("Bicie na: " + endFieldID);
									}
								}
							}
						} 
						/// <summary>
						/// Jeśli pole graniczne jest puste
						/// </summary>
						else if (BorderFieldState == FieldState.EMPTY || BorderFieldState == FieldState.MOVE_ALLOWED){
							/// <summary>
							/// Sprawdzamy czy pole graniczne jest z przodu
							/// </summary>
							if(BoardScript.isItFront(FieldId, closestFields[b], pID)) {
								/// <summary>
								/// jeśli pole jest z przodu to ustawiamy MOVE_ALLOWED
								/// </summary>
								CurrentFieldScript.fState = FieldState.MOVE_ALLOWED;
								/// <summary>
								/// a pionek moze wykonać ruch
								/// jeśli pionek ma bicie to nie może wykonać ruchu
								/// </summary>
								if(CurrentPawnScript.pState != PawnState.CAN_CAPTURE) {
									CurrentPawnScript.pState = PawnState.CAN_MOVE;
									//Debug.Log("Ustawiono dla pionka: "+CurrentPawnScript.name+", stan: "+CurrentPawnScript.pState);
								}	

								isThereMove = true;
							}
						}
					}
				}

			}
		}

		/// <summary>
		/// Sprawdzamy czy są jakieś ruchy lub bicia
		/// </summary>
		if (!isThereCapture && !isThereMove) {
			/// <summary>
			/// Jeśli nie ma możliwych ruchów lub bić to koniec gry i remis
			/// </summary>
			showResults(whoseTurnID+"");
		}
		Debug.Log ("Zainicjalizowano turę dla gracza nr: " + whoseTurnID);
	}

	/// <summary>
	/// Zwraca gracza który posiada prawo ruchu
	/// </summary>
	public GameObject getEnemyPlayer(){
		if (whoseTurnID == 1) return GameObject.Find ("Player" + 2);
		else return GameObject.Find ("Player" + 1);
	}

	/// <summary>
	/// Funkcja zmieniająca gracza
	/// </summary>
	public void changeTurn(){
		
	}

	public void showResults(string Winner){
		if (Winner == "Remis") {
			/// <summary>
			/// Wyświetlenie remisu		
			/// </summary>		
			panelGameEnd.SetActive (true);
			textWinner.text = "Wynik gry: REMIS";
		} else {
			/// <summary>
			/// Wyświetlenie informacji o tym który gracz wygrał
			/// </summary>
			panelGameEnd.SetActive (true);
			textWinner.text = "Wygrał: Gracz "+whoseTurnID;
		}
	}

	public void backToMainMenu(){
		/// <summary>
		/// Sprawdzamy platformę i zamykamy program
		/// </summary>
		Application.LoadLevel ("menu");
	}
}
