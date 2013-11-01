using UnityEngine;
using System.Collections;

public class scriptGota : MonoBehaviour {

	public colorBool colorGota;
	public GameObject casilla;
	public controlCasilla control;
	public int numCasilla;
	public LogicaControl scriptPadre;
	
	
	public void tocarCasilla() {
		if (scriptPadre == null)
			scriptPadre = GameObject.FindGameObjectWithTag("script").GetComponent<LogicaControl>();
		scriptPadre.cambiaColorGota(colorGota, casilla, control, numCasilla);
//		Debug.Log("Tocada la casilla y teñida de " + colorGota + ".");	
	}
	
	public void morir() {
		GameObject.Destroy(this.transform.parent.gameObject);
	}
}
