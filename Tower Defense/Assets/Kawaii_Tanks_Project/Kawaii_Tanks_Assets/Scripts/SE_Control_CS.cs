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
		thisAudioSource.volume = 0.2f ;
		thisAudioSource.Play () ;
	}
}
