using UnityEngine;
using System.Collections;

/// <summary>
// Klasa odpowiedzialna za perspektywę kamery(widok gry)
/// </summary>
public class CameraPerspective : MonoBehaviour {

	public GameObject buttontopview, buttonfreeview, leftjoy, rightjoy;
	public Camera camera; 
	private Vector3 camRot,camPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CameraZoomIn () {
		camera.fieldOfView = Mathf.Lerp(camera.fieldOfView,10,Time.deltaTime*5);
	}
	public void CameraZoomOut () {
		camera.fieldOfView = Mathf.Lerp(camera.fieldOfView,60,Time.deltaTime*5);
	}

	public void setTopView() {
		/// <summary>
		// ustawienie elementów interfejsu
		/// </summary>
		leftjoy.SetActive (false);
		rightjoy.SetActive (false);
		buttontopview.SetActive (false);
		buttonfreeview.SetActive (true);
		/// <summary>
		// zapamiętanie pozycji kamery
		/// </summary>
		camRot = camera.transform.rotation.eulerAngles;
		camPos = camera.transform.position;
		/// <summary>
		// ustawienie kamery nad planszą
		/// </summary>
		camera.transform.rotation = Quaternion.Euler (new Vector3 (90, 270, 0));
		camera.transform.position = new Vector3 (0, 10, 0);
	}

	public void setFreeView() {
		/// <summary>
		// ustawienie elementów interfejsu
		/// </summary>
		leftjoy.SetActive (true);
		rightjoy.SetActive (true);
		buttontopview.SetActive (true);
		buttonfreeview.SetActive (false);
		/// <summary>
		// przywrócenie kamery do poprzedniego widoku
		/// </summary>
		camera.transform.rotation = Quaternion.Euler (camRot);
		camera.transform.position = camPos;
	}
}
