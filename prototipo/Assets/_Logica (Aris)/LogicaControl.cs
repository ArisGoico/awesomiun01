﻿using UnityEngine;
using System.Collections;

public class LogicaControl : MonoBehaviour {
	
	//Variables publicas a inicializar desde el editor
	public int ancho			= 11;
	public int alto				= 11;
	public float probColor		= 0.1f;				//Probabilidad (sobre 1) para que una casilla se inicie con un color diferente al base
	
	public float tiempoInicio	= 5.0f;				//Tiempo en segundo spara que empiece a contar las condiciones de victoria y derrota
	
	//Variables publicas de referencia---------------------------------------------------------------------------------------------------------------
	//Prefabs
	public GameObject prefabCasilla;
	public GameObject prefabJugador;
	public GameObject prefabGota;
	public GameObject coloresIfaz;
	public GameObject blancosIfaz;
	
	//Colores (Materiales)
	public Material colorBase;
	public Material color1;
	public Material color2;
	public Material color3;
	public Material color4;
	public Material color5;
	public Material color6;
	public Material colorNegro;
	
	//Sonidos
	public AudioClip hatSample;
	public AudioClip hatComboSample;
	public AudioClip musicaSample;
	public AudioClip nota1Sample;
	public AudioClip nota2Sample;
	public AudioClip nota3Sample;
	public AudioClip nota4Sample;
	public AudioClip nota5Sample;
	public AudioClip nota6Sample;
	
	public AudioSource musicaPlayer;
	public AudioSource ritmoPlayer;
	public AudioSource sfxPlayer;
	
	//Variables privadas-----------------------------------------------------------------------------------------------------------------------------
	private Vector3 posInicial	= Vector3.zero;
	private controlCasilla color1Cont;
	private controlCasilla color2Cont;
	private controlCasilla color3Cont;
	private controlCasilla color4Cont;
	private controlCasilla color5Cont;
	private controlCasilla color6Cont;
	private controlCasilla colorBaseCont;
	private controlCasilla colorNegroCont;
	
	private float totalColores 		= 0;
//	private int totalNoBase 		= 0;
	private float tamañoInterfaz;
	
	private int dificultad			= 2;
	private bool showTutorial		= false;
	private int tutorialPhase		= 0;
	private TutorialScript tutScript;
	
	private float tiempoRitmo		= 0.6f;
	private float tiempoUltima		= 0.0f;
	private float ritmoSimpleCap	= 0.2f;
	private float ritmoComboCap		= 0.6f;
	private bool iniciaRitmo		= false;
	private bool ritmoComboPlaying	= false;
	
	private bool endGameShow		= false;
	private string endGameText		= "";
	/* DEBUG */
	public bool modoDebugGota		= false;
	
	//-----------------------------------------------------------------Funciones behavioural del script----------------------------------------------------------------------------
	
