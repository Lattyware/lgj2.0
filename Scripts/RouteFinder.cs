using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RouteFinder{
	
	public static readonly RouteFinder worker = new RouteFinder();
	
	private List<Vector3> allPoints = new List<Vector3>();
	private List<LoadedPath> allLoadedPaths = null;
	
	
	
	private RouteFinder(){
		Debug.Log("Entered Constructor");
	}
	
	public string aMethod(){
		return "Entered singleton correctly";
	}
	
	public static RouteFinder getInstance(){
		return worker;
	}
	
	public void drawLines(){
		List<Vector3> allPts = getRoute(allPoints[0], allPoints[allPoints.Count - 6]);
		if((allPts != null)&&(allPts.Count >= 2)){
			Vector3 pt1 = allPts[0];
			for(int i = 1; i < allPts.Count; i+= 1){
				Vector3 pt2 = allPts[i];
				Debug.DrawLine(pt1, pt2);
				pt1 = pt2;
			}
		}else{
			Debug.Log("Could not find route!" + allPts);
			Debug.DrawLine(allPoints[0], allPoints[allPoints.Count - 6], Color.red);
		}
	}
	
	private bool getIntersected(Vector3 pt1, Vector3 pt2){
		RaycastHit hit;
		bool hitsObject = Physics.Raycast(pt1, pt2 - pt1, out hit, Mathf.Infinity);
		return hitsObject;
	}
	
	public List<Vector3> getRoute(Vector3 startPoint, Vector3 endPoint){
		
		if(allLoadedPaths == null){
			loadRoutes();
		}
		
		foreach(LoadedPath lPath in allLoadedPaths){

			if((Vector3.Equals(lPath.getStartPoint(), startPoint))&&(Vector3.Equals(lPath.getEndPoint(), endPoint))){
				return lPath.getPath();
			}
		}
		
		return null;
		
	}
	
	public void loadRoutes(){
		
		allLoadedPaths = new List<LoadedPath>();
		
		for(int i = 0; i < allPoints.Count; i++){
			for(int i2 = 0; i2 < allPoints.Count; i2++){
				if(i != i2){
					Vector3 pt1 = allPoints[i];
					Vector3 pt2 = allPoints[i2];
						
					// set all distances to infinity initially
					DistanceContainer dc1 = null;
					DistanceContainer dc2 = null;
					DistanceContainer[] distances = new DistanceContainer[allPoints.Count];
					for(int i3 = 0; i3 < allPoints.Count; i3++){
						distances[i3] = new DistanceContainer(Mathf.Infinity, null, allPoints[i3]);
						if(i3 == i){
							dc1 = distances[i3];
						}
						if(i3 == i2){
							dc2 = distances[i3];
						}
					}
				
					distances[i].distance = 0;
					distances[i].prev = distances[i];
						
					List<int> removedIndices = new List<int>();
						
					DistanceContainer u = dc1;
					int breakCount = 0;
					while((removedIndices.Count < allPoints.Count)&&(breakCount < 100)){
						breakCount ++;
						
						//find the vertex with the smallest distance
						int index = 0;
						float currentShortestDist = Mathf.Infinity;
						for(int i3 = 0; i3 < allPoints.Count; i3++){
							if(removedIndices.IndexOf(i3) == -1){
								if(distances[i3].distance < currentShortestDist){
									currentShortestDist = distances[i3].distance;
									u = distances[i3];
									index = i3;
								}
							}
						}
						
						if(currentShortestDist == Mathf.Infinity){
							break;
						}
						
						removedIndices.Add(index);
						
						if(u.pt != pt2){
							//for each neighbour of u, calculate new distance
							for(int i3 = 0; i3 < allPoints.Count; i3++){
								Vector3 ptTemp = allPoints[i3];
								if(!getIntersected(u.pt, ptTemp)){
									float alt = currentShortestDist + Vector3.Distance(u.pt, ptTemp);
									if(alt < distances[i3].distance){
										distances[i3].distance = alt;
										distances[i3].prev = u;
									}
								}
							}
						}else{
							break;
						}
					}
					
					// iterate backwards through distance containers to get path
					List<Vector3> sequence = new List<Vector3>();
					if(u.pt == pt2){
						sequence.Add(u.pt);
						
						while((u.prev != u)&&(u.prev != null)){
							u = u.prev;
							sequence.Add(u.pt);
						}
						
						sequence.Reverse();
					}
					
						
					if(breakCount == 100){
						Debug.Log("Failed to calculate paths within enough iterations. Sequence has length: " + sequence.Count);
					}else{
						Debug.Log("Calculated path. Sequence has length: " + sequence.Count);
					}
						
					allLoadedPaths.Add(new LoadedPath(pt1, pt2, sequence));
						
				}
			}
		}
	}
	
	public void addRoutableObject(GameObject o){
		Bounds bounds = o.collider.bounds;
		Vector3 size = bounds.size;
		float sizeX = size.x /2;
		
		Vector3 center = o.transform.position;
		
		Vector3 forward = o.transform.forward;
		Vector3 right = o.transform.right;
		Vector3 left = right * (-1);
		Vector3 back = forward * (-1);
		
		Vector3 pt1 = (forward * sizeX) + (left * sizeX) + center;
		Vector3 pt2 = (right * sizeX) + (forward * sizeX) + center;
		Vector3 pt3 = (back * sizeX) + (right * sizeX) + center;
		Vector3 pt4 = (back * sizeX) + (left * sizeX) + center;

		allPoints.Add(pt1);
		allPoints.Add(pt2);
		allPoints.Add(pt3);
		allPoints.Add(pt4);
	
		Vector3 centerPosition = o.transform.position;
	}
	
	class DistanceContainer{
		public float distance;
		public DistanceContainer prev;
		public Vector3 pt;
		
		public DistanceContainer(float dist, DistanceContainer p, Vector3 point){
			distance = dist;
			prev = p;
			pt = point;
		}
	}
	
	class LoadedPath{
		Vector3 startPoint;
		Vector3 endPoint;
		List<Vector3> path;
		
		public LoadedPath(Vector3 sPt, Vector3 ePt, List<Vector3> _path){
			startPoint = sPt;
			endPoint = ePt;
			path = _path;
		}
				
		public Vector3 getStartPoint(){
			return startPoint;
		}
		
		public Vector3 getEndPoint(){
			return endPoint;
		}
		
		public List<Vector3> getPath(){
			return path;
		}
	}
	
}
