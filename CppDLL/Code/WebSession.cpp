#include "WebSession.h"
#include "WebCore.h"
#include <Awesomium/WebURL.h>
#include <Awesomium/WebSession.h>


extern "C" EXPORT_API void awe_websession_clear_cache()
{
	if (AwesomiumUnity::g_WebSession != nullptr)
		AwesomiumUnity::g_WebSession->ClearCache();
}


extern "C" EXPORT_API void awe_websession_clear_cookies()
{
	if (AwesomiumUnity::g_WebSession != nullptr)
		AwesomiumUnity::g_WebSession->ClearCookies();
}

extern "C" EXPORT_API bool awe_websession_isondisk()
{
	if (AwesomiumUnity::g_WebSession != nullptr)
		return AwesomiumUnity::g_WebSession->IsOnDisk();
	return false;
}

extern "C" EXPORT_API void awe_websession_setcookie(const wchar16* _URL, const wchar16* _CookieString, bool _IsHTTPOnly, bool _ForceSessionCookie)
{
	if (AwesomiumUnity::g_WebSession != nullptr)
		AwesomiumUnity::g_WebSession->SetCookie(Awesomium::WebURL(Awesomium::WebString(_URL)), Awesomium::WebString(_CookieString), _IsHTTPOnly, _ForceSessionCookie);
}