using UnityEngine;
//using System.Collections;
using System;

using UnityStandardAssets.CrossPlatformInput ;

// This script must be attached to Root Object of the tank.

public class ID_Control_CS : MonoBehaviour {

	[ Header ( "ID number settings" ) ]
	[ Tooltip ( "ID number for this tank." ) ] public int myID = 1 ;

	int currentID = 1 ;

	void Start () {
		BroadcastMessage ( "Get_ID" , myID , SendMessageOptions.DontRequireReceiver ) ;
		BroadcastMessage ( "Get_Current_ID" , currentID , SendMessageOptions.DontRequireReceiver ) ;
	}

	void Update () {
		if ( Input.GetKeyDown ( KeyCode.Escape ) ) {
			Application.Quit () ;
		}
		#if UNITY_ANDROID || UNITY_IPHONE
		if ( CrossPlatformInputManager.GetButtonDown ( "Switch" ) ) {
		#else
		if ( Input.GetKeyDown ( KeyCode.Return ) ) {
		#endif
			ID_Control_CS [] iDScripts = FindObjectsOfType < ID_Control_CS > () ;
			int [] iDArray = new int [ iDScripts.Length ] ;
			for ( int i = 0 ; i < iDScripts.Length ; i++ ) {
				iDArray [ i ] = iDScripts [ i ].myID ;
			}
			Array.Sort ( iDArray ) ;
			for ( int i = 0 ; i < iDArray.Length ; i++ ) {
				if ( iDArray [ i ] == currentID ) {
					if ( i == iDArray.Length - 1 ) {
						currentID = iDArray [ 0 ] ;
					} else {
						currentID = iDArray [ i + 1 ] ;
					}
					break ;
				}
			}
			BroadcastMessage ( "Get_Current_ID" , currentID , SendMessageOptions.DontRequireReceiver ) ;
		}
	}
}
