using UnityEngine;
using System.Collections;


public class scriptCasilla : MonoBehaviour {

	//Este script solo contiene variables publicas para usar por el resto de scripts
	public colorBool color;
	public int ordenControl;	
	public controlCasilla control;
	
	////////////////////////////////////////////////////////////////////////////////////////////	
	//TEST
	public bool red;
	public bool yellow;
	public bool blue;
	
	public void Update()
	{
		red = color.r;
		yellow = color.g;
		blue = color.b;	
	}
	////////////////////////////////////////////////////////////////////////////////////////////	
	
	public void aplicarRojo()
	{
		color.r = true;		
	}
	
	public void aplicarVerde()
	{
		color.g = true;		
	}
	
	public void aplicarAzul()
	{
		color.b = true;		
	}
	
	/* Devuelve true si se ha podido quitar rojo */
	public bool quitarRojo()
	{
		if(color.r)
		{
			color.r = false;
			return true;
		}
		else
			return false;
	}
	
	/* Devuelve true si se ha podido quitar verde */
	public bool quitarVerde()
	{
		if(color.g)
		{
			color.g = false;
			return true;
		}
		else
			return false;	
	}
	
	/* Devuelve true si se ha podido quitar azul */
	public bool quitarAzul()
	{
		if(color.b)
		{
			color.b = false;
			return true;
		}
		else
			return false;		
	}	
}