	void Start () {
		tutScript = this.gameObject.GetComponent<TutorialScript>();
		initControl();
		initAudio();
		Screen.showCursor = false;
		
		//Generacion del tablero
		GameObject tablero = GameObject.FindGameObjectWithTag("tablero");
		posInicial = tablero.transform.position;
		for (int i = 0; i < alto; i++) {
			for (int j = 0; j < ancho; j++) {
				GameObject casillaTemp;
				casillaTemp = Instantiate(prefabCasilla, posInicial + Vector3.right * j + Vector3.forward * i, prefabCasilla.transform.rotation) as GameObject;
				casillaTemp.name = "Casilla_" + i + "_" + j;
				casillaTemp.transform.parent = tablero.transform;
				colorBool colTemp = colorAleatorio(probColor);
				casillaTemp.GetComponent<scriptCasilla>().color = colTemp;
				Material matTemp = colBoolToMat(colTemp);
				casillaTemp.renderer.material = matTemp;
				controlCasilla controlTemp = matToControl(matTemp);
				controlTemp.agregar(casillaTemp);
				casillaTemp.GetComponent<scriptCasilla>().control = controlTemp;
			}
		}
	}
	
	
	void Update () {
		totalColores = color1Cont.numero + color2Cont.numero + color3Cont.numero + color4Cont.numero + color5Cont.numero + color6Cont.numero;
//		totalNoBase = ancho * alto - colorBaseCont.numero;
		
		tamañoInterfaz = totalColores / (ancho * alto);
		coloresIfaz.transform.localScale = new Vector3(tamañoInterfaz, tamañoInterfaz, tamañoInterfaz);
		blancosIfaz.transform.localScale = new Vector3(1.0f  - tamañoInterfaz, 1.0f  - tamañoInterfaz, 1.0f  - tamañoInterfaz);
		
		if (Time.time >= tiempoInicio) {
			if (condicionDerrota()) {
				Debug.Log("Se ha cumplido la condicion de derrota.");
				endGameShow = true;
				endGameText = "Game Over";
				SendMessage ("fadeOut");
				if(Input.anyKey){
					Application.LoadLevel("MainMenu");
				}
			}
			else if (condicionVictoria()) {
				Debug.Log("Se ha cumplido la condicion de victoria!!");
				endGameShow = true;
				endGameText = "Victory!";
				if(Input.anyKey){
					SendMessage ("fadeAndLoad", "MainMenu");
				}
			}
		}		
		controlRitmo();		//Control del tiempo y el sonido. Inicializa "iniciaRitmo" en intervalos adecuados
		
		/* DEBUG LANZAR GOTA */
		if(Input.GetKeyDown(KeyCode.X))
		{
			modoDebugGota = !modoDebugGota;		
		}
		if(modoDebugGota)
		{
			if(Input.GetButtonDown("Jump"))
				lanzarGota();	
		}
		else if (!showTutorial)
			controlGotas();		//Control de lanzamiento de gotas
		
		if (showTutorial)
			controlTutorial();
	}
	
