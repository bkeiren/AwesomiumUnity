#include "WebCore.h"

#include <Awesomium/WebCore.h>
#include <Awesomium/BitmapSurface.h>
#include <Awesomium/STLHelpers.h>
#include <Awesomium/JSObject.h>
#include <Awesomium/WebSession.h>

#include <map>
#include "JSMethodHandler.h"
#include "UnitySurfaceFactory.h"
#include "LoadListener.h"
#include "ViewListener.h"

using namespace Awesomium;

namespace AwesomiumUnity
{

namespace
{
	std::map<WebView*, JSValue>			g_JSUnityObjects;
	JSMethodHandler*					g_JSMethodHandler = nullptr;
	UnitySurfaceFactory*				g_SurfaceFactory = nullptr;
	LoadListener*						g_LoadListener = nullptr;
	ViewListener*						g_ViewListener = nullptr;
}

std::list<WebView*>						g_WebViews;
WebSession*								g_WebSession = nullptr;


CALLBACK_DEFINE_HELPER(BeginLoadingFrame);
CALLBACK_DEFINE_HELPER(FailLoadingFrame);
CALLBACK_DEFINE_HELPER(FinishLoadingFrame);
CALLBACK_DEFINE_HELPER(DocumentReady);
CALLBACK_DEFINE_HELPER(ChangeAddressBar);
CALLBACK_DEFINE_HELPER(AddConsoleMessage);
CALLBACK_DEFINE_HELPER(ShowCreatedWebView);
CALLBACK_DEFINE_HELPER(JavaScriptExecFinished);
CALLBACK_DEFINE_HELPER(JavaScriptResultBool);
CALLBACK_DEFINE_HELPER(JavaScriptResultInt);
CALLBACK_DEFINE_HELPER(JavaScriptResultFloat);
CALLBACK_DEFINE_HELPER(JavaScriptResultString);
CALLBACK_DEFINE_HELPER(JavaScriptResultNull);
CALLBACK_DEFINE_HELPER(JavaScriptResultUndefined);
CALLBACK_DEFINE_HELPER(JavaScriptResultArray);
CALLBACK_DEFINE_HELPER(JavaScriptMethodCall);
CALLBACK_DEFINE_HELPER(JavaScriptMethodCallWithReturnValue);


Awesomium::JSObject* FindJSUnityObjectForWebView( Awesomium::WebView* _WebView )
{
	std::map<WebView*, JSValue>::iterator it = g_JSUnityObjects.find(_WebView);

	if (it == g_JSUnityObjects.end())
		return nullptr;

	return (&(it->second.ToObject()));
}

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
												  bool _WebSecurity)
{
	WebCore* webCore = WebCore::instance();
	if (webCore == nullptr)
	{
		WebConfig config = WebConfig();
		config.log_level = Awesomium::kLogLevel_Verbose;
		config.log_path = WSLit("awesomium_logs\\");
		config.plugin_path = WSLit(_PluginPath);
		webCore = WebCore::Initialize(config);

		if (AwesomiumUnity::g_JSMethodHandler == nullptr)
			AwesomiumUnity::g_JSMethodHandler = new AwesomiumUnity::JSMethodHandler();

		if (AwesomiumUnity::g_SurfaceFactory == nullptr)
			AwesomiumUnity::g_SurfaceFactory = new AwesomiumUnity::UnitySurfaceFactory();

		webCore->set_surface_factory(AwesomiumUnity::g_SurfaceFactory);

		if (AwesomiumUnity::g_WebSession == nullptr)
		{
			WebPreferences prefs = WebPreferences();
			prefs.enable_gpu_acceleration = _GPUAcceleration;
			prefs.enable_web_gl = _WebGL;
			prefs.enable_javascript = _JavaScript;
			prefs.enable_plugins = _Plugins;
			prefs.enable_web_audio = _WebAudio;
			prefs.enable_remote_fonts = _RemoteFonts;
			prefs.enable_app_cache = _AppCache;
			prefs.enable_dart = _Dart;
			prefs.enable_local_storage = _HTML5LocalStorage;
			prefs.enable_smooth_scrolling = _SmoothScrolling;
			prefs.enable_web_security = _WebSecurity;
			AwesomiumUnity::g_WebSession = webCore->CreateWebSession(WebString(_WebSessionPath), prefs);
		}

		if (AwesomiumUnity::g_LoadListener == nullptr)
			AwesomiumUnity::g_LoadListener = new AwesomiumUnity::LoadListener();

		if (AwesomiumUnity::g_ViewListener == nullptr)
			AwesomiumUnity::g_ViewListener = new AwesomiumUnity::ViewListener();
	}
}

extern "C" EXPORT_API void awe_webcore_shutdown(  )
{
	if (WebCore::instance())
	{
		WebCore::instance()->Update();

		WebCore::Shutdown();

		if (AwesomiumUnity::g_WebSession != nullptr)
			AwesomiumUnity::g_WebSession->Release();

		delete AwesomiumUnity::g_ViewListener;
		AwesomiumUnity::g_ViewListener = nullptr;

		delete AwesomiumUnity::g_LoadListener;
		AwesomiumUnity::g_LoadListener = nullptr;

		delete AwesomiumUnity::g_SurfaceFactory;
		AwesomiumUnity::g_SurfaceFactory = nullptr;

		delete AwesomiumUnity::g_JSMethodHandler;
		AwesomiumUnity::g_JSMethodHandler = nullptr;
	}
}

