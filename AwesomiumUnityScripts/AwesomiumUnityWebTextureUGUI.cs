using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AwesomiumUnityWebTextureUGUI : MonoBehaviour
{
    #region Fields
    public int m_Width = 1024;
    public int m_Height = 512;
    public string m_URL = "http://www.google.com/";
    //public bool m_FlipX = false;
    //public bool m_FlipY = false;
    public bool m_Interactive = true;

    [SerializeField]
    private bool m_Transparent = false;

    private AwesomiumUnityWebView m_WebView = null;
    private bool m_MouseIsOver = false;
    private bool m_HasFocus = false;

    [SerializeField]
    private bool m_InitializeOnStart = true;
    private bool m_HasBeenInitialized = false;

    [SerializeField]	// Only for testing purposes.
    private Texture2D m_Texture = null;

    static private int[] MouseButtonsMap = { 0, 2, 1 };
    static private List<KeyCode> m_KeyCodeList = new List<KeyCode>(132);
    static private bool m_Is_KeyCodeList_Initialized;

    private bool m_IsKeyHeldDown;
    private KeyCode m_HeldDownKey;

    private RawImage m_RawImage;
    private PointerTrigger m_Trigger;

    private Vector2 m_ScrollDelta;
    public int m_ScrollSpeed = 75;

    //current mouse position
    [SerializeField] // Also only for testing purposes.
    private float m_XMousePos, m_YMousePos;

    #endregion

    #region Getters/Setters
    public AwesomiumUnityWebView WebView
    {
        get
        {
            return m_WebView;
        }
        set
        {
            if (value != null)
                m_WebView = value;
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

    public float MousePosition_X
    {
        get
        {
            return m_XMousePos;
        }
    }

    public float MousePosition_Y
    {
        get
        {
            return m_YMousePos;
        }
    }
    #endregion

    public void Initialize()
    {
        m_HasBeenInitialized = true;

        if (!m_Is_KeyCodeList_Initialized)
        {
            InitializeKeyLists();
            m_Is_KeyCodeList_Initialized = true;
        }

        AwesomiumUnityWebCore.EnsureInitialized();

        // Call resize which will create a texture and a webview for us if they do not exist yet at this point.
        Resize(m_Width, m_Height);

        m_RawImage = GetComponent<RawImage>();
        if (m_RawImage)
        {
            m_RawImage.texture = m_Texture;
        }
        else
        {
            Debug.LogWarning("There is no RawImage attached to this GameObject! AwesomiumUnityWebTextureUGUI will add RawImage component in order to render to a VISIBLE texture.");
            m_RawImage = gameObject.AddComponent<RawImage>();
            m_RawImage.texture = m_Texture;
        }

        m_WebView.SetTransparent(m_Transparent);
    }

    void Start()
    {
        if (m_InitializeOnStart && !m_HasBeenInitialized)
        {
            Initialize();

            // Now load the URL.
            LoadURL(m_URL);

            //while (m_WebView.IsLoading)
            //{
            //    AwesomiumUnityWebCore.Update();	
            //}
        }

        m_Trigger = gameObject.GetComponent<PointerTrigger>();
        if (!m_Trigger)
        {
            m_Trigger = gameObject.AddComponent<PointerTrigger>();
        }

        #region Subscribe_to_PointerTrigger_Events

        //NOTE: if you want non-PointerEventData events e.g. BaseEventData like ISelectHandler which requires "void OnSelect(BaseEventData dat)"
        //then you need to implement those interfaces OUTSIDE/WITHOUT PointerEvent.cs Script! i.e. here in this class.
        m_Trigger.Subscribe(PointerEventType.PointerEnter, myMouseEnter);
        m_Trigger.Subscribe(PointerEventType.PointerExit, myMouseExit);
        m_Trigger.Subscribe(PointerEventType.PointerDown, myMouseDown);
        m_Trigger.Subscribe(PointerEventType.PointerUp, myMouseUp);
        m_Trigger.Subscribe(PointerEventType.Scroll, myMouseScroll);
        m_Trigger.Subscribe(PointerEventType.BeginDrag, myBeginDrag);
        m_Trigger.Subscribe(PointerEventType.Drag, myDrag);
        m_Trigger.Subscribe(PointerEventType.EndDrag, myEndDrag);
        #endregion
    }

    void Update()
    {
        #region Unfocus?
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            if (!m_MouseIsOver)
            {
                Unfocus();
            }
        }
        #endregion

        #region Repaint
        if (m_WebView.IsDirty)
        {
            if (!m_Texture)
                Debug.LogError("The WebTexture does not have a texture assigned and will not paint.");
            else
                m_WebView.CopyBufferToTexture(m_Texture.GetNativeTexturePtr(), m_Texture.width, m_Texture.height);
        }
        #endregion

        #region InjectMouseMove_mouseHover_Awesomium
        //Make sure: 1. pivots are (X=0.5, Y=0.5) 
        //2. Both pivots and anchors are set in the middle!
        if (m_MouseIsOver)
        {
            if (m_Interactive && m_RawImage)
            {
                Vector2 localCursor;
                var rect1 = GetComponent<RectTransform>();
                var pos1 = Input.mousePosition;
                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect1, pos1,
                    null, out localCursor))
                    Debug.LogException(new System.Exception("Can't do ScreenPointToLocalPointInRectangle!"));

                m_XMousePos = localCursor.x;
                m_YMousePos = localCursor.y;

                //some math
                if (m_XMousePos < 0)
                    m_XMousePos = m_XMousePos + rect1.rect.width / 2;
                else m_XMousePos += rect1.rect.width / 2;

                if (m_YMousePos > 0)
                    m_YMousePos = m_YMousePos + rect1.rect.height / 2;
                else m_YMousePos += rect1.rect.height / 2;

                //flipping Y axis to make Awesomium happy
                m_YMousePos = rect1.rect.height - m_YMousePos;
                //Debug.Log(ypos);

                //Debug.Log("Correct Cursor Pos: " + xpos + " " + ypos);
                m_WebView.InjectMouseMove((int)m_XMousePos, (int)m_YMousePos);

                if (m_XMousePos < -1 || m_YMousePos < -1)
                {
                    Debug.LogError("Set pivots to: X=0.5, Y=0.5; Also set pivots and anchors to the middle of the current rect using Anchor Presets tool in the inspector.");

                }
            }
            else Debug.LogError("RawImage missing! (Perhaps browser is non-interactive?)");
        }
        #endregion

        /// Needs maintenance!
        /// 1. Cannot register held keys
        /// 2. Possible inability of registering shift keys e.g. Shift-Backslash does-
        ///    -NOT produce Question Mark (Might work after building the game)
        #region InjectKeyboardEvent_Awesomium
        if (m_Interactive && m_HasFocus)
        {
            KeyCode keyCodeUp = 0, keyCodeDown = 0, keyCodeHold = 0;
            AwesomiumUnityWebKeyModifiers keyModifier = 0;

            keyModifier = m_GetKeyModifier();

            #region Key_Hold
            //Not working
            //if (m_IsKeyHeldDown)
            //{
            //    if (!Input.GetKeyUp(m_HeldDownKey))
            //    {
            //        char myChar = (char)m_HeldDownKey;

            //        if (myChar >= 32 && myChar <= 126)
            //        {
            //            AwesomiumUnityWebKeyboardEvent keyEvent = new AwesomiumUnityWebKeyboardEvent();
            //            keyEvent.Type = AwesomiumUnityWebKeyType.Char;
            //            keyEvent.Text = new ushort[] { myChar, 0, 0, 0 };
            //            keyEvent.VirtualKeyCode = MapKeys(m_HeldDownKey);
            //            keyEvent.Modifiers = keyModifier;
            //            m_WebView.InjectKeyboardEvent(keyEvent);
            //        }
            //    }
            //    else
            //        m_IsKeyHeldDown = false;
            //}
            #endregion

            #region Key_Down
            keyCodeDown = m_GetKeyDown();

            if (keyCodeDown != 0)
            {
                //Debug.Log(keyCodeDown + " THIS IS DOWN!");
                char myChar = (char)keyCodeDown;

                //is it a character (letter/symbol/number)?
                if (myChar >= 32 && myChar <= 126)
                {
                    if(keyCodeDown != m_HeldDownKey){
                    AwesomiumUnityWebKeyboardEvent keyEvent = new AwesomiumUnityWebKeyboardEvent();
                    keyEvent.Type = AwesomiumUnityWebKeyType.Char;
                    keyEvent.Text = new ushort[] { myChar, 0, 0, 0 };
                    keyEvent.VirtualKeyCode = MapKeys(keyCodeDown);
                    keyEvent.Modifiers = keyModifier;
                    m_WebView.InjectKeyboardEvent(keyEvent);

                    //m_HeldDownKey = keyCodeDown;
                    //m_IsKeyHeldDown = true;
                    }
                }
                //KeyPad! (not a letter/symbol/number)
                else
                {
                    AwesomiumUnityWebKeyboardEvent keyEvent = new AwesomiumUnityWebKeyboardEvent();
                    keyEvent.Type = AwesomiumUnityWebKeyType.KeyDown;
                    keyEvent.VirtualKeyCode = MapKeys(keyCodeDown);
                    keyEvent.Modifiers = keyModifier;
                    m_WebView.InjectKeyboardEvent(keyEvent);
                }

            }
            #endregion

            #region Key_Up
            keyCodeUp = m_GetKeyUp();

            if (keyCodeUp != 0)
            {
                //Debug.Log(keyCodeUp + " THIS IS UP!");
                AwesomiumUnityWebKeyboardEvent keyEvent = new AwesomiumUnityWebKeyboardEvent();
                keyEvent.Type = AwesomiumUnityWebKeyType.KeyUp;
                keyEvent.VirtualKeyCode = MapKeys(keyCodeUp);
                keyEvent.Modifiers = keyModifier;
                m_WebView.InjectKeyboardEvent(keyEvent);
            }
            #endregion
        }
        #endregion

        #region Example of: Copy, Paste, Zoom-in etc...

        ///Important Note: Ctrl, Alt, and Shift keys won't properly work while in Unity Editor; they will work ONLY in built solutions

        //if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        //{
        //    m_WebView.CopyClipboard();
        //}
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    m_WebView.ZoomIn();
        //}
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    m_WebView.ZoomOut();
        //}
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    m_WebView.ZoomReset();
        //}
        #endregion
    }

    #region Important_Destroy_UnityCallbacks

    void OnDestroy()
    {
        if (m_WebView != null)
        {
            m_WebView.Destroy();
            m_WebView = null;
        }
    }

    void OnApplicationQuit()
    {
        if (m_WebView != null)
        {
            m_WebView.Destroy();
            m_WebView = null;
        }
    }
    #endregion

    #region Helper_Methods
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

    static private int MapMouseButtons(int _Button)
    {
        return MouseButtonsMap[_Button];
    }

    static private AwesomiumUnityWebKeyModifiers MapModifiers(Event e)
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

    static private AwesomiumUnityVirtualKey MapKeys(KeyCode keyCodeInput)
    {
        switch (keyCodeInput)
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

    static private AwesomiumUnityWebKeyModifiers m_GetKeyModifier()
    {
        int modifiers = 0;

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            modifiers |= (int)AwesomiumUnityWebKeyModifiers.ControlKey;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            modifiers |= (int)AwesomiumUnityWebKeyModifiers.ShiftKey;

        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            modifiers |= (int)AwesomiumUnityWebKeyModifiers.AltKey;

        return (AwesomiumUnityWebKeyModifiers)modifiers;
    }

    static private KeyCode m_GetKeyUp()
    {
        var myEnumerator = m_KeyCodeList.GetEnumerator();
        while (myEnumerator.MoveNext())
        {
            if (Input.GetKeyUp(myEnumerator.Current))
                return m_KeyCodeList.GetEnumerator().Current;
        }
        return 0;
    }

    static private KeyCode m_GetKeyDown()
    {
        var myEnumerator = m_KeyCodeList.GetEnumerator();
        while (myEnumerator.MoveNext())
        {
            if (Input.GetKeyDown(myEnumerator.Current))
                return myEnumerator.Current;
        }
        return 0;
    }

    //experimental: instead, you might need to use Input.GetKeyDown and use a bool to signify if button is still pressed;-
    //-that bool is reset on Input.GetKeyUp
    //Now as long as the bool is true, you inject keyboard event every frame regardless of Unity input stats.
    static private KeyCode m_GetKeyHold()
    {
        var myEnumerator = m_KeyCodeList.GetEnumerator();
        while (myEnumerator.MoveNext())
        {
            if (Input.GetKey(myEnumerator.Current))
                return myEnumerator.Current;
        }
        return 0;
    }

    static private void InitializeKeyLists()
    {
        #region AwesomeListRight?
        m_KeyCodeList.Add(KeyCode.KeypadPeriod);
        m_KeyCodeList.Add(KeyCode.KeypadDivide);
        m_KeyCodeList.Add(KeyCode.KeypadMultiply);
        m_KeyCodeList.Add(KeyCode.KeypadMinus);
        m_KeyCodeList.Add(KeyCode.KeypadPlus);
        m_KeyCodeList.Add(KeyCode.KeypadEnter);
        m_KeyCodeList.Add(KeyCode.KeypadEquals);
        m_KeyCodeList.Add(KeyCode.UpArrow);
        m_KeyCodeList.Add(KeyCode.DownArrow);
        m_KeyCodeList.Add(KeyCode.RightArrow);
        m_KeyCodeList.Add(KeyCode.LeftArrow);
        m_KeyCodeList.Add(KeyCode.Insert);
        m_KeyCodeList.Add(KeyCode.Home);
        m_KeyCodeList.Add(KeyCode.End);
        m_KeyCodeList.Add(KeyCode.PageUp);
        m_KeyCodeList.Add(KeyCode.PageDown);
        m_KeyCodeList.Add(KeyCode.F1);
        m_KeyCodeList.Add(KeyCode.F2);
        m_KeyCodeList.Add(KeyCode.F3);
        m_KeyCodeList.Add(KeyCode.F4);
        m_KeyCodeList.Add(KeyCode.F5);
        m_KeyCodeList.Add(KeyCode.F6);
        m_KeyCodeList.Add(KeyCode.F7);
        m_KeyCodeList.Add(KeyCode.F8);
        m_KeyCodeList.Add(KeyCode.F9);
        m_KeyCodeList.Add(KeyCode.F10);
        m_KeyCodeList.Add(KeyCode.F11);
        m_KeyCodeList.Add(KeyCode.F12);
        m_KeyCodeList.Add(KeyCode.F13);
        m_KeyCodeList.Add(KeyCode.F14);
        m_KeyCodeList.Add(KeyCode.F15);
        m_KeyCodeList.Add(KeyCode.Alpha0);
        m_KeyCodeList.Add(KeyCode.Alpha1);
        m_KeyCodeList.Add(KeyCode.Alpha2);
        m_KeyCodeList.Add(KeyCode.Alpha3);
        m_KeyCodeList.Add(KeyCode.Alpha4);
        m_KeyCodeList.Add(KeyCode.Alpha5);
        m_KeyCodeList.Add(KeyCode.Alpha6);
        m_KeyCodeList.Add(KeyCode.Alpha7);
        m_KeyCodeList.Add(KeyCode.Alpha8);
        m_KeyCodeList.Add(KeyCode.Alpha9);
        m_KeyCodeList.Add(KeyCode.Exclaim);
        m_KeyCodeList.Add(KeyCode.DoubleQuote);
        m_KeyCodeList.Add(KeyCode.Hash);
        m_KeyCodeList.Add(KeyCode.Dollar);
        m_KeyCodeList.Add(KeyCode.Ampersand);
        m_KeyCodeList.Add(KeyCode.Quote);
        m_KeyCodeList.Add(KeyCode.LeftParen);
        m_KeyCodeList.Add(KeyCode.RightParen);
        m_KeyCodeList.Add(KeyCode.Asterisk);
        m_KeyCodeList.Add(KeyCode.Plus);
        m_KeyCodeList.Add(KeyCode.Comma);
        m_KeyCodeList.Add(KeyCode.Minus);
        m_KeyCodeList.Add(KeyCode.Period);
        m_KeyCodeList.Add(KeyCode.Slash);
        m_KeyCodeList.Add(KeyCode.Colon);
        m_KeyCodeList.Add(KeyCode.Semicolon);
        m_KeyCodeList.Add(KeyCode.Less);
        m_KeyCodeList.Add(KeyCode.Equals);
        m_KeyCodeList.Add(KeyCode.Greater);
        m_KeyCodeList.Add(KeyCode.Question);
        m_KeyCodeList.Add(KeyCode.At);
        m_KeyCodeList.Add(KeyCode.LeftBracket);
        m_KeyCodeList.Add(KeyCode.Backslash);
        m_KeyCodeList.Add(KeyCode.RightBracket);
        m_KeyCodeList.Add(KeyCode.Caret);
        m_KeyCodeList.Add(KeyCode.Underscore);
        m_KeyCodeList.Add(KeyCode.BackQuote);
        m_KeyCodeList.Add(KeyCode.A);
        m_KeyCodeList.Add(KeyCode.B);
        m_KeyCodeList.Add(KeyCode.C);
        m_KeyCodeList.Add(KeyCode.D);
        m_KeyCodeList.Add(KeyCode.E);
        m_KeyCodeList.Add(KeyCode.F);
        m_KeyCodeList.Add(KeyCode.G);
        m_KeyCodeList.Add(KeyCode.H);
        m_KeyCodeList.Add(KeyCode.I);
        m_KeyCodeList.Add(KeyCode.J);
        m_KeyCodeList.Add(KeyCode.K);
        m_KeyCodeList.Add(KeyCode.L);
        m_KeyCodeList.Add(KeyCode.M);
        m_KeyCodeList.Add(KeyCode.N);
        m_KeyCodeList.Add(KeyCode.O);
        m_KeyCodeList.Add(KeyCode.P);
        m_KeyCodeList.Add(KeyCode.Q);
        m_KeyCodeList.Add(KeyCode.R);
        m_KeyCodeList.Add(KeyCode.S);
        m_KeyCodeList.Add(KeyCode.T);
        m_KeyCodeList.Add(KeyCode.U);
        m_KeyCodeList.Add(KeyCode.V);
        m_KeyCodeList.Add(KeyCode.W);
        m_KeyCodeList.Add(KeyCode.X);
        m_KeyCodeList.Add(KeyCode.Y);
        m_KeyCodeList.Add(KeyCode.Z);
        m_KeyCodeList.Add(KeyCode.Numlock);
        m_KeyCodeList.Add(KeyCode.CapsLock);
        m_KeyCodeList.Add(KeyCode.ScrollLock);
        m_KeyCodeList.Add(KeyCode.RightShift);
        m_KeyCodeList.Add(KeyCode.LeftShift);
        m_KeyCodeList.Add(KeyCode.RightControl);
        m_KeyCodeList.Add(KeyCode.LeftControl);
        m_KeyCodeList.Add(KeyCode.RightAlt);
        m_KeyCodeList.Add(KeyCode.LeftAlt);
        m_KeyCodeList.Add(KeyCode.LeftApple);
        m_KeyCodeList.Add(KeyCode.LeftWindows);
        m_KeyCodeList.Add(KeyCode.RightApple);
        m_KeyCodeList.Add(KeyCode.RightWindows);
        m_KeyCodeList.Add(KeyCode.AltGr);
        m_KeyCodeList.Add(KeyCode.Help);
        m_KeyCodeList.Add(KeyCode.Print);
        m_KeyCodeList.Add(KeyCode.SysReq);
        m_KeyCodeList.Add(KeyCode.Break);
        m_KeyCodeList.Add(KeyCode.Menu);
        m_KeyCodeList.Add(KeyCode.Backspace);
        m_KeyCodeList.Add(KeyCode.Delete);
        m_KeyCodeList.Add(KeyCode.Tab);
        m_KeyCodeList.Add(KeyCode.Clear);
        m_KeyCodeList.Add(KeyCode.Return);
        m_KeyCodeList.Add(KeyCode.Pause);
        m_KeyCodeList.Add(KeyCode.Escape);
        m_KeyCodeList.Add(KeyCode.Space);
        m_KeyCodeList.Add(KeyCode.Keypad0);
        m_KeyCodeList.Add(KeyCode.Keypad1);
        m_KeyCodeList.Add(KeyCode.Keypad2);
        m_KeyCodeList.Add(KeyCode.Keypad3);
        m_KeyCodeList.Add(KeyCode.Keypad4);
        m_KeyCodeList.Add(KeyCode.Keypad5);
        m_KeyCodeList.Add(KeyCode.Keypad6);
        m_KeyCodeList.Add(KeyCode.Keypad7);
        m_KeyCodeList.Add(KeyCode.Keypad8);
        m_KeyCodeList.Add(KeyCode.Keypad9);
        #endregion
    }
    #endregion

    #region Modifiable_UnityCallbacks
    void myMouseEnter(PointerEventData dat)
    {
        m_MouseIsOver = true;
        //use this if you're not using Overlay Canvas
        //myEnterEventCam = dat.enterEventCamera;
    }

    void myMouseExit(PointerEventData dat)
    {
        m_MouseIsOver = false;
        //myEnterEventCam = null;
    }

    void myMouseDown(PointerEventData dat)
    {
        #region focus
        if (!m_HasFocus) Focus();
        #endregion

        WebView.InjectMouseDown(MapMouseButtons((int)dat.button));
        //Debug.Log(UserWebInputManager.MapMouseButtons((int)dat.button));
    }

    void myMouseUp(PointerEventData dat)
    {
        #region focus
        if (!m_HasFocus) Focus();
        #endregion

        WebView.InjectMouseUp(MapMouseButtons((int)dat.button));
    }

    void myMouseScroll(PointerEventData dat)
    {
        m_ScrollDelta = dat.scrollDelta;
        WebView.InjectMouseWheel((int)m_ScrollDelta.y * m_ScrollSpeed, (int)m_ScrollDelta.x);
    }

    void myBeginDrag(PointerEventData dat)
    {
        //Debug.Log("Began Dragging! Set your flags here");
    }

    void myDrag(PointerEventData dat)
    {
        //Debug.Log("Dragging! Put some functionality here(e.g. for resizing: adjust sizeDelta)");
    }

    void myEndDrag(PointerEventData dat)
    {
        //Debug.Log("Just ende dragging! Reset your flags here and apply SetNativeSize if you were resizing");
    }
    #endregion

    #region Public_Callable_Methods
    public void Reload()
    {
        m_WebView.Reload();
    }

    public void Resize(int _Width, int _Height)
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

    public void LoadURL(string _URL)
    {
        m_URL = _URL;
        m_WebView.LoadURL(m_URL);
    }


    #endregion
}
