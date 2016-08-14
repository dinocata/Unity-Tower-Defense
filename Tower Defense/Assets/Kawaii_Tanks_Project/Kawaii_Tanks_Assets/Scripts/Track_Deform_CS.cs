using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This script must be attached to Tracks.

public class Track_Deform_CS : MonoBehaviour {
	
	public int anchorNum ;
	public Transform [] anchorArray ;
	public float [] widthArray ;
	public float [] heightArray ;

	Mesh thisMesh ;
	float [] initialPosArray ;
	Vector3 [] initialVertices ;
	List < List < int > > moveVerticesList = new List < List < int > > () ;

	void Start () {
		thisMesh = GetComponent < MeshFilter > ().mesh ;
		initialPosArray = new float [ anchorArray.Length ] ;
		initialVertices = thisMesh.vertices ;
		// Find vertices in the range.
		for ( int i = 0 ; i < anchorArray.Length ; i ++ ) {
			if ( anchorArray [ i ] != null ) {
				initialPosArray [ i ] = anchorArray [ i ].localPosition.x ;
				Vector3 anchorPos = transform.InverseTransformPoint ( anchorArray [ i ].position ) ;
				List < int > withinVerticesList = new List < int > () ;
				for ( int j = 0 ; j < thisMesh.vertices.Length ; j ++ ) {
					float distZ = Mathf.Abs ( anchorPos.z - thisMesh.vertices [ j ].z ) ;
					float distY = Mathf.Abs ( anchorPos.y - thisMesh.vertices [ j ].y ) ;
					if ( distZ <= widthArray [ i ] * 0.5f  && distY <= heightArray [ i ] * 0.5f ) {
						withinVerticesList.Add ( j ) ;
					}
				}
				moveVerticesList.Add ( withinVerticesList ) ;
			} else {
				Debug.LogError ( "Anchor Wheel is not assigned in " + this.name ) ;
				Destroy ( this );
			}
		}
	}
	
	void Update () {
		Vector3 [] tempVertices = new Vector3 [ initialVertices.Length ] ;
		initialVertices.CopyTo ( tempVertices , 0 ) ;
		for ( int i = 0 ; i < anchorArray.Length ; i ++ ) {
			float tempDist = anchorArray [ i ].localPosition.x - initialPosArray [ i ] ;
			for ( int j = 0 ; j < moveVerticesList [ i ].Count ; j ++ ) {
				tempVertices [ moveVerticesList [ i ] [ j ] ].y += tempDist ;
			}
		}
		thisMesh.vertices = tempVertices ;
	}

	void OnDrawGizmos () {
		if ( anchorArray.Length != 0 ) {
			Gizmos.color = Color.green ;
			for ( int i = 0 ; i < anchorArray.Length ; i++ ) {
				if ( anchorArray [ i ] != null ) {
					Vector3 tempSize = new Vector3 ( 0.0f , heightArray [ i ] , widthArray [ i ] ) ;
					Vector3 tempCenter = anchorArray [ i ].position ;
					Gizmos.DrawWireCube ( tempCenter , tempSize ) ;
				}
			}
		}
	}

}