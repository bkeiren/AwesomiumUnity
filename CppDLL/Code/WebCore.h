#pragma once

#include "UnityPlugin.h"
#include <Awesomium/Platform.h>
#include <Awesomium/JSValue.h>
#include <list>

namespace Awesomium
{
	class WebView;
	class WebSession;
	class JSObject;
}

namespace AwesomiumUnity
{

Awesomium::JSObject* FindJSUnityObjectForWebView( Awesomium::WebView* _WebView );

typedef void (*WebView_OnBeginLoadingFrame)(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const wchar16* url, bool is_error_page);
typedef void (*WebView_OnFailLoadingFrame)(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const wchar16* url, int error_code, const wchar16* error_desc);
typedef void (*WebView_OnFinishLoadingFrame)(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const wchar16* url);
typedef void (*WebView_OnDocumentReady)(Awesomium::WebView* caller, const wchar16* url);

typedef void (*WebView_OnChangeAddressBar)(Awesomium::WebView* caller, const wchar16*);
typedef void (*WebView_OnAddConsoleMessage)(Awesomium::WebView* caller, const wchar16* message, int line_number, const wchar16* source);
typedef void (*WebView_OnShowCreatedWebView)(Awesomium::WebView* caller, Awesomium::WebView* new_view, const wchar16* opener_url, const wchar16* target_url, bool is_popup);

typedef void(*WebView_OnJavaScriptExecFinished)(Awesomium::WebView* caller, int exec_id);
typedef void(*WebView_OnJavaScriptResultBool)(Awesomium::WebView* caller, bool b, int exec_id);
typedef void(*WebView_OnJavaScriptResultInt)(Awesomium::WebView* caller, int i, int exec_id);
typedef void(*WebView_OnJavaScriptResultFloat)(Awesomium::WebView* caller, float f, int exec_id);
typedef void(*WebView_OnJavaScriptResultString)(Awesomium::WebView* caller, const wchar16* string, int exec_id);
typedef void(*WebView_OnJavaScriptResultNull)(Awesomium::WebView* caller, int exec_id);
typedef		  WebView_OnJavaScriptResultNull WebView_OnJavaScriptResultUndefined;
typedef void(*WebView_OnJavaScriptResultArray)(Awesomium::WebView* caller, int length, int exec_id);

typedef void (*WebView_OnJavaScriptMethodCall)( Awesomium::WebView* caller, const wchar16* method_name );
typedef Awesomium::JSValue (*WebView_OnJavaScriptMethodCallWithReturnValue)( Awesomium::WebView* caller, const wchar16* method_name );


extern Awesomium::WebSession*	g_WebSession;


#define CALLBACK_NAME_HELPER(name)		g_WebView_##name##Callback
#define CALLBACK_DEFINE_HELPER(name)	WebView_On##name CALLBACK_NAME_HELPER(name) = nullptr
#define CALLBACK_DECLARE_HELPER(name)	extern WebView_On##name CALLBACK_NAME_HELPER(name)

CALLBACK_DECLARE_HELPER(BeginLoadingFrame);
CALLBACK_DECLARE_HELPER(FailLoadingFrame);
CALLBACK_DECLARE_HELPER(FinishLoadingFrame);
CALLBACK_DECLARE_HELPER(DocumentReady);
CALLBACK_DECLARE_HELPER(ChangeAddressBar);
CALLBACK_DECLARE_HELPER(AddConsoleMessage);
CALLBACK_DECLARE_HELPER(ShowCreatedWebView);
CALLBACK_DECLARE_HELPER(JavaScriptExecFinished);
CALLBACK_DECLARE_HELPER(JavaScriptResultBool);
CALLBACK_DECLARE_HELPER(JavaScriptResultInt);
CALLBACK_DECLARE_HELPER(JavaScriptResultFloat);
CALLBACK_DECLARE_HELPER(JavaScriptResultString);
CALLBACK_DECLARE_HELPER(JavaScriptResultNull);
CALLBACK_DECLARE_HELPER(JavaScriptResultUndefined);
CALLBACK_DECLARE_HELPER(JavaScriptResultArray);
CALLBACK_DECLARE_HELPER(JavaScriptMethodCall);
CALLBACK_DECLARE_HELPER(JavaScriptMethodCallWithReturnValue);

}

extern "C" EXPORT_API void awe_webcore_initialize(const wchar16* _WebSessionPath,
												  const char* _PluginPath,
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

extern "C" EXPORT_API void awe_webcore_shutdown();

extern "C" EXPORT_API void* awe_webcore_createwebview( int _Width, int _Height );

extern "C" EXPORT_API void awe_webcore_update();

extern "C" EXPORT_API bool awe_webcore_isrunning();

extern "C" EXPORT_API void awe_webcore_register_webview_callbacks(	AwesomiumUnity::WebView_OnBeginLoadingFrame				_BeginLoadingFrame,
																	AwesomiumUnity::WebView_OnFailLoadingFrame				_FailLoadingFrame,
																	AwesomiumUnity::WebView_OnFinishLoadingFrame			_FinishLoadingFrame,
																	AwesomiumUnity::WebView_OnDocumentReady					_DocumentReady,
																	AwesomiumUnity::WebView_OnChangeAddressBar				_ChangeAddressBar,
																	AwesomiumUnity::WebView_OnAddConsoleMessage				_AddConsoleMessage,
																	AwesomiumUnity::WebView_OnShowCreatedWebView			_ShowCreatedWebView,
																	AwesomiumUnity::WebView_OnJavaScriptExecFinished		_JavaScriptExecFinished,
																	AwesomiumUnity::WebView_OnJavaScriptResultBool			_JavaScriptResultBool,
																	AwesomiumUnity::WebView_OnJavaScriptResultInt			_JavaScriptResultInt,
																	AwesomiumUnity::WebView_OnJavaScriptResultFloat			_JavaScriptResultFloat,
																	AwesomiumUnity::WebView_OnJavaScriptResultString		_JavaScriptResultString,
																	AwesomiumUnity::WebView_OnJavaScriptResultNull			_JavaScriptResultNull,
																	AwesomiumUnity::WebView_OnJavaScriptResultUndefined		_JavaScriptResultUndefined,
																	AwesomiumUnity::WebView_OnJavaScriptResultArray			_JavaScriptResultArray,
																	AwesomiumUnity::WebView_OnJavaScriptMethodCall			_JavaScriptMethodCall,
																	AwesomiumUnity::WebView_OnJavaScriptMethodCallWithReturnValue _JavaScriptMethodCallWithReturnValue);