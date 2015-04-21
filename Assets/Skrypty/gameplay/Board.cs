using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public GameObject Field_bright;
	public GameObject Field_dark;
	public GameObject GameBoard;

	private GameObject[][] Fields;
	private int fc = 8;
	// Use this for initialization
	void Start () {
		fillBoardWithFields();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setFieldState(FieldState fStat, string idField) {
		// ustawiamy stan dla pola podany w argumencie
		GameObject.Find (idField).GetComponent<Field> ().fState = fStat;
	}

	public Vector3 getFieldCenterCoordinate(string idField){
		// szukamy pola o podanym numerze id 
		GameObject field = GameBoard.transform.FindChild (idField).gameObject;
		// zwracamy pozycję pola jako Vector3
		return new Vector3 (field.transform.position.x,(float)0.38,field.transform.position.z);
	}
//	funkcja generująca pola dla planszy
	void fillBoardWithFields() {
		Fields = new GameObject[fc][];
		for (int a = 0; a < 8; a++) 
		{
			Fields[a] = new GameObject[fc];
			for (int b = 0; b < 8; b++) 
			{
				// Tworzymy nowe pola na zmianę, raz ciemne raz jasne z przesunięciem co drugi rząd
				if ((a % 2) == 1)
				{
					if ((b % 2) == 1) { 
						Fields[a][b] = (GameObject)Instantiate(Field_dark); 
						Fields[a][b].AddComponent<BoxCollider>();
					}
					else Fields[a][b] = (GameObject)Instantiate(Field_bright);
				}
				else 
				{
					if ((b % 2) == 0) {
						Fields[a][b] = (GameObject)Instantiate(Field_dark);
						Fields[a][b].AddComponent<BoxCollider>();
					}
					else Fields[a][b] = (GameObject)Instantiate(Field_bright);
				}
				
				Fields[a][b].transform.localScale = new Vector3((float)3.77,(float)3.77,1);
				Fields[a][b].transform.position = new Vector3((float)(1.32-(a * 0.38)),(float)0.22,(float)(-1.339+(b * 0.38)));
				Fields[a][b].AddComponent<Field>();
				// przypisujemy nazwy do obiektów
				switch (a){
					case 0 :Fields[a][b].name = "A" + (b+1).ToString(); break;
					case 1 :Fields[a][b].name = "B" + (b+1).ToString(); break;
					case 2 :Fields[a][b].name = "C" + (b+1).ToString(); break;
					case 3 :Fields[a][b].name = "D" + (b+1).ToString(); break;
					case 4 :Fields[a][b].name = "E" + (b+1).ToString(); break;
					case 5 :Fields[a][b].name = "F" + (b+1).ToString(); break;
					case 6 :Fields[a][b].name = "G" + (b+1).ToString(); break;
					case 7 :Fields[a][b].name = "H" + (b+1).ToString(); break;
				}
				Fields[a][b].GetComponent<Field>().idField = Fields[a][b].name;
				
				// Nowo utworzone pole trzeba ustawić jako dziecko obiektu Board
				Fields[a][b].transform.parent = GameBoard.gameObject.transform;
			
				// dodajemy tag
				Fields[a][b].tag = "Field";
			}
		}
	}
	
	public bool moveCheck(){
		bool allow = true;
		return allow;
	}
}