using UnityEngine;
using System.Collections;

// typ wyliczeniowy określający stan pola: puste, nie puste, dozolony ruch
public enum FieldState{
	EMPTY = 0,
	NON_EMPTY,
	MOVE_ALLOWED,
	HIGHLIGHTED
}

public class Field : MonoBehaviour {

	public string idField;		// identyfikator pola
	public FieldState fState;	// stan pola
	public int pawnId;			// identyfikator pionka który stoi na polu

	// materiały dla pola niezaznaczonego i zaznaczonego
	private Material darkFieldMaterial, highlightedDarkFieldMaterial;	

	// Use this for initialization
	void Start () {
		darkFieldMaterial = Resources.Load("PoleCiemne", typeof(Material)) as Material;
		highlightedDarkFieldMaterial = Resources.Load("selectedDarkField", typeof(Material)) as Material;
	}

	void FixedUpdate () {
		OnFieldClick ();
	}

	public void setHighlighted() {
		this.gameObject.renderer.material = highlightedDarkFieldMaterial;
		this.fState = FieldState.HIGHLIGHTED;
	}

	public void unsetHighlighted() {
		this.gameObject.renderer.material = darkFieldMaterial;
		this.fState = FieldState.EMPTY;
	}

	private void OnFieldClick() {
		// sprawdzamy czy tapnięto ekran
		// warunek dla androida: Input.touchCount > 0
		// warunek dla Windowsa: 
		if (Input.GetMouseButton (0)) {
			Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rFieldHit;
			bool didHit = Physics.Raycast(toMouse, out rFieldHit, 500.0f);

			// sprawdzamy czy trafiliśmy na obiekt z tagiem Field
			if(didHit && rFieldHit.collider.gameObject.tag == "Field") {
				Field HitedFieldScript = rFieldHit.collider.gameObject.GetComponent<Field>(); 

				// jedynie obiekt podświetlony będzie reagować
				if(HitedFieldScript.fState == FieldState.HIGHLIGHTED)
				{	
					// trzeba sprawdzić czy są bicia i/lub ruchy
					Gameplay gS = Camera.main.GetComponent<Gameplay>();
					Board bS = rFieldHit.transform.parent.transform.gameObject.GetComponent<Board>();
					if(gS.isThereCapture) {
						// Player wykona ruch bijąc pionka
						// Szukamy obiektu playera
						Player playerS = GameObject.Find("Player"+gS.whoseTurnID).GetComponent<Player>();
						// Wykonujemy bicie
						playerS.makeCapture(HitedFieldScript.idField, playerS.pawnToCapture);
						// po tym trzeba sprawdzić czy są bicia dalej
						// jeśli JO to ustawiamy odpowiednie flagi dla pionka i pól
						// jeśli NE to kończymy turę
						Pawn p2m = playerS.Pawns[playerS.pawnToMove].GetComponent<Pawn>();
						if(!bS.areThereCaptures(p2m)) playerS.changeTurn(p2m);
					}
					else if (gS.isThereMove) {
						// jeśli nie ma bić, to wykonjemy prosty ruch na inne pole
						Player playerS = GameObject.Find("Player"+gS.whoseTurnID).GetComponent<Player>();
						playerS.makeMove(HitedFieldScript.idField);
					}

				}
			} 
		}
	}
}