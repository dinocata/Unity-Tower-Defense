using UnityEngine;
using System.Collections;

public class Break_Object_CS : MonoBehaviour {

	[ Header ( "Broken object settings" ) ]
	[ Tooltip ( "Prefab of the broken object." ) ] public GameObject brokenObject ;
	[ Tooltip ( "lag time for spawning the broken object. (Sec)" ) ] public float lag = 1.0f ;

	Transform thisTransform ;

	void Start () {
		thisTransform = transform ;
	}

	void OnJointBreak () {
		StartCoroutine ( "Broken" ) ;
	}

	void OnTriggerEnter () {
		StartCoroutine ( "Broken" ) ;
	}

	IEnumerator Broken () {
		yield return new WaitForSeconds ( lag ) ;
		if ( brokenObject ) {
			Instantiate ( brokenObject , thisTransform.position , thisTransform.rotation ) ;
		}
		Destroy ( gameObject ) ;
	}
}
