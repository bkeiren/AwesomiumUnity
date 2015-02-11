#pragma once

#include "UnityPlugin.h"
#include "LoadListener.h"

namespace Awesomium
{
	class WebView;
	class WebKeyboardEvent;
	struct WebTouchEvent;
	enum Error;
}

extern "C" EXPORT_API void awe_webview_loadurl( Awesomium::WebView* _Instance, char* _URL );

extern "C" EXPORT_API bool awe_webview_iscrashed( Awesomium::WebView* _Instance );

extern "C" EXPORT_API bool awe_webview_isloading( Awesomium::WebView* _Instance );

extern "C" EXPORT_API bool awe_webview_istransparent( Awesomium::WebView* _Instance );

extern "C" EXPORT_API void awe_webview_destroy( Awesomium::WebView* _Instance );

extern "C" EXPORT_API void awe_webview_copybuffertotexture( Awesomium::WebView* _Instance, void* _TextureNativePtr, int _UnityTextureWidth, int _UnityTextureHeight );

extern "C" EXPORT_API int awe_webview_surface_width( Awesomium::WebView* _Instance );

extern "C" EXPORT_API int awe_webview_surface_height( Awesomium::WebView* _Instance );

extern "C" EXPORT_API bool awe_webview_surface_isdirty( Awesomium::WebView* _Instance );

extern "C" EXPORT_API void awe_webview_reload( Awesomium::WebView* _Instance, bool _IgnoreCache );

extern "C" EXPORT_API void awe_webview_inject_mousemove( Awesomium::WebView* _Instance, int _X, int _Y );

extern "C" EXPORT_API void awe_webview_inject_mousedown( Awesomium::WebView* _Instance, int _Button );

extern "C" EXPORT_API void awe_webview_inject_mouseup( Awesomium::WebView* _Instance, int _Button );

extern "C" EXPORT_API void awe_webview_inject_mousewheel( Awesomium::WebView* _Instance, int _ScrollVert, int _ScrollHorz );

extern "C" EXPORT_API void awe_webview_inject_keyboardevent( Awesomium::WebView* _Instance, Awesomium::WebKeyboardEvent& _WebKeyBoardEvent );

extern "C" EXPORT_API void awe_webview_inject_touchevent( Awesomium::WebView* _Instance, Awesomium::WebTouchEvent& _WebTouchEvent );

extern "C" EXPORT_API void awe_webview_resize( Awesomium::WebView* _Instance, int _Width, int _Height );

extern "C" EXPORT_API void awe_webview_executejavascript( Awesomium::WebView* _Instance, char* _Script );

extern "C" EXPORT_API void awe_webview_focus( Awesomium::WebView* _Instance );

extern "C" EXPORT_API void awe_webview_unfocus( Awesomium::WebView* _Instance );

extern "C" EXPORT_API void awe_webview_stop( Awesomium::WebView* _Instance );

extern "C" EXPORT_API void awe_webview_settransparent( Awesomium::WebView* _Instance, bool _Transparent );

extern "C" EXPORT_API Awesomium::Error awe_webview_lasterror( Awesomium::WebView* _Instance );

extern "C" EXPORT_API bool awe_webview_cangoback( Awesomium::WebView* _Instance );

extern "C" EXPORT_API bool awe_webview_cangoforward( Awesomium::WebView* _Instance );

extern "C" EXPORT_API void awe_webview_goback( Awesomium::WebView* _Instance );

extern "C" EXPORT_API void awe_webview_goforward( Awesomium::WebView* _Instance );

extern "C" EXPORT_API void awe_webview_gotohistoryoffset( Awesomium::WebView* _Instance, int _Offset );

extern "C" EXPORT_API void awe_webview_js_setmethod( Awesomium::WebView* _Instance, char* _MethodName, bool _HasReturnValue );

// These should be called by managed C# code.
extern "C" EXPORT_API void awe_webview_register_callback_beginloadingframe( Awesomium::WebView* _Instance, AwesomiumUnity::LoadListener::BeginLoadingFrameCallback _Callback );
extern "C" EXPORT_API void awe_webview_register_callback_failloadingframe( Awesomium::WebView* _Instance, AwesomiumUnity::LoadListener::FailLoadingFrameCallback _Callback );
extern "C" EXPORT_API void awe_webview_register_callback_finishloadingframe( Awesomium::WebView* _Instance, AwesomiumUnity::LoadListener::FinishLoadingFrameCallback _Callback );
extern "C" EXPORT_API void awe_webview_register_callback_documentready( Awesomium::WebView* _Instance, AwesomiumUnity::LoadListener::DocumentReadyCallback _Callback );