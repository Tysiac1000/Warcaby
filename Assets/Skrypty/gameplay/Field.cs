using UnityEngine;
using System.Collections;

/// <summary>
/// Typ wyliczeniowy określający stan pola: puste, nie puste, dozwolony ruch
/// </summary>
public enum FieldState{
	EMPTY = 0,
	NON_EMPTY,
	MOVE_ALLOWED,
	HIGHLIGHTED
}

/// <summary>
/// Klasa Pola Gry
/// </summary>
public class Field : MonoBehaviour {

	/// <summary>	
	/// Identyfikator pola
	/// </summary>
	public string idField;		
	/// <summary>	
	/// Stan pola
	/// </summary>
	public FieldState fState;	
	/// <summary>
	/// Identyfikator pionka który stoi na polu
	/// </summary> 
	public int pawnId;			// 
	
	/// <summary>
	/// materiały dla pola niezaznaczonego i zaznaczonego
	/// </summary>
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
		this.gameObject.GetComponent<Renderer>().material = highlightedDarkFieldMaterial;
		this.fState = FieldState.HIGHLIGHTED;
	}

	public void unsetHighlighted() {
		this.gameObject.GetComponent<Renderer>().material = darkFieldMaterial;
		this.fState = FieldState.EMPTY;
	}

	private void OnFieldClick() {
		/// <summary>
		/// Sprawdzamy czy tapnięto ekran
		/// warunek dla androida: Input.touchCount > 0
		/// warunek dla Windowsa: 
		/// </summary>
		if (Input.touchCount > 0) {
			Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rFieldHit;
			bool didHit = Physics.Raycast(toMouse, out rFieldHit, 500.0f);

			/// <summary>
			/// Sprawdzamy czy trafiliśmy na obiekt z tagiem Field
			/// </summary>
			if(didHit && rFieldHit.collider.gameObject.tag == "Field") {
				Field HitedFieldScript = rFieldHit.collider.gameObject.GetComponent<Field>(); 

				/// <summary>
				/// Jedynie obiekt podświetlony będzie reagować
				/// </summary>
				if(HitedFieldScript.fState == FieldState.HIGHLIGHTED)
				{	
					/// <summary>
					/// Trzeba sprawdzić czy są bicia i/lub ruchy
					/// </summary>
					Gameplay gS = Camera.main.GetComponent<Gameplay>();
					Board bS = rFieldHit.transform.parent.transform.gameObject.GetComponent<Board>();
					if(gS.isThereCapture) {
						/// <summary>
						/// Player wykona ruch bijąc pionka
						/// Szukamy obiektu playera
						/// </summary>
						Player playerS = GameObject.Find("Player"+gS.whoseTurnID).GetComponent<Player>();
						/// <summary>
						/// Wykonujemy bicie
						/// </summary>
						playerS.makeCapture(HitedFieldScript.idField, playerS.pawnToCapture);
						/// <summary>
						/// Po tym trzeba sprawdzić czy są bicia dalej
						/// jeśli JO to ustawiamy odpowiednie flagi dla pionka i pól
						/// jeśli NE to kończymy turę
						/// </summary>
						Pawn p2m = playerS.Pawns[playerS.pawnToMove].GetComponent<Pawn>();
						if(!bS.areThereCaptures(p2m)) playerS.changeTurn(p2m);
					}
					else if (gS.isThereMove) {
						/// <summary>
						/// Jeśli nie bije, to wykonjemy prosty ruch na inne pole
						/// </summary>
						Player playerS = GameObject.Find("Player"+gS.whoseTurnID).GetComponent<Player>();
						playerS.makeMove(HitedFieldScript.idField);
					}

				}
			} 
		}
	}
}