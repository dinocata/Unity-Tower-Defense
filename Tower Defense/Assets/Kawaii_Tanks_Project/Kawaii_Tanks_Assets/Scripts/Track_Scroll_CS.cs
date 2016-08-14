using UnityEngine;
using System.Collections;

// This script must be attached to Tracks.

public class Track_Scroll_CS : MonoBehaviour {

	[ Header ( "Scroll Animation settings" ) ]
	[ Tooltip ( "Reference wheel." ) ] public Transform referenceWheel ;
	[ Tooltip ( "Scroll Rate for X axis." ) ] public float scrollRate = 0.0005f ;
	[ Tooltip ( "Texture Name in the shader." ) ] public string textureName = "_DetailAlbedoMap" ;

	Material thisMaterial ;
	float previousAng ;
	float offsetX ;

	void Start () {
		thisMaterial = GetComponent < Renderer > ().material ;
		if ( referenceWheel == null ) {
			Debug.LogError ( "Reference Wheel is not assigned in " + this.name ) ;
			Destroy ( this ) ;
		}
	}
	
	void Update () {
		float currentAng = referenceWheel.localEulerAngles.y ;
		float deltaAng = Mathf.DeltaAngle ( currentAng , previousAng ) ;
		offsetX += scrollRate * deltaAng ;
		thisMaterial.SetTextureOffset ( textureName , new Vector2 ( offsetX , 0.0f ) ) ;
		previousAng = currentAng ;
	}
}
