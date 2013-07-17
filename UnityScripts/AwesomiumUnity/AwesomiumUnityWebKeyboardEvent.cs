using System.Runtime.InteropServices;

public struct AwesomiumUnityWebKeyboardEvent
{
	public AwesomiumUnityWebKeyType Type;

	public AwesomiumUnityWebKeyModifiers Modifiers;
	
	public AwesomiumUnityVirtualKey VirtualKeyCode;
	
	public int NativeKeyCode;
	
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
	public char[] KeyIdentifier;
	
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	public ushort[] Text;
	
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	public ushort[] UnmodifiedText;
	
	public bool IsSystemKey;
}
