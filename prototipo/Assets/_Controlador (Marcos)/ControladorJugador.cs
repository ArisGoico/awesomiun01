using UnityEngine;
using System.Collections;

public class ControladorJugador : MonoBehaviour
{
	public float delayMovimiento = 0.25f;
	private float lastPressTime = 0.0f;
	private bool absorbiendo = false;
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
			if(Time.time > lastPressTime + delayMovimiento)
			{
				x = transform.position.x;
				z = transform.position.z;
				casillaActual = Utilidades.dameCasilla(x,z);
				casillaActualScript = casillaActual.GetComponent<scriptCasilla>();
				// Movimiento
				if(Input.GetAxisRaw("Vertical") != 0)
				{
					lastPressTime = Time.time;				
					if(Utilidades.hayCasilla(x,z+Input.GetAxisRaw("Vertical")))
					{
						animation.PlayQueued("bajar");
						transform.Translate(Vector3.forward*Input.GetAxisRaw("Vertical"),Space.World);
						animation.PlayQueued("subir");
					}
				}
				if(Input.GetAxisRaw("Horizontal") != 0)
				{
					lastPressTime = Time.time;				
					if(Utilidades.hayCasilla(x+Input.GetAxisRaw("Horizontal"),z))
					{
						animation.PlayQueued("bajar");
						transform.Translate(Vector3.right*Input.GetAxisRaw("Horizontal"),Space.World);
						animation.PlayQueued("subir");
					}
				}
				//Acciones casilla
				else if(Input.GetButton("Fire1") && !absorbiendo)
				{	
					absorbiendo = true;
					animation.PlayQueued("absorber");					
					lastPressTime = Time.time;
					if(casillaActualScript.quitarRojo())
						;//ANIMACION QUITAR COLOR
					colorBool color = casillaActualScript.color;
					logicaControl.cambiaColorJug(color, casillaActual, casillaActualScript.control, casillaActualScript.ordenControl);
				}
				else if(Input.GetButton("Fire2") && !absorbiendo)
				{
					absorbiendo = true;
					animation.PlayQueued("absorber");					
					lastPressTime = Time.time;
					if(casillaActualScript.quitarVerde())
						;//ANIMACION QUITAR COLOR
					colorBool color = casillaActualScript.color;
					logicaControl.cambiaColorJug(color, casillaActual, casillaActualScript.control, casillaActualScript.ordenControl);
				}
				else if(Input.GetButton("Fire3") && !absorbiendo)
				{
					absorbiendo = true;
					animation.PlayQueued("absorber");					
					lastPressTime = Time.time;
					if(casillaActualScript.quitarAzul())
						;//ANIMACION QUITAR COLOR
					colorBool color = casillaActualScript.color;
					logicaControl.cambiaColorJug(color, casillaActual, casillaActualScript.control, casillaActualScript.ordenControl);
				}				
			}
		
		//Zoom
		if ((Input.GetAxis("Mouse ScrollWheel") > 0) && (Camera.main.fieldOfView > 30))
		{
				Camera.main.fieldOfView -= 3;
	    }
	    if ((Input.GetAxis("Mouse ScrollWheel") < 0) && (Camera.main.fieldOfView < 90))
		{
				Camera.main.fieldOfView += 3;
	    }	
				
	}
	
	public void dejaAbsorbe() {
		absorbiendo = false;
	}
}
