using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;	// For DllImport.

public class AwesomiumUnityWebCore
{
	internal const string DllName = "AwesomiumUnity";
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    extern static private void awe_webcore_initialize(  [MarshalAs(UnmanagedType.LPWStr)]string _WebSessionPath,
                                                        [MarshalAs(UnmanagedType.LPStr)]string _PluginPath,
                                                        bool _GPUAcceleration, 
												        bool _WebGL, 
												        bool _JavaScript, 
												        bool _Plugins, 
												        bool _WebAudio, 
												        bool _RemoteFonts,
												        bool _AppCache,
												        bool _Dart,
												        bool _HTML5LocalStorage,
												        bool _SmoothScrolling,
												        bool _WebSecurity);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webcore_shutdown();
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private System.IntPtr awe_webcore_createwebview( int _Width, int _Height );

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webcore_destroywebview( System.IntPtr _WebView );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webcore_update();
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool awe_webcore_isrunning();
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    extern static private bool awe_webcore_register_webview_callbacks(  System.IntPtr _BeginLoadingFrame,
                                                                        System.IntPtr _FailLoadingFrame,
                                                                        System.IntPtr _FinishLoadingFrame,
                                                                        System.IntPtr _DocumentReady,
                                                                        System.IntPtr _ChangeAddressBar,
                                                                        System.IntPtr _AddConsoleMessage,
                                                                        System.IntPtr _ShowCreatedWebView,
                                                                        System.IntPtr _JavaScriptExecFinished,
	                                                                    System.IntPtr _JavaScriptResultBool,
	                                                                    System.IntPtr _JavaScriptResultInt,
	                                                                    System.IntPtr _JavaScriptResultFloat,
	                                                                    System.IntPtr _JavaScriptResultString,
	                                                                    System.IntPtr _JavaScriptResultNull,
	                                                                    System.IntPtr _JavaScriptResultUndefined,
	                                                                    System.IntPtr _JavaScriptResultArray,
                                                                        System.IntPtr _JavaScriptMethodCall,
                                                                        System.IntPtr _JavaScriptMethodCallWithReturnValue);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_jsmethodhandler_register_callback_onmethodcall( System.IntPtr _Callback );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_jsmethodhandler_register_callback_onmethodcallwithreturnvalue( System.IntPtr _Callback );
	
	
	private static bool                         m_IsUpdating = false;
	private static bool                         m_HasBeenShutdown = false;
	private static List<AwesomiumUnityWebView>  m_WebViews = new List<AwesomiumUnityWebView>();
	private static List<AwesomiumUnityWebView>  m_WebViewsQueuedForRemoval = new List<AwesomiumUnityWebView>();
    private static List<GCHandle>               m_CallbackHandles = new List<GCHandle>();  /// A list of GCHandles for registered callbacks to ensure they are not re-allocated.
    
    public static bool IsUpdating          
    { 
        get 
        { 
            return m_IsUpdating; 
        } 
    }


	public static AwesomiumUnityWebView FindWebViewByNativePtr(System.IntPtr _NativePtr)
	{
		return m_WebViews.Find ( w => w.NativePtr == _NativePtr );
	}

