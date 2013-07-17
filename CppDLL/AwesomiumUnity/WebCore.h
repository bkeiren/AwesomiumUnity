#pragma once

#include "UnityPlugin.h"

namespace Awesomium
{
	class WebView;
	class JSObject;
}

namespace AwesomiumUnity
{

Awesomium::JSObject* FindJSUnityObjectForWebView( Awesomium::WebView* _WebView );

}

extern "C" EXPORT_API void awe_webcore_initialize(  );

extern "C" EXPORT_API void awe_webcore_shutdown(  );

extern "C" EXPORT_API void* awe_webcore_createwebview( int _Width, int _Height );

extern "C" EXPORT_API void awe_webcore_update();

extern "C" EXPORT_API bool awe_webcore_isrunning();