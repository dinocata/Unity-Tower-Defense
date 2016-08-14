using UnityEngine;
using System.Collections;

// This script must be attached to "Barrel_Base".

public class Barrel_Control_CS : MonoBehaviour {

	[ Header ( "Recoil Brake settings" ) ]
	[ Tooltip ( "Time it takes to push back the barrel. (Sec)" ) ] public float recoilTime = 0.2f ;
	[ Tooltip ( "Time it takes to to return the barrel. (Sec)" ) ] public float returnTime = 1.0f ;
	[ Tooltip ( "Movable length for the recoil brake. (Meter)" ) ] public float Length = 0.3f ;

	Transform thisTransform ;

	bool isReady = true ;
	Vector3 initialPos ;
	const float HALF_PI = Mathf.PI * 0.5f ;

	void Start () {
		thisTransform = this.transform ;
		initialPos = thisTransform.localPosition ;
	}
	
	IEnumerator Recoil_Brake () {
		// Move backward.
		float Temp_Time = 0.0f ;
		while ( Temp_Time < recoilTime ) {
			float Rate = Mathf.Sin ( HALF_PI * ( Temp_Time / recoilTime ) ) ;
			thisTransform.localPosition = new Vector3 ( initialPos.x , initialPos.y , initialPos.z - ( Rate * Length ) ) ;
			Temp_Time += Time.deltaTime ;
			yield return null ;
		}
		// Return to the initial position.
		Temp_Time = 0.0f ;
		while ( Temp_Time < returnTime ) {
			float Rate = Mathf.Sin ( HALF_PI * ( Temp_Time / returnTime ) + HALF_PI ) ;
			thisTransform.localPosition = new Vector3 ( initialPos.x , initialPos.y , initialPos.z - ( Rate * Length ) ) ;
			Temp_Time += Time.deltaTime ;
			yield return null ;
		}
		//
		isReady = true ;
	}
	
	void Fire () {
		if ( isReady ) {
			isReady = false ;
			StartCoroutine ( "Recoil_Brake" ) ;
		}
	}
}
