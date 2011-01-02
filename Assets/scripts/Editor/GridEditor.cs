using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor (typeof (Grid))] 
public class GridEditor : Editor {

	public override void OnInspectorGUI ()
	{
		Grid obj;

		obj = target as Grid;

		if (obj == null) {
			return;
		}
	
		base.DrawDefaultInspector();
		EditorGUILayout.BeginHorizontal ();
		
		// Rebuild mesh when user click the Rebuild button
		if (GUILayout.Button("Rebuild")){
			obj.Rebuild();
		}
		EditorGUILayout.EndHorizontal ();
	}
}
