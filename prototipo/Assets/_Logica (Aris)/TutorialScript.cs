using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour {
	
	public GameObject player;
	
	public Texture2D splashImage;
	private Rect splashPos;
	
	public Texture2D[] tutorial;
	private Rect[] tutorialPos;
	
	private Texture2D tutorialSplash;
	private Rect tutorialSplashPos;
	
	public float splashTime			= 0.0f;
	private float splashTimeReal;
	private float splashTimeStart;
	
	private bool showSplash			= false;
	
	void Start () {
		float left;
		float up;
		
		if (splashImage != null) {
			left = Screen.width / 2.0f - splashImage.width / 2.0f;
			up = Screen.height / 2.0f - splashImage.height / 2.0f;
			splashPos = new Rect(left, up, splashImage.width, splashImage.height);
			showSplash = true;
			splashTimeReal = Time.time + splashTime;
			tutorialSplash = splashImage;
			tutorialSplashPos = splashPos;
			turnPauseOn();
		}
		
		if (tutorial.Length > 0) {
			tutorialPos = new Rect[tutorial.Length];
			for (int i = 0; i < tutorial.Length; i++) {
				left = Screen.width / 2.0f - tutorial[i].width / 2.0f;
				up = Screen.height / 2.0f - tutorial[i].height / 2.0f;
				tutorialPos[i] = new Rect(left, up, tutorial[i].width, tutorial[i].height);
			}
		}
		
	}
	
	void Update () {
		if (Time.realtimeSinceStartup >= splashTimeReal) {
			showSplash = false;
			turnPauseOff();
		}
		if (showSplash && Input.anyKey && (Time.realtimeSinceStartup - splashTimeStart > 2f)) {
			splashTimeReal = Time.realtimeSinceStartup;
		}
	}
	
	void OnGUI() {
		if (showSplash) {
			splashGUI();
		}
	}
	
	private void splashGUI() {
		GUI.Label(tutorialSplashPos, tutorialSplash);
	}
	
	private void turnPauseOn() {
		Time.timeScale = 0.0f;
		player.GetComponent<ControladorJugador>().enabled = false;
		BloomAndLensFlares script = Camera.main.GetComponent<BloomAndLensFlares>();
		script.bloomThreshhold = 0.0f;
	}
	
	private void turnPauseOff() {
		player.GetComponent<ControladorJugador>().enabled = true;
		BloomAndLensFlares script = Camera.main.GetComponent<BloomAndLensFlares>();
		script.bloomThreshhold = 0.3f;
		Time.timeScale = 1.0f;
	}
	
	public void launchTutorial(int tutScreen, float time = 10.0f) {
		Debug.Log("Launching tutorial: " + tutScreen + ".");
		if (tutScreen < tutorial.Length && tutScreen >= 0) {
			tutorialSplash = tutorial[tutScreen];
			tutorialSplashPos = tutorialPos[tutScreen];
			splashTimeStart = Time.realtimeSinceStartup;
			splashTimeReal = Time.realtimeSinceStartup + time;
			showSplash = true;
			turnPauseOn();
		}
	}
}
