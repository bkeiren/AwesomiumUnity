using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;	// For DllImport.

public class AwesomiumUnityWebView
{
	internal const string DllName = "AwesomiumUnity";

	/// DLL Imported functions.
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_loadurl( System.IntPtr _Instance, [MarshalAs(UnmanagedType.LPStr)]string _URL );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool awe_webview_iscrashed( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool awe_webview_isloading( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool awe_webview_istransparent( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_destroy( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_copybuffertotexture( System.IntPtr _Instance, System.IntPtr _TextureNativePtr, int _UnityTextureWidth, int _UnityTextureHeight );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private int awe_webview_surface_width( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private int awe_webview_surface_height( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool awe_webview_surface_isdirty( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool awe_webview_reload( System.IntPtr _Instance, bool _IgnoreCache );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_inject_mousemove( System.IntPtr _Instance, int _X, int _Y );

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_inject_mousedown( System.IntPtr _Instance, int _Button );

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_inject_mouseup( System.IntPtr _Instance, int _Button );

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_inject_mousewheel( System.IntPtr _Instance, int _ScrollVert, int _ScrollHorz );

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_inject_keyboardevent( System.IntPtr _Instance, ref AwesomiumUnityWebKeyboardEvent _WebKeyBoardEvent );

	//[DllImport(DllName)]
	//extern static private void awe_webview_inject_touchevent( System.IntPtr _Instance, ref AwesomiumUnityWebTouchEvent _WebTouchEvent );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_resize( System.IntPtr _Instance, int _Width, int _Height );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_executejavascript( System.IntPtr _Instance, [MarshalAs(UnmanagedType.LPStr)]string _Script, int _ExecutionID);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_executejavascriptwithresult( System.IntPtr _Instance, [MarshalAs(UnmanagedType.LPStr)]string _Script, int _ExecutionID);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_focus( System.IntPtr _Instance );

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_unfocus( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_stop( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_settransparent( System.IntPtr _Instance, bool _Transparent );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private AwesomiumUnityError awe_webview_lasterror( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool awe_webview_cangoback( System.IntPtr _Instance );

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool awe_webview_cangoforward( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_goback( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_goforward( System.IntPtr _Instance );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_gotohistoryoffset( System.IntPtr _Instance, int _Offset );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_js_setmethod( System.IntPtr _Instance, [MarshalAs(UnmanagedType.LPStr)]string _MethodName, bool _HasReturnValue );

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_register_callback_beginloadingframe( System.IntPtr _Instance, System.IntPtr _Callback );

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_register_callback_failloadingframe( System.IntPtr _Instance, System.IntPtr _Callback );

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_register_callback_finishloadingframe( System.IntPtr _Instance, System.IntPtr _Callback );

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_register_callback_documentready( System.IntPtr _Instance, System.IntPtr _Callback );
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void awe_webview_register_callback_changeaddressbar( System.IntPtr _Instance, System.IntPtr _Callback );


	/// Delegates.
    public delegate void OnJavaScriptMethodCall(AwesomiumUnityWebView _Caller);

	public delegate void OnBeginLoadingFrame(AwesomiumUnityWebView _Caller, string _URL, System.Int64 _FrameID, bool _IsMainFrame, bool _IsErrorPage);
    public delegate void OnFailLoadingFrame(AwesomiumUnityWebView _Caller, string _URL, System.Int32 _ErrorCode, string _ErrorDesc, System.Int64 _FrameID, bool _IsMainFrame);
    public delegate void OnFinishLoadingFrame(AwesomiumUnityWebView _Caller, string _URL, System.Int64 _FrameID, bool _IsMainFrame);
    public delegate void OnDocumentReady(AwesomiumUnityWebView _Caller, string _URL);
    
    public delegate void OnChangeAddressBar(AwesomiumUnityWebView _Caller, string _URL);
    public delegate void OnAddConsoleMessage(AwesomiumUnityWebView _Caller, string _Message, int _LineNumber, string _Source);
    public delegate void OnShowCreatedWebView(AwesomiumUnityWebView _Caller, AwesomiumUnityWebView _NewView, string _OpenerURL, string _TargetURL, bool _IsPopUp);

    // The following delegates define functions which are used as callbacks when a JavaScript action is executed.
    public delegate void OnJavaScriptExecFinished(AwesomiumUnityWebView _Caller);
    public delegate void OnJavaScriptResultBool(AwesomiumUnityWebView _Caller, bool _Bool);
    public delegate void OnJavaScriptResultInt(AwesomiumUnityWebView _Caller, int _Integer);
    public delegate void OnJavaScriptResultFloat(AwesomiumUnityWebView _Caller, float _Float);
    public delegate void OnJavaScriptResultString(AwesomiumUnityWebView _Caller, string _String);
    public delegate void OnJavaScriptResultNullOrUndefined(AwesomiumUnityWebView _Caller);
    public delegate void OnJavaScriptResultArray(AwesomiumUnityWebView _Caller, int _Length);


    // This class can be instanced by users and filled out to provide callbacks for a javascript execution.
    // TODO: Change this to events to allow for multiple callbacks.
    public class JavaScriptExecutionCallbacks
    {
        public OnJavaScriptExecFinished           ExecutionFinished;
        public OnJavaScriptResultBool             BoolResult;
        public OnJavaScriptResultInt              IntResult;
        public OnJavaScriptResultFloat            FloatResult;
        public OnJavaScriptResultString           StringResult;
        public OnJavaScriptResultNullOrUndefined  NullResult;
        public OnJavaScriptResultNullOrUndefined  UndefinedResult;
        public OnJavaScriptResultArray            ArrayResult;
    }

    private Dictionary<string, OnJavaScriptMethodCall>      m_JavaScriptMethodCallCallbacks = new Dictionary<string, OnJavaScriptMethodCall>();  // Callbacks for when specific JavaScript methods are called.
    private Dictionary<int, JavaScriptExecutionCallbacks>   m_JavaScriptCallbacks = new Dictionary<int, JavaScriptExecutionCallbacks>();         // Callbacks for when javascript events happen (execution finished, result available).
    private int                                             m_NextJavaScriptExecutionID = 1;

    private System.IntPtr m_Instance                        = System.IntPtr.Zero;

    /// Events.
    public event OnBeginLoadingFrame    BeginLoadingFrame;
    public event OnFailLoadingFrame     FailLoadingFrame;
    public event OnFinishLoadingFrame   FinishLoadingFrame;
    public event OnDocumentReady        DocumentReady;

    public event OnChangeAddressBar     ChangeAddressBar;
    public event OnAddConsoleMessage    AddConsoleMessage;
    public event OnShowCreatedWebView   ShowCreatedWebView;


	public static void TriggerBeginLoadingFrame(System.IntPtr _WebViewInstance, System.Int64 _FrameID, bool _IsMainFrame, string _URL, bool _IsErrorPage)
	{
		AwesomiumUnityWebView view = AwesomiumUnityWebCore.FindWebViewByNativePtr (_WebViewInstance);
        if (view != null && view.BeginLoadingFrame != null)
            view.BeginLoadingFrame(view, _URL, _FrameID, _IsMainFrame, _IsErrorPage);
	}

    public static void TriggerFailLoadingFrame(System.IntPtr _WebViewInstance, System.Int64 _FrameID, bool _IsMainFrame, string _URL, System.Int32 _ErrorCode, string _ErrorDesc)
    {
        AwesomiumUnityWebView view = AwesomiumUnityWebCore.FindWebViewByNativePtr(_WebViewInstance);
        if (view != null && view.FailLoadingFrame != null)
            view.FailLoadingFrame(view, _URL, _ErrorCode, _ErrorDesc, _FrameID, _IsMainFrame);
    }

    public static void TriggerFinishLoadingFrame(System.IntPtr _WebViewInstance, System.Int64 _FrameID, bool _IsMainFrame, string _URL)
    {
        AwesomiumUnityWebView view = AwesomiumUnityWebCore.FindWebViewByNativePtr(_WebViewInstance);
        if (view != null && view.FinishLoadingFrame != null)
            view.FinishLoadingFrame(view, _URL, _FrameID, _IsMainFrame);
    }

    public static void TriggerDocumentReady(System.IntPtr _WebViewInstance, string _URL)
    {
        AwesomiumUnityWebView view = AwesomiumUnityWebCore.FindWebViewByNativePtr(_WebViewInstance);
        if (view != null && view.DocumentReady != null)
            view.DocumentReady(view, _URL);
    }

    public static void TriggerChangeAddressBar(System.IntPtr _WebViewInstance, string _URL)
    {
        AwesomiumUnityWebView view = AwesomiumUnityWebCore.FindWebViewByNativePtr(_WebViewInstance);
        if (view != null && view.ChangeAddressBar != null)
            view.ChangeAddressBar(view, _URL);
    }

    public static void TriggerAddConsoleMessage(System.IntPtr _WebViewInstance, string _Message, int _LineNumber, string _Source)
    {
        AwesomiumUnityWebView view = AwesomiumUnityWebCore.FindWebViewByNativePtr(_WebViewInstance);
        if (view != null && view.AddConsoleMessage != null)
            view.AddConsoleMessage(view, _Message, _LineNumber, _Source);
    }

    public static void TriggerShowCreatedWebView(System.IntPtr _WebViewInstance, System.IntPtr _NewInstance, string _OpenerURL, string _TargetURL, bool _IsPopUp)
    {
        AwesomiumUnityWebView view = AwesomiumUnityWebCore.FindWebViewByNativePtr(_WebViewInstance);
        if (view != null && view.ShowCreatedWebView != null)
        {
			AwesomiumUnityWebView new_view = AwesomiumUnityWebCore.RegisterExistingWebView(_NewInstance);
            view.ShowCreatedWebView(view, new_view, _OpenerURL, _TargetURL, _IsPopUp);
        }
    }

    public static void TriggerJavaScriptExecFinished(System.IntPtr _WebViewInstance, int _ExecutionID)
    {
        AwesomiumUnityWebView caller = null;
        JavaScriptExecutionCallbacks callbacks = FindJavaScriptCallbacksForExecutionID(_WebViewInstance, _ExecutionID, out caller);
        if (callbacks != null && callbacks.ExecutionFinished != null)
        {
            callbacks.ExecutionFinished(caller);

            // Now that execution has finished we can remove the callbacks from our dictionary.
            caller.m_JavaScriptCallbacks.Remove(_ExecutionID);
        }
    }

    public static void TriggerJavaScriptResultCallbackBool(System.IntPtr _WebViewInstance, bool _Bool, int _ExecutionID)
    {
        AwesomiumUnityWebView caller = null;
        JavaScriptExecutionCallbacks callbacks = FindJavaScriptCallbacksForExecutionID(_WebViewInstance, _ExecutionID, out caller);
        if (callbacks != null && callbacks.BoolResult != null)
            callbacks.BoolResult(caller, _Bool);
    }

    public static void TriggerJavaScriptResultCallbackInt(System.IntPtr _WebViewInstance, int _Integer, int _ExecutionID)
    {
        AwesomiumUnityWebView caller = null;
        JavaScriptExecutionCallbacks callbacks = FindJavaScriptCallbacksForExecutionID(_WebViewInstance, _ExecutionID, out caller);
        if (callbacks != null && callbacks.IntResult != null)
            callbacks.IntResult(caller, _Integer);
    }

    public static void TriggerJavaScriptResultCallbackFloat(System.IntPtr _WebViewInstance, float _Float, int _ExecutionID)
    {
        AwesomiumUnityWebView caller = null;
        JavaScriptExecutionCallbacks callbacks = FindJavaScriptCallbacksForExecutionID(_WebViewInstance, _ExecutionID, out caller);
        if (callbacks != null && callbacks.FloatResult != null)
            callbacks.FloatResult(caller, _Float);
    }

    public static void TriggerJavaScriptResultCallbackString(System.IntPtr _WebViewInstance, string _String, int _ExecutionID)
    {
        AwesomiumUnityWebView caller = null;
        JavaScriptExecutionCallbacks callbacks = FindJavaScriptCallbacksForExecutionID(_WebViewInstance, _ExecutionID, out caller);
        if (callbacks != null && callbacks.StringResult != null)
            callbacks.StringResult(caller, _String);
    }

    public static void TriggerJavaScriptResultCallbackNull(System.IntPtr _WebViewInstance, int _ExecutionID)
    {
        AwesomiumUnityWebView caller = null;
        JavaScriptExecutionCallbacks callbacks = FindJavaScriptCallbacksForExecutionID(_WebViewInstance, _ExecutionID, out caller);
        if (callbacks != null && callbacks.NullResult != null)
            callbacks.NullResult(caller);
    }

    public static void TriggerJavaScriptResultCallbackUndefined(System.IntPtr _WebViewInstance, int _ExecutionID)
    {
        AwesomiumUnityWebView caller = null;
        JavaScriptExecutionCallbacks callbacks = FindJavaScriptCallbacksForExecutionID(_WebViewInstance, _ExecutionID, out caller);
        if (callbacks != null && callbacks.UndefinedResult != null)
            callbacks.UndefinedResult(caller);
    }

    public static void TriggerJavaScriptResultCallbackArray(System.IntPtr _WebViewInstance, int _Length, int _ExecutionID)
    {
        AwesomiumUnityWebView caller = null;
        JavaScriptExecutionCallbacks callbacks = FindJavaScriptCallbacksForExecutionID(_WebViewInstance, _ExecutionID, out caller);
        if (callbacks != null && callbacks.ArrayResult != null)
            callbacks.ArrayResult(caller, _Length);
    }

    public static void TriggerJavaScriptMethodCall(System.IntPtr _WebViewInstance, string _MethodName)
    {
        AwesomiumUnityWebView view = AwesomiumUnityWebCore.FindWebViewByNativePtr(_WebViewInstance);
        if (view != null)
            view.CallBoundJavaScriptCallback(_MethodName);
        else
            Debug.LogWarning("TriggerJavaScriptMethodCall: Could not find a matching AwesomiumUnityWebView even though there should exist one!");
    }

    // TODO!!!! RETURN VALUE!
    public static void TriggerJavaScriptMethodCallWithReturnValue(System.IntPtr _WebViewInstance, string _MethodName)
    {
        Debug.Log("WORK IN PROGRESS FUNCTION -- RECEIVED FUNCTION CALL WITH RETURN VALUE: " + _MethodName);

        // TODO: 
        // Change delegate and this function to return an object corresponding to a native Awesomium::JSValue object.
        // - Find the AwesomiumUnityWebView that corresponds to _WebViewCaller.
        // - Find a registered callback for _MethodName.
        // If a callback exists, call it and return it's return value.
    }

    private static JavaScriptExecutionCallbacks FindJavaScriptCallbacksForExecutionID(System.IntPtr _WebViewInstance, int _ExecutionID, out AwesomiumUnityWebView _Caller)
    {
        _Caller = AwesomiumUnityWebCore.FindWebViewByNativePtr(_WebViewInstance);
        JavaScriptExecutionCallbacks callbacks = null;
        if (_Caller != null)
            _Caller.m_JavaScriptCallbacks.TryGetValue(_ExecutionID, out callbacks);

        return callbacks;
    }
	
	
	public System.IntPtr NativePtr
	{
		get
		{
			return m_Instance;
		}
	}
	
	public AwesomiumUnityWebView( System.IntPtr _Instance )
	{
		m_Instance = _Instance;	
	}
	
	public void LoadURL( string _URL )
	{
		awe_webview_loadurl(m_Instance, _URL);
	}
	
	public bool IsCrashed
	{
		get
		{
			return awe_webview_iscrashed(m_Instance);	
		}
	}
	
	public bool IsLoading
	{
		get
		{
			return awe_webview_isloading(m_Instance);	
		}
	}
	
	public bool IsTransparent
	{
		get
		{
			return awe_webview_istransparent(m_Instance);	
		}
	}
	
	public void Destroy()
	{		
		AwesomiumUnityWebCore._QueueWebViewForRemoval(this);
	}
	
	public void CopyBufferToTexture( System.IntPtr _NativeTexturePtr, int _TextureWidth, int _TextureHeight )
	{
		if (_NativeTexturePtr == System.IntPtr.Zero) return;
		
		if (_TextureWidth != Width || _TextureHeight != Height) return;	// For now, only works if the texture has the exact same size as the webview.
		
		awe_webview_copybuffertotexture(m_Instance, _NativeTexturePtr, _TextureWidth, _TextureHeight);	// We pass Unity's width and height values of the texture
																														// because these may not match the actual width and height
																														// of the texture in memory, we need to compensate for that.
	}
			
	public int Width
	{
		get
		{
			return awe_webview_surface_width(m_Instance);		
		}
	}
		
	public int Height
	{
		get
		{
			return awe_webview_surface_height(m_Instance);		
		}
	}
		
	public bool IsDirty
	{
		get
		{
			return awe_webview_surface_isdirty(m_Instance);		
		}
	}
	
	public void Reload()
	{
		awe_webview_reload(m_Instance, false);
	}
	
	public void InjectMouseMove( int _X, int _Y )
	{
		awe_webview_inject_mousemove(m_Instance, _X, _Y);	
	}

	public void InjectMouseDown( int _Button )
	{
		awe_webview_inject_mousedown(m_Instance, _Button);
	}
	
	public void InjectMouseUp( int _Button )
	{
		awe_webview_inject_mouseup(m_Instance, _Button);
	}

	public void InjectMouseWheel( int _ScrollVert, int _ScrollHorz )
	{
		awe_webview_inject_mousewheel(m_Instance, _ScrollVert, _ScrollHorz);
	}

	public void InjectKeyboardEvent( AwesomiumUnityWebKeyboardEvent _WebKeyBoardEvent )
	{
		awe_webview_inject_keyboardevent(m_Instance, ref _WebKeyBoardEvent);
	}
	
	public void Resize( int _Width, int _Height )
	{
		awe_webview_resize(m_Instance, _Width, _Height);	
	}

    private int AllocateJavaScriptExecutionID()
    {
        return ++m_NextJavaScriptExecutionID;
    }

    private int GetJavaScriptExecutionIDAndRegisterCallbacks(JavaScriptExecutionCallbacks _Callbacks)
    {
        int execID = AllocateJavaScriptExecutionID();
        if (_Callbacks != null)
        {
            m_JavaScriptCallbacks.Add(execID, _Callbacks);
        }
        return execID;
    }
	
	public void ExecuteJavaScript( string _Script, OnJavaScriptExecFinished _ExecFinishedCallback )
	{
        JavaScriptExecutionCallbacks callbacks = null;
        if (_ExecFinishedCallback != null)
        {
            callbacks = new JavaScriptExecutionCallbacks();
            callbacks.ExecutionFinished += _ExecFinishedCallback;
        }
        int executionID = GetJavaScriptExecutionIDAndRegisterCallbacks(callbacks);
		awe_webview_executejavascript(m_Instance, _Script, executionID);
	}

	// Executes the javascript script _Script on the webview and calls callback(s) for the result.
	// If the result is an integer, the integer callback is called. If the result is a bool, the boolean callback is called (etc).
	// If the result is an array, the array callback is first called with the length as one of its parameters. Then, for each element in the array
	// the appropriate other callback is called (may recurse into another array callback). Finally, once the array iteration has finished
	// the array callback is called once again, this time with its length parameter being -1.
	public void ExecuteJavaScriptWithResult( string _Script, JavaScriptExecutionCallbacks _Callbacks )
	{
        int executionID = GetJavaScriptExecutionIDAndRegisterCallbacks(_Callbacks);
        awe_webview_executejavascriptwithresult(m_Instance, _Script, executionID);	
	}
	
	public void Focus()
	{
		awe_webview_focus(m_Instance);	
	}
	
	public void Unfocus()
	{
		awe_webview_unfocus(m_Instance);	
	}
	
	public void SetTransparent( bool _Transparent )
	{
		awe_webview_settransparent(m_Instance, _Transparent);
	}
		
	public void Stop()
	{
		awe_webview_stop(m_Instance);	
	}
	
	public AwesomiumUnityError LastError()
	{
		return awe_webview_lasterror(m_Instance);
	}
	
	public void GoBack()
	{
		awe_webview_goback(m_Instance);	
	}
	
	public void GoForward()
	{
		awe_webview_goforward(m_Instance);	
	}
	
	public bool CanGoBack()
	{
		return awe_webview_cangoback(m_Instance);	
	}
	
	public bool CanGoForward()
	{
		return awe_webview_cangoforward(m_Instance);	
	}
	
	public void GoToHistoryOffset( int _Offset )
	{
		awe_webview_gotohistoryoffset(m_Instance, _Offset);	
	}
	
	public void BindJavaScriptCallback( string _MethodName, OnJavaScriptMethodCall _Callback )
	{
		if (_Callback == null)
			return;
		
		m_JavaScriptMethodCallCallbacks[_MethodName] = _Callback;
		
		awe_webview_js_setmethod(m_Instance, _MethodName, false);
	}
	
	public void UnbindJavaScriptCallback( string _MethodName )
	{
        m_JavaScriptMethodCallCallbacks.Remove(_MethodName);	
	}
	
	public void CallBoundJavaScriptCallback( string _MethodName )
	{
		OnJavaScriptMethodCall callback = null;
        if (m_JavaScriptMethodCallCallbacks.TryGetValue(_MethodName, out callback))
            callback(this);
	}

    public void ClearNativePtr()
    {
        Debug.Log("Clearing native pointer for web view.");
        m_Instance = System.IntPtr.Zero;
    }
}
