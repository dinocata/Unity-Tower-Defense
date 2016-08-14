using UnityEngine;
using System.Collections;

// This script must be attached to all the Apparent Wheels.

public class Wheel_Sync_CS : MonoBehaviour {

	[ Header ( "Wheel Synchronizing settings" ) ]
	[ Tooltip ( "Reference wheel." ) ] public Transform referenceWheel ;
	[ Tooltip ( "Offset value for the size of Reference Wheel." ) ] public float referenceRadiusOffset = 0.0f ;
	[ Tooltip ( "Offset value for the size of this wheel." ) ] public float thisRadiusOffset = 0.0f ;

	Transform thisTransform ;
	float previousAng ;
	float radiusRate ;

	void Start () {
		thisTransform = transform ;
		if ( referenceWheel == null ) {
			Debug.LogError ( "Reference Wheel is not assigned in " + this.name ) ;
			Destroy ( this ) ;
			return ;
		}
		// Calculate radiusRate.
		if ( referenceWheel.GetComponent < MeshFilter > () ) {
			float thisRadius = GetComponent < MeshFilter > ().mesh.bounds.extents.z + thisRadiusOffset ;
			float referenceRadius = referenceWheel.GetComponent < MeshFilter > ().mesh.bounds.extents.z + referenceRadiusOffset ;
			if ( referenceRadius > 0 && thisRadius > 0 ) {
				radiusRate = referenceRadius / thisRadius ;
			}
		} else {
			Debug.LogError ( "Reference Wheel of " + this.name + " has no MeshFilter." ) ;
			Destroy ( this ) ;
		}
	}
	
	void Update () {
		if ( referenceWheel ) {
			float currentAng = referenceWheel.localEulerAngles.y ;
			float deltaAng = Mathf.DeltaAngle ( currentAng , previousAng ) ;
			thisTransform.localEulerAngles = new Vector3 ( thisTransform.localEulerAngles.x , thisTransform.localEulerAngles.y - ( radiusRate * deltaAng ) , thisTransform.localEulerAngles.z ) ;
			previousAng = currentAng ;
		}
	}
}
