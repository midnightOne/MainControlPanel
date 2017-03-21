using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;
using DG.Tweening;
using UnityEngine.UI;




public class mainProgram : MonoBehaviour {

	const int STATE_TECH_OFF = -1;
	const int STATE_COUNTDOWN = 0;
	const int STATE_LOWBATTERY = 1;
	const int STATE_NOBATTERY = 2;
	const int STATE_UNLOCK_SCREEN = 3;
	const int STATE_SATELLITE_DISCONNECTED_SCREEN = 4;
	const int STATE_SATELLITE_LOCKED = 5;
	const int STATE_HACKING = 6;
	const int STATE_HACKED = 7;

	public bool standaloneDebug = false;

	int testCountdown = 200;
	int state = STATE_TECH_OFF;

	public bool batteryCharged = false;
	public bool batteryInstalled = true;
	public bool doorOpen = false;
	public bool codeEnteredCorrect = false;
	public bool usbDriveConnected = false;
	public bool satSerialOk = false;
	//public bool codeEnteredCorrect = false;
	//public bool codeEnteredCorrect = false;
	public Material satelliteRed;

	public GameObject textureChangeObject;
	public GameObject unlockPanel;
	public GameObject messageBox;
	public GameObject inputBox;
	public GameObject progressBar;
	public GameObject hackWindow;

	public PotRotator[] potRotators; 

	public CircularSineWave wave_1;
	public CircularSineWave wave_2;


	private InputBoxController inputController;
	private MessageBoxController msgController;
	private GameObject black;
	private GameObject satellite;
	private GameObject lowBatteryIcon;
	private ProgressBarController progressBarController;
	private timer timerController;
	private float timeStamp;

	//private bool[] unlockButtonStates = {false, false, false, false, false, false};
	//private bool[] lightLit = {false, false, false, false, false, false, false, false, false, false};

	private string satSerial = "328AM5J8A";
	private UnlockObject unlockObject; 
	private GameObject radar;
	private SatSearchBarController radarController;

	private float joystickX = 0f;
	private float joystickY = 0f;

	private float satelliteInitialScale = 67.97f;
	private bool satelliteEasyFind = true;

	private int encoderPosition = 0;

	public int[] pots= {0,0,0,0,0};
	public const int samples = 20;
	private int currentReadingIndex = 0;
	private int[,] potReadings;

	private string buttonsData = "0000000000000000000000000000000000000000";
	private bool buttonDataInitialized = false;

	public WindowGrid windowGrid;
	public Archivecontroller archieve;

	private GameObject[] regions;

	public Text regionLabel;
	public Text iqLabel;
	public Text ageLabel;

	private string[] regionLabels;
	private string[] iqLabels;
	private string[] ageLabels;

	public int regionSelect = 0;
	public int iqSelect = 0;
	public int ageSelect = 0;
	
	bool displayDemographics = false;

	private GameObject BSM;
	private GameObject AZ;
	private GameObject Y;
	private GameObject X;
	private GameObject PK;
	private GameObject GSM;
	private GameObject LTE;
	private GameObject Dish;
	private GameObject Antenna;
	private GameObject Wifi;
	private GameObject Sync;

	private GameObject black_2;

	bool SyncEnabled = false;
	bool syncX = false;
	bool syncY = false;
	bool PK_Enabled = false;
	bool BSM_Enabled = false;
	bool AZ_Enabled = false;

	bool GSM_Enabled = false;
	bool LTE_Enabled = false;
	bool Wifi_Enabled = false;
	bool Antenna_Enabled = false;
	bool Dish_Enabled = false;
	bool Reserve_Enabled = false;

	bool commsDisable = false;
	bool firewall = false;

	private GameObject lowPowerWarning;
	private bool lowPowerWarningOn = true;

	private Text CoreTempText;
	private float coolingEfficiency = 0;
	private float temperature = 60f;

	//private int totalLoadPoints = 0;

	/**
	 *  FROM SERIAL CLASS
	 * */

	public static int charsNumber = 20;
	public static int transmittedCharsNumber = 3;


	public PowerManagement powerPanel;
	public static bool[] keyStates;
	static bool[] prevKeyStates;
	private SerialComm serialClass;

	public Camera mainCamera;



