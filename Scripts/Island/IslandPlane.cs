using UnityEngine;
using System.Collections;

public class IslandPlane : MonoBehaviour {

	// Use this for initialization
	void Start() {

	}
	
	void Awake() {
		Texture2D texture = new IslandGen(128, 32, 1.5f).texture;
		this.renderer.material.mainTexture = texture;
		texture.Apply();
	}
	
	// Update is called once per frame
	void Update() {
	
	}
}
