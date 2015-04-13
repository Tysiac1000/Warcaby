using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
// Options - Klasa zażądzająca opcjami gry
/// </summary>
public class Options : MonoBehaviour {

	// opcje dźwięku
	public float musicVolume;
	public float soundsVolume;
	public GameObject Music;
	public GameObject Sounds;
	// opcje grafiki
	public bool shadows;
	public int graphicsQuality;
	public int boardStyle;
	public int pawnStyle;

	// Use this for initialization
	void Start () {
		// inicjalizujemy ustawienia dźwięku
		musicVolume = 1;
		soundsVolume = 1;
		// inicjalizujemy ustawienia grafiki
		shadows = true;
		graphicsQuality = 3;
	}

	/*
	#region OPCJE DZWIEKU
	public void ZmianyDzwiekuOK(){
		if (GameObject.Find("ToggleMuzyka").GetComponent<Toggle>().isOn) {
			Music.audio.volume = 1;
			musicLoudness = true;
		} else {
			Music.audio.volume = 0;
			musicLoudness = false;
		}

		if (GameObject.Find("ToggleEfektyDz").GetComponent<Toggle>().isOn) {
			Sounds.audio.volume = 1;	
			soundsVolume = true;
		} else {
			Sounds.audio.volume = 0;
			soundsVolume = false;
		}
	}

	public void ZmianyDzwiekuAnuluj(){
		GameObject.Find("ToggleMuzyka").GetComponent<Toggle>().isOn = musicLoudness;	
		GameObject.Find("ToggleEfektyDz").GetComponent<Toggle>().isOn = soundsVolume;	
	}
	#endregion

	#region OPCJE GRAFIKI
	public void ZmianyGrafikiOK(){
		QualitySettings.SetQualityLevel ((int)GameObject.Find ("SliderTex").GetComponent<Slider> ().value, true);
	}
	public void ZmianyGrafikiAnuluj(){
		GameObject.Find("ToggleCienie").GetComponent<Toggle>().isOn = shadows;
		GameObject.Find ("SliderTex").GetComponent<Slider> ().value = graphicsQuality;
		GameObject.Find ("SliderEffCz").GetComponent<Slider> ().value = efekty;
	}
	#endregion
	*/
}