	private Process myProcess;
	// Use this for initialization
	void Start () {
		print ("Target framerate = " + Application.targetFrameRate);
		/*#if UNITY_EDITOR
		QualitySettings.vSyncCount = 0;  // VSync must be disabled
		Application.targetFrameRate = 30;
		#endif*/

		CircularSineWaveVis.enabled = false;
		graphDisplay.enabled = false;

		initSerial ();

		state = STATE_COUNTDOWN;

		black = GameObject.Find("Black");
		inputBox = GameObject.Find ("InputWindow");
		satellite = GameObject.Find ("Satellite");
		radar = GameObject.Find ("Radar");
		lowBatteryIcon = GameObject.Find ("LowBatteryIcon");

		//satelliteInitialScale = satellite.transform.localScale.x;

		potReadings = new int[5,samples];
		for (int i=0; i<5; i++) {
			for (int j=0; j<samples; j++) {
				potReadings[i,j] = 0;
			}
		}

		inputController = inputBox.GetComponent<InputBoxController> ();
		unlockObject = unlockPanel.GetComponent<UnlockObject> ();
		radarController = radar.GetComponent<SatSearchBarController> ();
		msgController = messageBox.GetComponent<MessageBoxController> ();
		progressBarController = progressBar.GetComponent<ProgressBarController> ();
		timerController = GameObject.Find ("timerText").GetComponent<timer> ();

		regions = new GameObject[] {
			GameObject.Find ("Australia"),
			GameObject.Find ("Africa"),
			GameObject.Find ("Eurasia"),
			GameObject.Find ("SouthAmerica"),
			GameObject.Find ("NorthAmerica")
		};

		BSM = GameObject.Find ("BSM");
		AZ = GameObject.Find ("AZ");
		Y = GameObject.Find ("Y");
		X = GameObject.Find ("X");
		PK = GameObject.Find ("PK");
		GSM = GameObject.Find ("GSM");
		LTE = GameObject.Find ("LTE");
		Dish = GameObject.Find ("Dish");
		Antenna = GameObject.Find ("Antenna");
		Wifi = GameObject.Find ("Wifi");
		Sync = GameObject.Find ("Синхронизация");

		lowPowerWarning = GameObject.Find ("OneCoreWarning");

		CoreTempText = GameObject.Find ("CoreTemp").GetComponent<Text> ();

		mainCamera.GetComponent<CameraFilterPack_FX_Glitch1> ().enabled = false;
		mainCamera.GetComponent<CameraFilterPack_TV_BrokenGlass> ().enabled = false;



		regionLabels = new string[] {"All/Все", "Australia\nАвстралия","Africa\nАфрика","Eurasia\nЕвразия","South America\nЮжная Америка","North America\nСеверная Америка"};
		iqLabels = new string[] {"All/Все","130+","115-130","100-115","85-100","0-86"};
		ageLabels = new string[] {"All/Все","60+","40-60","25-40","16-25","0-16"};

		black_2 = GameObject.Find ("Black_2");
		//unlockPanel.GetComponent<videoAnim> ().enabled = false;
		//unlockPanel.transform.Translate(0f, 500f,0f);

		lowBatteryIcon.SetActive(false);

		StartCoroutine("setPanelsInactive",1.6f);

	} 

	void requestButtonStates(){
		buttonsData = "0000000000000000000000000000000000000000";
		SerialComm.WriteToArduino("#B");

	}

	void initSerial(){
		keyStates = new bool[charsNumber];
		for(int i=0;i<charsNumber;i++){
			keyStates[i] = false;
		}
		serialClass = gameObject.GetComponent<SerialComm>();
	}




	//----------------------------------------------------------------------------------------------------
	//            UPDATE

	void calculateLoad(){
		int temp = 13;
		
		temp += SyncEnabled.GetHashCode()*(syncX.GetHashCode()+syncY.GetHashCode());
		temp += PK_Enabled.GetHashCode () + BSM_Enabled.GetHashCode() + AZ_Enabled.GetHashCode();
		temp += Reserve_Enabled.GetHashCode()*(GSM_Enabled.GetHashCode() + LTE_Enabled.GetHashCode() + Wifi_Enabled.GetHashCode() + Antenna_Enabled .GetHashCode() + Dish_Enabled.GetHashCode() );
		
		temp += (regionSelect == 0) ? 12 : 2;
		temp += (iqSelect == 0) ? 12 : 2;
		temp += (ageSelect == 0) ? 12 : 2;
		temp += firewall.GetHashCode ();
		
		GameObject.Find ("Loadtext").GetComponent<Text>().text = "" + temp;
		
		float load = temp / 30f;
		
		if (load > powerPanel.currentCapacity && state >= STATE_SATELLITE_LOCKED) {
			lowPowerWarning.SetActive (true);
		} else {
			lowPowerWarning.SetActive (false);
		}
		
		powerPanel.cores[0].loadLevel = Mathf.Min(load/powerPanel.cores[0].powerLevel,1f);
		load -= powerPanel.cores [0].powerLevel;
		powerPanel.cores[1].loadLevel = Mathf.Min(load/powerPanel.cores[1].powerLevel,1f);
		
		
		temperature += (load * powerPanel.currentCapacity);

		temperature = Mathf.Clamp(temperature,0f,powerPanel.currentCapacity*(1206*0.5f));

		/*if(powerPanel.cores[0].isOn && !powerPanel.cores[1].isOn){
			powerPanel.cores[0].loadLevel = Mathf.Min(temp/30f,1f);
		} else if(powerPanel.cores[0].isOn && powerPanel.cores[1].isOn){
			powerPanel.cores[0].loadLevel = powerPanel.cores[1].loadLevel = temp/60f;
		}

		OneCoreWarning.SetActive(!(powerPanel.cores[0].isOn && powerPanel.cores[1].isOn));*/
		
		
		//29+18=48 + 12 = 60;
		
		
	}

	void updatePotReadings(){
		float temp;
		for(int i=0;i<5;i++){
			temp=0;
			for(int j=0;j<samples;j++){
				temp += potReadings[i,j];
			}
			pots[i] = (int)(temp/samples);

		}
	}

	void updatePotVisuals(){

		float temp = (Mathf.RoundToInt (pots [4] / 20.0f) - 25) / 10f;

		wave_1.frequency = pots [0] * 0.002f;
		wave_1.amplitude = pots [1] * 0.02f;

		wave_2.frequency = pots [2] * 0.002f;
		wave_2.amplitude = pots [3] * 0.02f;

		wave_1.deltaT = temp;
		wave_2.deltaT = -temp;

		wave_2.delay = encoderPosition * 0.1f;
	}
	
