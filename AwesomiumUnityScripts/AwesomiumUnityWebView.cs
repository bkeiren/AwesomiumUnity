using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;	// For DllImport.

public class AwesomiumUnityWebView
{
	internal const string DllName = "AwesomiumUnity";
	
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
	extern static private void awe_webview_executejavascript( System.IntPtr _Instance, [MarshalAs(UnmanagedType.LPStr)]string _Script );
	
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

	
	public delegate void OnJavaScriptMethodCall();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void OnBeginLoadingFrame([MarshalAs(UnmanagedType.LPStr)]string _URL, System.Int64 _FrameID, bool _IsMainFrame, bool _IsErrorPage);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void OnFailLoadingFrame([MarshalAs(UnmanagedType.LPStr)]string _URL, System.Int32 _ErrorCode, [MarshalAs(UnmanagedType.LPStr)]string _ErrorDesc, System.Int64 _FrameID, bool _IsMainFrame);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void OnFinishLoadingFrame([MarshalAs(UnmanagedType.LPStr)]string _URL, System.Int64 _FrameID, bool _IsMainFrame);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void OnDocumentReady([MarshalAs(UnmanagedType.LPStr)]string _URL);
	
	private Dictionary<string, OnJavaScriptMethodCall> m_CallbacksOnJavaScriptMethodCall = new Dictionary<string, OnJavaScriptMethodCall>();
	

	private System.IntPtr m_Instance = System.IntPtr.Zero;
	
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
		awe_webview_destroy(m_Instance);
		m_Instance = System.IntPtr.Zero;
		
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
	
	public void ExecuteJavaScript( string _Script )
	{
		awe_webview_executejavascript(m_Instance, _Script);	
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
		
		m_CallbacksOnJavaScriptMethodCall[_MethodName] = _Callback;
		
		awe_webview_js_setmethod(m_Instance, _MethodName, false);
	}
	
	public void UnbindJavaScriptCallback( string _MethodName )
	{
		m_CallbacksOnJavaScriptMethodCall.Remove(_MethodName);	
	}
	
	public void CallBoundJavaScriptCallback( string _MethodName )
	{
		OnJavaScriptMethodCall callback = null;
		if (m_CallbacksOnJavaScriptMethodCall.TryGetValue(_MethodName, out callback))
		{
			callback();
		}
	}

	public void RegisterBeginLoadingFrameCallback( OnBeginLoadingFrame _Callback )
	{
		awe_webview_register_callback_beginloadingframe (NativePtr, Marshal.GetFunctionPointerForDelegate(_Callback));
	}

	public void RegisterFailLoadingFrameCallback( OnFailLoadingFrame _Callback )
	{
		awe_webview_register_callback_failloadingframe (NativePtr, Marshal.GetFunctionPointerForDelegate(_Callback));
	}

	public void RegisterFinishLoadingFrameCallback( OnFinishLoadingFrame _Callback )
	{
		awe_webview_register_callback_finishloadingframe (NativePtr, Marshal.GetFunctionPointerForDelegate(_Callback));
	}

	public void RegisterDocumentReadyCallback( OnDocumentReady _Callback )
	{
		awe_webview_register_callback_documentready (NativePtr, Marshal.GetFunctionPointerForDelegate(_Callback));
	}
}
