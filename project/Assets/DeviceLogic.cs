using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeviceLogic : MonoBehaviour {

	public int CurrentMode = 1;

	public float Voltage = 0;
	public float Amperage = 0;
	public float Temperature = 293f;

	public float[] StartStaticResistance = { 3000f, 3500f, 4000f, 4500f, 5000f };

	private float ButtonStartElevation = 0;
	private float ButtonDeep = 0.02f;

	private float Reostat1StartPosition = 0;
	private float Reostat2StartPosition = 0;
	private float ReostatStep = 0.03f;
	private float ReostatLength = 0.3f;

	// Use this for initialization
	void Start () {
		GameObject.Find ("Mode").GetComponent<Text> ().text = CurrentMode.ToString();

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
		}
	}

	public void UnpushAllButtons() {
		GameObject button = new GameObject();
		for (int i = 1; i <= 5; i++) {
			button = GameObject.Find("Diode-" + i);
			button.transform.position = new Vector3(button.transform.position.x, ButtonStartElevation, button.transform.position.z);
		}
	}

	public void MoveReostat(GameObject reostat, float ReostatStartPosition) {
		GameObject toggle = reostat.transform.Find ("Guide").gameObject.transform.Find ("Toggle").gameObject;
		if (toggle.transform.position.x > ReostatStartPosition - ReostatLength + ReostatStep) {
			toggle.transform.position = new Vector3 (toggle.transform.position.x - ReostatStep, toggle.transform.position.y, toggle.transform.position.z);
		} else {
			toggle.transform.position = new Vector3 (ReostatStartPosition, toggle.transform.position.y, toggle.transform.position.z);
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

	public void HandleInteraction(GameObject tumbler){
		switch (tumbler.name) {
		case "Tumbler-1": {
			SpinModeTumbler(tumbler);
			NextMode();
		} break;
		case "Diode-1": {
			PushButton(tumbler);
		} break;
		case "Diode-2": {
			PushButton(tumbler);
		} break;
		case "Diode-3": {
			PushButton(tumbler);
		} break;
		case "Diode-4": {
			PushButton(tumbler);
		} break;
		case "Diode-5": {
			PushButton(tumbler);
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
