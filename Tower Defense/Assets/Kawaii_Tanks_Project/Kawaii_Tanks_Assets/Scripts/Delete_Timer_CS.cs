using UnityEngine;
using System.Collections;

public class Delete_Timer_CS : MonoBehaviour {

	[ Header ( "Life time settings" ) ]
	[ Tooltip ( "Life time of this gameobject. (Sec)" ) ] public float lifeTime = 2.0f ;

	void Start () {
		Destroy ( this.gameObject , lifeTime ) ;
	}

}
