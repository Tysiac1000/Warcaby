using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityEditor;

/// <summary>
// typ wyliczeniowy określający kolor pionków
/// </summary>
public enum PawnsColors{
	WHITE = 0,
	BLACK
}

/// <summary>
// typ wyliczeniowy określający tryb gry: na jednym urządzeniu, host, klient
/// </summary>
public enum GameMode{
	ONE_DEVICE = 0,
	HOST,
	CLIENT
}

/// <summary>
// Options - Klasa zażądzająca opcjami gry
/// </summary>
public class Options : MonoBehaviour {

	public GameMode gMode;
	/// <summary>
	// opcje dźwięku
	/// </summary>
	public float musicVolume;
	private float musicVolumeDefault;
	public float soundsVolume;
	private float soundsVolumeDefault;
	public GameObject Music;
	public GameObject Sounds;
	/// <summary>
	// opcje grafiki
	/// </summary>
	public bool shadows;
	private bool shadowsDefault;
	public int graphicsQuality;
	private int graphicsQualityDefault;
	public int boardStyle;
	public int pawnStyle;

	// Use this for initialization
	void Start () {
		/// <summary>
		// inicjalizujemy ustawienia dźwięku
		/// </summary>
		/// <param>
		/// 1 ustawia domyślną głośność muzyki na 1
		/// </param>
		musicVolumeDefault = musicVolume = 1;
		/// <param>
		/// 1 ustawia domyślną głośność dzwięków gry na 1
		/// </param>
		soundsVolumeDefault = soundsVolume = 1;
		/// <summary>
		// inicjalizujemy ustawienia grafiki
		/// </summary>
		/// <param>
		/// true ustawia domyślne włączenie cieni w grze
		/// </param>
		shadowsDefault = shadows = true;
		/// <param>
		/// true ustawia domyślne ustawienie jakości cieni na 3
		/// </param>
		graphicsQualityDefault = graphicsQuality = 3;
	}

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	public void backToDefaultSettings() {
		musicVolume = musicVolumeDefault;
		GameObject.Find("sliderMusic").GetComponent<Slider>().value = musicVolumeDefault;
		soundsVolume = soundsVolumeDefault;
		GameObject.Find("sliderSounds").GetComponent<Slider>().value = soundsVolumeDefault;
		shadows = shadowsDefault;
		GameObject.Find("toggleShadows").GetComponent<Toggle>().isOn = shadowsDefault;
		graphicsQuality = graphicsQualityDefault;
		GameObject.Find("sliderGraphics").GetComponent<Slider>().value = graphicsQualityDefault;
	}

	#region OPCJE SIECIOWE
	public string gameplayName;
	public PawnsColors pawnColor;
	
	public const int gamePort = 25000;
	
	public void setGameplayName(string name) {
		this.gameplayName = name;
	}
	public string getGameplayName() {
		return this.gameplayName;
	}
	
	public void setPawnColor(PawnsColors color) {
		this.pawnColor = color;
	}
	public PawnsColors getPawnColor() {
		return this.pawnColor;
	}
	#endregion

	#region OPCJE DŹWIEKU
	public void setMusicVolume(float musicVol) {
		musicVolume = musicVol;
	}
	public float getMusicVolume() {
		return musicVolume;
	}
	public void setSoundVolume(float soundVol) {
		soundsVolume = soundVol;
	}
	public float getSoundVolume() {
		return soundsVolume;
	}
	#endregion

	#region OPCJE GRAFIKI
	public void setGraphicsQuality(int gQ) {
		graphicsQuality = gQ;
	}
	public int getGraphicsQuality() {
		return graphicsQuality;
	}
	public void setShadows(bool s) {
		shadows = s;
	}
	public bool getShadows() {
		return shadows;
	}
	#endregion


}
