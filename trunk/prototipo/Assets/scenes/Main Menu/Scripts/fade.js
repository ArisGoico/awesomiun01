public var fadeOutTexture: Texture2D;
public var fadeSpeed = 2.0;
public var fadeTime = 2.0;
public var alphaWait : boolean = true;

private var alpha = 1.0;
private var fadeDir = -1;
private var cargar:boolean = false;
private var cargarEscena:String = "";

function Start () {
	alpha=1;
	fadeIn();
}

function Update () {

}

function OnGUI(){
	if(alphaWait == false) {
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
	}
	alpha = Mathf.Clamp01(alpha);  
	
	GUI.color.a = alpha;
	GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	if (cargar && !cargarEscena.Equals("")) {
		Application.LoadLevel(cargarEscena);
	}
}

function fadeIn(){
    yield WaitForSeconds(fadeTime);
    alphaWait = false;
    fadeDir = -1;  
}
  
function fadeOut(){
    fadeDir = 1;
}

function fadeAndLoad(escena:String) {
	fadeOut();
	yield WaitForSeconds(fadeTime);
	cargarEscena = escena;
	cargar = true;
}