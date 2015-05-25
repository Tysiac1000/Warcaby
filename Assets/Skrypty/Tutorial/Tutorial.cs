using UnityEngine;
using System.Collections;
/// <summary>
/// Klasa samouczka 
/// </summary>
public class Tutorial : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	///  Funkcja powrotu do Menu Głównego
	/// </summary>
	public void BackToMenu () 
	{
		Application.LoadLevel("Menu");
	}
}