extern "C" EXPORT_API void* awe_webcore_createwebview( int _Width, int _Height )
{
	WebCore* webCore = WebCore::instance();
	if (webCore)
	{
		WebView* view = WebCore::instance()->CreateWebView(_Width, _Height, AwesomiumUnity::g_WebSession, kWebViewType_Offscreen);

		AwesomiumUnity::g_JSUnityObjects[view] = view->CreateGlobalJavascriptObject(WSLit("Unity"));

		// Attach listeners and handlers.
		view->set_js_method_handler(AwesomiumUnity::g_JSMethodHandler);
		view->set_load_listener(AwesomiumUnity::g_LoadListener);
		view->set_view_listener(AwesomiumUnity::g_ViewListener);

		AwesomiumUnity::g_WebViews.push_back(view);

		return (void*)view;
	}
	return nullptr;
}

extern "C" EXPORT_API void awe_webcore_destroywebview( WebView* _View )
{
	if (_View != nullptr)
	{			
		for (std::list<WebView*>::iterator it = AwesomiumUnity::g_WebViews.begin(); it != AwesomiumUnity::g_WebViews.end(); ++it)
		{
			if ((*it) == _View)
			{
				AwesomiumUnity::g_WebViews.erase(it);
				break;
			}
		}
		_View->Stop();

		while (_View->IsLoading())
		{
            WebCore::instance()->Update();
#ifdef UNITY_WIN
			Sleep(2);
#else
            sleep(2);
#endif
		}

		_View->Destroy();
	}
}

extern "C" EXPORT_API void awe_webcore_update()
{
	WebCore* webCore = WebCore::instance();
	if (webCore)
	{
		webCore->Update();
	}
}

extern "C" EXPORT_API bool awe_webcore_isrunning()
{
	return (WebCore::instance() != nullptr);
}

extern "C" EXPORT_API void awe_webcore_register_webview_callbacks(
	AwesomiumUnity::WebView_OnBeginLoadingFrame					_BeginLoadingFrame,
	AwesomiumUnity::WebView_OnFailLoadingFrame					_FailLoadingFrame,
	AwesomiumUnity::WebView_OnFinishLoadingFrame				_FinishLoadingFrame,
	AwesomiumUnity::WebView_OnDocumentReady						_DocumentReady,
	AwesomiumUnity::WebView_OnChangeAddressBar					_ChangeAddressBar,
	AwesomiumUnity::WebView_OnAddConsoleMessage					_AddConsoleMessage,
	AwesomiumUnity::WebView_OnShowCreatedWebView				_ShowCreatedWebView,
	AwesomiumUnity::WebView_OnJavaScriptExecFinished			_JavaScriptExecFinished,
	AwesomiumUnity::WebView_OnJavaScriptResultBool				_JavaScriptResultBool,
	AwesomiumUnity::WebView_OnJavaScriptResultInt				_JavaScriptResultInt,
	AwesomiumUnity::WebView_OnJavaScriptResultFloat				_JavaScriptResultFloat,
	AwesomiumUnity::WebView_OnJavaScriptResultString			_JavaScriptResultString,
	AwesomiumUnity::WebView_OnJavaScriptResultNull				_JavaScriptResultNull,
	AwesomiumUnity::WebView_OnJavaScriptResultUndefined			_JavaScriptResultUndefined,
	AwesomiumUnity::WebView_OnJavaScriptResultArray				_JavaScriptResultArray,
	AwesomiumUnity::WebView_OnJavaScriptMethodCall				_JavaScriptMethodCall,
	AwesomiumUnity::WebView_OnJavaScriptMethodCallWithReturnValue _JavaScriptMethodCallWithReturnValue)
{
#define ASSIGN_HELPER(name)		AwesomiumUnity::g_WebView_##name##Callback = _##name

	ASSIGN_HELPER(BeginLoadingFrame);
	ASSIGN_HELPER(FailLoadingFrame);
	ASSIGN_HELPER(FinishLoadingFrame);
	ASSIGN_HELPER(DocumentReady);
	ASSIGN_HELPER(ChangeAddressBar);
	ASSIGN_HELPER(AddConsoleMessage);
	ASSIGN_HELPER(ShowCreatedWebView);
	ASSIGN_HELPER(JavaScriptExecFinished);
	ASSIGN_HELPER(JavaScriptResultBool);
	ASSIGN_HELPER(JavaScriptResultInt);
	ASSIGN_HELPER(JavaScriptResultFloat);
	ASSIGN_HELPER(JavaScriptResultString);
	ASSIGN_HELPER(JavaScriptResultNull);
	ASSIGN_HELPER(JavaScriptResultUndefined);
	ASSIGN_HELPER(JavaScriptResultArray);
	ASSIGN_HELPER(JavaScriptMethodCall);
	ASSIGN_HELPER(JavaScriptMethodCallWithReturnValue);

#undef ASSIGN_HELPER
}
