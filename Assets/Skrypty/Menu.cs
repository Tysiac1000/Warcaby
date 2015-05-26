using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Klasa Menu Gównego
/// </summary>
public class Menu : MonoBehaviour {

	/// <param name="panelInfo"></param>
	/// <param name="mainCanva"></param>
	private GameObject panelInfo;
	public GameObject mainCanva;
	public GameObject options;
	public InputField inputGameName;
	public ToggleGroup toggleColorGroup;
	
	/// <summary>
	///  Uzyskujemy uchwyt do panel info
	/// </summary>
	void Start () {
		/// <summary>
		/// Szukamy obiektu gry 
		/// </summary>
		panelInfo = GameObject.Find("panelInfo");
	}

	/// <summary>
	/// Nasłuchiwanie na wciśnięcie klawisza ESC
	/// Update is called once per frame
	/// </summary>
	void Update () {

		// Zamknięcie aplikacji w przypadku wciśniecia przycisku powrotu
		if (Input.GetKey (KeyCode.Escape)) {
			if(Application.platform == RuntimePlatform.Android){
				Application.Quit();
			}
		}
	}

	/// <summary>
	/// Sprawdzenie platformy i zamknięcie aplikacji
	/// </summary>
	public void ExitApplication(){
		// Sprawdzamy platformę i zamykamy program
		if(Application.platform == RuntimePlatform.Android){
			Application.Quit();
		}
	}

	#region DO NOWYCH POZIOMÓW
	/// <summary>
	///Przejście do nowej sceny - utworzenie rozgrywki na jednym urządzeniu
	/// </summary>
	public void CreateGameOnOneDevice(){
		// ustawiamy tryb gry
		GameObject.Find ("Options").GetComponent<Options> ().gMode = GameMode.ONE_DEVICE;
		// ładujemy nowy poziom
		Application.LoadLevel("Gameplay");
	}

	/// <summary>
	///Przejście do nowej sceny - utworzenie rozgrywki
	/// </summary>
	public void CreateGame(){
		options.SendMessage ("setGameplayName", inputGameName.text);
		//Debug.Log(netConf.getGameplayName().ToString());

		string activeToggle = toggleColorGroup.ActiveToggles ().First ().ToString ();
		switch (activeToggle) {
			case "toggleWhite (UnityEngine.UI.Toggle)" : options.SendMessage ("setPawnColor", PawnsColors.WHITE);
				//Debug.Log("WHITE");
				break;
			case "toggleBlack (UnityEngine.UI.Toggle)" : options.SendMessage ("setPawnColor", PawnsColors.BLACK);
			//Debug.Log("BLACK");
				break;
		}

		// ustawiamy tryb gry
		GameObject.Find ("Options").GetComponent<Options> ().gMode = GameMode.HOST;

		Application.LoadLevel("Gameplay");
	}

	/// <summary>
	///Przejście do nowej sceny - dołączenie do rozgrywkui
	/// </summary>
	public void JoinGame(){
		// ustawiamy tryb gry
		GameObject.Find ("Options").GetComponent<Options> ().gMode = GameMode.CLIENT;

		Application.LoadLevel("Gameplay");
	}

	/// <summary>
	///Przejście do nowej sceny - samouczka
	/// </summary>
	public void StartTutorial(){
		Application.LoadLevel("Tutorial");
	}
	#endregion

	#region PANEL MENU
	public void changeMusicVolume() {
		Slider musicSlider = GameObject.Find("sliderMusic").GetComponent<Slider>();
		options.SendMessage ("setMusicVolume", musicSlider.value);
	}
	public void changeSoundsVolume() {
		Slider soundsSlider = GameObject.Find("sliderSounds").GetComponent<Slider>();
		options.SendMessage ("setSoundVolume", soundsSlider.value);
	}
	public void changeGraphicQuality() {
		Slider graphicSlider = GameObject.Find("sliderGraphics").GetComponent<Slider>();
		options.SendMessage ("setGraphicsQuality", graphicSlider.value);
	}
	public void changeShadows() {
		Toggle graphicSlider = GameObject.Find("toggleShadows").GetComponent<Toggle>();
		options.SendMessage ("setShadows", graphicSlider.isOn);
	}
	#endregion

	#region PANEL INFO
	/// <summary>
	/// Pokazujemy panel z informacjami
	/// </summary>
	public void Info(){
		// pokazujemy panel z informacjami
		panelInfo.SetActive (true);
	}

	/// <summary>
	/// Ukrywamy panel z informacjami
	/// </summary>
	public void InfoExit(){
		panelInfo.SetActive (false);
	}
	#endregion
	
}