	void updateSelection(){

		if (regionSelect == 0 && displayDemographics) {
			for(int i=0;i<regions.Length;i++){
				regions[i].SetActive(true);
			}
		} else {
			for(int i=0;i<regions.Length;i++){
				if(regionSelect-1 == i && displayDemographics){
					regions[i].SetActive(true);
				} else {
					regions[i].SetActive(false);
				}
			}
		}
		//
		regionLabel.text = regionLabels[regionSelect];
		iqLabel.text = iqLabels [iqSelect];
		ageLabel.text = ageLabels [ageSelect];
		//
	}

	bool overheatSent = false;
	void calculateCooling(){
		int temp = 0;
		for(int i = 0; i<4;i++ ){
			temp += pots[i];
		}
		coolingEfficiency = temp / 4000f;

		GameObject.Find("CoolingEfficiency").GetComponent<Text>().text = Math.Round(coolingEfficiency*100) + " %";

		float tempCap = 1206 - coolingEfficiency * 1206;

		temperature = Mathf.Clamp (temperature, 0f, tempCap);

		CoreTempText.text = Mathf.Round(temperature) + " °C";

		if(temperature >= 1201 && !overheatSent){
			SerialComm.WriteToArduino("#E");
			mainCamera.GetComponent<CameraFilterPack_FX_Glitch1> ().enabled = true;
			mainCamera.GetComponent<CameraFilterPack_TV_BrokenGlass> ().enabled = true;
			overheatSent = true;
		}
	}

	
	void Update () {
		UpdateSerial ();
		updatePotReadings ();
		updatePotVisuals ();
		updateSelection ();
		calculateLoad ();
		calculateCooling ();

		//test
		/*if(Input.GetKeyUp(KeyCode.R)){
			Application.LoadLevel(Application.loadedLevel);
		}*/


		//todo delete
		GameObject.Find ("Cube").transform.localEulerAngles = new Vector3(encoderPosition*360/44, 0, 0);

		if(state == STATE_COUNTDOWN){
			if(doorOpen){
				showLowBattery();
				state = STATE_LOWBATTERY;

			}

		} else if(state == STATE_LOWBATTERY){
			if(!batteryInstalled){
				fadeToBlack();
				state = STATE_NOBATTERY;
			}
		} else if(state == STATE_NOBATTERY){
			if(batteryInstalled){
				fadeFromBlack();
				if(!batteryCharged){
					state = STATE_LOWBATTERY;
				} else {
					state = STATE_UNLOCK_SCREEN;
					unlockPanel.SetActive(true);

					//unlockPanel.transform.Translate(0f, -500f,0f);
					//unlockPanel.GetComponent<videoAnim> ().enabled = true;
					unlockPanel.GetComponent<DOTweenAnimation>().DOPlayBackwards();
					//unlockPanel.GetComponent<videoAnim>().start();
					StartCoroutine("PlayUnlockInitAnimation",1f);

					lowBatteryIcon.SetActive(false);
					//GameObject.Find ("LowBatteryIcon").GetComponent<DOTweenAnimation> ().DOPlayBackwards ();
				}
			}
		} else if (state == STATE_UNLOCK_SCREEN){
			unlockObject.updateButtons (keyStates);
			if(!standaloneDebug){
				codeEnteredCorrect = unlockObject.checkComplete ();
			}
			if(codeEnteredCorrect){
				//unlockPanel.SetActive(true);
				SerialComm.WriteToArduino("#C"); //CODE CORRECT COMMAND

				unlockPanel.GetComponent<DOTweenAnimation>().DOPlayForward();
				StartCoroutine("DisableUnlockPanel",1f);
				state = STATE_SATELLITE_DISCONNECTED_SCREEN;
				//StartCoroutine("delayedShowMessageBox",1f);
				DOTween.PlayBackwards("SatelliteSearchTextFade");

				StartCoroutine("delayedShowRadar",2f);
				//satellite.transform.localScale = new Vector3(0,0,0);
				satellite.GetComponent<DOTweenAnimation>().DOPlayBackwards();


			}
		} else if(state == STATE_SATELLITE_DISCONNECTED_SCREEN){
			/*if( (joystickX != 0 || joystickY != 0) && msgController.on){
				hideMessageBox();
			}*/
			if(standaloneDebug){

				joystickX = Input.GetKey(KeyCode.RightArrow).GetHashCode() + Input.GetKey(KeyCode.LeftArrow).GetHashCode()*-1;
				joystickY = Input.GetKey(KeyCode.UpArrow).GetHashCode() + Input.GetKey(KeyCode.DownArrow).GetHashCode()*-1;

			}

			radarController.velocity.x = joystickX*10;//+= (joystickX*10-radarController.velocity.x)*0.9f;
			radarController.velocity.y = joystickY*10;
			//radarController.velocity.Normalize();
			//radarController.velocity *= 10;

			/*if(radarController.velocity.y < 10 | radarController.velocity.y > -10)
				radarController.velocity.y += joystickY;

			if(radarController.velocity.x < 10 | radarController.velocity.x > -10)
					radarController.velocity.x += joystickX;*/

			/*
			if(Input.GetKey("up")){
				if(radarController.velocity.y < 10)
					radarController.velocity.y += 1;
			} else if(Input.GetKey("down")) {
				if(radarController.velocity.y > -10)
					radarController.velocity.y -= 1;
			} else {
				radarController.velocity.y = 0;
			}

			if(Input.GetKey("right")){
				if(radarController.velocity.x < 10)
					radarController.velocity.x += 1;
			} else if(Input.GetKey("left")) {
				if(radarController.velocity.x > -10)
					radarController.velocity.x -= 1;
			} else {
				radarController.velocity.x = 0;
			}*/

			if(satelliteEasyFind){
				if(radarController.progress > 0.7f && radarController.progress < 0.95f){
					Vector3 tempVec = satellite.transform.localScale;
					tempVec.x = tempVec.y = tempVec.z = (radarController.progress - 0.7f)/0.3f*satelliteInitialScale;
					satellite.transform.localScale = tempVec;
					
				} else if(radarController.progress > 0.95f){
					state = STATE_SATELLITE_LOCKED;
					SerialComm.WriteToArduino("#S"); //SATELLITE LOCKED COMMAND
					radarController.locked = true;
					satellite.transform.localScale = new Vector3(satelliteInitialScale,satelliteInitialScale,satelliteInitialScale);
					//satellite.GetComponent<DOTweenAnimation>().DOPlayForward();
					//DOTween.PlayForward ("satelliteShow");
					DOTween.PlayForward("SatelliteSearchTextFade");
					if(!windowGrid.enabled){
						windowGrid.enabled = true;
					}
				} 
			} else {
				if(radarController.progress > 0.95f){
					state = STATE_SATELLITE_LOCKED;
					SerialComm.WriteToArduino("#S"); //SATELLITE LOCKED COMMAND
					radarController.locked = true;
					satellite.transform.localScale = new Vector3(satelliteInitialScale,satelliteInitialScale,satelliteInitialScale);
					//satellite.GetComponent<DOTweenAnimation>().DOPlayForward();
					DOTween.PlayForward("SatelliteSearchTextFade");
					//DOTween.PlayForward ("satelliteShow");
					if(!windowGrid.enabled){
						windowGrid.enabled = true;
					}
				} 
			}
		} else if(state == STATE_SATELLITE_LOCKED){
			//Vector3 temp = satellite.transform.position - radar.transform.position;
			//temp.z = 0;
			
			//print(satellite.transform.position + " - " + radar.transform.position + " = " + temp);

			if(usbDriveConnected){
				state = STATE_HACKING;
				hackWindow.SetActive(true);
				progressBar.SetActive(true);
				DOTween.PlayBackwards ("progressBarHide");
				DOTween.PlayBackwards ("hackWindowHide");
				progressBarController.setTitle("Hacking satellite...", "Взлом спутника...");

			}
		} else if(state == STATE_HACKING){
			if(progressBarController.progress < 0.32){

				progressBarController.progress += UnityEngine.Random.value*0.005f;

			} else if(progressBarController.progress < 1){
				if(!satSerialOk && !inputBox.activeSelf){
					inputBox.SetActive(true);
					inputBox.GetComponent<DOTweenAnimation>().DOPlayBackwards();
					inputController.createInputBox("Input satellite serial #");
					//StartCoroutine("delayedShowMessageBox",1f);
				}


				if(satSerialOk){
					progressBarController.progress += UnityEngine.Random.value*0.005f;
				} else {
					if(inputController.inputString.Length == satSerial.Length){
						if(inputController.inputString == satSerial){
							satSerialOk = true;
							//remove inputbox
							inputBox.GetComponent<DOTweenAnimation>().DOPlayForward();
							StartCoroutine("delayedDisableInputBox",1f);
						} else {
							//replace title with "try again" and clear input field
							inputController.changeTitle("Try again",true);
							inputController.clear();
						}
					}

				}


			} else {
				timerController.paused = true;

				//AudioController.Play("SatelliteRollback");
				foreach (Transform child in satellite.transform)
				{
					//child is your child transform
					child.gameObject.GetComponent<Renderer>().material = satelliteRed;
				}
				state = STATE_HACKED;
				StartCoroutine("delayedStartSatReboot");
				DOTween.PlayForward ("progressBarHide");
				DOTween.PlayForward ("hackWindowHide");
				progressBarController.progress = 0;
				GameObject.Find("HackingVideo").GetComponent<videoAnim>().pause();
				//TEST OVERHEAT FROM FLASH
				if(!overheatSent){
					SerialComm.WriteToArduino("#E");
					mainCamera.GetComponent<CameraFilterPack_FX_Glitch1> ().enabled = true;
					mainCamera.GetComponent<CameraFilterPack_TV_BrokenGlass> ().enabled = true;
					overheatSent = true;
				}
			}

		} else if(state == STATE_HACKED){
			progressBarController.progress = 1-timerController.timeLeft ()/timeStamp;
		}


		// SHOW PROGRESSBAR
		//DOTween.PlayBackwards ("progressBarHide");




		if (testCountdown > 0) {
			testCountdown--;
		} else if (testCountdown == 0){
			//showLowBattery();
			testCountdown--;
		}
	}

