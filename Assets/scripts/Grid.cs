using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
	
	public float size = 1;
	public int subdivisions = 2;
	private float cellSize;
	public float marginPercent = 0.1f;
	
	private List<Vector3> vertices = new List<Vector3>();
	private List<int> indices = new List<int>();
	
	public Material material;
	private GameObject [,,] cubes;
	private Color [,,] colors;
	private Color baseColor;
	private Color intersectionColor;
	
	// Use this for initialization
	void Start () {
		// I need to call rebuild - otherwise 
		Rebuild();
		colors = new Color[subdivisions,subdivisions,subdivisions];
		baseColor = cubes[0,0,0].renderer.material.color;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		for (int x=0;x<subdivisions;x++) for (int y=0;y<subdivisions;y++) for (int z=0;z<subdivisions;z++){
			// color = cubes[x,y,z].renderer.material.color;
			MeshRenderer mRenderer= cubes[x,y,z].GetComponent<MeshRenderer>();
			mRenderer.material.color =  Color.Lerp(mRenderer.material.color,colors[x,y,z], Time.deltaTime);
			colors[x,y,z] = baseColor;
		}
	}
	
	private void addQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4){
		// add first triangle
		int p1Index = vertices.Count;
		indices.Add(vertices.Count);
		vertices.Add(p1);

		indices.Add(vertices.Count);
		vertices.Add(p2);
		int p3Index = vertices.Count;
		indices.Add(vertices.Count);
		vertices.Add(p3);

		// Add second triangle
		indices.Add(p1Index); // reuse vertex from triangle above
		indices.Add(p3Index); // reuse vertex from triangle above
		indices.Add(vertices.Count);
		vertices.Add(p4);
	}
	
	private void addCube(Vector3 min, Vector3 max){
		if (min.x >=max.x){
			Debug.LogError("Too large margin");
			return;
		}
		float cellSize = max.x-min.x;
		addQuad(min, min+Vector3.right*cellSize, min+(Vector3.right+Vector3.up)*cellSize, min+Vector3.up*cellSize); // front facing
		addQuad(min, min+Vector3.forward*cellSize, min+(Vector3.forward+Vector3.right)*cellSize, min+Vector3.right*cellSize); // bottom
		addQuad(min, min+Vector3.up*cellSize, min+(Vector3.forward+Vector3.up)*cellSize, min+Vector3.forward*cellSize); // left
		addQuad(max, max-Vector3.forward*cellSize, max-(Vector3.forward+Vector3.up)*cellSize, max-Vector3.up*cellSize); // right
		addQuad(max, max-Vector3.up*cellSize, max-(Vector3.up+Vector3.right)*cellSize, max-Vector3.right*cellSize); // back
		addQuad(max, max-Vector3.right*cellSize, max-(Vector3.right+Vector3.forward)*cellSize, max-Vector3.forward*cellSize); // top		
	}
	
	public GameObject createCube(int x, int y, int z){
		GameObject gameObject = new GameObject(x+"_"+y+"_"+z);
		
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
		gameObject.renderer.material = material;
		
		// create cube
		Vector3 min = new Vector3(x*cellSize,y*cellSize,z*cellSize);
		Vector3 max = new Vector3((x+1)*cellSize, (y+1)*cellSize, (z+1)*cellSize);
		addCube(min+Vector3.one*cellSize*marginPercent, max-Vector3.one*cellSize*marginPercent);
		
		Vector3 []verticesV = new Vector3[vertices.Count];
		int[] indicesV = new int[indices.Count];

		for (int i=0;i<vertices.Count;i++){
			verticesV[i] = vertices[i];
		}
		for (int i=0;i<indices.Count;i++){
			indicesV[i] = indices[i];
		}
		vertices.Clear();
		indices.Clear();
		mesh.vertices = verticesV;
		mesh.triangles = indicesV;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();

		meshFilter.mesh = mesh;
		 
		// add as child
		gameObject.transform.parent = transform;
		return gameObject;
	}
	
	public void Rebuild(){
		for (int i=transform.childCount-1;i>=0;i--){
			GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
		}
		cellSize = size/subdivisions;
		cubes = new GameObject[subdivisions,subdivisions,subdivisions];
		for (int x=0;x<subdivisions;x++) for (int y=0;y<subdivisions;y++) for (int z=0;z<subdivisions;z++){
			cubes[x,y,z] = createCube(x,y,z);
		}
	}
	
	public Vector3i toGridSpace(Vector3 pos){
		return new Vector3i(
			(int)(pos.x/cellSize+(pos.x<0?-1:0)),
			(int)(pos.y/cellSize+(pos.y<0?-1:0)),
			(int)(pos.z/cellSize+(pos.z<0?-1:0))
			);
	}
	
	public Vector3 toWorldSpace(Vector3i pos){
		return new Vector3(
				pos.x*cellSize+(pos.x<0?1:0),
				pos.y*cellSize+(pos.y<0?1:0),
				pos.z*cellSize+(pos.z<0?1:0)
			);
	}
	
	private void CellHit(Vector3i position){
		colors[position.x,position.y,position.z] += intersectionColor;
	}
	
	private void gridTraversal(Vector3 origin, Vector3 direction){
		if (direction.sqrMagnitude==0){// no magnitude
			return;
		}
		Vector3i position = toGridSpace(origin);
		
		Vector3 tDelta = new Vector3(
			Mathf.Abs(direction.x)*cellSize,
			Mathf.Abs(direction.y)*cellSize,
			Mathf.Abs(direction.z)*cellSize
			);
		for (int i=0;i<3;i++){
			if (tDelta[i]!= 0)
				tDelta[i] = 1/tDelta[i];
		}
	
		Vector3i step = new Vector3i(
			(int) direction[0]>0?1:-1,
			(int) direction[1]>0?1:-1,
			(int) direction[2]>0?1:-1
			);
	
		Vector3 tMax = toWorldSpace(position);
		Vector3i outPlane = new Vector3i(0,0,0);
	
		for (int i=0;i<3;i++){
			if (direction[i] < 0) {
				tMax[i] = (tMax[i]-origin[i]) / direction[i];
				outPlane[i] = -1;
				// if position is already out of bounds return
				if (position[i] <= outPlane[i]) return; 
			} else {
				tMax[i] = (tMax[i]+1-origin[i]) / direction[i];
				outPlane[i] = subdivisions;
				// if position is already out of bounds return
				if (position[i] >= outPlane[i]) return;
			}
		}
		Vector4 v = new Vector4();
		//while (true){
		for (int i=0;i<200;i++){
			// examine cell
			if (position.x >= 0 && position.x < subdivisions &&
				position.y >= 0 && position.y < subdivisions &&
				position.z >= 0 && position.z < subdivisions ){
				CellHit(position);
			}
	
			// goto next cell
			if (tMax[0] < tMax[1])
			{
				if (tMax[0] < tMax[2])
				{
					v[0] = v[0]+1;
					position[0] += step[0];
					if (position[0] == outPlane[0]) return;
					tMax[0] += tDelta[0];
				}
				else
				{
					v[1] = v[1]+1;
					position[2] += step[2];
					if (position[2] == outPlane[2]) return;
					tMax[2] += tDelta[2];
				}
			} else {
				if (tMax[1] < tMax[2])
				{
					v[2] = v[2]+1;
					position[1] += step[1];
					if (position[1] == outPlane[1]) return;
					tMax[1] += tDelta[1];
				}
				else
				{
					v[3] = v[3]+1;
					position[2] += step[2];
					if (position[2] == outPlane[2]) return;
					tMax[2] += tDelta[2];
				}
			}
		}
		Debug.LogError("Problem using "+ origin+ " and "+direction+" position "+position.x+","+position.y+","+position.z+" out "+outPlane.x+","+outPlane.y+","+outPlane.z+" stat "+v);
	}
	
	public void doIntersection(Vector3 origin, Vector3 direction, Color color){
		intersectionColor = color;
		gridTraversal(origin, direction);
	}
}
