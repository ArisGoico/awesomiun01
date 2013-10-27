using UnityEngine;
using System.Collections;

public static class Utilidades 
{
	public static GameObject dameCasilla(float x, float z)
	{
		RaycastHit hit;
		GameObject obj = null;
		int layerMask = 1 << 8;
		if(Physics.Raycast(new Vector3(x,1,z), Vector3.down, out hit, Mathf.Infinity, layerMask))
		{
			obj = hit.collider.transform.parent.gameObject;		
		}		
		return obj;		
	}
	
	public static bool hayCasilla(float x, float z)
	{
		return dameCasilla(x,z) != null;
	}
}
