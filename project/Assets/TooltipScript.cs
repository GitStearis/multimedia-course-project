using UnityEngine;
using System.Collections;

public class TooltipScript : MonoBehaviour {
	private string currentToolTipText = "";
	private GUIStyle guiStyleFore;
	private GUIStyle guiStyleBack;
	
	public void Start()
	{
		guiStyleFore = new GUIStyle();
		guiStyleFore.normal.textColor = Color.white;
		guiStyleFore.alignment = TextAnchor.UpperCenter ;
		guiStyleFore.wordWrap = true;
		guiStyleBack = new GUIStyle();
		guiStyleBack.normal.textColor = Color.black;
		guiStyleBack.alignment = TextAnchor.UpperCenter ;
		guiStyleBack.wordWrap = true;
	}

	// Update is called once per frame
	public void Update () 
	{
		
	}
	
	public void OnMouseEnter()
	{
		Debug.Log("Entering");
	}
	
	public void OnMouseExit()
	{
		Debug.Log("Exiting");
	}
}
