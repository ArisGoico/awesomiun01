using UnityEngine;
using System.Collections;

public class ControladorJugador : MonoBehaviour
{
	public float delayMovimiento = 0.25f;
	public float delayAbsorber = 0.5f;
	private float lastPressTime = 0.0f;
	private bool absorbiendo = false;
	private LogicaControl logicaControl;
	private float x;
	private float z;
	private GameObject casillaActual;	
	private scriptCasilla casillaActualScript;
	public Transform absorberBits; 
	private Vector3 targetPosition;
	private bool moving = false;
	private float movementSpeed = 6.5f;
	
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
		if(moving)
		{			
			transform.position = Vector3.Lerp(transform.position,targetPosition,movementSpeed * Time.smoothDeltaTime);
			if(Mathf.Abs(transform.position.z - targetPosition.z) < 0.05f && Mathf.Abs(transform.position.x - targetPosition.x) < 0.05f)
			{
				transform.position = targetPosition;			
				moving = false;
				//Debug.Log("Final :"+Time.time);
			}
		}
		else
		{
			if(Input.anyKey)
				if(Time.time > lastPressTime + delayMovimiento)
				{
					x = transform.position.x;
					z = transform.position.z;
					casillaActual = Utilidades.dameCasilla(x,z);
					casillaActualScript = casillaActual.GetComponent<scriptCasilla>();
					// Movimiento
					if((Input.GetAxisRaw("Vertical") != 0) && !(animation.IsPlaying("subir") || animation.IsPlaying("bajar")))
					{
						lastPressTime = Time.time;				
						if(Utilidades.hayCasilla(x,z+Input.GetAxisRaw("Vertical")))
						{
							animation.PlayQueued("bajar");
							//transform.Translate(Vector3.forward*Input.GetAxisRaw("Vertical"),Space.World);
							targetPosition = transform.position + Vector3.forward*Input.GetAxisRaw("Vertical");
							//Debug.Log("Inicio :"+Time.time);
							moving = true;
							animation.PlayQueued("subir");
						}
					}
					if((Input.GetAxisRaw("Horizontal") != 0) && !(animation.IsPlaying("subir") || animation.IsPlaying("bajar")))
					{
						lastPressTime = Time.time;				
						if(Utilidades.hayCasilla(x+Input.GetAxisRaw("Horizontal"),z))
						{
							animation.PlayQueued("bajar");
							//transform.Translate(Vector3.right*Input.GetAxisRaw("Horizontal"),Space.World);
							targetPosition = transform.position + Vector3.right*Input.GetAxisRaw("Horizontal");
							moving = true;
							animation.PlayQueued("subir");
						}
					}
				}
				if(Time.time > lastPressTime + delayAbsorber){
					//Acciones casilla
					if(Input.GetButton("Fire1"))
					{						
						lastPressTime = Time.time;
						if(casillaActualScript.quitarRojo()){
							absorberBits.particleSystem.renderer.material.SetColor("_Emission", Color.red);
							animation.PlayQueued("absorber");
						}
						colorBool color = casillaActualScript.color;
						logicaControl.cambiaColorJug(color, casillaActual, casillaActualScript.control, casillaActualScript.ordenControl);
					}
					else if(Input.GetButton("Fire2"))// && !(animation.IsPlaying("subir")))
					{				
						lastPressTime = Time.time;
						if(casillaActualScript.quitarVerde()){
							absorberBits.particleSystem.renderer.material.SetColor("_Emission", Color.yellow);
							animation.PlayQueued("absorber");
						}
						colorBool color = casillaActualScript.color;
						logicaControl.cambiaColorJug(color, casillaActual, casillaActualScript.control, casillaActualScript.ordenControl);
					}
					else if(Input.GetButton("Fire3"))// && !(animation.IsPlaying("subir")))
					{				
						lastPressTime = Time.time;
						if(casillaActualScript.quitarAzul()){
							animation.PlayQueued("absorber");
							absorberBits.particleSystem.renderer.material.SetColor("_Emission", new Color(0.0f, 0.2f, 1.0f, 1.0f));
						}
						colorBool color = casillaActualScript.color;
						logicaControl.cambiaColorJug(color, casillaActual, casillaActualScript.control, casillaActualScript.ordenControl);
					}				
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
