using UnityEngine;
using System.Collections;


public class scriptCasilla : MonoBehaviour {

	//Este script solo contiene variables publicas para usar por el resto de scripts
	public colorBool color;
	public int ordenControl;
	public controlCasilla control;
	
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
	public void quitarRojo()
	{
		color.r = false;		
	}
	public void quitarVerde()
	{
		color.g = false;		
	}
	public void quitarAzul()
	{
		color.b = false;		
	}	
}
