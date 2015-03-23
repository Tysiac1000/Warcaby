using UnityEngine;
using System.Collections;

public class MenuBoard : MonoBehaviour {

	// zmienna określająca szybkość obracania planszy
	public float rotationSpeed;

	// Update is called once per frame
	void Update () {

		// ruch obrotowy planszy w menu głównym
		float ruch = rotationSpeed * Time.fixedDeltaTime;
		transform.RotateAround(Vector3.up, ruch * 100 * Time.deltaTime);
	}



}
