using UnityEngine;
using System.Collections;

/// <summary>
/// Typ wyliczeniowy określający stan pola: puste, nie puste, dozwolony ruch
/// </summary>
public enum FieldState{
	EMPTY = 0,
	NON_EMPTY,
	MOVE_ALLOWED
}
/// <summary>
/// Klasa Pola Gry
/// </summary>
public class Field : MonoBehaviour {

	public string idField;
	public FieldState fState;
	public bool isSelected;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		OnFieldClick ();
	}

	/// <summary>
	/// reakcja na kliknięcie pola w grze
	/// </summary>
	private void OnFieldClick() {
		if (Input.GetMouseButtonDown (0)) {
			Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rFieldHit;
			bool didHit = Physics.Raycast(toMouse, out rFieldHit, 500.0f);
			if(didHit && rFieldHit.collider.gameObject.tag == "Field") {
				Field HitedFieldScript = rFieldHit.collider.gameObject.GetComponent<Field>(); 

				if(HitedFieldScript.fState == FieldState.MOVE_ALLOWED)
				{	
					//rhHit.collider.gameObject.renderer.material = selectedPawnMat;
					
					isSelected = false;
					rFieldHit.collider.gameObject.GetComponent<Field>().isSelected = true;
				}
				
			} else {
				Debug.Log("Kliknęto w pustą przestrzeń");
			}
		}
	}
}