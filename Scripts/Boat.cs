using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour {
	
	public int numVikings;
	public int numSheep;
	public float speed = 1;
	GameObject sourceIsland;
	GameObject targettedIsland;
	
	
	
	public int maxCap = 20;
	
	public float rotAngle;
	
	// Use this for initialization
	void Start () {
		// calculate the angle rotated, each update around an island
	}
	
	public void InitializeVals(int nVikings, int nSheep, Island source, GameObject island){
		numVikings = nVikings;
		numSheep = nSheep;
		sourceIsland = source.gameObject;
		targettedIsland = island;
		rotAngle = this.transform.rotation.y;
		//this.transform.Rotate(new Vector3(0, 90, 0));
	}
	
	// Update is called once per frame
	void Update () {
		
		if(targettedIsland != null){
			
			
				
				// center at traversing island's transform position
				//Vector3 translation = traversingIsland.transform.position - this.transform.position;
				//this.transform.Translate(translation);
				//this.transform.Rotate(new Vector3(0, Time.deltaTime*rotIncrement, 0));
				//this.transform.Translate(Vector3.forward * Island.ISLAND_DISTANCE);
				

				
				Quaternion rotation = Quaternion.LookRotation(targettedIsland.transform.position - this.transform.position, Vector3.up);
				float str = Mathf.Min(2 * Time.deltaTime, 5);
				this.transform.rotation = Quaternion.Lerp(transform.rotation, rotation, str);
				//this.transform.LookAt(traversingIsland.transform.position);
				//this.transform.Rotate(new Vector3(0, -90, 0));
				//this.transform.transform.rotation = Quaternion.Lerp(transform.rotation, rotation, str);
				this.transform.Translate(Vector3.forward * (10 * Time.deltaTime));
				
				//this.transform.Translate(this.transform.right * speed );
				//this.transform.Translate(this.transform.right * Island.ISLAND_DISTANCE);
				
				//Vector3 forwardVector = this.transform.forward * speed;
				//float xComponent = forwardVector.x;
				
			
			//rotate to face targetted island
			
			//transform.LookAt(targettedIsland.transform.position);
			//transform.Rotate(new Vector3(0, 180, 0));
			
		}
		
	}
	
	public void loadVikings(int i) {
		
		if(numSheep==0) {
		
			if(numVikings+i<=maxCap) {
				
				numVikings+=i;
				
			}
			
		}
		
	}
	
	public void loadSheep(int i) {
		
		if(numVikings==0) {
		
			if(numSheep+i<=maxCap) {
				
				numSheep+=i;
				
			}
			
		}
		
	}
	
	public void unloadVikings(int i) {
		
		if(numVikings-i>=0) {
			
			numVikings-=i;
			
		} else {
			
			numVikings = 0;
			
		}
		
	}
	
	public void unloadSheep(int i) {
		
		if(numSheep-i>=0) {
			
			numSheep-=i;
			
		} else {
			
			numSheep = 0;
			
		}
		
	}
	
     void OnTriggerEnter(Collider col) {

		GameObject colObj = col.gameObject;
		
		if(this.gameObject.tag.Equals("PlayerBoat")){
		
			if(colObj.gameObject.tag.Equals("EnemyBoat")) {
							
				Boat boat = colObj.GetComponent<Boat>();
				
				if(!isWinner(boat)) {
					
					
					sourceIsland.GetComponent<Island>().launchedBoats.Remove(this.gameObject);
					DestroyObject(this.gameObject);
					
				} else {
					if(sourceIsland!=null) {				
						sourceIsland.GetComponent<Island>().launchedBoats.Remove(boat.gameObject);
					}
					DestroyObject(colObj.gameObject);
					
				}
				
			} else if(colObj.gameObject.tag.Equals("EnemyIsland")) {
							
				Island island = colObj.GetComponent<Island>();
				
				if(sackIsland(island)) {
					
					island.tag="PlayerIsland";
					this.sourceIsland.GetComponent<Island>().launchedBoats.Remove(this.gameObject);
					GameSystem.addIsland();
					GameObject old = targettedIsland.gameObject;
					targettedIsland = sourceIsland.gameObject;
					sourceIsland = old;
					this.sourceIsland.GetComponent<Island>().launchedBoats.Add(this.gameObject);
					
					
					
				} else {
					
					if(this.sourceIsland!=null) {
						
						this.sourceIsland.GetComponent<Island>().launchedBoats.Remove(this.gameObject);
						
					}
					
					DestroyObject(this.gameObject);
					
				}
				
			} else if(colObj.gameObject.tag.Equals("PlayerIsland") && colObj.gameObject!=sourceIsland) {
				
				Island island = colObj.GetComponent<Island>();
				
				island.vikingsNum+=numVikings;
				island.sheepNum+=numSheep;
				
				sourceIsland.GetComponent<Island>().launchedBoats.Remove(this.gameObject);
				DestroyObject(this.gameObject);
				
			}
			
		} else {
							
			if(colObj.gameObject.tag.Equals("PlayerBoat")) {
							
				Boat boat = colObj.GetComponent<Boat>();
				
				if(!isWinner(boat)) {
					
					
					sourceIsland.GetComponent<Island>().launchedBoats.Remove(this.gameObject);
					DestroyObject(this.gameObject);
					
				} else {
					if(sourceIsland!=null) {				
						sourceIsland.GetComponent<Island>().launchedBoats.Remove(boat.gameObject);
					}
					DestroyObject(colObj.gameObject);
					
				}
				
			} else if(colObj.gameObject.tag.Equals("PlayerIsland")) {
							
				Island island = colObj.GetComponent<Island>();
				
				if(sackIsland(island)) {
					
					island.tag="EnemyIsland";
					this.sourceIsland.GetComponent<Island>().launchedBoats.Remove(this.gameObject);
					GameSystem.addIsland();
					GameObject old = targettedIsland.gameObject;
					targettedIsland = sourceIsland.gameObject;
					sourceIsland = old;
					this.sourceIsland.GetComponent<Island>().launchedBoats.Add(this.gameObject);
					
					
					
				} else {
					
					if(this.sourceIsland!=null) {
						
						this.sourceIsland.GetComponent<Island>().launchedBoats.Remove(this.gameObject);
						
					}
					
					DestroyObject(this.gameObject);
					
				}
				
			} else if(colObj.gameObject.tag.Equals("EnemyIsland") && colObj.gameObject!=sourceIsland) {
				
				Island island = colObj.GetComponent<Island>();
				
				island.vikingsNum+=numVikings;
				island.sheepNum+=numSheep;
				
				sourceIsland.GetComponent<Island>().launchedBoats.Remove(this.gameObject);
				DestroyObject(this.gameObject);
				
			}
			
		}
		
	}
		
	
	
	//True if current object is winner
	public bool isWinner(Boat enemy) {
		
		if(numVikings<=enemy.numVikings) {
			
			numVikings=0;
			enemy.unloadVikings(numVikings);
			
			return false;
			
		} else {
			
			unloadVikings(enemy.numVikings);
			enemy.numVikings=0;
			
			return true;
			
		}
	}
	
	//True if island is sacked
	public bool sackIsland(Island island) {
		
		if(numVikings<=island.vikingsNum) {
			
			island.removeVikings(numVikings);
			numVikings=0;

			
			return false;
			
		} else {
			
			unloadVikings(island.vikingsNum);
			island.vikingsNum=0;
			
			return true;
			
		}
		
	}
	
	
}
