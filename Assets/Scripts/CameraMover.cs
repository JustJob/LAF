using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {

	public float speed;
	public int width;
	public int height;

	// Use this for initialization
	void Start () {
	}
	
	void FixedUpdate() {
		float horizontalSpeed = Input.GetAxis ("Horizontal");
		float verticalSpeed = Input.GetAxis ("Vertical");
		float screenHeight = camera.orthographicSize;
		float screenWidth = screenHeight * Screen.width / Screen.height; 

		transform.Translate (new Vector3(horizontalSpeed, verticalSpeed, 0) * speed);

		Vector3 newPosition = transform.position;
		if (newPosition.x + screenWidth > width) {
			newPosition.x = width - screenWidth;
		}
		if (newPosition.x < screenWidth) {
			newPosition.x = screenWidth;
		}
		if (newPosition.z + screenHeight > height) {
			newPosition.z = height - screenHeight;
		}
		if (newPosition.z < screenHeight) {
			newPosition.z = screenHeight;
		}

		transform.position = newPosition;
	}
}