	public void showMessageBox(string title, string text, float stayFor=2f){
		if (msgController.on) {
			StopCoroutine("delayedhideMessageBox");
			msgController.createMessageBox(title, text);
		} else {
			messageBox.SetActive(true);
			messageBox.GetComponent<DOTweenAnimation>().DOPlayBackwards();
			msgController.createMessageBox(title, text);
		}

		StartCoroutine("delayedhideMessageBox",stayFor);

	}

	IEnumerator delayedhideMessageBox(float delay)
	{
		yield return new WaitForSeconds(delay);  
		hideMessageBox ();
		yield break;

	}

	void hideMessageBox(){
		messageBox.GetComponent<DOTweenAnimation>().DOPlayForward();
		StartCoroutine("delayeddisableMessageBox",1f);
	}

	IEnumerator delayedStartSatReboot()
	{
		//Wait for the time defined at the delay parameter
		yield return new WaitForSeconds(10); 
		AudioController.Play("SatelliteRollback");
		yield return new WaitForSeconds(2);
		progressBarController.setTitle("Rolling back satellite...", "Восстановление спутника...");
		DOTween.PlayBackwards ("progressBarHide");
		timerController.paused = false;
		DOTween.Play("progressBarMove");
		timeStamp = timerController.timeLeft ();

		//Stop this coroutine
		yield break;
		//StopCoroutine("delayedShowMessageBox");
	}

