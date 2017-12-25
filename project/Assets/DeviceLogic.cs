using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class DeviceLogic : MonoBehaviour {

	public int CurrentMode = 1;
	public int ActiveDiode = 0;

	public float Voltage = 0;
	public float Amperage = 0;
	public float Temperature = 293f;

	public float[] StartStaticResistance = new float[5];

	private float ButtonStartElevation = 0;
	private float ButtonDeep = 0.02f;

	private float Reostat1StartPosition = 0;
	private float Reostat2StartPosition = 0;
	private float ReostatStep = 0.03f;
	private float ReostatLength = 0.3f;

	// Use this for initialization
	void Start () {
		GameObject.Find ("Mode").GetComponent<Text> ().text = CurrentMode.ToString();

		for (int i = 0; i < 5; i++)
		{
			StartStaticResistance[i] = 3000 + 500 * i;
		}

		ButtonStartElevation = GameObject.Find("Diode-1").transform.position.y; 

		Reostat1StartPosition = GameObject.Find("Reostat-1").gameObject.transform.Find ("Guide").gameObject.transform.Find ("Toggle").transform.position.x; 
		Reostat2StartPosition = GameObject.Find("Reostat-2").gameObject.transform.Find ("Guide").gameObject.transform.Find ("Toggle").transform.position.x;
	}

	public void InteractWithDevice() {
		RaycastHit hit;
		Ray ray = new Ray();

		if (Camera.main) {
			ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
		} else {
			ray = GameObject.Find("DeviceCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		}

		if (Physics.Raycast (ray, out hit)) {
			GameObject objectHit = hit.transform.gameObject;
			HandleInteraction(objectHit);

		}
	}

	public void PushButton(GameObject button) {
		if (button.transform.position.y == ButtonStartElevation) {
			UnpushAllButtons ();
			button.transform.position = new Vector3 (button.transform.position.x, button.transform.position.y - ButtonDeep, button.transform.position.z);
		} else {
			button.transform.position = new Vector3 (button.transform.position.x, button.transform.position.y + ButtonDeep, button.transform.position.z);
			ActiveDiode = 0;
		}
	}

	public void UnpushAllButtons() {
		GameObject button = new GameObject();

		for (int i = 1; i <= 5; i++) {
			button = GameObject.Find("Diode-" + i);
			button.transform.position = new Vector3(button.transform.position.x, ButtonStartElevation, button.transform.position.z);
		}
	}

	public void ResetReostats() {
		GameObject toggle = GameObject.Find ("Reostat-1").gameObject.transform.Find ("Guide").gameObject.transform.Find ("Toggle").gameObject;
		toggle.transform.position = new Vector3 (Reostat1StartPosition, toggle.transform.position.y, toggle.transform.position.z);
		toggle = GameObject.Find ("Reostat-2").gameObject.transform.Find ("Guide").gameObject.transform.Find ("Toggle").gameObject;
		toggle.transform.position = new Vector3 (Reostat2StartPosition, toggle.transform.position.y, toggle.transform.position.z);

		Voltage = 0;
		Amperage = 0;
	}

	public void MoveReostat(GameObject reostat, float ReostatStartPosition) {
		GameObject toggle = reostat.transform.Find ("Guide").gameObject.transform.Find ("Toggle").gameObject;
		Debug.Log (reostat.name);
		if (toggle.transform.position.x > ReostatStartPosition - ReostatLength + ReostatStep) {
			if (CurrentMode != 3 && reostat.name == "Reostat-1")
			{
				toggle.transform.position = new Vector3 (toggle.transform.position.x - ReostatStep, toggle.transform.position.y, toggle.transform.position.z);
				Voltage += 0.1f;
				Voltage = (float)Math.Round (Voltage, 1);
				Execute();
			}
			else if (CurrentMode == 3 && reostat.name == "Reostat-2")
			{
				toggle.transform.position = new Vector3 (toggle.transform.position.x - ReostatStep * 2, toggle.transform.position.y, toggle.transform.position.z);
				Voltage += 5.0f;
				Execute();
			}
		} else {
			toggle.transform.position = new Vector3 (ReostatStartPosition, toggle.transform.position.y, toggle.transform.position.z);
			Voltage = 0;
		}
	}

	public void NextMode() {
		if (CurrentMode >= 3) {
			CurrentMode = 1;
		} else {
			CurrentMode++;
		}
		GameObject.Find ("Mode").GetComponent<Text> ().text = CurrentMode.ToString();
	}

	public void SpinModeTumbler(GameObject tumbler) {
		tumbler.transform.Find("Spinner").gameObject.transform.Rotate (0, 120f, 0);
	}

	public void Execute() {
		switch (CurrentMode) {
		case 1: {
			if (ActiveDiode != 0 && Voltage != 0)
			{
				Amperage = (float)Math.Round(((Voltage / StartStaticResistance [ActiveDiode - 1] + UnityEngine.Random.Range(0.00001f, 0.00003f)) * (float)Math.Pow((Voltage * 10), 3)), 6);
			}
			else 
			{
				Amperage = 0;
			}
		} break;
		case 2: {
			if (ActiveDiode != 0 && Voltage != 0)
			{
				Amperage = (float)Math.Round(((Voltage / StartStaticResistance [ActiveDiode - 1] + UnityEngine.Random.Range(0.00001f, 0.00003f)) * (float)Math.Pow(Voltage, 0.05f) * 2), 6);
			}
			else 
			{
				Amperage = 0;
			}
		} break;
		case 3: {
			if (ActiveDiode != 0 && Voltage != 0)
			{
				Amperage = (float)Math.Round((Voltage / StartStaticResistance [ActiveDiode - 1] + UnityEngine.Random.Range(0.00003f, 0.0001f)) * (float)Math.Log(Voltage), 6);
			}
			else 
			{
				Amperage = 0;
			}
		} break;
		}
		GameObject.Find ("VoltmeterIndicator").GetComponent<Text> ().text = Voltage.ToString();
		GameObject.Find ("MilliampermeterIndicator").GetComponent<Text> ().text = Amperage.ToString();
	}

	public void HandleInteraction(GameObject tumbler){
		switch (tumbler.name) {
		case "Tumbler-1": {
			SpinModeTumbler(tumbler);
			UnpushAllButtons();
			ResetReostats();
			NextMode();
			Execute();
		} break;
		case "Diode-1": {
			ActiveDiode = 1;
			PushButton(tumbler);
			Execute();
		} break;
		case "Diode-2": {
			ActiveDiode = 2;
			PushButton(tumbler);
			Execute();
		} break;
		case "Diode-3": {
			ActiveDiode = 3;
			PushButton(tumbler);
			Execute();
		} break;
		case "Diode-4": {
			ActiveDiode = 4;
			PushButton(tumbler);
			Execute();
		} break;
		case "Diode-5": {
			ActiveDiode = 5;
			PushButton(tumbler);
			Execute();
		} break;
		case "Reostat-1": {
			MoveReostat(tumbler, Reostat1StartPosition);
		} break;
		case "Reostat-2": {
			MoveReostat(tumbler, Reostat2StartPosition);
		} break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			InteractWithDevice();
		}
	}
}
