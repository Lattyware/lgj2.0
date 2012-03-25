using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {
	
	public float speed;

	// Use this for initialization
	void Start () {
	
		speed = 50;
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKey(KeyCode.UpArrow)) {
			
			transform.Translate(Vector3.up*Time.deltaTime*speed);
			
		}
		
		if(Input.GetKey(KeyCode.DownArrow)) {
			
			transform.Translate(Vector3.down*Time.deltaTime*speed);
			
		}
		
		if(Input.GetKey(KeyCode.LeftArrow)) {
			
			transform.Translate(Vector3.left*Time.deltaTime*speed);
			
		}
		
		if(Input.GetKey(KeyCode.RightArrow)) {
				
			transform.Translate(Vector3.right*Time.deltaTime*speed);
			
		}
		
		if(Input.GetAxis("Mouse ScrollWheel") < 0){
			
			Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize+(1*10),1);
			
		}
		
		if(Input.GetAxis("Mouse ScrollWheel") > 0){
			
			Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize-(1*10),1);
			
		}
		
		
		
	}
}