	IEnumerator delayedShowMessageBox(float delay)
	{
		//Wait for the time defined at the delay parameter
		yield return new WaitForSeconds(delay);  
		
		messageBox.SetActive(true);
		messageBox.GetComponent<DOTweenAnimation>().DOPlayBackwards();
		msgController.createMessageBox("Satellite disconnected", "Please use the joystick to move radar. The radar will show proximity of satellite by the number of green dots.");

		
		//Stop this coroutine
		yield break;
		//StopCoroutine("delayedShowMessageBox");
	}

	IEnumerator delayeddisableMessageBox(float delay)
	{
		//Wait for the time defined at the delay parameter
		yield return new WaitForSeconds(delay);  
		
		messageBox.SetActive(false);
		msgController.on = false;
		//Stop this coroutine
		yield break;
		//StopCoroutine("delayedShowMessageBox");
	}

	IEnumerator delayedDisableInputBox(float delay)
	{
		//Wait for the time defined at the delay parameter
		yield return new WaitForSeconds(delay);  
		
		inputBox.SetActive(false);
		
		//Stop this coroutine
		yield break;
		//StopCoroutine("delayedShowMessageBox");
	}



	IEnumerator delayedShowRadar(float delay)
	{
		//Wait for the time defined at the delay parameter
		yield return new WaitForSeconds(delay);  
		
		DOTween.Play ("SatSearchBarShow");
		
		//Stop this coroutine
		yield break;
		//StopCoroutine("delayedShowRadar");
	}

	
	IEnumerator delayedCall(Action function, float delay)
	{ 
		yield return new WaitForSeconds(delay); 
		function ();
		yield break;
	}

	IEnumerator setPanelsInactive(float delay)
	{
		//Wait for the time defined at the delay parameter
		yield return new WaitForSeconds(delay);  
		
		unlockPanel.SetActive (false);
		messageBox.SetActive (false);
		hackWindow.SetActive (false);
		progressBar.SetActive (false);
		inputBox.SetActive (false);

		requestButtonStates ();

		//Stop this coroutine
		yield break;
		//StopCoroutine("setUnlockPanelInactive");
	}

	IEnumerator PlayUnlockInitAnimation(float delay)
	{
		//Wait for the time defined at the delay parameter
		yield return new WaitForSeconds(delay);  
		
		unlockPanel.GetComponent<videoAnim> ().start ();
		
		//Stop this coroutine
		yield break;
		//StopCoroutine("PlayUnlockInitAnimation");
	}

	IEnumerator DisableUnlockPanel(float delay)
	{
		//Wait for the time defined at the delay parameter
		yield return new WaitForSeconds(delay);  
		
		unlockPanel.SetActive (false);
		
		//Stop this coroutine
		yield break;
		//StopCoroutine("DisableUnlockPanel");
	}

	//----------------------------------------------------------------------------------------------------
	// 


	void animateForward(string objectName){
		DOTweenAnimation[] tweens = GameObject.Find(objectName).GetComponents<DOTweenAnimation>();

		foreach (DOTweenAnimation tween in tweens) {
			tween.DOPlayForward();
		}
	}

	void animateBack(string objectName){
		DOTweenAnimation[] tweens = GameObject.Find(objectName).GetComponents<DOTweenAnimation>();
		
		foreach (DOTweenAnimation tween in tweens) {
			tween.DOPlayBackwards();
		}
	}




	void fadeToBlack(){
		black.GetComponent<DOTweenAnimation> ().DOPlayBackwards ();
	}

	void fadeFromBlack(){
		black.GetComponent<DOTweenAnimation> ().DOPlayForward ();
		//print("FADE FROM BLACK -------------------");
	}

	void showLowBattery(){
		lowBatteryIcon.SetActive(true);
		foreach (DOTweenAnimation ease in GameObject.Find ("CanvasObj").GetComponents<DOTweenAnimation> ()) {
			ease.DOPlay();
		}
		GameObject.Find ("LowBatteryIcon").GetComponent<DOTweenAnimation> ().DOPlay ();
	}

