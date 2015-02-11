using UnityEngine;
using System.Collections;


public class AwesomiumUnityWebTexture : MonoBehaviour 
{
	public int m_Width = 512;
	public int m_Height = 512;
	public string m_URL = "http://www.google.com/";
	public bool m_FlipX = false;
	public bool m_FlipY = false;
	public bool m_Interactive = true;
	
	[SerializeField]
	private bool m_Transparent = false;
	
	private AwesomiumUnityWebView m_WebView = null;
	private bool m_MouseIsOver = false;
	private bool m_HasFocus = false;

	[SerializeField]	// Only for testing purposes.
	private Texture2D m_Texture = null;
	
	static private int[] MouseButtonsMap = {0, 2, 1};
	
	public AwesomiumUnityWebView WebView
	{
		get	
		{
			return m_WebView;	
		}
	}
	
	public Texture2D WebTexture
	{
		get
		{
			return m_Texture;	
		}
	}
	
	public bool Transparent
	{
		get	
		{
			return m_Transparent;	
		}
		set
		{
			if (value != m_Transparent)
			{
				m_Transparent = value;
				m_WebView.SetTransparent(m_Transparent);
			}
		}
	}
	
	// Use this for initialization
	void Awake () 
	{
		AwesomiumUnityWebCore.EnsureInitialized();
		
		// Call resize which will create a texture and a webview for us since both do not exist yet at this point.
		Resize(m_Width, m_Height);
		
		if (guiTexture)
		{
			guiTexture.texture = m_Texture;
		}
		else if (renderer && renderer.material)
		{		
			renderer.material.mainTexture = m_Texture;
			renderer.material.mainTextureScale = new Vector2(	Mathf.Abs(renderer.material.mainTextureScale.x) * (m_FlipX ? -1.0f : 1.0f),
																Mathf.Abs(renderer.material.mainTextureScale.y) * (m_FlipY ? -1.0f : 1.0f));
		}
		else
		{
			Debug.LogWarning("There is no Renderer or guiTexture attached to this GameObject! AwesomiumUnityWebTexture will render to a texture but it will not be visible.");
		}
		
		// Now load the URL.
		// IMPORTANT: For some reason, a WebView MUST have loaded something atleast ONCE before calling ANY other function on it (think input injection).
		// Therefore, there is no option available to delay the loading of the URL and it is forced in this constructor. (Note how we wait until the loading
		// is complete before we exit the constructor).
		LoadURL(m_URL);
		
		while (m_WebView.IsLoading)
		{
			AwesomiumUnityWebCore.Update();	
		}
		
		m_WebView.SetTransparent(m_Transparent);
	}
	
