using UnityEngine;
using System.Collections;

public class IslandGen {
	
	public Texture2D texture;
	
	/**
	 * Generates a new random island.
	 * size: The (approximate) radius in pixels of the island.
	 * jaggedness: Increase the odds of more jagged islands.
	 **/
	public IslandGen(int size, int segment_size, float jaggedness) {
		Random.seed = (int) System.DateTime.Now.Ticks;
		
		int segments = size/segment_size;
		float[,] initial = new float[size/segment_size+1, size/segment_size+1];
		
		// Fill with random values.
		for (int x=0; x<=segments; x++) {
			for (int y=0; y<=segments; y++) {
				if (x<1 | y<1 | x>segments-1 | y>segments-1) {
					initial[x, y] = 0;
				} else {
					initial[x, y] = Random.value*2;
				}
			}
		}
		
		// Midpoint displacement.
		float[,] old = initial;
		int old_size = segments;	
		float error = jaggedness;
		while(old_size < size) {
			int current_size = old_size*2;
			float[,] current = new float[current_size+2, current_size+2];
			int cx = 0;
			int cy = 0;
			for (int x=0; x<old_size; x++) {
				for (int y=0; y<old_size; y++) {
					int l=cx, c=cx+1, r=cx+2;
					int t=cy, m=cy+1, b=cy+2;
					
					float tl = old[x, y];
					float tr = old[x+1, y];
					float bl = old[x, y+1];
					float br = old[x+1, y+1];
					float mc = (tl+tr+bl+br)/4+Random.Range(-error, error);
					float mr = (tr+mc+br)/3+Random.Range(-error, error);
					float ml = (tl+mc+bl)/3+Random.Range(-error, error);
					float bc = (bl+mc+br)/3+Random.Range(-error, error);
					float tc = (tl+mc+tr)/3+Random.Range(-error, error);
							
					current[l, t] = tl;
					current[r, t] = tr;
					current[l, b] = bl;
					current[r, b] = br;
					current[r, m] = mr;
					current[l, m] = ml;
					current[c, b] = bc;
					current[c, t] = tc;
					current[c, m] = mc;
					
					cy += 2;
				}
				cy = 0;
				cx += 2;
			}
			error *= Mathf.Pow (2, -jaggedness);
			old_size = current_size;
			old = current;
		}
		
		Color[] final = new Color[size*size];
		for (int x=0; x < size-1; x++) {
			for (int y=0; y < size-1; y++) {
				float val = old[x, y];
				if (val > 0.5f) {
					final[x*size+y] = Color.green/val*0.7f;
				} else if (val > 0.3f) {
					final[x*size+y] = Color.yellow;
				} else {
					final[x*size+y] = Color.clear;
				}
			}
		}
		this.texture = new Texture2D(size, size);
		this.texture.SetPixels(final);
	}
	
}
