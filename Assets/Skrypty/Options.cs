using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityEditor;

// typ wyliczeniowy określający kolor pionków
public enum PawnsColors{
	WHITE = 0,
	BLACK
}

// typ wyliczeniowy określający tryb gry: na jednym urządzeniu, host, klient
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
	// opcje dźwięku
	public float musicVolume;
	private float musicVolumeDefault;
	public float soundsVolume;
	private float soundsVolumeDefault;
	public GameObject Music;
	public GameObject Sounds;
	// opcje grafiki
	public int graphicsQuality;
	private int graphicsQualityDefault;
	public int boardStyle;
	public int pawnStyle;

	// Use this for initialization
	void Start () {
		// inicjalizujemy ustawienia dźwięku
		musicVolumeDefault = musicVolume = 1;
		soundsVolumeDefault = soundsVolume = 1;
		// inicjalizujemy ustawienia grafiki
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
		AudioListener.volume = soundVol;
	}
	public float getSoundVolume() {
		return soundsVolume;
	}
	#endregion

	#region OPCJE GRAFIKI
	public void setGraphicsQuality(int gQ) {
		graphicsQuality = gQ;
		string[] names = QualitySettings.names;
		QualitySettings.SetQualityLevel (graphicsQuality, true);
	}
	public int getGraphicsQuality() {
		return graphicsQuality;
	}

	#endregion


}
