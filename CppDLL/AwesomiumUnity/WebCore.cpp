#include "WebCore.h"

#include <Awesomium/WebCore.h>
#include <Awesomium/BitmapSurface.h>
#include <Awesomium/STLHelpers.h>
#include <Awesomium/JSObject.h>

#include <map>
#include "JSMethodHandler.h"
#include "UnitySurfaceFactory.h"

using namespace Awesomium;

namespace AwesomiumUnity
{

namespace
{
	WebSession* g_WebSession = 0;
	JSMethodHandler* g_JSMethodHandler = 0;
	std::map<WebView*, JSValue> m_JSUnityObjects;
	UnitySurfaceFactory* g_SurfaceFactory = 0;
}

Awesomium::JSObject* FindJSUnityObjectForWebView( Awesomium::WebView* _WebView )
{
	std::map<WebView*, JSValue>::iterator it = m_JSUnityObjects.find(_WebView);

	if (it == m_JSUnityObjects.end())
		return 0;

	return (&(it->second.ToObject()));
}

}

extern "C" EXPORT_API void awe_webcore_initialize(  )
{
	WebCore* webCore = WebCore::instance();
	if (webCore == 0)
	{
		WebConfig config = WebConfig();
		config.log_level = Awesomium::kLogLevel_Verbose;
		config.log_path = WSLit("awesomium_logs\\");
		webCore = WebCore::Initialize(config);

		if (AwesomiumUnity::g_SurfaceFactory == 0)
			AwesomiumUnity::g_SurfaceFactory = new AwesomiumUnity::UnitySurfaceFactory();

		webCore->set_surface_factory(AwesomiumUnity::g_SurfaceFactory);

		if (AwesomiumUnity::g_WebSession == 0)
		{
			WebPreferences prefs = WebPreferences();
			prefs.enable_gpu_acceleration = true;
			prefs.enable_web_gl = true;
			prefs.enable_javascript = true;
			prefs.enable_plugins = true;
			prefs.enable_web_audio = true;
			prefs.enable_remote_fonts = true;
			AwesomiumUnity::g_WebSession = webCore->CreateWebSession(WebString(), prefs);
		}
	}
}

extern "C" EXPORT_API void awe_webcore_shutdown(  )
{
	if (WebCore::instance())
	{
		WebCore::Shutdown();

		delete AwesomiumUnity::g_SurfaceFactory;
		AwesomiumUnity::g_SurfaceFactory = 0;

		delete AwesomiumUnity::g_JSMethodHandler;
		AwesomiumUnity::g_JSMethodHandler = 0;
	}
}

extern "C" EXPORT_API void* awe_webcore_createwebview( int _Width, int _Height )
{
	WebCore* webCore = WebCore::instance();
	if (webCore)
	{
		WebView* view = WebCore::instance()->CreateWebView(_Width, _Height, AwesomiumUnity::g_WebSession, kWebViewType_Offscreen);

		if (AwesomiumUnity::g_JSMethodHandler == 0)
			AwesomiumUnity::g_JSMethodHandler = new AwesomiumUnity::JSMethodHandler();

		AwesomiumUnity::m_JSUnityObjects[view] = view->CreateGlobalJavascriptObject(WSLit("Unity"));
		view->set_js_method_handler(AwesomiumUnity::g_JSMethodHandler);

		return (void*)view;
	}
	return 0;
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
	return (WebCore::instance() != 0);
}