	// WebView pass-through callbacks.
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void WebView_OnBeginLoadingFrame(System.IntPtr _WebViewInstance, System.Int64 _FrameID, bool _IsMainFrame, [MarshalAs(UnmanagedType.LPWStr)]string _URL, bool _IsErrorPage);
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnFailLoadingFrame(System.IntPtr _WebViewInstance, System.Int64 _FrameID, bool _IsMainFrame, [MarshalAs(UnmanagedType.LPWStr)]string _URL, System.Int32 _ErrorCode, [MarshalAs(UnmanagedType.LPWStr)]string _ErrorDesc);
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnFinishLoadingFrame(System.IntPtr _WebViewInstance, System.Int64 _FrameID, bool _IsMainFrame, [MarshalAs(UnmanagedType.LPWStr)]string _URL);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnDocumentReady(System.IntPtr _WebViewInstance, [MarshalAs(UnmanagedType.LPWStr)]string _URL);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnChangeAddressBar(System.IntPtr _WebViewInstance, [MarshalAs(UnmanagedType.LPWStr)]string _URL);
    
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnAddConsoleMessage(System.IntPtr _WebViewInstance, [MarshalAs(UnmanagedType.LPWStr)]string _Message, int _LineNumber, [MarshalAs(UnmanagedType.LPWStr)]string _Source);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnShowCreatedWebView(System.IntPtr _WebViewInstance, System.IntPtr _NewInstance, [MarshalAs(UnmanagedType.LPWStr)]string _OpenerURL, [MarshalAs(UnmanagedType.LPWStr)]string _TargetURL, bool _IsPopUp);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnJavaScriptExecFinished(System.IntPtr _WebViewInstance, int _ExecutionID);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnJavaScriptResultCallbackBool(System.IntPtr _WebViewInstance, [MarshalAs(UnmanagedType.Bool)]bool _Bool, int _ExecutionID);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnJavaScriptResultCallbackInt(System.IntPtr _WebViewInstance, int _Integer, int _ExecutionID);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnJavaScriptResultCallbackFloat(System.IntPtr _WebViewInstance, float _Float, int _ExecutionID);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnJavaScriptResultCallbackString(System.IntPtr _WebViewInstance, [MarshalAs(UnmanagedType.LPWStr)]string _String, int _ExecutionID);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnJavaScriptResultCallbackBoolNullOrUndefined(System.IntPtr _WebViewInstance, int _ExecutionID);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnJavaScriptResultCallbackArray(System.IntPtr _WebViewInstance, int _Length, int _ExecutionID);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnJavaScriptMethodCall(System.IntPtr _WebViewInstance, [MarshalAs(UnmanagedType.LPWStr)]string _MethodName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void WebView_OnJavaScriptMethodCallWithReturnValue(System.IntPtr _WebViewInstance, [MarshalAs(UnmanagedType.LPWStr)]string _MethodName);
    // TODO!!!!!!!^ RETURN VALUE!

	
	public static bool IsRunning
	{
		get 	
		{
			return awe_webcore_isrunning();	
		}
	}
	
	public static void EnsureInitialized()
	{
		if (!IsRunning)
		{
            Debug.Log("WebSession Preferences: \n" + AwesomiumUnityWebSession.Preferences.ToString());

			awe_webcore_initialize( AwesomiumUnityWebSession.Preferences.WebSessionPath,
                                    AwesomiumUnityWebSession.Preferences.PluginPath,
                                    AwesomiumUnityWebSession.Preferences.GPUAcceleration,
                                    AwesomiumUnityWebSession.Preferences.WebGL,
                                    AwesomiumUnityWebSession.Preferences.JavaScript,
                                    AwesomiumUnityWebSession.Preferences.Plugins,
                                    AwesomiumUnityWebSession.Preferences.WebAudio,
                                    AwesomiumUnityWebSession.Preferences.RemoteFonts,
                                    AwesomiumUnityWebSession.Preferences.AppCache,
                                    AwesomiumUnityWebSession.Preferences.Dart,
                                    AwesomiumUnityWebSession.Preferences.HTML5LocalStorage,
                                    AwesomiumUnityWebSession.Preferences.SmoothScrolling,
                                    AwesomiumUnityWebSession.Preferences.WebSecurity);
		}
		/*else
		{
			Debug.LogWarning("The web core has already been initialized. You must call Shutdown() before initializing again");
		}*/

		RegisterAndAllocWebViewCallbacks ();
		
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

        if (m_HasBeenShutdown)  // In the editor we may reach this point twice, so just to be safe we check this.
            return;

        m_HasBeenShutdown = true;

		foreach (AwesomiumUnityWebView webView in m_WebViews)
		{
			webView.Destroy();
		}
        RemoveQueuedWebViews();

        FreeWebViewCallbacks();
		
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

    public static AwesomiumUnityWebView RegisterExistingWebView(System.IntPtr _NativePtr)
    {
        foreach (AwesomiumUnityWebView wv in m_WebViews)
            if (wv.NativePtr == _NativePtr)
                return wv;

        AwesomiumUnityWebView new_wv = new AwesomiumUnityWebView(_NativePtr);
        m_WebViews.Add(new_wv);
        return new_wv;
    }
	
	public static void Update()
	{
        m_IsUpdating = true;
		RemoveQueuedWebViews();
		awe_webcore_update();
        m_IsUpdating = false;
	}
	
	// DO NOT CALL MANUALLY.
	public static void _QueueWebViewForRemoval( AwesomiumUnityWebView _WebView )
	{
		if (!m_WebViewsQueuedForRemoval.Contains(_WebView))
			m_WebViewsQueuedForRemoval.Add(_WebView);
	}
	
	private static void RemoveQueuedWebViews()
	{
		foreach (AwesomiumUnityWebView webView in m_WebViewsQueuedForRemoval)
		{
            awe_webcore_destroywebview(webView.NativePtr);
            webView.ClearNativePtr();
			m_WebViews.Remove(webView);	
		}
		m_WebViewsQueuedForRemoval.Clear();
	}

	private static void RegisterAndAllocWebViewCallbacks()
	{
        if (m_CallbackHandles.Count != 0)
            return;

        System.IntPtr beginLoadingFrame     = AllocDelegateAndGetFunctionPointer(new WebView_OnBeginLoadingFrame(AwesomiumUnityWebView.TriggerBeginLoadingFrame));
        System.IntPtr failLoadingFrame      = AllocDelegateAndGetFunctionPointer(new WebView_OnFailLoadingFrame(AwesomiumUnityWebView.TriggerFailLoadingFrame));
        System.IntPtr finishLoadingFrame    = AllocDelegateAndGetFunctionPointer(new WebView_OnFinishLoadingFrame(AwesomiumUnityWebView.TriggerFinishLoadingFrame));
        System.IntPtr documentReady         = AllocDelegateAndGetFunctionPointer(new WebView_OnDocumentReady(AwesomiumUnityWebView.TriggerDocumentReady));
        System.IntPtr changeAddressBar      = AllocDelegateAndGetFunctionPointer(new WebView_OnChangeAddressBar(AwesomiumUnityWebView.TriggerChangeAddressBar));
        System.IntPtr addConsoleMessage     = AllocDelegateAndGetFunctionPointer(new WebView_OnAddConsoleMessage(AwesomiumUnityWebView.TriggerAddConsoleMessage));
        System.IntPtr showCreatedWebView    = AllocDelegateAndGetFunctionPointer(new WebView_OnShowCreatedWebView(AwesomiumUnityWebView.TriggerShowCreatedWebView));
        System.IntPtr jsExecFinished        = AllocDelegateAndGetFunctionPointer(new WebView_OnJavaScriptExecFinished(AwesomiumUnityWebView.TriggerJavaScriptExecFinished));
        System.IntPtr jsResultBool          = AllocDelegateAndGetFunctionPointer(new WebView_OnJavaScriptResultCallbackBool(AwesomiumUnityWebView.TriggerJavaScriptResultCallbackBool));
        System.IntPtr jsResultInt           = AllocDelegateAndGetFunctionPointer(new WebView_OnJavaScriptResultCallbackInt(AwesomiumUnityWebView.TriggerJavaScriptResultCallbackInt));
        System.IntPtr jsResultFloat         = AllocDelegateAndGetFunctionPointer(new WebView_OnJavaScriptResultCallbackFloat(AwesomiumUnityWebView.TriggerJavaScriptResultCallbackFloat));
        System.IntPtr jsResultString        = AllocDelegateAndGetFunctionPointer(new WebView_OnJavaScriptResultCallbackString(AwesomiumUnityWebView.TriggerJavaScriptResultCallbackString));
        System.IntPtr jsResultNull          = AllocDelegateAndGetFunctionPointer(new WebView_OnJavaScriptResultCallbackBoolNullOrUndefined(AwesomiumUnityWebView.TriggerJavaScriptResultCallbackNull));
        System.IntPtr jsResultUndefined     = AllocDelegateAndGetFunctionPointer(new WebView_OnJavaScriptResultCallbackBoolNullOrUndefined(AwesomiumUnityWebView.TriggerJavaScriptResultCallbackUndefined));
        System.IntPtr jsResultArray         = AllocDelegateAndGetFunctionPointer(new WebView_OnJavaScriptResultCallbackArray(AwesomiumUnityWebView.TriggerJavaScriptResultCallbackArray));
        System.IntPtr jsMethodCall          = AllocDelegateAndGetFunctionPointer(new WebView_OnJavaScriptMethodCall(AwesomiumUnityWebView.TriggerJavaScriptMethodCall));
        System.IntPtr jsMethodCallWithReturnValue = AllocDelegateAndGetFunctionPointer(new WebView_OnJavaScriptMethodCallWithReturnValue(AwesomiumUnityWebView.TriggerJavaScriptMethodCallWithReturnValue));
        
		awe_webcore_register_webview_callbacks(beginLoadingFrame,
                                               failLoadingFrame,
                                               finishLoadingFrame,
                                               documentReady,
                                               changeAddressBar,
                                               addConsoleMessage,
                                               showCreatedWebView,
                                               jsExecFinished,
                                               jsResultBool,
                                               jsResultInt,
                                               jsResultFloat,
                                               jsResultString,
                                               jsResultNull,
                                               jsResultUndefined,
                                               jsResultArray,
                                               jsMethodCall,
                                               jsMethodCallWithReturnValue);
	}

    private static System.IntPtr AllocDelegateAndGetFunctionPointer(System.Delegate _Delegate)
    {
        AllocCallbackHandle(_Delegate);
        return Marshal.GetFunctionPointerForDelegate(_Delegate);
    }

    private static void FreeWebViewCallbacks()
    {
        foreach (GCHandle handle in m_CallbackHandles)
            handle.Free();
        m_CallbackHandles.Clear();
    }

    private static void AllocCallbackHandle(System.Delegate _Delegate)
    {
        // First check to see if there is already a handle allocated for this object because we don't want to be doing this multiple times
        // For an object.
        foreach (GCHandle existing_handle in m_CallbackHandles)
            if (existing_handle.Target == _Delegate)
                return;
        
        GCHandle handle = GCHandle.Alloc(_Delegate);
        m_CallbackHandles.Add(handle);
    }
}
