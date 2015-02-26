#include "ViewListener.h"
#include "WebCore.h"

namespace AwesomiumUnity
{

static int sNextID = 1;

ViewListener::ViewListener()
#ifdef _DEBUG
	: mID(sNextID++)
#endif
{
}

ViewListener::~ViewListener()
{
}

void ViewListener::OnChangeAddressBar(Awesomium::WebView* caller, const Awesomium::WebURL& url)
{
	if (g_WebView_ChangeAddressBarCallback != nullptr)
		g_WebView_ChangeAddressBarCallback(caller, url.spec().data());
}

void ViewListener::OnAddConsoleMessage(Awesomium::WebView* caller, const Awesomium::WebString& message, int line_number, const Awesomium::WebString& source)
{
	if (g_WebView_AddConsoleMessageCallback != nullptr)
		g_WebView_AddConsoleMessageCallback(caller, message.data(), line_number, source.data());
}

void ViewListener::OnShowCreatedWebView(Awesomium::WebView* caller, Awesomium::WebView* new_view, const Awesomium::WebURL& opener_url, const Awesomium::WebURL& target_url, const Awesomium::Rect& initial_pos, bool is_popup)
{
	if (g_WebView_ShowCreatedWebViewCallback != nullptr)
		g_WebView_ShowCreatedWebViewCallback(caller, new_view, opener_url.spec().data(), target_url.spec().data(), is_popup);
}

}