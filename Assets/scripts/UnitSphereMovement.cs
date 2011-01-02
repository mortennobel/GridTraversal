using UnityEngine;

/**
 * This creates a point on a sphere and moves the point around in a random direction
 */
public class UnitSphereMovement {
	private Vector3 position;
	private Vector3 rotationAxis;
	
	public UnitSphereMovement(){
		position = Random.onUnitSphere;
		rotationAxis = Random.onUnitSphere;
	}
	// Update is called once per frame
	public Vector3 Update (float rotationSpeed, float radius) {
		position = Quaternion.AngleAxis(rotationSpeed*Time.deltaTime,rotationAxis)*position;
		return position*radius;
	}
	
	
}
