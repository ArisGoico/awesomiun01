using UnityEngine;
using System.Collections;

public class scriptGota : MonoBehaviour {

	public colorBool colorGota;
	
	
	public void tocarCasilla() {
		Debug.Log("Tocada la casilla y teñida de " + colorGota + ".");	
	}
	
	public void morir() {
		GameObject.Destroy(this.transform.parent.gameObject);
	}
}
