using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectIsland : MonoBehaviour {
	
	public GameObject targettedIsland;
	public GameSystem gSystem;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		 if (Physics.Raycast(ray, out hit, 100)){
            //Debug.DrawLine(ray.origin, hit.point);
			//Debug.Log(hit.collider.tag);
			if(Input.GetMouseButtonDown(0)){
				if(hit.collider.gameObject.tag.Equals("PlayerIsland")){
					bool isSelected = hit.collider.gameObject.GetComponent<Island>().selectIsland();
					if(isSelected){
						gSystem.addSelectedIsland(hit.collider.GetComponent<Island>());
					}else{
						gSystem.removeSelectedIsland(hit.collider.GetComponent<Island>());
					}
				}
				
				if(hit.collider.gameObject.tag.Equals("EnemyIsland")){
					targettedIsland = hit.collider.gameObject;
					List<Island> islands = gSystem.getSelectedIslands();

					if(islands.Count > 0){
						targettedIsland.GetComponent<Island>().flashTargetted();
						
						foreach(Island island in islands){
							island.GetComponent<Island>().targetIsland(targettedIsland);
						}
					}
				}
			}
		}
	}
}
