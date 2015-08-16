using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AwesomiumUnityWebTexture))]
public class Example0 : MonoBehaviour 
{
	private AwesomiumUnityWebTexture m_WebTexture = null;
	
	// This example page already contains some buttons that will call our functions.
	// You can view the page source freely on GitHub at https://github.com/Rycul/AwesomiumUnity/blob/master/AwesomiumUnityScripts/Examples/Example0/index.php.
	public string m_URL = "http://htmlpreview.github.io/?https://github.com/Rycul/AwesomiumUnity/blob/master/AwesomiumUnityScripts/Examples/Example0/index.php";

	public GameObject m_Prefab = null;

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

		m_WebTexture.Initialize ();

		// Bind some C# functions to javascript functions.
		m_WebTexture.WebView.BindJavaScriptCallback("PlayGame", this.Callback_PlayGame);	// Can be called from the HTML page by using: Unity.PlayGame();
		m_WebTexture.WebView.BindJavaScriptCallback("GoToOptions", this.Callback_GoToOptions);	// Unity.GoToOptions();
		m_WebTexture.WebView.BindJavaScriptCallback("QuitGame", this.Callback_QuitGame);	// Unity.QuitGame();

        m_WebTexture.WebView.FinishLoadingFrame += myFinishLoadingCallback;
        m_WebTexture.WebView.ChangeAddressBar   += myChangeAddressBarCallback;
        m_WebTexture.WebView.ShowCreatedWebView += myShowCreatedWebView;

		m_WebTexture.LoadURL(m_URL);
	}
	
	void Callback_PlayGame(AwesomiumUnityWebView _Caller)
	{
		Debug.Log("CLICKED PLAY GAME!");
	}

    void Callback_GoToOptions(AwesomiumUnityWebView _Caller)
	{
		Debug.Log("CLICKED OPTIONS!");
	}

    void Callback_QuitGame(AwesomiumUnityWebView _Caller)
	{
		Debug.Log("CLICKED QUIT GAME!");
	}

	void myFinishLoadingCallback(AwesomiumUnityWebView _Caller, string url, System.Int64 frameid, bool ismainframe)
	{
		Debug.Log ("Finished loading URL: " + url + ", frameid: " + frameid + ", ismainframe: " + ismainframe);
	}

	void myChangeAddressBarCallback(AwesomiumUnityWebView _Caller, string url)
	{
		Debug.Log ("Changed url to: " + url);
	}

    void myShowCreatedWebView(AwesomiumUnityWebView _Caller, AwesomiumUnityWebView _NewView, string _OpenerURL, string _TargetURL, bool _IsPopUp)
    {
        Debug.Log("Created new web view from '" + _OpenerURL + "' to target '" + _TargetURL + "' (is popup: " + _IsPopUp + ").");

		if (m_Prefab != null) 
		{
			GameObject go = GameObject.Instantiate (m_Prefab) as GameObject;
			AwesomiumUnityWebTexture webtexture = go.GetComponent<AwesomiumUnityWebTexture> ();
            webtexture.m_Width = 1280;
            webtexture.m_Height = 720;
			webtexture.WebView = _NewView;
			webtexture.Initialize();
		}
    }

	void JSStringResultCallback(AwesomiumUnityWebView _Caller, string _String)
	{
		Debug.Log ("JS Result: " + _String);
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.M))
		{
            AwesomiumUnityWebView.JavaScriptExecutionCallbacks callbacks = new AwesomiumUnityWebView.JavaScriptExecutionCallbacks();
            callbacks.StringResult = JSStringResultCallback;
            m_WebTexture.WebView.ExecuteJavaScriptWithResult("document.title", callbacks);
		}

        if (Input.GetKeyUp(KeyCode.S))
        {
            m_WebTexture.WebView.ExecuteJavaScript("alert(\"test\");", null);
        }
	}
}
