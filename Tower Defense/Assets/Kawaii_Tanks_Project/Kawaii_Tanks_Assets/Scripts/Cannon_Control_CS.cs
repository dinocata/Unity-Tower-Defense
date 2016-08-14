using UnityEngine;
using System.Collections;

// This script must be attached to "Cannon_Base".

public class Cannon_Control_CS : MonoBehaviour {

	[ Header ( "Cannon movement settings" ) ]
	[ Tooltip ( "Rotation Speed. (Degree per Second)" ) ] public float rotationSpeed = 5.0f ;
	[ Tooltip ( "Angle range for slowing down. (Degree)" ) ] public float bufferAngle = 1.0f ;
	[ Tooltip ( "Maximum elevation angle. (Degree)" ) ] public float maxElev = 15.0f ;
	[ Tooltip ( "Maximum depression angle. (Degree)" ) ] public float maxDep = 10.0f ;

	Transform thisTransform ;
	Turret_Control_CS turretScript ;
	float currentAng ;

	void Start () {
		thisTransform = this.transform ;
		turretScript = thisTransform.GetComponentInParent < Turret_Control_CS > () ;
		if ( turretScript == null ) {
			Debug.LogError ( "Cannon_Base cannot find Turret_Control_CS." ) ;
			Destroy ( this ) ;
		}
		currentAng = thisTransform.localEulerAngles.x ;
	}

	void FixedUpdate () {
		// Calculate Angle.
		float targetAng = Mathf.Rad2Deg * ( Mathf.Asin ( ( turretScript.local_TargetPos.y - thisTransform.localPosition.y ) / Vector3.Distance ( thisTransform.localPosition , turretScript.local_TargetPos ) ) ) ;
		targetAng += Mathf.DeltaAngle ( 0.0f , thisTransform.localEulerAngles.x ) + turretScript.adjustAng.y ;
		// Calculate Speed Rate
		float speedRate = -Mathf.Lerp ( 0.0f , 1.0f , Mathf.Abs ( targetAng ) / ( rotationSpeed * Time.fixedDeltaTime + bufferAngle ) ) * Mathf.Sign ( targetAng ) ;
		// Rotate
		currentAng += rotationSpeed * speedRate * Time.fixedDeltaTime ;
		currentAng = Mathf.Clamp ( currentAng , -maxElev , maxDep ) ;
		thisTransform.localRotation = Quaternion.Euler ( new Vector3 ( currentAng , 0.0f , 0.0f ) ) ;
	}

}
