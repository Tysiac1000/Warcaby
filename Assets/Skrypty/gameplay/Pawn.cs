using UnityEngine;
using System.Collections;
	/// <summary>
	/// Klasa Pionka
	/// </summary>
public class Pawn : MonoBehaviour {

	public GameObject shadow;
	public int pawnID;
	public bool isSelected;
	/// <summary>
	/// Zmienna przechowujaca  kolory pionków, oraz materialy z których są zrobione 
	/// </summary>
	private Material selectedPawnMat,whitePawn,blackPawn;
	
	
	
	/// <summary>
	/// Funkcja inicjuje pionka 
	/// </summary>
	void Start () {
		Vector3 position = new Vector3(this.transform.position.x,this.transform.position.y + 0.01F,this.transform.position.z);
		selectedPawnMat = Resources.Load("selectedPawn", typeof(Material)) as Material;
		whitePawn = Resources.Load("whitePawn", typeof(Material)) as Material;
		blackPawn = Resources.Load("blackPawn", typeof(Material)) as Material;
		isSelected = false;
	}
	
	/// <summary>
	/// W funkcji Update wywołana jest funkcja OnPawnClick
	/// </summary>
	// Update is called once per frame
	void Update () {
		OnPawnClick ();
	}
	/// <summary>
	/// Funkcja prywatna sprawdzająca czy pionek został klikniety 
	/// </summary>
	private void OnPawnClick() {
		if (Input.GetMouseButtonDown(0)) {
			Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rhPawnHit;
			bool didHit = Physics.Raycast(toMouse, out rhPawnHit, 500.0f);
			if(didHit && rhPawnHit.collider.gameObject.tag == "Pawn") {
				if(rhPawnHit.collider.gameObject.transform.parent.gameObject.GetComponent<Player>().playerID == Camera.main.GetComponent<Gameplay>().whoseTurnID)
				{
					if(this.gameObject.transform.parent.gameObject.name == "Player1") {
						this.gameObject.renderer.material = blackPawn;
					} else this.gameObject.renderer.material = whitePawn;
					
					rhPawnHit.collider.gameObject.renderer.material = selectedPawnMat;

					isSelected = false;
					rhPawnHit.collider.gameObject.GetComponent<Pawn>().isSelected = true;
				}

			} else {
				Debug.Log("Kliknęto w pustą przestrzeń");
			}
		}
	}
	/// <summary>
	/// Funkcja publiczna umożliwiająca wykonanie ruchu 
	/// </summary>
	/// <param name="fieldId">Id pola na które przesunięty ma być pionek</param>
	public void move(string fieldId) {
		// szukamy pozycji na którą postawimy pionek
		Vector3 dest = GameObject.Find ("Board").GetComponent<Board> ().getFieldCenterCoordinate (fieldId);
		// przenosimy ten pionek na pozycję dest
		this.gameObject.transform.localPosition = dest;
	}
	/// <summary>
	/// Funkcja publiczna niszcząca pionek
	/// </summary>
	public void destroy() {
		
	}
}
