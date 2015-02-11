using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AwesomiumUnityWebTexture))]
public class Example0 : MonoBehaviour 
{
	private AwesomiumUnityWebTexture m_WebTexture = null;
	
	// This example page already contains some buttons that will call our functions.
	// You can view the page source freely on GitHub at https://github.com/Rycul/AwesomiumUnity/blob/master/AwesomiumUnityScripts/Examples/Example0/index.php.
	private string m_URL = "http://htmlpreview.github.io/?https://github.com/Rycul/AwesomiumUnity/blob/master/AwesomiumUnityScripts/Examples/Example0/index.php";

	// Use this for initialization
	void Start () 
	{
		// Obtain the web texture component.
		m_WebTexture = GetComponent<AwesomiumUnityWebTexture>();
		
		// Check to make sure we have an instance.
		if (m_WebTexture == null)
		{
			DestroyImmediate(this);
		}
		
		// Bind some C# functions to javascript functions.
		m_WebTexture.WebView.BindJavaScriptCallback("PlayGame", this.Callback_PlayGame);	// Can be called from the HTML page by using: Unity.PlayGame();
		m_WebTexture.WebView.BindJavaScriptCallback("GoToOptions", this.Callback_GoToOptions);	// Unity.GoToOptions();
		m_WebTexture.WebView.BindJavaScriptCallback("QuitGame", this.Callback_QuitGame);	// Unity.QuitGame();

		m_WebTexture.WebView.RegisterFinishLoadingFrameCallback (myFinishLoadingCallback);

		m_WebTexture.LoadURL(m_URL);
	}
	
	void Callback_PlayGame()
	{
		Debug.Log("CLICKED PLAY GAME!");
	}
	
	void Callback_GoToOptions()
	{
		Debug.Log("CLICKED OPTIONS!");
	}
	
	void Callback_QuitGame()
	{
		Debug.Log("CLICKED QUIT GAME!");
	}

	void myFinishLoadingCallback(string url, System.Int64 frameid, bool ismainframe)
	{
		Debug.Log ("URL: " + url + ", frameid: " + frameid + ", ismainframe: " + ismainframe);
	}
}
