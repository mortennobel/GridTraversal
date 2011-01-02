using UnityEngine;

[System.Serializable]
public class Vector3i {
	public int x;
	public int y;
	public int z;
	 
	public Vector3i(){}
	
	public Vector3i(int x,int y,int z){
		this.x = x;
		this.y = y;
		this.z = z;
	}
	
	public Vector3i(Vector3 v){
		x = (int)v.x;
		y = (int)v.y;
		z = (int)v.z;
	}
	
	public static Vector3i operator +(Vector3i c1,Vector3i c2){
		return new Vector3i(c1.x+c2.x, c1.y+c2.y, c1.z+c2.z);
	}
	
	public static Vector3 operator +(Vector3i c1,Vector3 c2){
		return new Vector3(c1.x+c2.x, c1.y+c2.y, c1.z+c2.z);
	}
	
	public static Vector3 operator +(Vector3 c1,Vector3i c2){
		return new Vector3(c1.x+c2.x, c1.y+c2.y, c1.z+c2.z);
	}
	
	public static Vector3i operator -(Vector3i c1,Vector3i c2){
		return new Vector3i(c1.x-c2.x, c1.y-c2.y, c1.z-c2.z);
	}
	
	public static Vector3i operator *(Vector3i c1,int c2){
		return new Vector3i(c1.x*c2, c1.y*c2, c1.z*c2);
	}
	
	public static Vector3 operator *(Vector3i c1,float c2){
		return new Vector3(c1.x*c2, c1.y*c2, c1.z*c2);
	}
	
	public static Vector3i operator *(int c1,Vector3i c2){
		return new Vector3i(c1*c2.x, c1*c2.y, c1*c2.z);
	}
	
	public static Vector3 operator *(float c1,Vector3i c2){
		return new Vector3(c1*c2.x, c1*c2.y, c1*c2.z);
	}
	
	// allow callers to initialize
	public int this[int idx]
	{
		get { return idx==0?x:(idx==1?y:z); }
		set { 
			switch (idx){
			case 0:
				x = value;
				break;
			case 1:
				y = value;
				break;
			default:
				z = value;
				break;
			}
		}
	}
	
	public Vector3 toVector3(){
		return new Vector3(x,y,z);
	}
}