	void startRainmeter(){

		myProcess = new Process();
		myProcess.StartInfo.FileName = System.Environment.ExpandEnvironmentVariables(@"%PROGRAMFILES%\Rainmeter\Rainmeter.exe");
		myProcess.Start();


	}

	void stopRainmeter(){
		foreach (Process p in Process.GetProcessesByName("Rainmeter")) {
			p.CloseMainWindow();
		}
	}             

	void UpdateSerial () {
		//return;
		//int data=0;
		for(int j=0;j<5;j++){

			string tempString = serialClass.ReadFromArduino ();
			//print (tempString);
			GameObject.Find("debugText").GetComponent<Text>().text = tempString;
			string[] stringArray;


			if (tempString != null) {
				if (tempString.StartsWith ("S")) { //Start quest
					timerController.paused = false;
					timerController.setStartTime();
				} else if (tempString.StartsWith ("k") && tempString.Length >= transmittedCharsNumber) { //key
					if (tempString.StartsWith ("k--")) {
						for (int i =0; i<keyStates.Length; i++) {
							keyStates [i] = false;
						}
						unlockObject.reset ();
					} else {
						//print (tempString);
						//data = (int)tempString[1] + 256*(int)tempString[2]; 
						tempString = hexToBin (tempString.Substring (1));
						//tempString = Convert.ToString(data, 2);
						//print (tempString);
						for (int i =0; i<charsNumber; i++) {
							
							if (tempString.Length > i) {
								keyStates [i] = (tempString [tempString.Length - 1 - i] == '1' ? true : false);
							} else {
								keyStates [i] = false;
							}
							
							
						}
					}
				} else if (tempString.StartsWith ("s")) { //sound
					
					//AudioController.Play(audioIDs[int.Parse(tempString.Substring(1), System.Globalization.NumberStyles.HexNumber)]);
				} else if (tempString.StartsWith ("m")) { //show slave monitor
					print ("--------------------- SLAVE ON");
					GameObject.Find ("Camera-Slices").GetComponent<Camera> ().enabled = true;
				} else if (tempString.StartsWith ("M")) { //show MAIN monitor
					print ("--------------------- SLAVE OFF");
					GameObject.Find ("Camera-Slices").GetComponent<Camera> ().enabled = false;
				} else if (tempString.StartsWith ("U")) { // USB connected
					if (tempString [1] == '1') { // FIRST USB
						usbDriveConnected = true;
					} else {
						
					}
				} else if (tempString.StartsWith ("u")) { // USB DISconnected
					if (tempString [1] == '1') { // FIRST USB
						usbDriveConnected = false;
					} else {
						
					}
					
					
				} else if (tempString.StartsWith ("o")) { // "OTHER" - ONE CHAR COMMANDS
					//for(int i=1;i<tempString.Length;i++){
					if (tempString [1] == 'D') { // Door opened

						doorOpen = true;
						GameObject.Find ("Camera-Slices").GetComponent<Camera> ().enabled = false;
					} else if (tempString [1] == 'C') { // Battery charged
						batteryCharged = true;
					} else if (tempString [1] == 'c') { // Battery DIScharged
						batteryCharged = false;
					} else if (tempString [1] == 'B') { // Battery installed
						batteryInstalled = true;
					} else if (tempString [1] == 'b') { // Battery UNinstalled
						batteryInstalled = false;
					}
					//}
					
				} else if (tempString.StartsWith ("J")) { // JOYSTICK UPDATE

					if(state >= STATE_SATELLITE_LOCKED){
						SerialComm.WriteToArduino("#S"); //SATELLITE LOCKED COMMAND
					}

					joystickX = (hexToDec (tempString.Substring (1, 1)) - 7) / 7f;
					joystickY = (hexToDec (tempString.Substring (2, 1)) - 7) / 7f;

					//joystickX = ((int)Char.GetNumericValue (tempString [1]) - 4)/4f;
					//joystickY = ((int)Char.GetNumericValue (tempString [2]) - 4)/4f;
				} else if (tempString.StartsWith ("E")) { // ENCODER UPDATE
					
					encoderPosition = int.Parse (tempString.Substring (1)); 

					//joystickX = (hexToDec(tempString.Substring(1,1)) - 7)/7f;
					//joystickY = (hexToDec(tempString.Substring(2,1)) - 7)/7f;
					
					//joystickX = ((int)Char.GetNumericValue (tempString [1]) - 4)/4f;
					//joystickY = ((int)Char.GetNumericValue (tempString [2]) - 4)/4f;
				} else if (tempString.StartsWith ("P")) {
					stringArray = tempString.Substring (1).Split ('P');
					
					if (stringArray.Length >= 5) {
						for (int i=0; i<5; i++) {
							potReadings [i, currentReadingIndex] = int.Parse (stringArray [i]);
							//pots[i] = int.Parse(stringArray[i]);
							if (potRotators [i] != null) {
								potRotators [i].value = potReadings [i, currentReadingIndex];
							}
						}
						currentReadingIndex = (currentReadingIndex + 1) % samples;
					}
				} else if (tempString.StartsWith (">")) {
					//tempString = tempString.Substring (1); 
					tempString = longHexToBin (tempString.Substring (1));
					
					while(tempString.Length < 40)
					{
						tempString += '0';
					}
					//print(tempString);
					/*while(tempString.Length < 64){
						tempString = "0" + tempString;
					}*/

					if (buttonsData.Length != tempString.Length) {
						buttonsData = tempString;
						//print(buttonsData.Length);
					} else {
						//print(tempString);
						for (int i=0; i<tempString.Length; i++) {
							if (buttonsData [i] != tempString [i] || !buttonDataInitialized) {
								buttonMatrixChanged (i, tempString [i]);
								print ("Different Index = " + i);
							}
						}
					}

					buttonsData = tempString;
					buttonDataInitialized = true;

				} else if (tempString.StartsWith ("R")) {
					regionSelect = int.Parse (tempString.Substring (1, 1));
					iqSelect = int.Parse (tempString.Substring (2, 1));
					ageSelect = int.Parse (tempString.Substring (3, 1));
				} else if (tempString.StartsWith ("F")) {
					if (tempString [1] == '1') {
						DOTween.PlayBackwards ("flash_hide");
						black_2.SetActive (false);
					} else {
						DOTween.PlayForward ("flash_hide");
						black_2.SetActive (true);
					}
				}

				if(tempString.StartsWith ("<")){
					Application.LoadLevel(Application.loadedLevel);
				}


				
				if (tempString.StartsWith ("f")) { // 6 PUZZLE FRAGMENTS
					print ("--------------------- " + tempString);
					
					
					textureChangeObject.GetComponent<TextureChange> ().changeTexture ((int)Char.GetNumericValue (tempString [1]) - 1, (int)Char.GetNumericValue (tempString [2]));
					
				}
			} else { //NOHING to read
				break;
			}
			
		}
		/*for(int i = 0;i<keyNumber;i++){

			if(keyStates[i] != prevKeyStates[i]){

			}

		}*/
	}

