#pragma strict
//interfaz
var isVisible:boolean = true;
var textoStart:GameObject;
var textoVol:GameObject;
var textoDif:GameObject;
var textoDif2:GameObject;
var textoQuit:GameObject;
var cursorMenu:GameObject;
var volBar:GameObject;
var volBarBg:GameObject;
var camara:GameObject;

enum Dificultad {Bajo, Medio, Alto};

//sonidos
var selectSnd:AudioClip;
var startSnd:AudioClip;

//control
private var opcionSeleccionada:int = 0;
private var vertical:boolean = false;
private var horizontal:boolean = false;
private var seleccionando:boolean = false;
private var subiendoVolumen:boolean = false;
private var sliderVol:float = 1.0f;
private var cambiandoDificultad:boolean = false;
private var dif:Dificultad=Dificultad.Medio;

function Start () {
	isVisible = false;
	textoStart.guiText.enabled=false;
	textoVol.guiText.enabled=false;
	textoDif.guiText.enabled=false;
	textoQuit.guiText.enabled=false;
	cursorMenu.renderer.enabled=false;
	textoVisible();
	cursorVisible();
	sliderVol = PlayerPrefs.GetFloat("vol");
	volBar.guiTexture.pixelInset.width=sliderVol*195;
	dif = PlayerPrefs.GetInt("dif");
	textoDif2.guiText.text = "\t" + dif.ToString();
}

function OnGUI () {
	if (isVisible) {
		//recoger inputs y cargar escenas
		if (!vertical && !subiendoVolumen && !cambiandoDificultad && (Input.GetAxisRaw("Vertical") != 0)) {
			vertical = true;
			audio.PlayOneShot(selectSnd);
			opcionSeleccionada = (opcionSeleccionada - Input.GetAxisRaw("Vertical")) % 4;
			if (opcionSeleccionada < 0) opcionSeleccionada = 3;
			Debug.Log(opcionSeleccionada);
			colocaCursor(opcionSeleccionada);
			esperaVertical(0.2f);
		}
		
		if (!seleccionando && Input.GetButtonDown("Fire1")) {
			seleccionando = true;
			audio.PlayOneShot(startSnd);
			switch (opcionSeleccionada) {
				case 0:
					//cargar escena juego
					//dif se guarda en cada cambio;
					PlayerPrefs.SetFloat("vol", sliderVol);
					PlayerPrefs.Save();
					audio.PlayOneShot(startSnd);
					camara.GetComponent(fade).fadeAndLoad("juego_01");
					break;
				case 1:
					//SliderVolumen
					cursorVisible();
					textoVol.guiText.enabled = subiendoVolumen;
					subiendoVolumen=!subiendoVolumen;	
					volBar.guiTexture.enabled = subiendoVolumen;	
					volBarBg.guiTexture.enabled = subiendoVolumen;	
					break;
				case 2:
					//dificultad
					cursorVisible();
					if (!cambiandoDificultad){
						textoDif2.guiText.color = Color.black;
					} else {
						textoDif2.guiText.color = Color(0.235,0.235,0.235);
					}
						cambiandoDificultad = !cambiandoDificultad;
					break;
				case 3:
					Application.Quit();
			}
			esperaSeleccionando(0.3f);
		}
		if (subiendoVolumen) {
			if (!horizontal && Input.GetAxisRaw("Horizontal")!= 0) {
				horizontal = true;
				sliderVol += Input.GetAxis("Horizontal")*0.1f;
				if (sliderVol > 1) sliderVol = 1; 
				else if (sliderVol < 0) sliderVol = 0; 
				else {
					Debug.Log(sliderVol);
					audio.PlayOneShot(selectSnd);
				}
				volBar.guiTexture.pixelInset.width=sliderVol*195;
				esperaHorizontal(0.1f);
				audio.volume = sliderVol;
			}
		}
		if (!horizontal && cambiandoDificultad && Input.GetAxisRaw("Horizontal") != 0) {
			horizontal = true;
			audio.PlayOneShot(selectSnd);
			dif = (dif + 1*Mathf.FloorToInt(Input.GetAxisRaw("Horizontal")));
			if (dif >2) dif = Dificultad.Bajo;
			if (dif <0) dif = Dificultad.Alto;
			textoDif2.guiText.text = "\t" + dif.ToString();
			PlayerPrefs.SetInt("dif", dif);
			esperaHorizontal(0.3f);
		}
					
	}		
}

function esperaVertical (segundos:float) {
	yield new WaitForSeconds(segundos);
	vertical =false;
}
function esperaHorizontal (segundos:float) {
	yield new WaitForSeconds(segundos);
	horizontal =false;
}

function esperaSeleccionando (segundos:float) {
	yield new WaitForSeconds(segundos);
	seleccionando =false;
}

function colocaCursor (opcionSeleccionada:int) {
	//decrementos de 0.6 a partir de -3
	switch (opcionSeleccionada) {
		case 0:
			cursorMenu.transform.position.y = -3;
			break;
		case 1:
			cursorMenu.transform.position.y = -3.6;
			break;
		case 2:
			cursorMenu.transform.position.y = -4.2;
			break;
		case 3:
			cursorMenu.transform.position.y = -4.8;
			break;
	}
	
}

function textoVisible () {
	yield new WaitForSeconds(1.0f);
	isVisible = !isVisible;
	textoStart.guiText.enabled=true;
	textoVol.guiText.enabled=true;
	textoDif.guiText.enabled=true;
	textoQuit.guiText.enabled=true;
}

function cursorVisible () { 
	cursorMenu.renderer.enabled = !cursorMenu.renderer.enabled;
}