	void OnMouseOver()
	{
		if (!m_Interactive) return;
		
		if (m_WebView != null && !m_WebView.IsLoading)
		{
			if (guiTexture)
			{
				Rect guiTextureScreenRect = guiTexture.GetScreenRect();
				int MouseX = (int)Input.mousePosition.x;
				int MouseY = Screen.height - (int)Input.mousePosition.y;
				
				MouseX = (int)(((MouseX - guiTextureScreenRect.x) / guiTextureScreenRect.width) * m_Width);
				MouseY = (int)(((MouseY - guiTextureScreenRect.y) / guiTextureScreenRect.height) * m_Height);

				m_WebView.InjectMouseMove(MouseX, MouseY);
			}
			else if (collider)
			{
				RaycastHit HitInfo;
				if (this.collider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out HitInfo, 10000.0f))
				{			
					Vector2 v = HitInfo.textureCoord;
					v.y = 1.0f - v.y;
					v.Scale(new Vector2((float)m_WebView.Width, (float)m_WebView.Height));

					if (m_FlipX)
						v.x = m_WebView.Width - v.x;

					if (m_FlipY)
						v.y = m_WebView.Height - v.y;
					
					Debug.Log("MOUSE: " + v);
					m_WebView.InjectMouseMove((int)v.x, (int)v.y);
				}	
			}
		}
	}
	
	void OnMouseEnter()
	{
		m_MouseIsOver = true;
	}
	
	void OnMouseExit()
	{
		m_MouseIsOver = false;
	}
	
	void OnGUI()
	{
		// This function should do input injection (if enabled), and drawing.
		if (m_WebView == null || m_WebView.IsLoading) return;
		
		Event e = Event.current;
		
		switch (e.type)
		{
		case EventType.Repaint:	
		{
			if (m_WebView.IsDirty)
			{
				m_WebView.CopyBufferToTexture(m_Texture.GetNativeTexturePtr(), m_Texture.width, m_Texture.height);
			}
			break;	
		}
		case EventType.MouseDown:
		{
			if (!m_Interactive) 
				break;
			
			if (m_MouseIsOver)
			{
				m_WebView.InjectMouseDown(MapMouseButtons(Event.current.button));
			}
			break;	
		}
		case EventType.MouseUp:
		{
			if (!m_Interactive)
				break;
				
			if (m_MouseIsOver)
			{
				Focus();
				m_WebView.InjectMouseUp(MapMouseButtons(Event.current.button));
			}
			else
			{
				Unfocus();
			}
			break;	
		}
		case EventType.KeyDown:
		{
			if (!m_Interactive) 
				break;
			
			if (m_HasFocus)
			{
				if (e.character == 0)
	            {
	                AwesomiumUnityWebKeyboardEvent keyEvent = new AwesomiumUnityWebKeyboardEvent();
	                keyEvent.Type = AwesomiumUnityWebKeyType.KeyDown;
	                keyEvent.VirtualKeyCode = MapKeys(e);
	                keyEvent.Modifiers = MapModifiers(e);
	                m_WebView.InjectKeyboardEvent(keyEvent);
	            }
	            else
	            {
	                AwesomiumUnityWebKeyboardEvent keyEvent = new AwesomiumUnityWebKeyboardEvent();
	                keyEvent.Type = AwesomiumUnityWebKeyType.Char;
	                keyEvent.Text = new ushort[] { e.character, 0, 0, 0 };
	                keyEvent.Modifiers = MapModifiers(e);
	                m_WebView.InjectKeyboardEvent(keyEvent);
	            }
			}
			break;	
		}
		case EventType.KeyUp:
		{
			if (!m_Interactive)
				break;
			
			if (m_HasFocus)
			{
				AwesomiumUnityWebKeyboardEvent keyEvent = new AwesomiumUnityWebKeyboardEvent();
	            keyEvent.Type = AwesomiumUnityWebKeyType.KeyUp;
	            keyEvent.VirtualKeyCode = MapKeys(e);
	            keyEvent.Modifiers = MapModifiers(e);
	            m_WebView.InjectKeyboardEvent(keyEvent);
			}
			break;	
		}
		case EventType.ScrollWheel:
		{
			if (!m_Interactive)
				break;
			
			if (m_HasFocus)
			{
				m_WebView.InjectMouseWheel((int)e.delta.y * -10, (int)e.delta.x);
			}
			break;	
		}
		}
	}
	
	static private int MapMouseButtons( int _Button )
	{
		return MouseButtonsMap[_Button];
	}
	
	static private AwesomiumUnityWebKeyModifiers MapModifiers( Event e )
    {
        int modifiers = 0;

        if (e.control)
            modifiers |= (int)AwesomiumUnityWebKeyModifiers.ControlKey;

        if (e.shift)
            modifiers |= (int)AwesomiumUnityWebKeyModifiers.ShiftKey;

        if (e.alt)
            modifiers |= (int)AwesomiumUnityWebKeyModifiers.AltKey;

        return (AwesomiumUnityWebKeyModifiers)modifiers;
    }

    static private AwesomiumUnityVirtualKey MapKeys( Event e )
    {
        switch (e.keyCode)
        {
            case KeyCode.Backspace: return AwesomiumUnityVirtualKey.BACK;
            case KeyCode.Delete: return AwesomiumUnityVirtualKey.DELETE;
            case KeyCode.Tab: return AwesomiumUnityVirtualKey.TAB;
            case KeyCode.Clear: return AwesomiumUnityVirtualKey.CLEAR;
            case KeyCode.Return: return AwesomiumUnityVirtualKey.RETURN;
            case KeyCode.Pause: return AwesomiumUnityVirtualKey.PAUSE;
            case KeyCode.Escape: return AwesomiumUnityVirtualKey.ESCAPE;
            case KeyCode.Space: return AwesomiumUnityVirtualKey.SPACE;
            case KeyCode.Keypad0: return AwesomiumUnityVirtualKey.NUMPAD0;
            case KeyCode.Keypad1: return AwesomiumUnityVirtualKey.NUMPAD1;
            case KeyCode.Keypad2: return AwesomiumUnityVirtualKey.NUMPAD2;
            case KeyCode.Keypad3: return AwesomiumUnityVirtualKey.NUMPAD3;
            case KeyCode.Keypad4: return AwesomiumUnityVirtualKey.NUMPAD4;
            case KeyCode.Keypad5: return AwesomiumUnityVirtualKey.NUMPAD5;
            case KeyCode.Keypad6: return AwesomiumUnityVirtualKey.NUMPAD6;
            case KeyCode.Keypad7: return AwesomiumUnityVirtualKey.NUMPAD7;
            case KeyCode.Keypad8: return AwesomiumUnityVirtualKey.NUMPAD8;
            case KeyCode.Keypad9: return AwesomiumUnityVirtualKey.NUMPAD9;
            case KeyCode.KeypadPeriod: return AwesomiumUnityVirtualKey.DECIMAL;
            case KeyCode.KeypadDivide: return AwesomiumUnityVirtualKey.DIVIDE;
            case KeyCode.KeypadMultiply: return AwesomiumUnityVirtualKey.MULTIPLY;
            case KeyCode.KeypadMinus: return AwesomiumUnityVirtualKey.SUBTRACT;
            case KeyCode.KeypadPlus: return AwesomiumUnityVirtualKey.ADD;
            case KeyCode.KeypadEnter: return AwesomiumUnityVirtualKey.SEPARATOR;
            case KeyCode.KeypadEquals: return AwesomiumUnityVirtualKey.UNKNOWN;
            case KeyCode.UpArrow: return AwesomiumUnityVirtualKey.UP;
            case KeyCode.DownArrow: return AwesomiumUnityVirtualKey.DOWN;
            case KeyCode.RightArrow: return AwesomiumUnityVirtualKey.RIGHT;
            case KeyCode.LeftArrow: return AwesomiumUnityVirtualKey.LEFT;
            case KeyCode.Insert: return AwesomiumUnityVirtualKey.INSERT;
            case KeyCode.Home: return AwesomiumUnityVirtualKey.HOME;
            case KeyCode.End: return AwesomiumUnityVirtualKey.END;
            case KeyCode.PageUp: return AwesomiumUnityVirtualKey.PRIOR;
            case KeyCode.PageDown: return AwesomiumUnityVirtualKey.NEXT;
            case KeyCode.F1: return AwesomiumUnityVirtualKey.F1;
            case KeyCode.F2: return AwesomiumUnityVirtualKey.F2;
            case KeyCode.F3: return AwesomiumUnityVirtualKey.F3;
            case KeyCode.F4: return AwesomiumUnityVirtualKey.F4;
            case KeyCode.F5: return AwesomiumUnityVirtualKey.F5;
            case KeyCode.F6: return AwesomiumUnityVirtualKey.F6;
            case KeyCode.F7: return AwesomiumUnityVirtualKey.F7;
            case KeyCode.F8: return AwesomiumUnityVirtualKey.F8;
            case KeyCode.F9: return AwesomiumUnityVirtualKey.F9;
            case KeyCode.F10: return AwesomiumUnityVirtualKey.F10;
            case KeyCode.F11: return AwesomiumUnityVirtualKey.F11;
            case KeyCode.F12: return AwesomiumUnityVirtualKey.F12;
            case KeyCode.F13: return AwesomiumUnityVirtualKey.F13;
            case KeyCode.F14: return AwesomiumUnityVirtualKey.F14;
            case KeyCode.F15: return AwesomiumUnityVirtualKey.F15;
            case KeyCode.Alpha0: return AwesomiumUnityVirtualKey.NUM_0;
            case KeyCode.Alpha1: return AwesomiumUnityVirtualKey.NUM_1;
            case KeyCode.Alpha2: return AwesomiumUnityVirtualKey.NUM_2;
            case KeyCode.Alpha3: return AwesomiumUnityVirtualKey.NUM_3;
            case KeyCode.Alpha4: return AwesomiumUnityVirtualKey.NUM_4;
            case KeyCode.Alpha5: return AwesomiumUnityVirtualKey.NUM_5;
            case KeyCode.Alpha6: return AwesomiumUnityVirtualKey.NUM_6;
            case KeyCode.Alpha7: return AwesomiumUnityVirtualKey.NUM_7;
            case KeyCode.Alpha8: return AwesomiumUnityVirtualKey.NUM_8;
            case KeyCode.Alpha9: return AwesomiumUnityVirtualKey.NUM_9;
            case KeyCode.Exclaim: return AwesomiumUnityVirtualKey.NUM_1;
            case KeyCode.DoubleQuote: return AwesomiumUnityVirtualKey.OEM_7;
            case KeyCode.Hash: return AwesomiumUnityVirtualKey.NUM_3;
            case KeyCode.Dollar: return AwesomiumUnityVirtualKey.NUM_4;
            case KeyCode.Ampersand: return AwesomiumUnityVirtualKey.NUM_7;
            case KeyCode.Quote: return AwesomiumUnityVirtualKey.OEM_7;
            case KeyCode.LeftParen: return AwesomiumUnityVirtualKey.NUM_9;
            case KeyCode.RightParen: return AwesomiumUnityVirtualKey.NUM_0;
            case KeyCode.Asterisk: return AwesomiumUnityVirtualKey.NUM_8;
            case KeyCode.Plus: return AwesomiumUnityVirtualKey.OEM_PLUS;
            case KeyCode.Comma: return AwesomiumUnityVirtualKey.OEM_COMMA;
            case KeyCode.Minus: return AwesomiumUnityVirtualKey.OEM_MINUS;
            case KeyCode.Period: return AwesomiumUnityVirtualKey.OEM_PERIOD;
            case KeyCode.Slash: return AwesomiumUnityVirtualKey.OEM_2;
            case KeyCode.Colon: return AwesomiumUnityVirtualKey.OEM_1;
            case KeyCode.Semicolon: return AwesomiumUnityVirtualKey.OEM_1;
            case KeyCode.Less: return AwesomiumUnityVirtualKey.OEM_COMMA;
            case KeyCode.Equals: return AwesomiumUnityVirtualKey.OEM_PLUS;
            case KeyCode.Greater: return AwesomiumUnityVirtualKey.OEM_PERIOD;
            case KeyCode.Question: return AwesomiumUnityVirtualKey.OEM_2;
            case KeyCode.At: return AwesomiumUnityVirtualKey.NUM_2;
            case KeyCode.LeftBracket: return AwesomiumUnityVirtualKey.OEM_4;
            case KeyCode.Backslash: return AwesomiumUnityVirtualKey.OEM_102;
            case KeyCode.RightBracket: return AwesomiumUnityVirtualKey.OEM_6;
            case KeyCode.Caret: return AwesomiumUnityVirtualKey.NUM_6;
            case KeyCode.Underscore: return AwesomiumUnityVirtualKey.OEM_MINUS;
            case KeyCode.BackQuote: return AwesomiumUnityVirtualKey.OEM_3;
            case KeyCode.A: return AwesomiumUnityVirtualKey.A;
            case KeyCode.B: return AwesomiumUnityVirtualKey.B;
            case KeyCode.C: return AwesomiumUnityVirtualKey.C;
            case KeyCode.D: return AwesomiumUnityVirtualKey.D;
            case KeyCode.E: return AwesomiumUnityVirtualKey.E;
            case KeyCode.F: return AwesomiumUnityVirtualKey.F;
            case KeyCode.G: return AwesomiumUnityVirtualKey.G;
            case KeyCode.H: return AwesomiumUnityVirtualKey.H;
            case KeyCode.I: return AwesomiumUnityVirtualKey.I;
            case KeyCode.J: return AwesomiumUnityVirtualKey.J;
            case KeyCode.K: return AwesomiumUnityVirtualKey.K;
            case KeyCode.L: return AwesomiumUnityVirtualKey.L;
            case KeyCode.M: return AwesomiumUnityVirtualKey.M;
            case KeyCode.N: return AwesomiumUnityVirtualKey.N;
            case KeyCode.O: return AwesomiumUnityVirtualKey.O;
            case KeyCode.P: return AwesomiumUnityVirtualKey.P;
            case KeyCode.Q: return AwesomiumUnityVirtualKey.Q;
            case KeyCode.R: return AwesomiumUnityVirtualKey.R;
            case KeyCode.S: return AwesomiumUnityVirtualKey.S;
            case KeyCode.T: return AwesomiumUnityVirtualKey.T;
            case KeyCode.U: return AwesomiumUnityVirtualKey.U;
            case KeyCode.V: return AwesomiumUnityVirtualKey.V;
            case KeyCode.W: return AwesomiumUnityVirtualKey.W;
            case KeyCode.X: return AwesomiumUnityVirtualKey.X;
            case KeyCode.Y: return AwesomiumUnityVirtualKey.Y;
            case KeyCode.Z: return AwesomiumUnityVirtualKey.Z;
            case KeyCode.Numlock: return AwesomiumUnityVirtualKey.NUMLOCK;
            case KeyCode.CapsLock: return AwesomiumUnityVirtualKey.CAPITAL;
            case KeyCode.ScrollLock: return AwesomiumUnityVirtualKey.SCROLL;
            case KeyCode.RightShift: return AwesomiumUnityVirtualKey.RSHIFT;
            case KeyCode.LeftShift: return AwesomiumUnityVirtualKey.LSHIFT;
            case KeyCode.RightControl: return AwesomiumUnityVirtualKey.RCONTROL;
            case KeyCode.LeftControl: return AwesomiumUnityVirtualKey.LCONTROL;
            case KeyCode.RightAlt: return AwesomiumUnityVirtualKey.RMENU;
            case KeyCode.LeftAlt: return AwesomiumUnityVirtualKey.LMENU;
            case KeyCode.LeftApple: return AwesomiumUnityVirtualKey.LWIN;
            case KeyCode.LeftWindows: return AwesomiumUnityVirtualKey.LWIN;
            case KeyCode.RightApple: return AwesomiumUnityVirtualKey.RWIN;
            case KeyCode.RightWindows: return AwesomiumUnityVirtualKey.RWIN;
            case KeyCode.AltGr: return AwesomiumUnityVirtualKey.UNKNOWN;
            case KeyCode.Help: return AwesomiumUnityVirtualKey.HELP;
            case KeyCode.Print: return AwesomiumUnityVirtualKey.PRINT;
            case KeyCode.SysReq: return AwesomiumUnityVirtualKey.UNKNOWN;
            case KeyCode.Break: return AwesomiumUnityVirtualKey.PAUSE;
            case KeyCode.Menu: return AwesomiumUnityVirtualKey.MENU;
            default: return 0;
        }

    }
	
	public void Reload()
	{
		m_WebView.Reload();
	}
		
	public void Resize( int _Width, int _Height )
	{
		m_Width = _Width;
		m_Height = _Height;
		
		if (m_Texture == null)
		{
			m_Texture = new Texture2D(m_Width, m_Height, TextureFormat.RGBA32, false);
		}
		else
		{	
			m_Texture.Resize(m_Width, m_Height, TextureFormat.RGBA32, false);
			m_Texture.Apply(false, false);
		}
		//m_Texture.filterMode = FilterMode.Point;
		
		if (m_WebView != null) 
		{
			m_WebView.Resize(m_Width, m_Height);
		}
		else
		{
			m_WebView = AwesomiumUnityWebCore.CreateWebView(m_Width, m_Height);			
		}
	}
	
	public void LoadURL( string _URL )
	{
		m_URL = _URL;
		m_WebView.LoadURL(m_URL);
	}
	
	void OnDestroy()
	{
		if (m_WebView != null)
		{
			m_WebView.Destroy ();
			m_WebView = null;
		}
	}
	
	void OnApplicationQuit()
	{
			
	}
	
	void Focus()
	{
		m_WebView.Focus();
		m_HasFocus = true;	
	}
	
	void Unfocus()
	{
		m_WebView.Unfocus();
		m_HasFocus = false;
	}
}
