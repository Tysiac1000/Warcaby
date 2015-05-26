using UnityEngine;
using System.Collections;

/// <summary>
/// Klasu MenuBoard Odpowiada za kontrolę planszy w menu głównym
/// </summary>
public class MenuBoard : MonoBehaviour {
	
	/// <summary>
	// zmienna określająca szybkość obracania planszy	
	/// </summary>
	public float rotationSpeed;
	
	// Update is called once per frame
	/// <summary>
	// ruch obrotowy planszy w menu głównym
	/// </summary>
	void Update () {
		float ruch = rotationSpeed * Time.fixedDeltaTime;
		transform.RotateAround(Vector3.up, ruch * 100 * Time.deltaTime);
	}
	
	
	
}
