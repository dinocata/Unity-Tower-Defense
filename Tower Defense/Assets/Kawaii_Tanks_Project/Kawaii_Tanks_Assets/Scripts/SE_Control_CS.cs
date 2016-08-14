using UnityEngine;
using System.Collections;

// This script must be attached to "Engine_Sound".

public class SE_Control_CS : MonoBehaviour {

	[ Header ( "Engine Sound settings" ) ]
	[ Tooltip ( "Maximum Speed of this tank." ) ] public float maxSpeed = 7.0f ;
	[ Tooltip ( "Minimum Pitch" ) ] public float minPitch = 1.0f ;
	[ Tooltip ( "Maximum Pitch" ) ] public float maxPitch = 2.0f ;
	[ Tooltip ( "Minimum Volume" ) ] public float minVolume = 0.1f ;
	[ Tooltip ( "Maximum Volume" ) ] public float maxVolume = 0.3f ;
	[ Tooltip ( "Reference Right wheel." ) ] public Rigidbody rightReferenceWheel ;
	[ Tooltip ( "Reference Left wheel." ) ]public Rigidbody leftReferenceWheel ;

	AudioSource thisAudioSource ;
	float leftCircumference ;
	float rightCircumference ;
	float currentRate ;
	const float DOUBLE_PI = Mathf.PI * 2.0f ;

	void Start () {
		thisAudioSource = GetComponent < AudioSource > () ;
		if ( thisAudioSource == null ) {
			Debug.LogError ( "AudioSource is not attached to" + this.name ) ;
			Destroy ( this );
		}
		thisAudioSource.loop = true ;
		thisAudioSource.volume = 0.0f ;
		thisAudioSource.Play () ;
		// Calculate Circumference.
		if ( leftReferenceWheel && rightReferenceWheel ) {
			leftCircumference = DOUBLE_PI * leftReferenceWheel.GetComponent < SphereCollider > ().radius ;
			rightCircumference = DOUBLE_PI * rightReferenceWheel.GetComponent < SphereCollider > ().radius ;
		} else {
			Debug.LogError ( "Reference Wheels are not assigned in "+ this.name ) ;
			Destroy ( this );
		}
	}
	
	void Update () {
		float leftVelocity ;
		float rightVelocity ;
		leftVelocity = leftReferenceWheel.angularVelocity.magnitude / DOUBLE_PI * leftCircumference ;
		rightVelocity = rightReferenceWheel.angularVelocity.magnitude / DOUBLE_PI * rightCircumference ;
		float targetRate = ( leftVelocity + rightVelocity ) / 2.0f / maxSpeed ;
		currentRate = Mathf.MoveTowards ( currentRate , targetRate , 0.02f ) ;
		thisAudioSource.pitch = Mathf.Lerp ( minPitch , maxPitch , currentRate ) ;
		thisAudioSource.volume = Mathf.Lerp ( minVolume , maxVolume , currentRate ) ;
	}
}
