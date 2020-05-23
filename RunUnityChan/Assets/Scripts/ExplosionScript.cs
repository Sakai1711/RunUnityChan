using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

	public void DeleteExplosion ()
	{
		Destroy(gameObject);
	}
}
