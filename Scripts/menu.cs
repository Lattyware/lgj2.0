using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour {

	void Start() {

	}

	void OnGUI() {
		if(GUI.Button(new Rect(10,10,300,50), "Tutorial")) {
		}
		if(GUI.Button(new Rect(10,70,300,50), "Skirmish")) {
		}
		if(GUI.Button(new Rect(10,190,300,50), "Exit")) {
		}
	}
}
