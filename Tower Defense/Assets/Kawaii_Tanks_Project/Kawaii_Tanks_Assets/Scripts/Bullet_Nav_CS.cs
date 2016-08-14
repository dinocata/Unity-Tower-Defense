using UnityEngine;
using System.Collections;

public class Bullet_Nav_CS : MonoBehaviour {

	[ Header ( "Bullet settings" ) ]
	[ Tooltip ( "Life time of the bullet. (Sec)" ) ] public float lifeTime = 5.0f ;
	[ Tooltip ( "Prefab of the broken object." ) ] public GameObject brokenObject ;

	Transform thisTransform ;
	Rigidbody thisRigidbody ;

	bool  isLive = true ;
	bool isRayHit = false ;

	Vector3 nextPos ;
	GameObject hitObject ;
	Vector3 hitNormal ;

	void Awake () {  // (Note.) Sometimes OnCollisionEnter() is called earlier than Start().
		thisTransform = transform ;
		thisRigidbody = GetComponent < Rigidbody > () ;
	}

	void Start () {
		Destroy ( this.gameObject, lifeTime ) ;
	}

	void FixedUpdate () {
		if ( isLive ) {
			thisTransform.LookAt ( thisTransform.position + thisRigidbody.velocity ) ;
			if ( isRayHit == false ) {
				Ray ray = new Ray ( thisTransform.position , thisRigidbody.velocity ) ;
				int layerMask = ~ ( 1 << 10 ) ; // Ignore suspensions.
				RaycastHit raycastHit ;
				Physics.Raycast ( ray , out raycastHit , thisRigidbody.velocity.magnitude * Time.fixedDeltaTime , layerMask ) ;
				if ( raycastHit.collider ) {
					nextPos = raycastHit.point ;
					//hitObject = raycastHit.collider.gameObject ;
					//hitNormal = raycastHit.normal ;
					isRayHit = true ;
				}
			} else {
				thisTransform.position = nextPos ;
				thisRigidbody.position = nextPos ;
				isLive = false ;
				Hit () ;
			}
		}
	}

	void OnCollisionEnter ( Collision collision ) {
		if ( isLive ) {
			isLive = false ;
			//hitObject = collision.gameObject ;
			//hitNormal = collision.contacts [0].normal ;
			Hit () ;
		}
	}

	void Hit () {
		if ( brokenObject ) {
			Instantiate ( brokenObject , thisTransform.position , Quaternion.identity ) ;
		}
		//
		// Write your damage process using "hitObject" and "hitNormal".
		//
		Destroy ( this.gameObject ) ;
	}
}
