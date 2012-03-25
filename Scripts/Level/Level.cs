using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {

	private static readonly int ISLANDS = 8;
	
	public GameObject islandPrefab;
	public GameObject seaPrefab;
	
	public void Start() {
		Random.seed = (int) System.DateTime.Now.Ticks;
		
		List<GameObject> islands = new List<GameObject>();
		GameObject sea = (GameObject) Instantiate(seaPrefab, new Vector3(0, 0, 0), Quaternion.identity); 
		
		Vector3 overlap = islandPrefab.renderer.bounds.size;		
		Vector3 centre = sea.renderer.bounds.center;
		Vector3 size = sea.renderer.bounds.size;
		
		float error = 0.25f; //How varied the splits are, resulting in less gridy layouts. Higher -> Less on map. Lower -> Grid layout.
		float border = 1.2f; //How much gap to leave around the edge to avoid falling off. Higher -> Less on map. Lower -> Islands falling off map.
		
		SortedAreaStack areas = new SortedAreaStack(new Bounds(centre, size-overlap*(border+error)));
		areas.repeatedSplit(Level.ISLANDS, error); //Split to fit the given number of islands. Will fit more on, but may place islands on top of each other if too many requested.
		//areas.splitUntil(error, islandPrefab.renderer.bounds.extents); //Split repeatedly until no more islands will fit. Will give up earlier.
		
		foreach (Bounds area in areas) {
			GameObject island = (GameObject) Instantiate(islandPrefab, area.center, Quaternion.identity);
			islands.Add(island);
		}
	}
	
	private class SortedAreaStack : IEnumerable {
		
		private List<Bounds> stack;
		
		public SortedAreaStack(Bounds initial) {
			this.stack = new List<Bounds>();
			this.stack.Add(initial);
		}
		
		public IEnumerator GetEnumerator() {
			return this.stack.GetEnumerator();
		}
		
		public void splitUntil(float error, Vector3 min) {
			min *= (1+error);
			
			while (true) {
				Bounds current = this.Pop();
				Pair split = this.split(current, error);
				
				if (smallerThan(split.first, min) | smallerThan(split.second, min)) {
					this.Add(current);
					return;
				}
				
				this.stack.Add(split.first);
				this.stack.Add(split.second);
				this.stack.Sort(compareAreasByArea);
			}
		}
		
		private bool smallerThan(Bounds current, Vector3 min) {
			return current.extents.x < min.x | current.extents.z < min.z;
		}
		
		public void repeatedSplit(int times, float error) {
			for (int i=0; i<times; i++) {
				Pair split = this.split(this.Pop(), error);
				this.stack.Add(split.first);
				this.stack.Add(split.second);
				this.stack.Sort(compareAreasByArea);
			}		
		}
		
		public void Add(Bounds item) {
			this.stack.Add(item);
			this.stack.Sort(compareAreasByArea);
		}
		
		public Bounds Pop() {
			Bounds current = this.stack[0];
			this.stack.Remove(current);
			return current;
		}
		
		public void debug() {
			foreach (Bounds bound in this.stack) {
				Vector3 c = bound.center;
				Vector3 e = bound.extents;
				Debug.Log("X: "+c.x+" Y: "+c.z+" W: "+e.x+" H: "+e.z);
			}
		}
		
		private int compareAreasByArea(Bounds x, Bounds y) {
			float xSize = x.size.x*x.size.z;
			float ySize = y.size.x*y.size.z;
			if (xSize == ySize) {
				return 0;
			} else if (xSize < ySize) {
				return 1;
			} else {
				return -1;
			}
		}
			
		private Pair split(Bounds area, float error) {
			// TODO: Throw in some randomness.
			Vector3 centre = area.center;
			float hw = area.extents.x;
			float hh = area.extents.z;
			float hd = area.extents.y;
			float marginw = error*hw/2*Random.Range(-1, 1);
			float marginh = error*hh/2*Random.Range(-1, 1);
			if (hw > hh) {
				return new Pair(
					new Bounds(new Vector3(centre.x-(hw/2)+marginw, centre.y, centre.z-marginh), new Vector3(hw+marginw, hd*2, hh*2)),
					new Bounds(new Vector3(centre.x+(hw/2)-marginw, centre.y, centre.z+marginh), new Vector3(hw-marginw, hd*2, hh*2))
				);
			} else {
				return new Pair(
					new Bounds(new Vector3(centre.x-marginw, centre.y, centre.z-(hw/2)+marginh), new Vector3(hw*2, hd*2, hh+marginh)),
					new Bounds(new Vector3(centre.x+marginw, centre.y, centre.z+(hw/2)-marginh), new Vector3(hw*2, hd*2, hh-marginh))
				);
			}
		} 
		
		private class Pair {
			public Bounds first;
			public Bounds second;
			
			public Pair(Bounds first, Bounds second) {
				this.first = first;
				this.second = second;
			}
		}
		
	}
	
}