	void buttonMatrixChanged(int index, char value){
		bool b = value == '0';
		//windowGrid
		if (index == 4) { // охлаждение
			windowGrid.windowsEnabled [5] = b;
			CircularSineWaveVis.enabled = b;
			graphDisplay.enabled = b;
		} else if (index == 5) { // Спутник
			windowGrid.windowsEnabled [4] = b;
		} else if (index == 7) { // Спутник
			windowGrid.windowsEnabled [3] = b;
		} else if (index == 13) { // Спутник
			windowGrid.windowsEnabled [2] = b;
			archieve.isOn = b;
		} else if (index == 33 && b) {
			//print ("Unable to stop the enslavement of the world once it's started!");
			showMessageBox ("Error", "Unable to stop the enslavement of the world once it's started!  \n \n Невозможно остановить порабощение после его запуска!", 8f);
		} else if (index == 32 && !b) {
			//print ("Unable to stop the enslavement of the world once it's started!");
			showMessageBox ("WARNING!", "Hacking world's nuclear missiles, nuclear apocalypse in 60 minutes!  \n \n Взлом ядерных боеголовок, атомная война через 60 минут!", 8f);
		} else if (index == 24 && !b) {
			//print ("Unable to stop the enslavement of the world once it's started!");
			showMessageBox ("WARNING!", "You don't wanna know what that does, but you are dead anyways... \n \n Вам лучше не знать что вы только что запустили, но жить вам осталось не долго...", 8f);
		} else if (index == 11 && !b) {
			powerPanel.core_1_inc ();
		} else if (index == 10 && !b) {
			powerPanel.core_1_dec ();
		} else if (index == 19 && !b) {
			powerPanel.core_2_inc ();
		} else if (index == 18 && !b) {
			powerPanel.core_2_dec ();
		} else if (index == 9) {
			if (powerPanel.ionsPurged) {
				showMessageBox ("ERROR!", "Can not start CORE 1! No ions detected! \n \n Не возможно запустить ЯДРО 1 при сброшенных ионах!", 8f);

			} else {
				powerPanel.cores [0].isOn = !b;
			}
		} else if (index == 8) {
			if (powerPanel.ionBlock && !b) {
				showMessageBox ("ERROR!", "Can not start CORE 2! Ion block detected! \n \n Не возможно запустить ЯДРО 2! Обнаружен блок ионов!", 8f);

			} else if (powerPanel.ionsPurged) {
				showMessageBox ("ERROR!", "Can not start CORE 2! No ions detected! \n \n Не возможно запустить ЯДРО 2 при сброшенных ионах!", 8f);
				
			} else {
				powerPanel.cores [1].isOn = !b;
			}

		} else if (index == 3 && !b) {
			if (powerPanel.ionsPurged) {
				showMessageBox ("NOTE", "Need to refill ions first. \n \n Перед увеличением скорости, нужно заменить ионы.", 8f);
			} else {
				powerPanel.ionSpeed_inc ();
			}
		} else if (index == 2 && !b) {
			powerPanel.ionSpeed_dec ();
		} else if (index == 1 && !b) { //Сброс
			if (!powerPanel.ionBlock) {
				showMessageBox ("NOTE", "No Ion Errors detected, Ion purge not needed. \n \n Не обнаружены ошибки ионов, сброс не требуется.", 8f);
			} else {
				powerPanel.ionPurge ();
				//powerPanel.cores [0].isOn = false;
				//powerPanel.cores [1].isOn = false;
			}
		} else if (index == 0 && !b) { //Замена
			if (powerPanel.ionsPurged) {
				powerPanel.ionRefill ();

			} else {
				showMessageBox ("NOTE", "Need to purge active ions first. \n \n Перед заменой требуется сбросить активные.", 8f);
			}
		} else if (index == 6) { // Спутник
			if (b) {
				DOTween.PlayBackwards ("CommWindowHide");
			} else {
				DOTween.PlayForward ("CommWindowHide");
			}
		} else if (index == 12) { // Спутник
			if (b) {
				displayDemographics = true;
				DOTween.PlayBackwards ("DemographicsHide");
			} else {
				displayDemographics = false;
				DOTween.PlayForward ("DemographicsHide");
			}
		} else if (index == 39) { // Синхр маг полей вкл
			Sync.SetActive (b);
			X.SetActive (b && syncX);
			Y.SetActive (b && syncY);
			SyncEnabled = b;
		} else if (index == 38) { //X
			X.SetActive (b && SyncEnabled);
			syncX = b;
		} else if (index == 37) { //Y
			Y.SetActive (b && SyncEnabled);
			syncY = b;
		} else if (index == 36) { //PK
			PK.SetActive (b);
			PK_Enabled = b;
		} else if (index == 35) { //BSM
			BSM.SetActive (b);
			BSM_Enabled = b;
		} else if (index == 34) { //AZ
			AZ.SetActive (b);
			AZ_Enabled = b;
		} else if (index == 26) { //Reserve CH
			b = !b;
			Reserve_Enabled = b;
			GSM.SetActive(b && GSM_Enabled);
			LTE.SetActive(b && LTE_Enabled);
			Wifi.SetActive(b && Wifi_Enabled);
			Antenna.SetActive(b && Antenna_Enabled);
			Dish.SetActive(b && Dish_Enabled);

		} else if (index == 28) { //GSM
			GSM.SetActive(b && Reserve_Enabled);
			GSM_Enabled = b;
		} else if (index == 29) { //LTE
			LTE.SetActive(b && Reserve_Enabled);
			LTE_Enabled = b;
		} else if (index == 30) { //Wifi
			Wifi.SetActive(b && Reserve_Enabled);
			Wifi_Enabled = b;
		} else if (index == 31) { //Antenna-Radio
			Antenna.SetActive(b && Reserve_Enabled);
			Antenna_Enabled = b;
		} else if (index == 27) { //Dish(sat)
			Dish.SetActive(b && Reserve_Enabled);
			Dish_Enabled = b;
		} else if(index == 16){
			commsDisable  = !b;

		} else if(index == 17){
			firewall = !b;
		}


		/*
		bool SyncEnabled = false;
		bool syncX = false;
		bool syncY = false;
		bool PK_Enabled = false;
		bool BSM_Enabled = false;
		bool AZ_Enabled = false;

		bool GSM_Enabled = false;
		bool LTE_Enabled = false;
		bool Wifi_Enabled = false;
		bool Antenna_Enabled = false;
		bool Dish_Enabled = false;
		bool Reserve_Enabled = false;

		 * 
		 * */


		//
	} 

