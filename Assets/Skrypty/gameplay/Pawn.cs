using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PawnState {
	CAN_MOVE = 0,			// może ruszyć
	CAN_CAPTURE,			// może bić
	CANT_MOVE,				// pionek zablokowany
	CAN_MOVE_AS_KING,		// może ruszyć jako damka
	CAN_CAPTURE_AS_KING,	// może bić jako damka
	CAPTURED             	// zbity
}

public class Pawn : MonoBehaviour {

	public GameObject shadow;

	public int pawnID;			// identyfikator pionka
	public string fieldID;		// identyfikator pola na którym stoi pionek
	public PawnState pState;	// stan pionka

	public bool isSelected;		// czy pionek jest zaznaczony

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
		// sprawdzamy czy tapnięto ekran
		// warunek dla androida: Input.touchCount > 0
		// warunek dla Windowsa: 
		if (Input.GetMouseButton (0)) {
			Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rhPawnHit;
			bool didHit = Physics.Raycast(toMouse, out rhPawnHit, 500.0f);

			// sprawdzamy czy trafiliśmy na obiekt z tagiem Pawn
			if(didHit && rhPawnHit.collider.gameObject.tag == "Pawn") {

				// sprawdzamy czy to pionek gracza, który ma ruch i który nie został zbity
				int whoseTurn = Camera.main.GetComponent<Gameplay>().whoseTurnID;
				if(rhPawnHit.collider.gameObject.transform.parent.gameObject.GetComponent<Player>().playerID == whoseTurn && rhPawnHit.collider.gameObject.GetComponent<Pawn>().pState != PawnState.CAPTURED)
				{
					Pawn cPawn = rhPawnHit.transform.GetComponent<Pawn>();
					Gameplay gp = Camera.main.GetComponent<Gameplay>();
					Board bS = GameObject.Find ("Board").GetComponent<Board>();
					Player cP = GameObject.Find("Player"+whoseTurn).GetComponent<Player>();


					// sprawdzamy czy są bicia 
					if (gp.isThereCapture){
						// jeśli tak to może zostać podświetlony tylko pionek z biciem
						if (cPawn.pState == PawnState.CAN_CAPTURE && cPawn.pState != PawnState.CAPTURED){
							highlightPawn(rhPawnHit);
							/*  w momencie tapnięcia na pionek który ma możliwość bicia, zostaną
							 *  podświetlone pola na które możemy wskoczyć bijąc wroga,
							 *  Przed tym należy "odświetlić" prędzej podświetlone pola, jeśli jest więcej bić
							 */

							// trzeba wyczyścić poprzednie podświetlenia
							List<GameObject> darkField = bS.getDarkFieldsList();
							foreach (GameObject dField in darkField){
								if (dField.GetComponent<Field>().fState == FieldState.HIGHLIGHTED) dField.GetComponent<Field>().unsetHighlighted();
							}

							// szukamy bić
							bS.searchForCaptures(cPawn);

						}
					} else if (gp.isThereMove){
						// jeśli nie to moze zostać podświetlony pionek z ruchem
						if (cPawn.pState == PawnState.CAN_MOVE && cPawn.pState != PawnState.CAPTURED){
							highlightPawn(rhPawnHit);
							/*  w tym momencie należy podświetlić pola na które chcemy ruszyć
							 *  ale przed tym należy "odświetlić" inne wcześniej podświetlone pola
							 *  Podświetlone pole umożliwia nań tapnięcie i wykonanie ruchu 
							 */

							// trzeba wyczyścić poprzednie podświetlenia
							List<GameObject> darkField = bS.getDarkFieldsList();
							foreach (GameObject dField in darkField){
								if (dField.GetComponent<Field>().fState == FieldState.HIGHLIGHTED) dField.GetComponent<Field>().unsetHighlighted();
							}

							// odczytujemy id pól znajdujących się przed pionkiem
							List<string> FrontFields = bS.getFrontFields(cPawn.fieldID,(int)(cPawn.pawnID/100));

							// sprawdzamy pola przed pionkiem czy są puste
							foreach (string ffields in FrontFields){
								Field ffieldsScript = GameObject.Find(ffields).GetComponent<Field>(); 
								// jeśli pole jest puste to podświetlamy
								if (ffieldsScript.fState == FieldState.EMPTY || ffieldsScript.fState == FieldState.MOVE_ALLOWED){
									ffieldsScript.setHighlighted();
									// zapamiętujemy id pionka ktory wykonuje bicie
									GameObject.Find("Player"+whoseTurn).GetComponent<Player>().pawnToMove = (cPawn.pawnID-(whoseTurn*100)-1);
								}
							}
						}
					}
				}
			} 
		}
	}

	// funkcja podświetla pionka
	private void highlightPawn(RaycastHit rhPawnHit) {
		if (this.gameObject.GetComponent<Pawn> ().pState != PawnState.CAPTURED) {
						if (this.gameObject.transform.parent.gameObject.name == "Player1") {
								this.gameObject.renderer.material = blackPawn;
						} else
								this.gameObject.renderer.material = whitePawn;
				}

		rhPawnHit.collider.gameObject.renderer.material = selectedPawnMat;
		
		isSelected = false;
		rhPawnHit.collider.gameObject.GetComponent<Pawn>().isSelected = true;
	}

	//funkcja odswietlająca pionka 
	public void unHighlightPawn(GameObject pawn){
		if(pawn.gameObject.transform.parent.gameObject.name == "Player1") {
			pawn.collider.gameObject.renderer.material = blackPawn;
		} else pawn.collider.gameObject.renderer.material = whitePawn;

	}

	// funkcja rusza pionka
	public void move(string fieldId) {
		// szukamy pozycji na którą postawimy pionek
		Vector3 dest = GameObject.Find ("Board").GetComponent<Board> ().getFieldCenterCoordinate (fieldId);
		// przenosimy ten pionek na pozycję dest
		this.gameObject.transform.localPosition = dest;
	}
	
	public void putOut() {
		this.gameObject.transform.position = new Vector3 ((float)2.5,this.gameObject.transform.localPosition.y,this.gameObject.transform.localPosition.z);
	}
}
