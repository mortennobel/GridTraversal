using UnityEngine;
using System.Collections;

public class OrbitAround : MonoBehaviour {
	
	public Vector3 position;
	public float speed = 1;
	public float sinusSpeed = 1;
	public float elevationHeight = 60;
	public float radius = 2;
	
	private float polar;
	private float elevation;
	private Vector3 pos = new Vector3();
	
	public static void SphericalToCartesian(float radius, float polar, float elevation, out Vector3 outCart){
		float a = radius * Mathf.Cos(elevation);
        outCart.x = a * Mathf.Cos(polar);
		outCart.y =	radius * Mathf.Sin(elevation);
		outCart.z = a * Mathf.Sin(polar);
	}

	
	// Update is called once per frame
	void Update () {
		polar+= Time.deltaTime*speed;
		elevation += Time.deltaTime*sinusSpeed;
		SphericalToCartesian(radius, polar, Mathf.Sin(elevation)*elevationHeight, out pos);
		transform.position = position+pos;
		transform.LookAt(position);
	}
}
