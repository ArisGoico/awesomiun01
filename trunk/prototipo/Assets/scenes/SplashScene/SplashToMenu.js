public var logoWait = 3;
public var fadeOutTexture: Texture2D;
public var fadeSpeed = 2.0;
public var fadeTime = 2.0;
public var alphaWait : boolean = true;

private var alpha = 1.0;
private var fadeDir = -1;

function Start () {
	alpha=1;
	fadeIn();
	yield WaitForSeconds (logoWait);
	fadeOut();
	yield WaitForSeconds(fadeTime);
	Application.LoadLevel ("MainMenu");
}

function Update () {
	if (Input.anyKeyDown)
		//fadeOut();
		Application.LoadLevel ("MainMenu");
}

function OnGUI(){
	if(alphaWait == false) {
	alpha += fadeDir * fadeSpeed * Time.deltaTime;
	}
	alpha = Mathf.Clamp01(alpha);  
	
	GUI.color.a = alpha;
	GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
}

function fadeIn(){
    yield WaitForSeconds(fadeTime);
    alphaWait = false;
    fadeDir = -1;  
}
  
function fadeOut(){
    fadeDir = 1; 
}