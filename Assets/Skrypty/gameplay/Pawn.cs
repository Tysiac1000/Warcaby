using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PawnState {
	/// <summary>
	/// Może ruszyć
	/// </summary>
	CAN_MOVE = 0,			
	/// <summary>
	/// Może bić
	/// </summary>
	CAN_CAPTURE,			
	/// <summary>
	/// Pionek zablokowany
	/// </summary>
	CANT_MOVE,				
	/// <summary>
	/// Może ruszyć jako damka
	/// </summary>
	CAN_MOVE_AS_KING,		
	/// <summary>
	/// Może bić jako damka
	/// </summary>
	CAN_CAPTURE_AS_KING,	
	/// <summary>
	/// Zbity
	/// </summary>
	CAPTURED             	
}

/// <summary>
/// Klasa Pionka
/// </summary>
public class Pawn : MonoBehaviour {

	public GameObject shadow;
	/// <summary>
	/// Identyfikator pionka
	/// </summary>
	public int pawnID;			
	/// <summary>
	/// Identyfikator pola na którym stoi pionek
	/// </summary>
	public string fieldID;		
	/// <summary>
	/// Stan pionka
	/// </summary>
	public PawnState pState;	
	/// <summary>
	/// Czy pionek jest zaznaczony
	/// </summary>
	public bool isSelected;		
	
	private Material selectedPawnMat,whitePawn,blackPawn;

	// Use this for initialization
	void Start () {
		Vector3 position = new Vector3(this.transform.position.x,this.transform.position.y + 0.01F,this.transform.position.z);
		selectedPawnMat = Resources.Load("selectedPawn", typeof(Material)) as Material;
		whitePawn = Resources.Load("whitePawn", typeof(Material)) as Material;
		blackPawn = Resources.Load("blackPawn", typeof(Material)) as Material;
		isSelected = false;
		pState = PawnState.CANT_MOVE;
	}

	void FixedUpdate() {
		OnPawnClick ();
	}

