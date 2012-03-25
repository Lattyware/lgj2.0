using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {

	private static readonly int ISLANDS = 10;
	
	public GameObject islandPrefab;
	
	public void Start() {
		List<GameObject> islands = new List<GameObject>();
		
		for (int i=0; i<Level.ISLANDS; i++) {
			GameObject island = (GameObject) Instantiate(islandPrefab, new Vector3(1,1,1), Quaternion.identity);
			island.transform.localScale = Vector3.one*0.5f;
			islands.Add(island);
		}
		
	}
	
}