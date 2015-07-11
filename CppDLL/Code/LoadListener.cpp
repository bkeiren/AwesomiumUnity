#include "LoadListener.h"
#include "WebCore.h"

namespace AwesomiumUnity
{

#ifdef _DEBUG
static int sNextID = 1;
#endif

LoadListener::LoadListener()
#ifdef _DEBUG
	: mID(sNextID++)
#endif
{

}

LoadListener::~LoadListener()
{
}

void LoadListener::OnBeginLoadingFrame(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const Awesomium::WebURL& url, bool is_error_page)
{
	if (g_WebView_BeginLoadingFrameCallback != nullptr)
		g_WebView_BeginLoadingFrameCallback(caller, frame_id, is_main_frame, url.spec().data(), is_error_page);
}

void LoadListener::OnFailLoadingFrame(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const Awesomium::WebURL& url, int error_code, const Awesomium::WebString& error_desc)
{
	if (g_WebView_FailLoadingFrameCallback != nullptr)
		g_WebView_FailLoadingFrameCallback(caller, frame_id, is_main_frame, url.spec().data(), error_code, error_desc.data());
}

void LoadListener::OnFinishLoadingFrame(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const Awesomium::WebURL& url)
{
	if (g_WebView_FinishLoadingFrameCallback != nullptr)
		g_WebView_FinishLoadingFrameCallback(caller, frame_id, is_main_frame, url.spec().data());
}

void LoadListener::OnDocumentReady(Awesomium::WebView* caller, const Awesomium::WebURL& url)
{
	if (g_WebView_DocumentReadyCallback != nullptr)
		g_WebView_DocumentReadyCallback(caller, url.spec().data());
}

}