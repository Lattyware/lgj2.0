using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIScript : MonoBehaviour {
	
	public List<GameObject> islands = new List<GameObject>();
	public List<GameObject> playerIslands = new List<GameObject>();
	public GameSystem gameSys;
	float timer = 0;
	List<bool> priorityList = new List<bool>();
	GameObject targetIsland;

	// Use this for initialization
	void Start () {
		GameObject[] tempObjectArray = GameObject.FindGameObjectsWithTag("EnemyIsland");
		for(int i = 0;i<tempObjectArray.Length;i++){
			islands.Add(tempObjectArray[i]);
		}
		tempObjectArray = GameObject.FindGameObjectsWithTag("PlayerIsland");
		for(int i = 0;i<tempObjectArray.Length;i++){
			playerIslands.Add(tempObjectArray[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		timer += Time.deltaTime;
		if(timer > 3){
			//System.Random random = new System.Random();
			//Debug.Log(randomNumber);
			timer = 0;
			
				foreach(GameObject temp in islands){
					bool isMilitary = false;
					foreach(GameObject playerTemp in playerIslands){
					Debug.Log(Vector3.Distance(temp.transform.position, playerTemp.transform.position));
						float dist = Vector3.Distance(temp.transform.position, playerTemp.transform.position);
						if(dist < 70){
							isMilitary = true;
							if(targetIsland != null && Vector3.Distance(targetIsland.transform.position,playerTemp.transform.position) < dist){
								targetIsland = playerTemp;
							}else if(targetIsland == null){
								targetIsland = playerTemp;
							}
						}
					}
					Island tempIsland = temp.GetComponent<Island>();
					Debug.Log(isMilitary);
					if(isMilitary){
						int randomNumber = (int)(Random.value*5);
						if(randomNumber == 0){
							if(tempIsland.dockNum < 2){
								tempIsland.addDock();
							}
//							if(tempIsland.sumThings() >= tempIsland.maxThings){
//								tempIsland.removeDock();
//							}else{
//								tempIsland.addDock();
//							}
						}else if(randomNumber == 2 || randomNumber == 3 || randomNumber == 1){
							if(tempIsland.sumThings() >= tempIsland.maxThings){
								tempIsland.removeTown();
							}else{
								tempIsland.addTown();
							}
						}else if(randomNumber == 4){
							if(tempIsland.sumThings() >= tempIsland.maxThings){
								tempIsland.removeFarm();
							}else{
								tempIsland.addFarm();
							}
						}
						tempIsland.launchBoat(targetIsland,10);
					}else{
						int randomNumber = (int)(Random.value*5);
						if(randomNumber == 0){
							if(tempIsland.sumThings() >= tempIsland.maxThings){
								tempIsland.removeDock();
							}else{
								tempIsland.addDock();
							}
						}else if(randomNumber == 1){
							if(tempIsland.sumThings() >= tempIsland.maxThings){
								tempIsland.removeTown();
							}else{
								tempIsland.addTown();
							}
						}else if(randomNumber == 4 || randomNumber == 3 || randomNumber == 2){
							if(tempIsland.sumThings() >= tempIsland.maxThings){
								tempIsland.removeFarm();
							}else{
								tempIsland.addFarm();
							}
						}
					}
//					if(randomNumber == 0){
//						if(tempIsland.sumThings() >= tempIsland.maxThings){
//							tempIsland.removeDock();
//						}else{
//							tempIsland.addDock();
//						}
//					}else if(randomNumber == 1){
//						if(tempIsland.sumThings() >= tempIsland.maxThings){
//							tempIsland.removeTown();
//						}else{
//							tempIsland.addTown();
//						}
//					}else if(randomNumber == 2){
//						if(tempIsland.sumThings() >= tempIsland.maxThings){
//							tempIsland.removeFarm();
//						}else{
//							tempIsland.addFarm();
//						}
//					}
				}
			setSlider();
			
		}
	}
	
	void setSlider(){
		foreach(GameObject temp in islands){
			Island tempIsland = temp.GetComponent<Island>();
			//tempIsland.vikingsPer = ((tempIsland.townNum) / tempIsland.maxThings * (tempIsland.farmNum) / tempIsland.maxThings) * 100;
			float numOfBuildings = tempIsland.townNum - tempIsland.farmNum;
			if(numOfBuildings > 0){
				tempIsland.vikingsPer = numOfBuildings * 20;
			}else{
				tempIsland.vikingsPer = 100 - (-numOfBuildings) * 20;
			}
			//Debug.Log(tempIsland.vikingsPer);
		}
	}
	
	void getPriority(){
		foreach(GameObject temp in islands){
			
		}
	}
}
