#pragma once

#include "UnityPlugin.h"
#include <Awesomium/Platform.h>

using namespace Awesomium;

// Clears the session cache asynchronously.
extern "C" EXPORT_API void awe_websession_clear_cache();

// Clears the session cookies asynchronously.
extern "C" EXPORT_API void awe_websession_clear_cookies();

// Returns whether the session is stored on disk (versus in-memory, in which case all data is lost upon exist).
extern "C" EXPORT_API bool awe_websession_isondisk();

// Sets a cookie. Cookie string example: "key1=value1; key2=value2".
extern "C" EXPORT_API void awe_websession_setcookie(const wchar16* _URL, const wchar16* _CookieString, bool _IsHTTPOnly, bool _ForceSessionCookie);