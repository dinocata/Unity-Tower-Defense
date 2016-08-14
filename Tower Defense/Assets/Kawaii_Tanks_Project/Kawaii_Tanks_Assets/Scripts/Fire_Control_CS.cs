using UnityEngine;
using System.Collections;

using UnityStandardAssets.CrossPlatformInput;

// This script must be attached to "Cannon_Base".

public class Fire_Control_CS : MonoBehaviour {

	[ Header ( "Fire control settings" ) ]
	[ Tooltip ( "Loading time. (Sec)" ) ] public float reloadTime = 4.0f ;
	[ Tooltip ( "Recoil force with firing." ) ] public float recoilForce = 5000.0f ;

	bool isReady = true ;

	Transform thisTransform ;
	Rigidbody parentRigidbody ;

	int myID ;
	bool isCurrentID = true ;

	void Start () {
		thisTransform = this.transform ;
		parentRigidbody = GetComponentInParent < Rigidbody > () ;
		if ( parentRigidbody == null ) {
			Debug.LogError ( "Rigidbody is not found in MainBody." ) ;
			Destroy ( this ) ;
		}
	}
	
	void Update () {
		if ( isCurrentID ) {
			#if UNITY_ANDROID || UNITY_IPHONE
			if ( CrossPlatformInputManager.GetButtonUp ( "GunCam_Press" ) && isReady ) {
			#else
			if ( Input.GetKey ( KeyCode.Space ) && isReady ) {
			#endif
			// Send message to this and 'Barrel_Control' and 'Fire_Spawn'.
				BroadcastMessage ( "Fire" , SendMessageOptions.DontRequireReceiver ) ;
			}
		}
	}

	void Fire () {
		parentRigidbody.AddForceAtPosition ( -thisTransform.forward * recoilForce , thisTransform.position , ForceMode.Impulse ) ;
		isReady = false ;
		StartCoroutine ( "Reload" ) ;
	}
	
	IEnumerator Reload () {
		yield return new WaitForSeconds ( reloadTime ) ;
		isReady = true ;
	}

	void Get_ID ( int idNum ) {
		myID = idNum ;
	}
	
	void Get_Current_ID ( int idNum ) {
		if ( idNum == myID ) {
			isCurrentID = true ;
		} else {
			isCurrentID = false ;
		}
	}
}
