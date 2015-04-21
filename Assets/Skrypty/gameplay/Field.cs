using UnityEngine;
using System.Collections;

// typ wyliczeniowy określający stan pola: puste, nie puste, dozwolony ruch
public enum FieldState{
	EMPTY = 0,
	NON_EMPTY,
	MOVE_ALLOWED
}

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

	// reakcja na kliknięcie pola w grze
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