using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public struct FieldCords{
	public int Y;
	public int X;
}

/// <summary>
/// Klasa Planszy
/// </summary>
public class Board : MonoBehaviour {

	public GameObject Field_bright;
	public GameObject Field_dark;

	public GameObject[][] Fields;
	private int fc = 8;
	// Use this for initialization
	void Start () {
		fillBoardWithFields();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Zwraca listę ciemnych pól
	/// </summary>
	public List<GameObject> getDarkFieldsList() {
		List<GameObject> fieldsList = new List<GameObject>();
		for (int a = 0; a < 8; a++) {
						for (int b = 0; b < 8; b++) {
								if ((a % 2) == 1) {
										if ((b % 2) == 1)
												fieldsList.Add (Fields [a] [b]);
								} else {
										if ((b % 2) == 0)
												fieldsList.Add (Fields [a] [b]);
								}
						}
				}
		return fieldsList;
	}

	/// <summary>
	/// Funkcja zamienia ID pola ze stringa do postaci współrzędnych
	/// </summary>
	public FieldCords TranslateCords(string sID) {
		FieldCords fc;
		byte[] FieldCodes = Encoding.ASCII.GetBytes (sID);
		fc.Y = FieldCodes [0] - 65;
		fc.X = FieldCodes [1] - 49;
		return fc;
	}

	/// <summary>
	/// Ustawia status pola
	/// <summary>
	public void setFieldState(FieldState fStat, string idField) {
		/// <param>
		/// Konwertujemy ID pola
		/// </param>
		FieldCords fc = TranslateCords(idField);
		/// <summary>
		/// Ustawiamy stan dla pola podany w argumencie
		/// </summary>
		//GameObject.Find (idField).GetComponent<Field> ().fState = fStat;
		Fields[fc.Y][fc.X].GetComponent<Field> ().fState = fStat;
	}

	/// <summary>
	/// zwraca status pola
	/// </summary>
	public FieldState getFieldState(string idField) {
		/// <summary>
		// konwertujemy id pola
		/// </summary>
		FieldCords fc = TranslateCords(idField);
		/// <summary>
		/// zwracamy status pola
		/// </summary>
		return Fields[fc.Y][fc.X].GetComponent<Field> ().fState;
	}

	/// <summary>
	/// zwraca współrzędne środka pola 
	/// </summary>
	public Vector3 getFieldCenterCoordinate(string idField){
		/// <summary>
		/// Konwertujemy id pola
		/// </summary>
		FieldCords fc = TranslateCords(idField);
		/// <summary>
		// Szukamy pola o podanym numerze ID 
		/// </summary>
		GameObject field = Fields[fc.Y][fc.X].gameObject;
		/// <summary>
		/// Zwracamy pozycję pola jako Vector3
		/// </summary>
		return new Vector3 (field.transform.position.x,(float)0.38,field.transform.position.z);
	}

	/// <summary>
	// zwracamy ID pola
	/// </summary>
	public int getPawnIDonField(string idField){
		/// <summary>
		/// Konwertujemy ID pola
		/// </summary>		// konwertujemy id pola
		FieldCords fc = TranslateCords(idField);
		/// <summary>
		// zwracamy ID pola
		/// </summary>
		return Fields[fc.Y][fc.X].GetComponent<Field>().pawnId;
	}

	/// <summary>
	/// Zwraca id pola w lini prostej, jeśli pole jest zbyt blisko krawędzi planszy to zwraca napis brak_pola
	/// </summary>
	public string getFieldIDinLine(string startFieldID, string middleFieldID){
		byte[] startFieldCodes = Encoding.ASCII.GetBytes (startFieldID);
		int ysID = startFieldCodes[0];
		int xsID = startFieldCodes[1]-48;
		byte[] middleFieldCodes = Encoding.ASCII.GetBytes (middleFieldID);
		int ymID = middleFieldCodes[0];
		int xmID = middleFieldCodes[1]-48;

		int yeID, xeID;

		if (ymID > ysID) {
						yeID = ymID + 1;
						if (yeID > 72) return "brak_pola";
				} else {
						yeID = ymID - 1;
						if (yeID < 65) return "brak_pola";
				}
		if (xmID > xsID) {
						xeID = xmID + 1;
						if (xeID > 8) return "brak_pola";
				} else {
						xeID = xmID - 1;
						if (xeID < 1) return "brak_pola";
				}

		return ((char)yeID).ToString () + xeID.ToString ();
	}

	/// <summary>
	/// Sprawdza czy pole sąsiadujące z pionkiem jest z przodu
	/// </summary>
	public bool isItFront(string sFieldID, string eFieldID, int pID) {

		byte[] startFieldCodes = Encoding.ASCII.GetBytes (sFieldID);
		int ysID = startFieldCodes[0];
		byte[] endFieldCodes = Encoding.ASCII.GetBytes (eFieldID);
		int yeID = endFieldCodes[0];

		if (pID == 1) {
				if (yeID > ysID) return true;
				else return false;
		} else if (pID == 2) {
				if (yeID < ysID) return true;
				else return false;
		} else return false;
	}

	/// <summary>
	/// Funkcja zwraca id-ki pól sąsiadujących z polem o podanym id jako argument
	/// </summary>
	public List<string> getSurroundingFields(string idField){
		Debug.Log ("Sprawdzam pole: "+ idField);
		byte[] asciiBytes = Encoding.ASCII.GetBytes (idField);
		int yID = asciiBytes[0];
		int xID = asciiBytes[1]-48;

		List<string> closestFields = new List<string>();
		if( (yID+1) < 73 && (xID+1) < 9 ) closestFields.Add (((char)(yID+1)).ToString() + (xID+1).ToString());   // prawe górne pole
		if( (yID+1) < 73 && (xID-1) > 0 ) closestFields.Add (((char)(yID+1)).ToString() + (xID-1).ToString());   // lewe górne pole
		if( (yID-1) > 64 && (xID+1) < 9 ) closestFields.Add (((char)(yID-1)).ToString() + (xID+1).ToString());   // prawe dolne pole
		if( (yID-1) > 64 && (xID-1) > 0 ) closestFields.Add (((char)(yID-1)).ToString() + (xID-1).ToString());   // lewe dolne pole
		return closestFields;
	}

	public List<string> getSurroundingFieldsWithEnemies(string idField, int pID){
		List<string> surroundingFields = getSurroundingFields (idField);
		List<string> SurroundingFieldsWithEnemies = new List<string>();

		foreach (string sField in surroundingFields) {
			Field fS = this.transform.FindChild(sField).GetComponent<Field>();
			/// <summary>
			/// Sprawdzamy czy coś stoi na polu
			/// </summary>
			if (fS.fState == FieldState.NON_EMPTY) {
				/// <summary>
				/// Sprawdzamy czy to pionek wroga
				/// </summary>
				if ((int)(fS.pawnId/100) != pID) {
					SurroundingFieldsWithEnemies.Add(sField);
				}
			}
		}
		return SurroundingFieldsWithEnemies;
	}

	public List<string> getFrontFields(string idField, int pID) {
		List<string> bFields = getSurroundingFields (idField);
		List<string> frontFields = new List<string>();

		foreach (string field in bFields) {
			if (isItFront(idField,field,pID)) {
				frontFields.Add(field);
			}
		}
		return frontFields;
	}

	/// <summary>
	/// Funkcja wypełnia plansze polami
	/// </summary>
	void fillBoardWithFields() {
		Fields = new GameObject[fc][];
		for (int a = 0; a < 8; a++) 
		{
			Fields[a] = new GameObject[fc];
			for (int b = 0; b < 8; b++) 
			{
				/// <summary>
				/// Tworzymy nowe pola na zmianę, raz ciemne raz jasne z przesunięciem co drugi rząd
				/// </summary>
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
				/// <summary>
				/// przypisujemy nazwy do obiektów
				/// </summary>
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
				
				/// <summary>
				/// Nowo utworzone pole trzeba ustawić jako dziecko obiektu Board
				/// </summary>
				Fields[a][b].transform.parent = this.gameObject.transform;
			
				/// <summary>
				/// Dodajemy tag
				/// </summary>
				Fields[a][b].tag = "Field";
			}
		}
		Debug.Log ("Wypełniono planszę polami");
	}
	
	public bool searchForCaptures(Pawn cPawn){
		Gameplay gp = Camera.main.GetComponent<Gameplay>();
		Player eP = gp.getEnemyPlayer().GetComponent<Player>();
		Player cP = GameObject.Find("Player"+gp.whoseTurnID).GetComponent<Player>();

		bool isCapture = false;

		/// <summary>
		// Odczytujemy id pól znajdujących się w sąsiedztwie pionka, na których stoi wróg
		/// </summary>
		List<string> SurrFieldsWithEnemies = getSurroundingFieldsWithEnemies(cPawn.fieldID, (int)(cPawn.pawnID/100) );
		
		bool isCaptureAnywhere = false;
		/// <summary>
		/// Sprawdzamy czy pola za pionkiem wroga w lini prostej są wolne
		/// </summary>
		foreach (string sfields in SurrFieldsWithEnemies){
			/// <summary>
			/// Najpierw szukamy id pola za pionkiem wroga
			/// </summary>
			string fieldIDinLine = getFieldIDinLine(cPawn.fieldID, sfields);
			/// <summary>
			/// Trzeba uzyskać id pionka wroga do bicia
			/// </summary>
			int pawn2capID = getPawnIDonField(sfields);
			/// <summary>
			/// ID pionka trzeba przetłumaczyć na index
			/// </summary>
			int pawn2CapIndex;
			if (gp.whoseTurnID == 1) pawn2CapIndex = pawn2capID-201;
			else pawn2CapIndex = pawn2capID-101;
			Pawn enemyPawn2Cap = eP.Pawns[pawn2CapIndex].GetComponent<Pawn>();
			
			/// <summary>
			/// Zabezpieczenie przed sytuacją kiedy nie ma możłiwości bicia pionków, 
			/// wtedy musi nastąpić zmiana tury
			/// </summary>
			
			if (fieldIDinLine != "brak_pola" && enemyPawn2Cap.pState != PawnState.CAPTURED) {
				Field fieldScript = GameObject.Find(fieldIDinLine).GetComponent<Field>(); 
				/// <summary>
				/// Sprawdzamy czy to pole jest puste 
				/// </summary>
				if (fieldScript.fState == FieldState.EMPTY || fieldScript.fState == FieldState.MOVE_ALLOWED){

					isCapture = true;
					/// <summary>
					/// Jeśli tak to podświetlamy
					/// </summary>
					fieldScript.setHighlighted();
					/// <param>
					/// Zapamiętujemy id pionka ktory wykonuje bicie
					/// </param>
					cP.pawnToMove = (cPawn.pawnID-(gp.whoseTurnID*100)-1);

					//Debug.Log("id bitego pionka: " + pawn2capID);
					cP.pawnToCapture = pawn2capID;
				}
			}
		}

		return isCapture;
	}

	public bool areThereCaptures(Pawn cPawn){
		Gameplay gp = Camera.main.GetComponent<Gameplay>();
		Player eP = gp.getEnemyPlayer().GetComponent<Player>();
		Player cP = GameObject.Find("Player"+gp.whoseTurnID).GetComponent<Player>();
		
		bool isCapture = false;
		
		/// <summary>
		/// Odczytujemy id pól znajdujących się w sąsiedztwie pionka, na których stoi wróg
		/// </summary>
		List<string> SurrFieldsWithEnemies = getSurroundingFieldsWithEnemies(cPawn.fieldID, (int)(cPawn.pawnID/100) );
		
		bool isCaptureAnywhere = false;
		/// <summary>
		/// Sprawdzamy czy pola za pionkiem wroga w lini prostej są wolne
		/// </summary>
		foreach (string sfields in SurrFieldsWithEnemies){
			/// <summary>
			/// Najpierw szukamy id pola za pionkiem wroga
			/// </summary>
			string fieldIDinLine = getFieldIDinLine(cPawn.fieldID, sfields);
			/// <summary>
			/// Trzeba uzyskać id pionka wroga do bicia
			/// </summary>
			int pawn2capID = getPawnIDonField(sfields);
			/// <summary>
			/// ID pionka trzeba przetłumaczyć na index
			/// </summary>
			int pawn2CapIndex;
			if (gp.whoseTurnID == 1) pawn2CapIndex = pawn2capID-201;
			else pawn2CapIndex = pawn2capID-101;
			Pawn enemyPawn2Cap = eP.Pawns[pawn2CapIndex].GetComponent<Pawn>();
			
			if (fieldIDinLine != "brak_pola" && enemyPawn2Cap.pState != PawnState.CAPTURED) {
				Field fieldScript = GameObject.Find(fieldIDinLine).GetComponent<Field>(); 
				/// <summary>
				/// Sprawdzamy czy to pole jest puste 
				/// <summary>
				if (fieldScript.fState == FieldState.EMPTY || fieldScript.fState == FieldState.MOVE_ALLOWED){
					isCapture = true;
				}
			}
		}
		
		return isCapture;
	}
}