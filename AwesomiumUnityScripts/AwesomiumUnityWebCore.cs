using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;	// For DllImport.

public class AwesomiumUnityWebCore
{
	internal const string DllName = "AwesomiumUnity";
	
	[DllImport(DllName)]
	extern static private void awe_webcore_initialize();
	
	[DllImport(DllName)]
	extern static private void awe_webcore_shutdown();
	
	[DllImport(DllName)]
	extern static private System.IntPtr awe_webcore_createwebview( int _Width, int _Height );
	
	[DllImport(DllName)]
	extern static private void awe_webcore_update();
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool awe_webcore_isrunning();
	
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void OnJavaScriptMethodCall( System.IntPtr _WebViewCaller, [MarshalAs(UnmanagedType.LPWStr)]string _MethodName );
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void OnJavaScriptMethodCallWithReturnValue( System.IntPtr _WebViewCaller, [MarshalAs(UnmanagedType.LPWStr)]string _MethodName );
	// TODO!!!!!!!^ RETURN VALUE!
	
	[DllImport(DllName)]
	extern static private void awe_jsmethodhandler_register_callback_onmethodcall( OnJavaScriptMethodCall _Callback );
	
	[DllImport(DllName)]
	extern static private void awe_jsmethodhandler_register_callback_onmethodcallwithreturnvalue( OnJavaScriptMethodCallWithReturnValue _Callback );
	
	
	
	private static List<AwesomiumUnityWebView> m_WebViews = new List<AwesomiumUnityWebView>();
	private static List<AwesomiumUnityWebView> m_WebViewsQueuedForRemoval = new List<AwesomiumUnityWebView>();
	
	
	public static bool IsRunning
	{
		get 	
		{
			return awe_webcore_isrunning();	
		}
	}
	
	public static void Initialize()
	{
		if (!IsRunning)
		{		
			awe_webcore_initialize();
		}
		else
		{
			Debug.LogError("The web core has already been initialized. You must call Shutdown() before initializing again");
		}
		
		// Registers a C# callback to be called from the unmanaged C++ DLL.
		awe_jsmethodhandler_register_callback_onmethodcall(Callback_OnJavaScriptMethodCall);
		
		// Registers a C# callback to be called from the unmanaged C++ DLL.
		awe_jsmethodhandler_register_callback_onmethodcallwithreturnvalue(Callback_OnJavaScriptMethodCallWithReturnValue);
		
		CreateWebCoreHelper();
	}
	
	public static void CreateWebCoreHelper()
	{
		if (AwesomiumUnityWebCoreHelper.Instance == null)
		{
			GameObject go = new GameObject("AwesomiumUnityWebCoreHelper");
			go.AddComponent<AwesomiumUnityWebCoreHelper>();
		}	
	}
	
	public static void Shutdown()
	{
		if (!IsRunning)
		{
			Debug.LogError("The web core has not been initialized. You must call Initialize() before being able to call Shutdown()");
			return;
		}
		RemoveQueuedWebViews();
		foreach (AwesomiumUnityWebView webView in m_WebViews)
		{
			webView.Destroy();	
		}
		
		if (Application.isEditor) return;	// Due to the fact that Awesomium doesn't like to be initialized and shutdown multiple times per process,
											// combined with the fact that the Unity Editor IS a single process (regardless of entering or exiting playmode),
											// we have to make sure NOT to attempt to shutdown because then we will crash when we enter playmode again.
		awe_webcore_shutdown();	
	}
	
	public static AwesomiumUnityWebView CreateWebView( int _Width, int _Height )
	{
		if (!IsRunning)
		{
			Debug.LogError("You must call AwesomiumUnityWebCore.Initialize() before attempting to use the webcore");
			return null;
		}
			
		System.IntPtr intPtr = awe_webcore_createwebview(_Width, _Height);
		AwesomiumUnityWebView webView = new AwesomiumUnityWebView(intPtr);
		m_WebViews.Add(webView);
		return webView;
	}
	
	public static void Update()
	{
		RemoveQueuedWebViews();
		awe_webcore_update();
	}
	
	// DO NOT CALL MANUALLY.
	public static void _QueueWebViewForRemoval( AwesomiumUnityWebView _WebView )
	{
		m_WebViewsQueuedForRemoval.Add(_WebView);	
	}
	
	private static void RemoveQueuedWebViews()
	{
		foreach (AwesomiumUnityWebView webView in m_WebViewsQueuedForRemoval)
		{
			m_WebViews.Remove(webView);	
		}
		m_WebViewsQueuedForRemoval.Clear();
	}
	
	public static void Callback_OnJavaScriptMethodCall( System.IntPtr _WebViewCaller, [MarshalAs(UnmanagedType.LPWStr)]string _MethodName )
	{
		//Debug.Log("RECEIVED FUNCTION CALL: " + _MethodName);
		
		foreach (AwesomiumUnityWebView webView in m_WebViews)
		{
			if (webView.NativePtr == _WebViewCaller)	
			{
				webView.CallBoundJavaScriptCallback(_MethodName);
				return;	
			}
		}
		Debug.LogError("Callback_OnJavaScriptMethodCall: Could not find a matching AwesomiumUnityWebView even though there should exist one!");
	}
	
	// TODO!!!! RETURN VALUE!
	public static void Callback_OnJavaScriptMethodCallWithReturnValue( System.IntPtr _WebViewCaller, [MarshalAs(UnmanagedType.LPWStr)]string _MethodName )
	{
		Debug.Log("WORK IN PROGRESS FUNCTION -- RECEIVED FUNCTION CALL WITH RETURN VALUE: " + _MethodName);	
		
		// TODO: 
		// Change delegate and this function to return an object corresponding to a native Awesomium::JSValue object.
		// - Find the AwesomiumUnityWebView that corresponds to _WebViewCaller.
		// - Find a registered callback for _MethodName.
			// If a callback exists, call it and return it's return value.
	}
}
