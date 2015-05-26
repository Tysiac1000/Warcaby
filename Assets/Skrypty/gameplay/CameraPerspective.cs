using UnityEngine;
using System.Collections;

/// <summary>
/// Klasa Odpoweidzialna za ustawienia kamery
/// </summary>
public class CameraPerspective : MonoBehaviour {

	public GameObject buttontopview, buttonfreeview, leftjoy, rightjoy;
	public Camera camera; 
	public float ZoomSpeed;
	private Vector3 camRot,camPos;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CameraZoomIn () {
		float fov = camera.fieldOfView;
		camera.fieldOfView = Mathf.Lerp(camera.fieldOfView,fov-ZoomSpeed,Time.deltaTime*5);
	}
	public void CameraZoomOut () {
		float fov = camera.fieldOfView;
		camera.fieldOfView = Mathf.Lerp(camera.fieldOfView,fov+ZoomSpeed,Time.deltaTime*5);
	}

	public void setTopView() {
		/// <summary>
		/// Ustawienie elementów interfejsu
		/// </summary>
		leftjoy.SetActive (false);
		buttontopview.SetActive (false);
		buttonfreeview.SetActive (true);
		/// <summary>
		/// Zapamiętanie pozycji kamery
		/// </summary>
		camRot = camera.transform.rotation.eulerAngles;
		camPos = camera.transform.position;
		/// <summary>
		/// Ustawienie kamery nad planszą
		/// </summary>
		camera.transform.rotation = Quaternion.Euler (new Vector3 (90, 270, 0));
		camera.transform.position = new Vector3 (0, 10, 0);
	}

	public void setFreeView() {
		/// <summary>
		/// Ustawienie elementów interfejsu
		/// </summary>
		leftjoy.SetActive (true);
		buttontopview.SetActive (true);
		buttonfreeview.SetActive (false);
		/// <summary>
		/// Przywrócenie kamery do poprzedniego widoku
		/// </summary>
		camera.transform.rotation = Quaternion.Euler (camRot);
		camera.transform.position = camPos;
	}
}
