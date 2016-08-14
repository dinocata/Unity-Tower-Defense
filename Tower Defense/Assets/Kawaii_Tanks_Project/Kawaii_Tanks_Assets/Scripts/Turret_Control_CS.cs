using UnityEngine;
using System.Collections;
using UnityEngine.UI ;

using UnityStandardAssets.CrossPlatformInput ;

// This script must be attached to "Turret_Base".

public class Turret_Control_CS : MonoBehaviour {

	[ Header ( "Turret movement settings" ) ]
	[ Tooltip ( "Maximum Rotation Speed. (Degree per Second)" ) ] public float rotationSpeed = 15.0f ;
	[ Tooltip ( "Time to reach the maximum speed. (Sec)" ) ] public float acceleration_Time = 0.2f ;
	[ Tooltip ( "Angle range for slowing down. (Degree)" ) ] public float bufferAngle = 5.0f ;
	[ Tooltip ( "Name of Image for marker." ) ] public string markerName = "Marker" ;

	Transform thisTransform ;
	Transform rootTransform ;

	float currentAng ;
	Vector3 targetPos ;
	Transform targetTransform ;
	Vector3 targetOffset ;
	[HideInInspector] public Vector3 local_TargetPos ; // Reference to "Cannon_Control_CS".
	[HideInInspector] public Vector3 adjustAng ; // Reference to "Cannon_Control_CS".
	Vector3 previousMousePos ;
	float speedRate ;
	Image markerImage ;

	#if UNITY_ANDROID || UNITY_IPHONE
	bool isButtonDown = false ;
	public float spherecastRasius = 2.5f ;
	#endif

	int myID ;
	bool isCurrentID = true ;

	void Start () {
		thisTransform = this.transform ;
		rootTransform = thisTransform.root ;
		currentAng = thisTransform.localEulerAngles.y ;
		targetPos = thisTransform.position + thisTransform.forward * 500.0f ;
		Image [] Temp_Images = GameObject.FindObjectsOfType < Image > () ;
		foreach ( Image Temp_Image in Temp_Images ) {
			if ( Temp_Image.name == markerName ) {
				markerImage = Temp_Image ;
			}
		}
		if ( markerImage ) {
			markerImage.enabled = false ;
		} else {
			Debug.LogWarning ( markerName + "Image for Marker cannot be found in the scene." ) ;
		}
	}

	void LateUpdate () {
		if ( isCurrentID ) {
			#if UNITY_ANDROID || UNITY_IPHONE
			Mobile_Control () ;
			#else
			Mouse_Control () ;
			#endif
			if ( markerImage ) {
				markerImage.transform.position = Camera.main.WorldToScreenPoint ( targetPos ) ;
				if ( markerImage.transform.position.z < 0 ) {
					markerImage.enabled = false ;
				} else {
					markerImage.enabled = true ;
				}
			}
		}
	}

	#if !UNITY_ANDROID && !UNITY_IPHONE
	void Mouse_Control () {
		if ( Input.GetMouseButtonDown ( 0 ) ) {
			Cast_Ray ( Input.mousePosition , true ) ;
			previousMousePos = Input.mousePosition ;
			BroadcastMessage ( "GunCam_On" , SendMessageOptions.DontRequireReceiver ) ;
		} else if ( Input.GetMouseButton ( 0 ) ) {
			adjustAng += ( Input.mousePosition - previousMousePos ) * 0.02f ;
			previousMousePos = Input.mousePosition ;
		} else if ( Input.GetMouseButtonUp ( 0 ) ) {
			BroadcastMessage ( "GunCam_Off" , SendMessageOptions.DontRequireReceiver ) ;
			adjustAng = Vector3.zero ;
			targetTransform = null ;
		} else {
			Cast_Ray ( Input.mousePosition , false ) ;
		}
	}
	#endif

	#if UNITY_ANDROID || UNITY_IPHONE
	void Mobile_Control () {
		// Set Target.
		if ( CrossPlatformInputManager.GetButtonDown ( "Target_Press" ) ) {
			Cast_Ray_Mobile ( new Vector3 ( CrossPlatformInputManager.GetAxis ( "Target_X" ) , CrossPlatformInputManager.GetAxis ( "Target_Y" ) , 0.0f ) ) ;
		}
		// Control GunCam and Aiming.
		if ( CrossPlatformInputManager.GetButtonDown ( "GunCam_Press" ) ) {
			isButtonDown = true ;
			previousMousePos = new Vector3 ( CrossPlatformInputManager.GetAxis ( "GunCam_X" ) , CrossPlatformInputManager.GetAxis ( "GunCam_Y" ) , 0.0f ) ;
			BroadcastMessage ( "GunCam_On" , SendMessageOptions.DontRequireReceiver ) ;
			return ;
		}
		if ( isButtonDown && CrossPlatformInputManager.GetButton ( "GunCam_Press" ) ) {
			Vector3 currentMousePos = new Vector3 ( CrossPlatformInputManager.GetAxis ( "GunCam_X" ) , CrossPlatformInputManager.GetAxis ( "GunCam_Y" ) , 0.0f ) ;
			adjustAng += ( currentMousePos - previousMousePos ) * 0.02f ;
			previousMousePos = currentMousePos ;
			return ;
		}
		if ( CrossPlatformInputManager.GetButtonUp ( "GunCam_Press" ) ) {
			isButtonDown = false ;
			adjustAng = Vector3.zero ;
			BroadcastMessage ( "GunCam_Off" , SendMessageOptions.DontRequireReceiver ) ;
		}
	}
	#endif

