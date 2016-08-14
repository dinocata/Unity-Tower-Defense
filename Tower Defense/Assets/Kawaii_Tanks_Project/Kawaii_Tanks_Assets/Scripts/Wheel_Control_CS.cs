using UnityEngine;
using System.Collections;

using UnityStandardAssets.CrossPlatformInput;

// This script must be attached to "MainBody".

public class Wheel_Control_CS : MonoBehaviour {

	[ Header ( "Driving settings" ) ]
	[ Tooltip ( "Torque added to each wheel." ) ] public float wheelTorque = 2000.0f ; // Reference to "Wheel_Rotate".
	[ Tooltip ( "Maximum Speed (Meter per Second)" ) ] public float maxSpeed = 7.0f ; // Reference to "Wheel_Rotate".
	[ Tooltip ( "Rate for ease of turning." ) , Range ( 0.0f , 1.0f ) ] public float turnClamp = 0.5f ;
	[ Tooltip ( "Velocity the parking brake automatically works." ) ] public float autoParkingBrakeVelocity = 0.5f ;
	[ Tooltip ( "Lag time for activating the parking brake." ) ] public float autoParkingBrakeLag = 0.5f ;
	[ Tooltip ( "'Solver Iteration Count' of all the rigidbodies in this tank." ) ] public int solverIterationCount = 7 ;

	[HideInInspector] public float leftRate ; // Reference to "Wheel_Rotate".
	[HideInInspector] public float rightRate ; // Reference to "Wheel_Rotate".

	Rigidbody thisRigidbody ;

	bool isParkingBrake = false ;
	float lagCount ;

	int myID ;
	bool isCurrentID = true ;

	void Awake () {
		// Layer Collision Settings.
		// Layer9 >> for wheels.
		// Layer10 >> for Suspensions.
		// Layer11 >> for MainBody.
		for ( int i =0 ; i <= 11 ; i ++ ) {
			Physics.IgnoreLayerCollision ( 9 , i , false ) ; // Reset settings.
			Physics.IgnoreLayerCollision ( 11 , i , false ) ; // Reset settings.
		}
		Physics.IgnoreLayerCollision ( 9 , 9 , true ) ; // Wheels do not collide with each other.
		Physics.IgnoreLayerCollision ( 9 , 11 , true ) ; // Wheels do not collide with MainBody.
		for ( int i =0 ; i <= 11 ; i ++ ) {
			Physics.IgnoreLayerCollision ( 10 , i , true ) ; // Suspensions do not collide with anything.
		}
	}

	void Start () {
		this.gameObject.layer = 11 ; // Layer11 >> for MainBody.
		thisRigidbody = GetComponent < Rigidbody > () ;
		thisRigidbody.solverIterationCount = solverIterationCount ;
		BroadcastMessage ( "Get_Wheel_Control" , this , SendMessageOptions.DontRequireReceiver ) ; // Send this reference to all the "Wheel_Rotate".
	}
	
	void Update () {
		if ( isCurrentID ) {
			#if UNITY_ANDROID || UNITY_IPHONE
			float vertical = CrossPlatformInputManager.GetAxis ( "Vertical" ) ;
			float horizontal = Mathf.Clamp ( CrossPlatformInputManager.GetAxis ( "Horizontal" ) , -turnClamp , turnClamp ) ;
			#else
			float vertical = Input.GetAxis ( "Vertical" ) ;
			float horizontal = Mathf.Clamp ( Input.GetAxis ( "Horizontal" ) , -turnClamp , turnClamp ) ;
			#endif
			leftRate = Mathf.Clamp ( -vertical - horizontal , -1.0f , 1.0f ) ;
			rightRate = Mathf.Clamp ( vertical - horizontal , -1.0f , 1.0f ) ;
		}
	}

	void FixedUpdate () {
		// Auto Parking Brake using 'RigidbodyConstraints'.
		if ( leftRate == 0.0f && rightRate == 0.0f ) {
			float velocityMag = thisRigidbody.velocity.magnitude ;
			float angularVelocityMag = thisRigidbody.angularVelocity.magnitude ;
			if ( isParkingBrake == false ) {
				if ( velocityMag < autoParkingBrakeVelocity && angularVelocityMag < autoParkingBrakeVelocity ) {
					lagCount += Time.fixedDeltaTime ;
					if ( lagCount > autoParkingBrakeLag ) {
						isParkingBrake = true ;
						thisRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY ;
					}
				}
			} else {
				if ( velocityMag > autoParkingBrakeVelocity || angularVelocityMag > autoParkingBrakeVelocity ) {
					isParkingBrake = false ;
					thisRigidbody.constraints = RigidbodyConstraints.None ;
					lagCount = 0.0f ;
				}
			}
		} else {
			isParkingBrake = false ;
			thisRigidbody.constraints = RigidbodyConstraints.None ;
			lagCount = 0.0f ;
		}
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
