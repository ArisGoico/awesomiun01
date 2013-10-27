using UnityEngine;
using System.Collections;

public class ControladorJugador : MonoBehaviour
{
	private float lastPresTime = 0.0f;
	
	public void Update()
	{
		if(Time.time > lastPresTime + 0.25)
		{
			if(Input.GetButton("UP"))
			{
				lastPresTime = Time.time;				
				if(Utilidades.hayCasilla(transform.position.x,transform.position.z+1))
					transform.Translate(Vector3.forward,Space.World);
			}
			else if(Input.GetButton("LEFT"))
			{
				lastPresTime = Time.time;				
				if(Utilidades.hayCasilla(transform.position.x-1,transform.position.z))
					transform.Translate(Vector3.left,Space.World);
			}
			else if(Input.GetButton("RIGHT"))
			{
				lastPresTime = Time.time;				
				if(Utilidades.hayCasilla(transform.position.x+1,transform.position.z))
					transform.Translate(Vector3.right,Space.World);
			}
			else if(Input.GetButton("DOWN"))
			{
				lastPresTime = Time.time;				
				if(Utilidades.hayCasilla(transform.position.x,transform.position.z-1))
					transform.Translate(Vector3.back,Space.World);
			}	
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
	    {
	        if(Camera.main.fieldOfView > 30)
				Camera.main.fieldOfView -= 3;
	    }
	    if (Input.GetAxis("Mouse ScrollWheel") < 0)
	    {
	        if(Camera.main.fieldOfView < 90)
				Camera.main.fieldOfView += 3;
	    }
	}
	
	
}