	void Cast_Ray ( Vector3 screenPos , bool isLockOn ) {
		Ray ray = Camera.main.ScreenPointToRay ( screenPos ) ;
		RaycastHit raycastHit ;
		if ( Physics.Raycast ( ray , out raycastHit , 1000.0f ) ) {
			Transform colliderTransform = raycastHit.collider.transform ;
			if ( colliderTransform.root != rootTransform ) {
				if ( isLockOn ) {
					if ( raycastHit.transform.GetComponent < Rigidbody > () ) { //When 'raycastHit.collider.transform' is Turret, 'raycastHit.transform' is the MainBody.
						targetTransform = colliderTransform ;
						targetOffset = targetTransform.InverseTransformPoint ( raycastHit.point ) ;
						return ;
					} else {
						targetTransform = null ;
					}
				}
				targetPos = raycastHit.point ;
			} else { // Ray hits itsel
				screenPos.z = 50.0f ;
				targetPos = Camera.main.ScreenToWorldPoint ( screenPos ) ;
			}
		} else { // Ray does not hit anythig.
			screenPos.z = 500.0f ;
			targetPos = Camera.main.ScreenToWorldPoint ( screenPos ) ;
		}
	}

	#if UNITY_ANDROID || UNITY_IPHONE
	void Cast_Ray_Mobile ( Vector3 screenPos ) {
		Ray ray = Camera.main.ScreenPointToRay ( screenPos ) ;
		RaycastHit [] raycastHits ;
		raycastHits = Physics.SphereCastAll ( ray , spherecastRasius , 1000.0f ) ;
		foreach ( RaycastHit raycastHit in raycastHits ) {
			Transform colliderTransform = raycastHit.collider.transform ;
			if ( colliderTransform.root != rootTransform ) {
				if ( raycastHit.transform.GetComponent < Rigidbody > () ) { //When 'raycastHit.collider.transform' is Turret, 'raycastHit.transform' is the MainBody.
					targetTransform = colliderTransform ;
					targetOffset = targetTransform.InverseTransformPoint ( raycastHit.point ) ;
					return ;
				}
			}
		}
		Cast_Ray ( screenPos , true ) ;
	}
	#endif

	void FixedUpdate () {
		// Calculate Angle.
		if ( targetTransform ) {
			targetPos = targetTransform.position + ( targetTransform.forward * targetOffset.z ) + ( targetTransform.right * targetOffset.x ) + ( targetTransform.up * targetOffset.y ) ;
		}
		local_TargetPos = thisTransform.InverseTransformPoint ( targetPos ) ;
		float targetAng = Vector2.Angle ( Vector2.up , new Vector2 ( local_TargetPos.x , local_TargetPos.z ) ) * Mathf.Sign ( local_TargetPos.x ) ;
		targetAng += adjustAng.x ;
		// Calculate Turn Rate.
		float targetSpeedRate = Mathf.Lerp ( 0.0f , 1.0f , Mathf.Abs ( targetAng ) / ( rotationSpeed * Time.fixedDeltaTime + bufferAngle ) ) * Mathf.Sign ( targetAng ) ;
		// Calculate Rate
		speedRate = Mathf.MoveTowardsAngle ( speedRate , targetSpeedRate , Time.fixedDeltaTime / acceleration_Time ) ;
		// Rotate
		currentAng += rotationSpeed * speedRate * Time.fixedDeltaTime ;
		thisTransform.localRotation = Quaternion.Euler ( new Vector3 ( 0.0f , currentAng , 0.0f ) ) ;
	}

	void Get_ID ( int idNum ) {
		myID = idNum ;
	}
	
	void Get_Current_ID ( int idNum ) {
		if ( idNum == myID ) {
			isCurrentID = true ;
		} else {
			isCurrentID = false ;
			if ( markerImage ) {
				markerImage.enabled = false ;
			}
		}
	}
}