	void OnGUI() {
		GUI.skin.label.fontSize = 10;
		GUI.skin.label.normal.textColor = Color.black;
		if (modoDebugGota) {
			GUI.Label(new Rect(10, 10, 300, 20), "Num. casillas otro color: " + totalColores);
			GUI.Label(new Rect(10, 35, 300, 20), "Num. casillas base: " + colorBaseCont.numero);
		}
		GUI.skin.label.fontSize = 48;
		GUI.skin.label.normal.textColor = Color.black;
		if (endGameShow) {
			GUI.Label(new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 50, 300, 100), endGameText);
		}
	}
	
	
	
	//-----------------------------------------------------------------Inicio de las funciones del script--------------------------------------------------------------------------
	
	private void initControl() {
		color1Cont = new controlCasilla(ancho*alto);
		color2Cont = new controlCasilla(ancho*alto);
		color3Cont = new controlCasilla(ancho*alto);
		color4Cont = new controlCasilla(ancho*alto);
		color5Cont = new controlCasilla(ancho*alto);
		color6Cont = new controlCasilla(ancho*alto);
		colorBaseCont = new controlCasilla(ancho*alto);
		colorNegroCont = new controlCasilla(ancho*alto);
		if (PlayerPrefs.HasKey("dif")) {
			dificultad = PlayerPrefs.GetInt("dif");
			Debug.Log("Dificultad encontrada en PlayerPrefs == " + dificultad + ".");
		}
		else {
			dificultad = 2;
			Debug.Log("Dificultad no encontrada en PlayerPrefs.");
		}
		if (dificultad == 0) {
			if (tutScript != null) {
				showTutorial = true;
				tiempoInicio = 2000f;
			}
			else {
				Debug.Log("El script de tutorial esta vacio.");
			}
		}
	}
	
	private void initAudio() {
		if (PlayerPrefs.HasKey("vol")) {
			musicaPlayer.volume = PlayerPrefs.GetFloat("vol") * 0.2f;
			ritmoPlayer.volume = PlayerPrefs.GetFloat("vol") * 0.2f;
			sfxPlayer.volume = PlayerPrefs.GetFloat("vol") * 0.6f;
		}
		else {
			musicaPlayer.volume = 0.2f;
			ritmoPlayer.volume = 0.2f;
			sfxPlayer.volume = 0.6f;
		}
		musicaPlayer.Play();
		tiempoRitmo = hatSample.length;
	}
	
	private void controlRitmo() {
		/*
		Control del tiempo y el sonido
		Cada "tiempoRitmo", lanzar un hat o hatCombo si procede, pero no de uno en uno sino con autoplay.
		Si se quiere bajar de hatCombo a hat solamente, se desactiva el autoplay y cuando este en .isPlaying a false, se pone el otro.
		*/
		float tempMod = Time.time % tiempoRitmo;
		iniciaRitmo = tempMod < 0.02f || tempMod > (tiempoRitmo - 0.02f);
//		iniciaRitmo = (!ritmoPlayer.isPlaying || (ritmoPlayer.isPlaying && ((ritmoPlayer.clip.length - ritmoPlayer.time) < 0.05f || (ritmoPlayer.time) < 0.05f)));
		float condDerrotaF = condicionDerrotaFloat();
//		Debug.Log("CondDerrotaF: " + condDerrotaF);
		
		//Subir de sin ritmo a ritmo simple
		if (condDerrotaF > ritmoSimpleCap && !ritmoPlayer.isPlaying && iniciaRitmo && !ritmoComboPlaying) {
			ritmoPlayer.loop = true;
			ritmoPlayer.clip = hatSample;
			ritmoPlayer.Play();
			ritmoComboPlaying = false;
		}
		
		//Subir de ritmo simple a rimto rapido
		if (condDerrotaF > ritmoComboCap && ritmoPlayer.isPlaying && iniciaRitmo && !ritmoComboPlaying) {
			ritmoPlayer.clip = hatComboSample;
			ritmoPlayer.Play();
			ritmoComboPlaying = true;
		}
		
		//Bajar de ritmo rapido a ritmo simple
		if (ritmoComboPlaying && condDerrotaF < ritmoComboCap && iniciaRitmo) {
			ritmoPlayer.loop = false;
			ritmoComboPlaying = false;
		}
		
		//Quitar ritmo
		if (ritmoPlayer.isPlaying && condDerrotaF < ritmoSimpleCap && iniciaRitmo) {
			ritmoPlayer.Stop();
			ritmoComboPlaying = false;
		}
	}
	
	private void controlGotas() {
	/*
	Las gotas caen en casillas de color base en principio (se puede hacer que esto varie con la dificultad).
	Cuando una gota cae, eliminar la casilla del array de control y moverla al que corresponda.
	
	Las gotas tambien se lanzan en intervalos de tiempo controlados, para que el sonido salga sincronizado con la musica. Al menos, dentro de unos limites razonables.
	Para ello, la animacion de la gota cayendo debe durar la mitad de "tiempoRitmo" y el sonido sonar cuando toque el tablero.
	*/
		if (iniciaRitmo && (Time.time - tiempoUltima) > tiempoRitmo) {
			float probGotas = 0.0f;
			switch (dificultad) {
				case 0:
					probGotas = 0.02f;
					break;
				case 1:
					probGotas = 0.03f;
					break;
				case 2: 
					probGotas = 0.08f;
					break;
				case 3:
					probGotas = 0.2f;
					break;
				default:
					Debug.LogError("La dificultad seleccionada es erronea. Dificultad: " + dificultad + ".");
					break;
			}
			if (Random.Range(0.0f, 1.0f) < probGotas) {
				if (colorBaseCont.numero == 0)
					return;
				tiempoUltima = Time.time;
				int numCas = Random.Range(0, colorBaseCont.numero - 1);
				GameObject casillaTemp = colorBaseCont.array[numCas];
				GameObject gotaTemp;
				gotaTemp = Instantiate(prefabGota, casillaTemp.transform.position, casillaTemp.transform.rotation) as GameObject;
//				colorBaseCont.quitar(numCas);
				scriptGota gotaScriptTemp = gotaTemp.GetComponentInChildren<scriptGota>();
				gotaScriptTemp.colorGota = colorAleatorio();
				gotaScriptTemp.casilla = casillaTemp;
				gotaScriptTemp.scriptPadre = this as LogicaControl;
				gotaScriptTemp.numCasilla = numCas;
				gotaScriptTemp.control = colorBaseCont;
				gotaTemp.GetComponentInChildren<Renderer>().material.SetColor("_Emission", colBoolToMat(gotaScriptTemp.colorGota).GetColor("_SpecColor"));
			}
		}
	}
	
	/* DEBUG LANZAR GOTA */
	private void lanzarGota() 
	{		
		int numCas = Random.Range(0, colorBaseCont.numero - 1);
		GameObject casillaTemp = colorBaseCont.array[numCas];
		GameObject gotaTemp;
		gotaTemp = Instantiate(prefabGota, casillaTemp.transform.position, casillaTemp.transform.rotation) as GameObject;
		scriptGota gotaScriptTemp = gotaTemp.GetComponentInChildren<scriptGota>();
		gotaScriptTemp.colorGota = colorAleatorio();
		gotaScriptTemp.casilla = casillaTemp;
		gotaScriptTemp.scriptPadre = this as LogicaControl;
		gotaScriptTemp.numCasilla = numCas;
		gotaScriptTemp.control = colorBaseCont;
		gotaTemp.GetComponentInChildren<Renderer>().material.SetColor("_Emission", colBoolToMat(gotaScriptTemp.colorGota).GetColor("_SpecColor"));		
	}
	/* -----------------------*/
	
	public void cambiaColorGota(colorBool col, GameObject cas, controlCasilla cont, int numCas) {
		Material matTemp = colBoolToMat(col);
		controlCasilla controlTemp = matToControl(matTemp);
		cont.quitar(numCas);
		controlTemp.agregar(cas);
		cas.renderer.material = matTemp;
		scriptCasilla casilla = cas.GetComponent<scriptCasilla>();
		casilla.color = col;
		casilla.control = controlTemp;
		sfxPlayer.clip = colBoolToSFX(col);
		sfxPlayer.Play();
	}
	
	public void cambiaColorJug(colorBool col, GameObject cas, controlCasilla cont, int numCas) {
		Material matTemp = colBoolToMat(col);
		controlCasilla controlTemp = matToControl(matTemp);
		cont.quitar(numCas);
		controlTemp.agregar(cas);
		cas.renderer.material = matTemp;
		scriptCasilla casilla = cas.GetComponent<scriptCasilla>();
		casilla.control = controlTemp;
	}
	
	private colorBool colorAleatorio(float prob) {
		colorBool colorSalida = new colorBool();
		colorSalida.r = false;
		colorSalida.g = false;
		colorSalida.b = false;
		if (Random.Range(0.0f, 1.0f) < prob) {
			int t = Random.Range(0,6);
			switch (t) {
				case 0:
					colorSalida.r = true;
					break;
				case 1:
					colorSalida.g = true;
					break;
				case 2:
					colorSalida.b = true;
					break;
				case 3:
					colorSalida.r = true;
					colorSalida.g = true;
					break;
				case 4:
					colorSalida.r = true;
					colorSalida.b = true;
					break;
				case 5:
					colorSalida.g = true;
					colorSalida.b = true;
					break;
				default:
					Debug.LogError("Algo ha ocurrido durante la eleccion aleatoria de color. Valor de la tirada = " + t + ".");
					break;
			}
		}
		return colorSalida;
	}
	
	private colorBool colorAleatorio() {
		colorBool colorSalida = new colorBool();
		colorSalida.r = false;
		colorSalida.g = false;
		colorSalida.b = false;
		int t = Random.Range(0, 6);
		switch (t) {
			case 0:
				colorSalida.r = true;
				break;
			case 1:
				colorSalida.g = true;
				break;
			case 2:
				colorSalida.b = true;
				break;
			case 3:
				colorSalida.r = true;
				colorSalida.g = true;
				break;
			case 4:
				colorSalida.r = true;
				colorSalida.b = true;
				break;
			case 5:
				colorSalida.g = true;
				colorSalida.b = true;
				break;
			default:
				Debug.LogError("Algo ha ocurrido durante la eleccion aleatoria de color. Valor de la tirada = " + t + ".");
				break;
		}
		return colorSalida;
	}
	
	public Material colBoolToMat(colorBool col) {
		Material matSalida = colorBase;
		if (col.r && !col.g && !col.b)			//Solo R
			matSalida = color1;
		else if (!col.r && col.g && !col.b)		//Solo G
			matSalida = color2;
		else if (!col.r && !col.g && col.b)		//Solo B
			matSalida = color3;
		else if (col.r && col.g && !col.b)		//Dos, R y G
			matSalida = color4;
		else if (col.r && !col.g && col.b)		//Dos, R y B
			matSalida = color5;
		else if (!col.r && col.g && col.b)		//Dos, G y B
			matSalida = color6;
			
		return matSalida;
	}
	
	private AudioClip colBoolToSFX(colorBool col) {
		AudioClip sfxSalida = nota1Sample;
		if (col.r && !col.g && !col.b)			//Solo R
			sfxSalida = nota1Sample;
		else if (!col.r && col.g && !col.b)		//Solo G
			sfxSalida = nota2Sample;
		else if (!col.r && !col.g && col.b)		//Solo B
			sfxSalida = nota3Sample;
		else if (col.r && col.g && !col.b)		//Dos, R y G
			sfxSalida = nota4Sample;
		else if (col.r && !col.g && col.b)		//Dos, R y B
			sfxSalida = nota5Sample;
		else if (!col.r && col.g && col.b)		//Dos, G y B
			sfxSalida = nota6Sample;
			
		return sfxSalida;
	}
	
	public controlCasilla matToControl(Material mat) {
		if (mat.Equals(color1))
			return color1Cont;
		else if (mat.Equals(color2))
			return color2Cont;
		else if (mat.Equals(color3))
			return color3Cont;
		else if (mat.Equals(color4))
			return color4Cont;
		else if (mat.Equals(color5))
			return color5Cont;
		else if (mat.Equals(color6))
			return color6Cont;
		else if (mat.Equals(colorNegro))
			return colorNegroCont;
		return colorBaseCont;
	}
	
	private bool condicionDerrota() {
		
		//Condicion: mas de color que base
		if (totalColores > colorBaseCont.numero)
			return true;
		/* Espacio para otras posibles condiciones
		
		//Si el rojo supera el 50% de ocupacion
		if (((float)color1Cont.numero) > (ancho * alto) * 0.5f)
			return true;		
		*/
		
		return false;
	}
	
	private float condicionDerrotaFloat() {
		
		//Condicion: mas de color que base --> if (totalColores > colorBaseCont.numero)
		return Mathf.InverseLerp(0.0f, (float)colorBaseCont.numero, totalColores);
		/* Espacio para otras posibles condiciones */
		
		
//		return 0.0f;
	}
	
	private bool condicionVictoria() {
	
		//Condicion: todo del color base
		if (totalColores == 0)
			return true;
//		if (colorBaseCont.numero == ancho * alto)
//			return true;
		/* Espacio para otras posibles condiciones
		
		//Condicion: sobrevivir sin perder tiempoSupervivencia segundos
		if (Time.time > tiempoSupervivencia)
			return true;		
		*/
		
		return false;
	}
	
	private void controlTutorial() {
		Debug.Log("Entering controlTutorial function.");
		if (Time.time > 10f && tutorialPhase == 0) {
			tutScript.launchTutorial(0, 15f);
			tutorialPhase = 1;
		}
		if (Time.time > 15f && tutorialPhase == 1) {
			tutScript.launchTutorial(1, 15f);
			tutorialPhase = 2;
		}
		if (Time.time > 20f && tutorialPhase == 2) {
			tutScript.launchTutorial(2, 15f);
			tutorialPhase = 2;
			tiempoInicio = Time.time + 10f;
			showTutorial = false;
		}
	}
}
