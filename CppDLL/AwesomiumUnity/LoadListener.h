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

	typedef void (*BeginLoadingFrameCallback)(const wchar16*, int64, bool, bool);
	typedef void (*FailLoadingFrameCallback)(const wchar16*, int, const wchar16*, int64, bool);
	typedef void (*FinishLoadingFrameCallback)(const wchar16*, int64, bool);
	typedef void (*DocumentReadyCallback)(const wchar16*);

	virtual void OnBeginLoadingFrame(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const Awesomium::WebURL& url, bool is_error_page);
	virtual void OnFailLoadingFrame(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const Awesomium::WebURL& url, int error_code, const Awesomium::WebString& error_desc);
	virtual void OnFinishLoadingFrame(Awesomium::WebView* caller, int64 frame_id, bool is_main_frame, const Awesomium::WebURL& url);
	virtual void OnDocumentReady(Awesomium::WebView* caller, const Awesomium::WebURL& url);

	void RegisterBeginLoadingFrameCallback(BeginLoadingFrameCallback _Callback)			{ mBeginLoadingFrameCallbacks.push_back(_Callback); }
	void RegisterFailLoadingFrameCallback(FailLoadingFrameCallback _Callback)			{ mFailLoadingFrameCallbacks.push_back(_Callback); }
	void RegisterFinishLoadingFrameCallback(FinishLoadingFrameCallback _Callback)		{ mFinishLoadingFrameCallbacks.push_back(_Callback); }
	void RegisterDocumentReadyCallback(DocumentReadyCallback _Callback)					{ mDocumentReadyCallbacks.push_back(_Callback); }
private:
	std::vector<BeginLoadingFrameCallback>	mBeginLoadingFrameCallbacks;
	std::vector<FailLoadingFrameCallback>	mFailLoadingFrameCallbacks;
	std::vector<FinishLoadingFrameCallback>	mFinishLoadingFrameCallbacks;
	std::vector<DocumentReadyCallback>		mDocumentReadyCallbacks;
#ifdef _DEBUG
	int										mID;
#endif
};

}