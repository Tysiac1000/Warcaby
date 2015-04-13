using UnityEngine;
using System.Collections;

/// <summary>
/// Klasa Menu Gównego
/// </summary>

public class Menu : MonoBehaviour 

	/// <param name="panelInfo"></param>
	/// <param name="mainCanva"></param>

	private GameObject panelInfo;
	public GameObject mainCanva;

	/// <summary>
	/// Uzyskujemy uchwyt do panel info
	/// </summary>
	void Start () {	
		panelInfo = GameObject.Find("panelInfo");
	}

	/// <summary>
	/// Nasłuchiwanie na wciśnięcie klawisza ESC
	/// </summary>
	// Update is called once per frame
	void Update () {

		// Zamknięcie aplikacji w przypadku wciśniecia przycisku powrotu
		if (Input.GetKey (KeyCode.Escape)) {
			if(Application.platform == RuntimePlatform.Android){
				Application.Quit();
			}
		}
	}



	// FUNKCJE WŁASNE ----------------------------------------------------
	/// <summary>
	///Przejście do nowej sceny - utworzenie rozgrywki
	/// </summary>
	public void CreateGame(){
		Application.LoadLevel("rozgrywka");
	}
	/// <summary>
	///Przejście do nowej sceny - dołączenie do rozgrywkui
	/// </summary>
	public void JoinGame(){
		Application.LoadLevel("rozgrywka");
	}
	/// <summary>
	///Przejście do nowej sceny - samouczka
	/// </summary>
	public void StartTutorial(){

	}

	#region PANEL INFO
	/// <summary>
	/// Pokazujemy panel z informacjami
	/// </summary>
	public void Info(){
		panelInfo.SetActive (true);
	}
	/// <summary>
	/// Ukrywamy panel z informacjami
	/// </summary>
	public void InfoExit(){
		panelInfo.SetActive (false);
	}
	#endregion

/// <summary>
/// Sprawdzenie platformy i zamknięcie aplikacji
/// </summary>
	public void ExitApplication(){
		if(Application.platform == RuntimePlatform.Android){
			Application.Quit();
		}
	}

}
