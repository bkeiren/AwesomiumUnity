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

class LoadListener	: public Awesomium::WebViewListener::Load
{
public:
	LoadListener();
	virtual ~LoadListener();

	virtual void OnBeginLoadingFrame(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const Awesomium::WebURL& url, bool is_error_page);
	virtual void OnFailLoadingFrame(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const Awesomium::WebURL& url, int error_code, const Awesomium::WebString& error_desc);
	virtual void OnFinishLoadingFrame(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const Awesomium::WebURL& url);
	virtual void OnDocumentReady(Awesomium::WebView* caller, const Awesomium::WebURL& url);
private:
#ifdef _DEBUG
	int										mID;
#endif
};

}