#include "JSMethodHandler.h"
#include "WebCore.h"
#include <Awesomium/JSValue.h>

namespace AwesomiumUnity
{

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
	if (AwesomiumUnity::g_WebView_JavaScriptMethodCallCallback)
		AwesomiumUnity::g_WebView_JavaScriptMethodCallCallback(caller, method_name.data());
}

Awesomium::JSValue JSMethodHandler::OnMethodCallWithReturnValue(	Awesomium::WebView* caller,
																	unsigned int remote_object_id,
																	const Awesomium::WebString& method_name,
																	const Awesomium::JSArray& args )
{
	if (AwesomiumUnity::g_WebView_JavaScriptMethodCallWithReturnValueCallback)
		return AwesomiumUnity::g_WebView_JavaScriptMethodCallWithReturnValueCallback(caller, method_name.data());
	return Awesomium::JSValue::Null();
}

}