using UnityEngine;
using System.Collections;

public class LogicaControl : MonoBehaviour {
	
	//Variables publicas a inicializar desde el editor
	public int ancho			= 10;
	public int alto				= 10;
	public float probColor		= 0.1f;				//Probabilidad (sobre 1) para que una casilla se inicie con un color diferente al base
	
	public float tiempoInicio	= 5.0f;				//Tiempo en segundo spara que empiece a contar las condiciones de victoria y derrota
	
	//Variables publicas de referencia
	public GameObject prefabCasilla;
	public GameObject prefabJugador;
	public GameObject prefabGota;
	
	public Material colorBase;
	public Material color1;
	public Material color2;
	public Material color3;
	public Material color4;
	public Material color5;
	public Material color6;
	public Material colorNegro;
	
	//Variables privadas
	private Vector3 posInicial	= Vector3.zero;
	private controlCasilla color1Cont;
	private controlCasilla color2Cont;
	private controlCasilla color3Cont;
	private controlCasilla color4Cont;
	private controlCasilla color5Cont;
	private controlCasilla color6Cont;
	private controlCasilla colorBaseCont;
	private controlCasilla colorNegroCont;
	
	private int totalColores 	= 0;
	private int totalNoBase 	= 0;
	
	
	//-----------------------------------------------------------------Funciones behavioural del script----------------------------------------------------------------------------
	
	void Start () {
		initControl();
		Screen.showCursor = false;
		
		//Generacion del tablero
		GameObject tablero = GameObject.FindGameObjectWithTag("tablero");
		posInicial = tablero.transform.position;
		for (int i = 0; i < alto; i++) {
			for (int j = 0; j < ancho; j++) {
				GameObject casillaTemp;
				//Quaternion.Euler(new Vector3(90, 0, 0))
				casillaTemp = Instantiate(prefabCasilla, posInicial + Vector3.right * j + Vector3.forward * i, prefabCasilla.transform.rotation) as GameObject;
				casillaTemp.name = "Casilla_" + i + "_" + j;
				casillaTemp.transform.parent = tablero.transform;
				colorBool colTemp = colorAleatorio(probColor);
				casillaTemp.GetComponent<scriptCasilla>().color = colTemp;
				Material matTemp = colBoolToMat(colTemp);
				casillaTemp.renderer.material = matTemp;
				(matToControl(matTemp)).agregar(casillaTemp);
			}
		}
	}
	
	
	void Update () {
		totalColores = color1Cont.numero + color2Cont.numero + color3Cont.numero + color4Cont.numero + color5Cont.numero + color6Cont.numero;
		totalNoBase = ancho * alto - colorBaseCont.numero;
		
		if (Time.time >= tiempoInicio) {
			if (condicionDerrota()) {
				Debug.Log("Se ha cumplido la condicion de derrota.");
			}
			else if (condicionVictoria()) {
				Debug.Log("Se ha cumplido la condicion de victoria!!");
			}
		}
	}
	
	void OnGUI() {
		GUI.Label(new Rect(10, 10, 300, 20), "Num. casillas otro color: " + totalColores);
		GUI.Label(new Rect(10, 35, 300, 20), "Num. casillas base: " + colorBaseCont.numero);
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
	}
	
	private colorBool colorAleatorio(float prob) {
		colorBool colorSalida;
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
	
	private Material colBoolToMat(colorBool col) {
		Material matSalida = colorBase;
		if (col.r && !col.g && !col.b)		//Solo R
			matSalida = color1;
		else if (!col.r && col.g && !col.b)		//Solo G
			matSalida = color2;
		else if (!col.r && !col.g && col.b)		//Solo B
			matSalida = color3;
		else if (col.r && col.g && !col.b)		//Dos, R y G
			matSalida = color4;
		else if (!col.r && col.g && col.b)		//Dos, R y B
			matSalida = color5;
		else if (!col.r && col.g && col.b)		//Dos, G y B
			matSalida = color6;
			
		return matSalida;
	}
	
	private controlCasilla matToControl(Material mat) {
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
	
	private bool condicionVictoria() {
	
		//Condicion: todo del color base
		if (colorBaseCont.numero == ancho * alto)
			return true;
		/* Espacio para otras posibles condiciones
		
		//Condicion: sobrevivir sin perder tiempoSupervivencia segundos
		if (Time.time > tiempoSupervivencia)
			return true;		
		*/
		
		return false;
	}
}
