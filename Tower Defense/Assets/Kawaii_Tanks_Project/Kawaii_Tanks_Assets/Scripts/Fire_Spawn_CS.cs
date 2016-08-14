using UnityEngine;
using System.Collections;

// This script must be attached to "Fire_Point".

public class Fire_Spawn_CS : MonoBehaviour {

	[ Header ( "Firing settings" ) ]
	[ Tooltip ( "Prefab of muzzle fire." ) ] public GameObject firePrefab ;
	[ Tooltip ( "Prefab of bullet." ) ] public GameObject bulletPrefab ;
	[ Tooltip ( "Speed of the bullet. (Meter per Second)" ) ] public float bulletVelocity = 250.0f ;
	[ Tooltip ( "Offset distance for spawning the bullet. (Meter)" ) ] public float spawnOffset = 1.0f ;

	Transform thisTransform ;

	void Start () {
		thisTransform = this.transform ;
	}

	void Fire () {
		// Muzzle Fire
		if ( firePrefab ) {
			GameObject fireObject = Instantiate ( firePrefab , thisTransform.position , thisTransform.rotation ) as GameObject ;
			fireObject.transform.parent = thisTransform ;
		}
		// Shot Bullet
		if ( bulletPrefab ) {
			GameObject bulletObject = Instantiate ( bulletPrefab , thisTransform.position + thisTransform.forward * spawnOffset , thisTransform.rotation ) as GameObject ;
			bulletObject.GetComponent < Rigidbody > ().velocity = thisTransform.forward * bulletVelocity ;
		}

	}
}
