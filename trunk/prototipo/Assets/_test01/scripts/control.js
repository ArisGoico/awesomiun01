#pragma strict

var limitex:float = 4;
var limitey:float = 4;
var speed : float = 10;
private var color:Color= Color(0,0,0);
var proyeccion:GameObject;
private var coloreando = false;

function Start () {
	proyeccion.light.color = color;
	proyeccion.light.intensity = 0.1f;

}

function Update () {
	if (Input.GetAxisRaw("Mouse X") != 0 ) {
		transform.Translate(Vector3(Input.GetAxis("Mouse X")*speed*Time.deltaTime,0,0));
		if (transform.position.x > limitex) { 
			transform.position.x = limitex;
		}
		if (transform.position.x < -limitex) { 
			transform.position.x = -limitex;
		}
	}
	if (Input.GetAxisRaw("Mouse Y") != 0 ) {
		transform.Translate(Vector3(0,Input.GetAxis("Mouse Y")*speed*Time.deltaTime,0));
		if (transform.position.z > limitey) { 
			transform.position.z = limitey;
		}
		if (transform.position.z < -limitey) { 
			transform.position.z = -limitey;
		}
	}
	if (Input.GetButtonDown("Fire1")) {
		//ciclar por colores
		color.r = (color.r + 128) % 256;
		proyeccion.light.color = color;
	}
	
	if (Input.GetButtonDown("Fire2")) {
		//ciclar por colores
		color.g = (color.g + 128) % 256;
		proyeccion.light.color = color;
	}
	
	if (Input.GetButtonDown("Fire3")) {
		//ciclar por colores
		color.b = (color.b + 128) % 256;
		proyeccion.light.color = color;
	}
	if (!coloreando && Input.GetButton("Jump")) { 
		Debug.Log("disparando");
		//lanzar color a objeto colisionado
		coloreando = true;
		var hit: RaycastHit;
		Debug.DrawLine (transform.position,transform.position-Vector3(0,50,0));
   		if (Physics.Raycast(transform.position, transform.position-Vector3(0,50,0), hit)){
   			hit.transform.renderer.material.color += color;
   			Debug.Log(hit.transform);
   		}
   		delayColor(0.2f);
		//Raycast.hit.collider.renderer.material.color += color; o algo asi 
	}
}

function delayColor (segundos:float) {
	yield new WaitForSeconds(segundos);
	coloreando = false;
}	