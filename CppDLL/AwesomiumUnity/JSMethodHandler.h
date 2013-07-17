#pragma once

#include "UnityPlugin.h"

#include <Awesomium/JSObject.h>

namespace Awesomium
{
	class WebView;
	class WebString;
	class JSArray;
}

namespace AwesomiumUnity
{

typedef void (*OnMethodCall)( Awesomium::WebView* _Caller, const wchar16* _MethodName );

typedef Awesomium::JSValue (*OnMethodCallWithReturnValue)( Awesomium::WebView* _Caller, const wchar16* _MethodName );

class JSMethodHandler	: public Awesomium::JSMethodHandler
{
public:
	JSMethodHandler();
	~JSMethodHandler();

	void OnMethodCall(	Awesomium::WebView* caller,
						unsigned int remote_object_id,
						const Awesomium::WebString& method_name,
						const Awesomium::JSArray& args);

	Awesomium::JSValue OnMethodCallWithReturnValue(	Awesomium::WebView* caller,
													unsigned int remote_object_id,
													const Awesomium::WebString& method_name,
													const Awesomium::JSArray& args);
private:
};

}

// This should be called by the managed C# code at startup.
extern "C" EXPORT_API void awe_jsmethodhandler_register_callback_onmethodcall( AwesomiumUnity::OnMethodCall _Callback );

// This should also be called by the managed C# code at startup.
extern "C" EXPORT_API void awe_jsmethodhandler_register_callback_onmethodcallwithreturnvalue( AwesomiumUnity::OnMethodCallWithReturnValue _Callback );