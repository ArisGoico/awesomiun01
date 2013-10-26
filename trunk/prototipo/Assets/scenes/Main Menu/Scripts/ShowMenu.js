#pragma strict
var isVisible:boolean=true;
var textoStart:GameObject;
var textoOpt:GameObject;
var textoQuit:GameObject;
var cursorMenu:GameObject;

var selectSnd:AudioClip;
var startSnd:AudioClip;

private var opcionSeleccionada:int=0;
private var seleccionando:boolean = false;

function Start () {
	/*isVisible = false;
	textoStart.guiText.enabled=false;
	textoOpt.guiText.enabled=false;
	textoQuit.guiText.enabled=false;
	cursorMenu.renderer.enabled=false;*/

}

function OnGUI () {
	if (isVisible) {
		//recoger inputs y cargar escenas
		if (!seleccionando && (Input.GetAxisRaw("Vertical") != 0)) {
			seleccionando = true;
			audio.PlayOneShot(selectSnd);
			opcionSeleccionada = (opcionSeleccionada - Input.GetAxisRaw("Vertical")) % 3;
			if (opcionSeleccionada < 0) opcionSeleccionada = 2;
			Debug.Log(opcionSeleccionada);
			colocaCursor(opcionSeleccionada);
			esperaSegs(0.2f);
		}
		
		if (Input.GetButtonDown("Fire1")) {
			audio.PlayOneShot(startSnd);
			switch (opcionSeleccionada) {
				case 0:
					Application.LoadLevel("juego_01");
					break;
				case 1:
					//pantalla opciones
				case 2:
					Application.Quit();
			}
		}
	}
}

function esperaSegs (segundos:float) {
	yield new WaitForSeconds(segundos);
	seleccionando =false;
}

function colocaCursor (opcionSeleccionada:int) {
	//0 = -3; 1 = -3.61; 2 = -4.22
	switch (opcionSeleccionada) {
		case 0:
			cursorMenu.transform.position.y = -3;
			break;
		case 1:
			cursorMenu.transform.position.y = -3.61;
			break;
		case 2:
			cursorMenu.transform.position.y = -4.22;
			break;
	}
	
}

function turnVisible () {
	transform.animation.PlayQueued("dejaPaso");	
	transform.GetChild(3).animation.Play();	
	isVisible = !isVisible;
	textoStart.guiText.enabled = !textoStart.guiText.enabled;
	textoOpt.guiText.enabled = !textoOpt.guiText.enabled;
	textoQuit.guiText.enabled = !textoQuit.guiText.enabled;
	cursorMenu.renderer.enabled = !cursorMenu.renderer.enabled; 
}
