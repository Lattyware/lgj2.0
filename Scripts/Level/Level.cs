using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {

	private static readonly int ISLANDS = 10;
	
	public GameObject islandPrefab;
	public GameObject seaPrefab;
	
	public void Start() {
		List<GameObject> islands = new List<GameObject>();
		GameObject sea = (GameObject) Instantiate(seaPrefab, new Vector3(0, 0, 0), Quaternion.identity); 
		
		Vector3 overlap = islandPrefab.renderer.bounds.size;
		
		Vector3 centre = sea.renderer.bounds.center;
		Vector3 size = sea.renderer.bounds.size;
		
		SortedAreaStack areas = new SortedAreaStack(new Bounds(centre, size-overlap));
		areas.repeatedSplit(Level.ISLANDS);
		
		for (int i=0; i<Level.ISLANDS; i++) {
			GameObject island = (GameObject) Instantiate(islandPrefab, areas.Pop().center, Quaternion.identity);
			islands.Add(island);
		}
		
	}
	
	private class SortedAreaStack {
		
		private List<Bounds> stack;
		
		public SortedAreaStack(Bounds initial) {
			this.stack = new List<Bounds>();
			this.stack.Add(initial);
		}
		
		public void repeatedSplit(int times) {
			for (int i=0; i<times; i++) {
				Pair split = this.split(this.Pop(), 0.3f);
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
			error *= Random.Range(-1, 1);
			if (hw > hh) {
				error *= hw/2;
				return new Pair(
					new Bounds(new Vector3(centre.x-(hw/2)+error, centre.y, centre.z), new Vector3(hw+error, hd*2, hh*2)),
					new Bounds(new Vector3(centre.x+(hw/2)-error, centre.y, centre.z), new Vector3(hw-error, hd*2, hh*2))
				);
			} else {
				error *= hh/2;
				return new Pair(
					new Bounds(new Vector3(centre.x, centre.y, centre.z-(hw/2)+error), new Vector3(hw*2, hd*2, hh+error)),
					new Bounds(new Vector3(centre.x, centre.y, centre.z+(hw/2)-error), new Vector3(hw*2, hd*2, hh-error))
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