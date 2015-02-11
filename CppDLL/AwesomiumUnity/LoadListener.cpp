#include "LoadListener.h"

namespace AwesomiumUnity
{

static int sNextID = 1;

LoadListener::LoadListener()
#ifdef _DEBUG
	: mID(sNextID++)
#endif
{

}

LoadListener::~LoadListener()
{
	mBeginLoadingFrameCallbacks.clear();
	mFailLoadingFrameCallbacks.clear();
	mFinishLoadingFrameCallbacks.clear();
	mDocumentReadyCallbacks.clear();
}

void LoadListener::OnBeginLoadingFrame(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const Awesomium::WebURL& url, bool is_error_page)
{
	for (BeginLoadingFrameCallback callback : mBeginLoadingFrameCallbacks)
		if (callback != nullptr)
			callback(url.spec().data(), frame_id, is_main_frame, is_error_page);
}

void LoadListener::OnFailLoadingFrame(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const Awesomium::WebURL& url, int error_code, const Awesomium::WebString& error_desc)
{
	for (FailLoadingFrameCallback callback : mFailLoadingFrameCallbacks)
		if (callback != nullptr)
			callback(url.spec().data(), error_code, error_desc.data(), frame_id, is_main_frame);
}

void LoadListener::OnFinishLoadingFrame(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const Awesomium::WebURL& url)
{
	for (FinishLoadingFrameCallback callback : mFinishLoadingFrameCallbacks)
		if (callback != nullptr)
			callback(url.spec().data(), frame_id, is_main_frame);
}

void LoadListener::OnDocumentReady(Awesomium::WebView* caller, const Awesomium::WebURL& url)
{
	for (DocumentReadyCallback callback : mDocumentReadyCallbacks)
		if (callback != nullptr)
			callback(url.spec().data());
}

}