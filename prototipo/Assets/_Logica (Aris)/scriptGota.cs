using UnityEngine;
using System.Collections;

public class scriptGota : MonoBehaviour {

	public colorBool colorGota;
	public GameObject casilla;
	public controlCasilla control;
	public int numCasilla;
	public LogicaControl scriptPadre;
	public GameObject splash;
	private GameObject splashInstance;
	
	
	public void tocarCasilla() {
		if (scriptPadre == null)
			scriptPadre = GameObject.FindGameObjectWithTag("script").GetComponent<LogicaControl>();
		scriptPadre.cambiaColorGota(colorGota, casilla, control, numCasilla);
		splashInstance = (GameObject)Instantiate(splash, transform.position, splash.transform.rotation);
		splashInstance.particleSystem.renderer.material = transform.renderer.material;
		
//		Debug.Log("Tocada la casilla y teñida de " + colorGota + ".");	
	}
	
	public void morir() {
		GameObject.Destroy(splashInstance, 2.5f);
		GameObject.Destroy(this.transform.parent.gameObject);
		
	}
}
