using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameSystem : MonoBehaviour {
	
	List<GameObject> islandsObjects = new List<GameObject>();
	List<Island> islandScript = new List<Island>();
	List<Island> selectedIslandScripts = new List<Island>();
	bool islandSelected = false;
	float vikingsPer = 0;
	bool[] selectedIslands;
	
	// Use this for initialization
	void Start () {
		GameObject[] tempObjects = GameObject.FindGameObjectsWithTag("PlayerIsland");
		foreach (GameObject obj in tempObjects) {
			islandsObjects.Add(obj);
		}
		foreach(GameObject temp in islandsObjects){
			islandScript.Add(temp.GetComponent<Island>());
		}
	}
	
	// Update is called once per frame
	void Update () {
		islandSelected = false;
		foreach(Island temp in islandScript){
			if(temp.isSelected){
				temp.vikingsPer = this.vikingsPer;
				islandSelected = true;
			}
		}
	
	}
	
	public List<Island> getSelectedIslands(){
		return selectedIslandScripts;
	}
	
	public void addSelectedIsland(Island island){
		selectedIslandScripts.Add(island);
	}
	
	public void removeSelectedIsland(Island island){
		selectedIslandScripts.Remove(island);
	}
	
	void OnGUI(){
		if(islandSelected){
			vikingsPer = GUI.HorizontalSlider (new Rect (85, 30, 100, 30), vikingsPer, 0.0f, 100.0f);
			GUI.Label(new Rect (15, 25, 100, 30), "Sheep " + (100 - (int)vikingsPer));
			GUI.Label(new Rect (195, 25, 100, 30), "Vikings " + (int)vikingsPer);
			
			if(GUI.Button(new Rect(15,50,30,30), "-")) {
				foreach(Island temp in islandScript){
					if(temp.isSelected){
						temp.removeTown();
						islandSelected = true;
					}
				}
			}
			GUI.Label(new Rect(50,53,100,30),"Town");
			if(GUI.Button(new Rect(85,50,30,30), "+")) {
				foreach(Island temp in islandScript){
					if(temp.isSelected){
						temp.addTown();
						islandSelected = true;
					}
				}
			}
			
			if(GUI.Button(new Rect(15,93,30,30), "-")) {
				foreach(Island temp in islandScript){
					if(temp.isSelected){
						temp.removeFarm();
						islandSelected = true;
					}
				}
			}
			GUI.Label(new Rect(50,93,100,30),"Farm");
			if(GUI.Button(new Rect(85,93,30,30), "+")) {
				foreach(Island temp in islandScript){
					if(temp.isSelected){
						temp.addFarm();
						islandSelected = true;
					}
				}
			}
			if(GUI.Button(new Rect(15,143,30,30), "-")) {
				foreach(Island temp in islandScript){
					if(temp.isSelected){
						temp.removeDock();
						islandSelected = true;
					}
				}
			}
			GUI.Label(new Rect(50,143,100,30),"Dock");
			if(GUI.Button(new Rect(85,143,30,30), "+")) {
				foreach(Island temp in islandScript){
					if(temp.isSelected){
						temp.addDock();
						islandSelected = true;
					}
				}
			}
			
			
		}
	}
}
