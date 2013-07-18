public enum AwesomiumUnityError
{
	None = 0,        ///< No error (everything is cool!)
	BadParameters,   ///< Bad parameters were supplied.
	ObjectGone,      ///< The object no longer exists.
	ConnectionGone,  ///< The IPC connection no longest exists.
	TimedOut,        ///< The operation timed out.
	WebViewGone,     ///< The WebView no longer exists.
	Generic,         ///< A generic error was encountered.
}