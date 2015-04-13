using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// Klasa Buttons.Control
/// </summary>
public class ButtonsControl : Menu{
	/// <param name="click">Dzwięku "clicku" n</param>
	public AudioClip click;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	/// <summary>
	/// Odtwarzanie dzwięku 
	/// </summary>
	public void PlayClickSound(){
		this.GetComponent<AudioSource> ().Play ();
	}
}
