using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Island : MonoBehaviour {
	
	public bool isSelected = false;
	public Color32 selectedColour;
	public Color32 orginalColour;
	public Color32 targettedColour;
	public GameObject boat;
	
	public float vikingsInc;
	public float sheepInc;
	public static float ISLAND_DISTANCE = 15;
	
	public float vikingsPer;
	
	public int vikingsNum;
	public int sheepNum;
	
	public float incTime;
	public float incTimeVal;
	
	public int townNum;
	public int farmNum;
	public int dockNum;
	
	public int maxThings;
	
	public TextMesh dispText;
	
	float flashTimer = 0f;
	public float flashDuration = 0.2f;
	bool isTargetted = false;
	bool isFlashing = false;
	
	List<Boat> launchedBoats = new List<Boat>();
	
	
	// Use this for initialization
	void Start () {
		
		vikingsInc = 10;
		sheepInc = 10;
		
		vikingsNum = 0;
		sheepNum = 0;
		
		vikingsPer = 10;
		
		incTime = Time.time;
		incTimeVal = 20;
		
		townNum = 0;
		farmNum = 3;
		dockNum = 0;
		
		maxThings = 5;
		
		Texture2D texture = new IslandGen(1024, 256, 1.5f).texture;
		this.renderer.material.mainTexture = texture;
		texture.Apply();
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(incTime+incTimeVal <= Time.time){
			
			incTime = Time.time;
			
			vikingsNum += (int) Mathf.Floor((vikingsPer/100)*vikingsInc)*townNum;
			sheepNum += (int) Mathf.Floor (((100-vikingsPer)/100)*sheepInc)*farmNum;
			
			Debug.Log("Vikings: " + vikingsNum);
			Debug.Log("Sheep: " + sheepNum);
			
		}
		
		dispText.text = "Vikings: " + vikingsNum + "\nSheep: " + sheepNum + "\nTowns: " + townNum + "\nFarms: " + farmNum;
		
		//flash red if enemy island is selected
		flashTimer += Time.deltaTime;
		if((flashTimer > flashDuration)&&(isFlashing)){
			isFlashing = false;
			if(!isSelected){
				this.renderer.material.color = orginalColour;
			}else{
				this.renderer.material.color = selectedColour;
			}
			flashTimer = 0;
		}
		
		// spawn longboats
	}
	
	
	
	public bool selectIsland()
	{
		isSelected = !isSelected;
		if(isSelected){
			this.renderer.material.color = selectedColour;
		}else{
			this.renderer.material.color = orginalColour;
		}
		
		return isSelected;
	}
	
	public void flashTargetted(){
		isTargetted = true;
		if(isTargetted){
			isFlashing = true;
			flashTimer = 0;
			this.renderer.material.color = targettedColour;
		}else{
			this.renderer.material.color = orginalColour;
		}
	}
	
	void launchBoat(GameObject tIsland, int rotation){
		Vector3 pos = transform.position;
		GameObject newBoat = (GameObject)GameObject.Instantiate(boat, pos, Quaternion.identity);
		newBoat.tag="PlayerBoat";
		// position the boat at the correct angle off the shore of the island
		newBoat.transform.Rotate(new Vector3(0, rotation, 0));
		newBoat.transform.Translate(Vector3.forward * ISLAND_DISTANCE);
		newBoat.GetComponent<Boat>().InitializeVals(10, 10, this, tIsland);
	}
	
	public void targetIsland(GameObject tIsland){
		launchBoat(tIsland, 0);
	}
	
	public void addTown() {
		
		if(sumThings()+1<=maxThings) {
			
			townNum++;
				
		}
		
	}
	
	public void addFarm() {
		
		if(sumThings()+1<=maxThings) {
			
			farmNum++;
				
		}		
	}
	
	public void addDock() {
		
		if(sumThings()+1<=maxThings) {
			
			dockNum++;
				
		}
		
	}
	
	public void removeTown() {
		
		if(townNum-1>=0) {
			
			townNum--;
				
		}
		
	}
	
	public void removeFarm() {
		
		if(farmNum-1>=0) {
			
			farmNum--;
				
		}
		
	}
	
	public void removeDock() {
		
		if(dockNum-1>=0) {
			
			dockNum--;
				
		}
		
	}
	
	public int sumThings() {
			
		return townNum + farmNum + dockNum;
		
	}
	
	public void removeSheep(int i) {
		
		if(sheepNum-i>=0) {
			
			sheepNum-=i;
			
		} else {
			
			sheepNum = 0;
			
		}
		
	}
	
	public void removeVikings(int i) {
		
		if(vikingsNum-i>=0) {
			
			vikingsNum-=i;
			
		} else {
			
			vikingsNum = 0;
			
		}
		
	}
	
}
