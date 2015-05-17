using UnityEngine;
using System.Collections;

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
		// ustawienie elementów interfejsu
		leftjoy.SetActive (false);
		buttontopview.SetActive (false);
		buttonfreeview.SetActive (true);
		// zapamiętanie pozycji kamery
		camRot = camera.transform.rotation.eulerAngles;
		camPos = camera.transform.position;
		// ustawienie kamery nad planszą
		camera.transform.rotation = Quaternion.Euler (new Vector3 (90, 270, 0));
		camera.transform.position = new Vector3 (0, 10, 0);
	}

	public void setFreeView() {
		// ustawienie elementów interfejsu
		leftjoy.SetActive (true);
		buttontopview.SetActive (true);
		buttonfreeview.SetActive (false);
		// przywrócenie kamery do poprzedniego widoku
		camera.transform.rotation = Quaternion.Euler (camRot);
		camera.transform.position = camPos;
	}
}
