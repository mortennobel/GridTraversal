using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class Intersection : MonoBehaviour {
	public Grid grid;
	private UnitSphereMovement from;
	private UnitSphereMovement to;
	private LineRenderer lineRenderer;
	public Color color;
	public float rotationSpeed = 1;
	public float radius = 1;
	public Vector3 center = new Vector3(1,1,1);
	

	
	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
		from = new UnitSphereMovement();
		to = new UnitSphereMovement();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 f = center+from.Update(rotationSpeed,radius);
		Vector3 t = center+to.Update(rotationSpeed,radius);
		lineRenderer.SetPosition(0, t);
		lineRenderer.SetPosition(1, f);
		grid.doIntersection(f, t-f, color);
	}
	
	/*void OnGUI(){
		lineRenderer.SetPosition(0, to);
		lineRenderer.SetPosition(1, from);
		grid.doIntersection(from, to-from, color);
		gridSpaceFrom = grid.toGridSpace(from);
		gridSpaceTo = grid.toGridSpace(to);
	}*/
	
}
