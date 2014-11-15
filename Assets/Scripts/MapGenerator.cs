using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class Map {
	private int[] tileValues;
	private int height;
	private int width;

	public Map(string filePath) {
		string[] lines = File.ReadAllLines (filePath);
		List<int> mapValues = new List<int> ();

		height = lines.Length;
		foreach (string line in lines) {
			string[] tiles = line.Split(' ');
			if(tiles.Length > 0) {
				width = tiles.Length;
				
				foreach (string tile in tiles) {
					int value;
					if(int.TryParse(tile, out value)) {
						mapValues.Add(value);
					} else {
						Debug.LogWarning ("invalid value: " + tile);						
					}
				}
			}
		}
		tileValues = mapValues.ToArray ();
	}

	public void Write(string filePath) {
		StreamWriter outFile = new StreamWriter (filePath);
		for (int i = 0; i < height; i++) {
			for(int j = 0; j < width; j++) {
				outFile.Write (tileValues[i * width + j].ToString());
				outFile.Write(' ');				
			}

			outFile.Write('\n');
		}
	}

	public int Height {
		get {
			return height;
		}
	}

	public int Width {
		get {
			return width;
		}
	}

	public int[] TileValues {
		get {
			return tileValues;
		}
	}
};

public class MapGenerator : MonoBehaviour {
	public Material material;
	public int matImageWidth = 1;
	public int matImageHeight = 1;
	private Map map;

	void Start() {
		map = new Map (@"Assets/maps/map.txt");
		generateMap (map);
	}

	public void generateMap(Map map) {
		for (int x = 0; x < map.Width; x++) {
			for (int y = 0; y < map.Height; y++) {
				generateTile (map.TileValues[y * map.Width + x], x, map.Height - y - 1);
			}
		}
	}

	private GameObject generateTile(int tileNum, int x, int y) {
		GameObject newTile = new GameObject("tile", typeof(MeshRenderer), typeof(MeshFilter));
		newTile.transform.position = new Vector3(x, 0, y);
		newTile.transform.parent = transform;

		Vector3[] vertices = {
			new Vector3(0,0,0),
			new Vector3(1,0,0),
			new Vector3(1,0,1),
			new Vector3(0,0,1)
		};

		int[] triangles = {
			2, 0, 3,
			1, 0, 2
		};
		
		float xMin = (tileNum % matImageHeight) / (float)matImageWidth;
		float xMax = xMin + (1.0f / matImageWidth);
		float yMax = 1.0f - (tileNum / matImageWidth) / (float)matImageHeight;
		float yMin = yMax - (1.0f / matImageHeight);

		Vector2[] uvCords = {
			new Vector2(xMin,yMin),
			new Vector2(xMax,yMin),
			new Vector2(xMax,yMax),
			new Vector2(xMin,yMax)
		};
		
		MeshRenderer meshRenderer = newTile.GetComponent<MeshRenderer> ();
		MeshFilter meshFilter = newTile.GetComponent<MeshFilter> ();

		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvCords;

		meshFilter.mesh = mesh;
		meshRenderer.material = material;
		meshRenderer.receiveShadows = false;
		meshRenderer.castShadows = false;

		return newTile;
	}
}
