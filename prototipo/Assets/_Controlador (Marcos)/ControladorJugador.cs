using UnityEngine;
using System.Collections;

public class ControladorJugador : MonoBehaviour
{
	private float lastPresTime = 0.0f;
	private LogicaControl logicaControl;
	private float x;
	private float z;
	private GameObject casillaActual;	
	private scriptCasilla casillaActualScript;
	
	public void Start()
	{
		//Inicializa variables de uso comun
		logicaControl = GameObject.FindGameObjectWithTag("script").GetComponent<LogicaControl>();
			
		
		//Posiciona al jugador en el centro del tablero
		int xMedio = logicaControl.ancho/2;
		int zMedio = logicaControl.alto/2;		
		transform.Translate(new Vector3(xMedio,0,zMedio));	
	}
	
	public void Update()
	{		
		if(Input.anyKey)
			if(Time.time > lastPresTime + 0.25)
			{
				x = transform.position.x;
				z = transform.position.z;
				casillaActual = Utilidades.dameCasilla(x,z);
				casillaActualScript = casillaActual.GetComponent<scriptCasilla>();
				// Movimiento
				if(Input.GetButton("UP"))
				{
					lastPresTime = Time.time;				
					if(Utilidades.hayCasilla(x,z+1))
						transform.Translate(Vector3.forward,Space.World);
				}
				else if(Input.GetButton("LEFT"))
				{
					lastPresTime = Time.time;				
					if(Utilidades.hayCasilla(x-1,z))
						transform.Translate(Vector3.left,Space.World);
				}
				else if(Input.GetButton("RIGHT"))
				{
					lastPresTime = Time.time;				
					if(Utilidades.hayCasilla(x+1,z))
						transform.Translate(Vector3.right,Space.World);
				}
				else if(Input.GetButton("DOWN"))
				{
					lastPresTime = Time.time;				
					if(Utilidades.hayCasilla(x,z-1))
						transform.Translate(Vector3.back,Space.World);
				}	
				//Acciones casilla
				else if(Input.GetButton("RED"))
				{
					casillaActualScript.quitarRojo();
					colorBool color = casillaActualScript.color;
					logicaControl.cambiaColor(color,casillaActual,logicaControl.matToControl(logicaControl.colBoolToMat(color)),1);
				}
				else if(Input.GetButton("GREEN"))
				{
					casillaActualScript.quitarVerde();
					colorBool color = casillaActualScript.color;
					logicaControl.cambiaColor(color,casillaActual,logicaControl.matToControl(logicaControl.colBoolToMat(color)),1);
				}
				else if(Input.GetButton("BLUE"))
				{
					casillaActualScript.quitarAzul();
					colorBool color = casillaActualScript.color;
					logicaControl.cambiaColor(color,casillaActual,logicaControl.matToControl(logicaControl.colBoolToMat(color)),1);
				}				
			}
		
		//Zoom
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