	/*
	 * Охлаждение - 4
	 * Спутник - 5
	 * Коммуникатор - 6
	 * Питание - 7
	 * Демография - 12
	 * Архив - 13
	 * освещение - 14 
	 * Стирание памяти - 15 
	 * Отображение карты - 23
	 * Контакты - 22
	 * Погода - 21
	 * Новости - 20
	 * */

	/*
	 * ОСН КАНАЛ спутника - 25 
	 * Синхр маг полей вкл - 39
	 * X - 38
	 * Y - 37
	 * 
	 * PK - 36
	 * BSM - 35	
	 * AZ - 34
	 * 
	 * RESERVE channel - 26
	 * 
	 * GSM - 28
	 * LTE - 29
	 * 
	 * Wifi - 30
	 * Radio - 31
	 * Sat - 27
	 * 
	 * Запуск порабощения - 33
	 * Запуск Ядерных ракет - 32longHexToBin
	 * ??????? - 24
	 * 
	 * Отключение коммуникаций - 16
	 * Firewall - 17
	 * */

	/*
	 * Скорость + 3
	 * Скорость - 2
	 * Сброс ионов 1
	 * Замена ионов 0
	 * 
	 * Ядро 1 + 11
	 * Ядро 1 - 10	
	 * Ядро 1 Вкл 9
	 * 
	 * Ядро 2 + 19
	 * Ядро 2 - 18
	 * Ядро 2 Вкл 8
	 * 
	 * */

	public void dropCapsule(){
		SerialComm.WriteToArduino("#D"); //CODE CORRECT COMMAND
	}


	string decToBin(string dec){
		return System.Convert.ToString(System.Convert.ToInt64(dec),2);
	}
	
	string hexToBin(string hex){
		return System.Convert.ToString(int.Parse(hex, System.Globalization.NumberStyles.HexNumber),2);
	}
	string longHexToBin(string hex){
		return Reverse(System.Convert.ToString(long.Parse(hex, System.Globalization.NumberStyles.HexNumber),2));
	}

	string Reverse( string s )
	{
		char[] charArray = s.ToCharArray();
		Array.Reverse( charArray );
		return new string( charArray );
	}

	int hexToDec(string hex){
		return int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
	}
}
