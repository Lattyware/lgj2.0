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
	
	public GameObject dock;
	public GameObject town;
	public GameObject sheepFarm;
	public Vector3[] dockPos = new Vector3[5];
	public Vector3[] dockRot = new Vector3[5]; 
	List<GameObject> docks = new List<GameObject>();
	List<GameObject> towns = new List<GameObject>();
	List<GameObject> sheepFarms = new List<GameObject>();
	public Vector3[] farmGrid = new Vector3[5];
	public Vector3[] townGrid = new Vector3[5];
	
	int currentPos = 0;
	
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
		farmNum = 0;
		dockNum = 0;
		
		maxThings = 5;
		
		Texture2D texture = new IslandGen(1024, 256, 1.5f).texture;
		this.renderer.material.mainTexture = texture;
		texture.Apply();
	
		
		dockPos[0] = (new Vector3(this.transform.localScale.x * 4,0,0));
		dockPos[1] = (new Vector3(-this.transform.localScale.x * 5,0,0));
		dockPos[2] = (new Vector3(0,0,-this.transform.localScale.x  * 4));
		dockPos[3] = (new Vector3(0,0,this.transform.localScale.x  * 4));
		dockPos[4] = (new Vector3(this.transform.localScale.x * 3,0,this.transform.localScale.x  * 3));
		dockRot[0] = new Vector3(0,0,0);
		dockRot[1] = new Vector3(0,180,0);
		dockRot[2] = new Vector3(0,90,0);
		dockRot[3] = new Vector3(0,-90,0);
		dockRot[4] = new Vector3(0,-45,0);
		farmGrid[0] = new Vector3(-10,0,-4);
		farmGrid[1] = new Vector3(-10,0,-2);
		farmGrid[2] = new Vector3(-10,0,0);
		farmGrid[3] = new Vector3(-10,0,2);
		farmGrid[4] = new Vector3(-10,0,4);
		townGrid[0] = new Vector3(-12,0,-4);
		townGrid[1] = new Vector3(-12,0,-2);
		townGrid[2] = new Vector3(-12,0,0);
		townGrid[3] = new Vector3(-12,0,2);
		townGrid[4] = new Vector3(-12,0,4);
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
			GameObject tempObject = (GameObject)Instantiate(town,this.collider.bounds.center,new Quaternion(0, 0, 0, 0));
			tempObject.transform.Translate(townGrid[townNum - 1]);
			towns.Add(tempObject);
				
		}
		
	}
	
	public void addFarm() {
		
		if(sumThings()+1<=maxThings) {
			farmNum++;
			GameObject tempObject = (GameObject)Instantiate(sheepFarm,this.collider.bounds.center,new Quaternion(0, 0, 0, 0));
			tempObject.transform.Translate(farmGrid[(farmNum - 1)]);
			sheepFarms.Add(tempObject);
		}		
	}
	
	public void addDock() {
		
		if(sumThings()+1<=maxThings) {
			
			dockNum++;
			GameObject tempObject = (GameObject)Instantiate(dock,this.collider.bounds.center,new Quaternion(0, 0, 0, 0));
			tempObject.transform.Translate(dockPos[dockNum - 1]);
			tempObject.transform.Rotate(dockRot[dockNum - 1]);
			docks.Add(tempObject);
		}
		
	}
	
	public void removeTown() {
		
		if(sumThings()-1>=0) {
			DestroyObject(towns.ToArray()[townNum - 1]);
			towns.RemoveAt(townNum - 1);
			townNum--;
				
		}
		
	}
	
	public void removeFarm() {
		
		if(sumThings()-1>=0) {
			DestroyObject(sheepFarms.ToArray()[farmNum - 1]);
			sheepFarms.RemoveAt(farmNum - 1);
			farmNum--;
				
		}
		
	}
	
	public void removeDock() {
		
		if(sumThings()-1>=0) {
			
			DestroyObject(docks.ToArray()[dockNum - 1]);
			docks.RemoveAt(dockNum - 1);
			
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
