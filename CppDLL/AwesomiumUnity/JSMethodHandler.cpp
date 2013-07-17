#include "JSMethodHandler.h"
#include <Awesomium/JSValue.h>

namespace AwesomiumUnity
{

namespace
{
	OnMethodCall g_OnMethodCallCallback = 0;
	OnMethodCallWithReturnValue g_OnMethodCallWithReturnValueCallback = 0;
}

JSMethodHandler::JSMethodHandler()
{

}

JSMethodHandler::~JSMethodHandler()
{

}

void JSMethodHandler::OnMethodCall(		Awesomium::WebView* caller,
										unsigned int remote_object_id,
										const Awesomium::WebString& method_name,
										const Awesomium::JSArray& args )
{
	if (g_OnMethodCallCallback)
		g_OnMethodCallCallback(caller, method_name.data());
}

Awesomium::JSValue JSMethodHandler::OnMethodCallWithReturnValue(	Awesomium::WebView* caller,
																	unsigned int remote_object_id,
																	const Awesomium::WebString& method_name,
																	const Awesomium::JSArray& args )
{
	if (g_OnMethodCallWithReturnValueCallback)
		return g_OnMethodCallWithReturnValueCallback(caller, method_name.data());
	return Awesomium::JSValue(0);
}

}

extern "C" EXPORT_API void awe_jsmethodhandler_register_callback_onmethodcall( AwesomiumUnity::OnMethodCall _Callback )
{
	AwesomiumUnity::g_OnMethodCallCallback = _Callback;
}

extern "C" EXPORT_API void awe_jsmethodhandler_register_callback_onmethodcallwithreturnvalue( AwesomiumUnity::OnMethodCallWithReturnValue _Callback )
{
	AwesomiumUnity::g_OnMethodCallWithReturnValueCallback = _Callback;
}