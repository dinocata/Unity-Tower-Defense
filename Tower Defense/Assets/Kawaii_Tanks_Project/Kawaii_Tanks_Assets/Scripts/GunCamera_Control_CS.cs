using UnityEngine;
using System.Collections;
using UnityEngine.UI ;

public class GunCamera_Control_CS : MonoBehaviour {

	[ Header ( "Gun Camera settings" ) ]
	[ Tooltip ( "Main Camera of this tank." ) ] public Camera mainCamera ;
	[ Tooltip ( "Name of Image for Reticle." ) ] public string reticleName = "Reticle" ;

	Camera thisCamera ;
	Image reticleImage ;

	int myID ;

	void Awake () {
		thisCamera = GetComponent < Camera > () ;
		thisCamera.enabled = false ;
		thisCamera.tag = "MainCamera" ; 
	}

	void Start () {
		if ( mainCamera == null ) {
			Debug.LogError ( "'Main Camera is not assigned in Gun_Camera." ) ;
			Destroy ( this ) ;
		}
		Image [] Temp_Images = GameObject.FindObjectsOfType < Image > () ;
		foreach ( Image Temp_Image in Temp_Images ) {
			if ( Temp_Image.name == reticleName ) {
				reticleImage = Temp_Image ;
			}
		}
		if ( reticleImage ) {
			reticleImage.enabled = false ;
		} else {
			Debug.LogWarning ( reticleName + " (Image for Reticle) cannot be found in the scene." ) ;
		}
	}

	void GunCam_On () {
		mainCamera.enabled = false ;
		thisCamera.enabled = true ;
		if ( reticleImage ) {
			reticleImage.enabled = true ;
		}
	}

	void GunCam_Off () {
		thisCamera.enabled = false ;
		mainCamera.enabled = true ;
		if ( reticleImage ) {
			reticleImage.enabled = false ;
		}
	}

	void Get_ID ( int idNum ) {
		myID = idNum ;
	}

	void Get_Current_ID ( int idNum ) {
		if ( idNum != myID ) {
			thisCamera.enabled = false ;
			if ( reticleImage ) {
				reticleImage.enabled = false ;
			}
		}
	}
}
