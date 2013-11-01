using UnityEngine;

public class colorBool {
	public bool r;
	public bool g;
	public bool b;
}

public class controlCasilla {
	
	public controlCasilla(int num) {
		array = new GameObject[num];
		numero = 0;
	}
	
	public void agregar(GameObject temp) {
		if (numero < array.Length - 1) {
			array[numero] = temp;
			temp.GetComponent<scriptCasilla>().ordenControl = numero;
			numero = numero + 1;
		}
		else {
//			Debug.LogError("El array de control en el que se intenta isertar una nueva casilla esta ya lleno.");
		}
	}
	
	public void quitar(int pos) {
		if (numero > 1 && pos != numero - 1) {
			array[pos] = array[numero - 1];
			array[numero - 1] = null;
			numero = numero - 1;
		}
		else if (numero > 1 && pos == numero - 1) {
			array[numero - 1] = null;
			numero = numero - 1;
		}
		else if (numero == 1) {
			array[0] = null;
			numero = 0;
		}
		else {
//			Debug.LogError("Ha fallado la operacion para quitar una posicion del array de control. Numero de elementos = " + numero + ".");
		}
	}
	
	public GameObject[] array;
	public int numero;
}