	private void OnPawnClick() {
		/// <summary>
		/// Sprawdzamy czy tapnięto ekran
		/// warunek dla androida: Input.touchCount > 0
		/// warunek dla Windowsa: 
		/// </summary>
		if (Input.touchCount > 0) {
			Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rhPawnHit;
			bool didHit = Physics.Raycast(toMouse, out rhPawnHit, 500.0f);

			/// <summary>
			/// Sprawdzamy czy trafiliśmy na obiekt z tagiem Pawn
			/// </summary>
			if(didHit && rhPawnHit.collider.gameObject.tag == "Pawn") {

				/// <summary>
				/// Sprawdzamy czy to pionek gracza, który ma ruch i który nie został zbity
				/// </summary>
				int whoseTurn = Camera.main.GetComponent<Gameplay>().whoseTurnID;
				if(rhPawnHit.collider.gameObject.transform.parent.gameObject.GetComponent<Player>().playerID == whoseTurn && rhPawnHit.collider.gameObject.GetComponent<Pawn>().pState != PawnState.CAPTURED)
				{
					Pawn cPawn = rhPawnHit.transform.GetComponent<Pawn>();
					Gameplay gp = Camera.main.GetComponent<Gameplay>();
					Board bS = GameObject.Find ("Board").GetComponent<Board>();
					Player cP = GameObject.Find("Player"+whoseTurn).GetComponent<Player>();


					/// <summary>
					/// Sprawdzamy czy są bicia 
					/// </summary>
					if (gp.isThereCapture){
						/// <summary>
						/// Jeśli tak to może zostać podświetlony tylko pionek z biciem
						/// </summary>
						if (cPawn.pState == PawnState.CAN_CAPTURE && cPawn.pState != PawnState.CAPTURED){
							highlightPawn(rhPawnHit);
							/// <summary>
							/// W momencie tapnięcia na pionek który ma możliwość bicia, zostaną
							/// podświetlone pola na które możemy wskoczyć bijąc wroga,
							/// Przed tym należy "odświetlić" prędzej podświetlone pola, jeśli jest więcej bić
							/// <summary>
			

							/// <summary>
							/// Trzeba wyczyścić poprzednie podświetlenia
							/// </summary>
							List<GameObject> darkField = bS.getDarkFieldsList();
							foreach (GameObject dField in darkField){
								if (dField.GetComponent<Field>().fState == FieldState.HIGHLIGHTED) dField.GetComponent<Field>().unsetHighlighted();
							}

							/// <summary>
							// Szukamy  czy są możliwe bicia
							/// </summary>
							bS.searchForCaptures(cPawn);

						}
					} else if (gp.isThereMove){
						/// <summary>
						/// Jeśli nie to moze zostać podświetlony pionek z ruchem
						/// </summary>
						if (cPawn.pState == PawnState.CAN_MOVE && cPawn.pState != PawnState.CAPTURED){
							highlightPawn(rhPawnHit);
							/// <summary>	
							///  W tym momencie należy podświetlić pola na które chcemy ruszyć
							///	 ale przed tym należy "odświetlić" inne wcześniej podświetlone pola
							///	 Podświetlone pole umożliwia nań tapnięcie i wykonanie ruchu 
							/// </summary>

							
							/// <summary>
							/// Trzeba wyczyścić poprzednie podświetlenia
							/// </summary>
							List<GameObject> darkField = bS.getDarkFieldsList();
							foreach (GameObject dField in darkField){
								if (dField.GetComponent<Field>().fState == FieldState.HIGHLIGHTED) dField.GetComponent<Field>().unsetHighlighted();
							}

							/// <summary>
							/// Odczytujemy id pól znajdujących się przed pionkiem
							/// </summary>
							List<string> FrontFields = bS.getFrontFields(cPawn.fieldID,(int)(cPawn.pawnID/100));

							/// <summary>
							/// Sprawdzamy pola przed pionkiem czy są puste
							/// </summary>
							foreach (string ffields in FrontFields){
								Field ffieldsScript = GameObject.Find(ffields).GetComponent<Field>(); 
								/// <summary>
								/// Jeśli pole jest puste to podświetlamy
								/// </summary>
								if (ffieldsScript.fState == FieldState.EMPTY || ffieldsScript.fState == FieldState.MOVE_ALLOWED){
									ffieldsScript.setHighlighted();
									/// <summary>
									/// Zapamiętujemy id pionka ktory wykonuje bicie
									/// </summary>
									GameObject.Find("Player"+whoseTurn).GetComponent<Player>().pawnToMove = (cPawn.pawnID-(whoseTurn*100)-1);
								}
							}
						}
					}
				}
			} 
		}
	}

	/// <summary>
	/// Funkcja podświetla pionka
	/// </summary>
	private void highlight
	private void highlightPawn(RaycastHit rhPawnHit) {
		if (this.gameObject.GetComponent<Pawn> ().pState != PawnState.CAPTURED) {
						if (this.gameObject.transform.parent.gameObject.name == "Player1") {
								this.gameObject.GetComponent<Renderer>().material = blackPawn;
						} else
								this.gameObject.GetComponent<Renderer>().material = whitePawn;
				}

		rhPawnHit.collider.gameObject.GetComponent<Renderer>().material = selectedPawnMat;
		
		isSelected = false;
		rhPawnHit.collider.gameObject.GetComponent<Pawn>().isSelected = true;
	}

	/// <summary>
	/// Funkcja odswietlająca pionka 
	/// </summary>
	public void unHighlightPawn(GameObject pawn){
		if(pawn.gameObject.transform.parent.gameObject.name == "Player1") {
			pawn.GetComponent<Collider>().gameObject.GetComponent<Renderer>().material = blackPawn;
		} else pawn.GetComponent<Collider>().gameObject.GetComponent<Renderer>().material = whitePawn;

	}

	/// <summary>
	/// Funkcja rusza pionka
	/// </summary>
	public void move(string fieldId) {
		/// <summary>
		/// Szukamy pozycji na którą postawimy pionek
		/// </summary>
		Vector3 dest = GameObject.Find ("Board").GetComponent<Board> ().getFieldCenterCoordinate (fieldId);
		/// <summary>	
		/// Przenosimy ten pionek na pozycję dest
		/// </summary>
		this.gameObject.transform.localPosition = dest;
	}
	
	public void putOut() {
		this.gameObject.transform.position = new Vector3 ((float)2.5,this.gameObject.transform.localPosition.y,this.gameObject.transform.localPosition.z);
	}
}
