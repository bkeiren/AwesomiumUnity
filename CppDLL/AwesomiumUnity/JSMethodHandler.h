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