#pragma once

#include "UnityPlugin.h"

#include <Awesomium/WebURL.h>
#include <Awesomium/WebViewListener.h>
#include <vector>

namespace Awesomium
{
}

namespace AwesomiumUnity
{

class ViewListener	: public Awesomium::WebViewListener::View
{
public:
	ViewListener();
	virtual ~ViewListener();
	
	virtual void OnChangeTitle(Awesomium::WebView* caller, const Awesomium::WebString& title) {}
	virtual void OnChangeAddressBar(Awesomium::WebView* caller, const Awesomium::WebURL& url);
	virtual void OnChangeTooltip(Awesomium::WebView* caller, const Awesomium::WebString& tooltip) {}
	virtual void OnChangeTargetURL(Awesomium::WebView* caller, const Awesomium::WebURL& url) {}
	virtual void OnChangeCursor(Awesomium::WebView* caller, Awesomium::Cursor cursor) {}
	virtual void OnChangeFocus(Awesomium::WebView* caller, Awesomium::FocusedElementType focused_type) {}
	virtual void OnAddConsoleMessage(Awesomium::WebView* caller, const Awesomium::WebString& message, int line_number, const Awesomium::WebString& source);
	virtual void OnShowCreatedWebView(Awesomium::WebView* caller, Awesomium::WebView* new_view, const Awesomium::WebURL& opener_url, const Awesomium::WebURL& target_url, const Awesomium::Rect& initial_pos, bool is_popup);

private:
#ifdef _DEBUG
	int										mID;
#endif
};

}