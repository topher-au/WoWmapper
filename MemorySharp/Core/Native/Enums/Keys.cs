using System;

namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     The key codes and modifiers list.
    /// </summary>
    [Flags]
    public enum Keys
    {
        /// <summary>
        ///     a
        /// </summary>
        A = 0x41,

        /// <summary>
        ///     The add
        /// </summary>
        Add = 0x6b,

        /// <summary>
        ///     The alt
        /// </summary>
        Alt = 0x40000,

        /// <summary>
        ///     The apps
        /// </summary>
        Apps = 0x5d,

        /// <summary>
        ///     The attn
        /// </summary>
        Attn = 0xf6,

        /// <summary>
        ///     The b
        /// </summary>
        B = 0x42,

        /// <summary>
        ///     The back
        /// </summary>
        Back = 8,

        /// <summary>
        ///     The browser back
        /// </summary>
        BrowserBack = 0xa6,

        /// <summary>
        ///     The browser favorites
        /// </summary>
        BrowserFavorites = 0xab,

        /// <summary>
        ///     The browser forward
        /// </summary>
        BrowserForward = 0xa7,

        /// <summary>
        ///     The browser home
        /// </summary>
        BrowserHome = 0xac,

        /// <summary>
        ///     The browser refresh
        /// </summary>
        BrowserRefresh = 0xa8,

        /// <summary>
        ///     The browser search
        /// </summary>
        BrowserSearch = 170,

        /// <summary>
        ///     The browser stop
        /// </summary>
        BrowserStop = 0xa9,

        /// <summary>
        ///     The c
        /// </summary>
        C = 0x43,

        /// <summary>
        ///     The cancel
        /// </summary>
        Cancel = 3,

        /// <summary>
        ///     The capital
        /// </summary>
        Capital = 20,

        /// <summary>
        ///     The caps lock
        /// </summary>
        CapsLock = 20,

        /// <summary>
        ///     The clear
        /// </summary>
        Clear = 12,

        /// <summary>
        ///     The control
        /// </summary>
        Control = 0x20000,

        /// <summary>
        ///     The control key
        /// </summary>
        ControlKey = 0x11,

        /// <summary>
        ///     The crsel
        /// </summary>
        Crsel = 0xf7,

        /// <summary>
        ///     The d
        /// </summary>
        D = 0x44,

        /// <summary>
        ///     The d0
        /// </summary>
        D0 = 0x30,

        /// <summary>
        ///     The d1
        /// </summary>
        D1 = 0x31,

        /// <summary>
        ///     The d2
        /// </summary>
        D2 = 50,

        /// <summary>
        ///     The d3
        /// </summary>
        D3 = 0x33,

        /// <summary>
        ///     The d4
        /// </summary>
        D4 = 0x34,

        /// <summary>
        ///     The d5
        /// </summary>
        D5 = 0x35,

        /// <summary>
        ///     The d6
        /// </summary>
        D6 = 0x36,

        /// <summary>
        ///     The d7
        /// </summary>
        D7 = 0x37,

        /// <summary>
        ///     The d8
        /// </summary>
        D8 = 0x38,

        /// <summary>
        ///     The d9
        /// </summary>
        D9 = 0x39,

        /// <summary>
        ///     The decimal
        /// </summary>
        Decimal = 110,

        /// <summary>
        ///     The delete
        /// </summary>
        Delete = 0x2e,

        /// <summary>
        ///     The divide
        /// </summary>
        Divide = 0x6f,

        /// <summary>
        ///     Down
        /// </summary>
        Down = 40,

        /// <summary>
        ///     The e
        /// </summary>
        E = 0x45,

        /// <summary>
        ///     The end
        /// </summary>
        End = 0x23,

        /// <summary>
        ///     The enter
        /// </summary>
        Enter = 13,

        /// <summary>
        ///     The erase EOF
        /// </summary>
        EraseEof = 0xf9,

        /// <summary>
        ///     The escape
        /// </summary>
        Escape = 0x1b,

        /// <summary>
        ///     The execute
        /// </summary>
        Execute = 0x2b,

        /// <summary>
        ///     The exsel
        /// </summary>
        Exsel = 0xf8,

        /// <summary>
        ///     The f
        /// </summary>
        F = 70,

        /// <summary>
        ///     The f1
        /// </summary>
        F1 = 0x70,

        /// <summary>
        ///     The F10
        /// </summary>
        F10 = 0x79,

        /// <summary>
        ///     The F11
        /// </summary>
        F11 = 0x7a,

        /// <summary>
        ///     The F12
        /// </summary>
        F12 = 0x7b,

        /// <summary>
        ///     The F13
        /// </summary>
        F13 = 0x7c,

        /// <summary>
        ///     The F14
        /// </summary>
        F14 = 0x7d,

        /// <summary>
        ///     The F15
        /// </summary>
        F15 = 0x7e,

        /// <summary>
        ///     The F16
        /// </summary>
        F16 = 0x7f,

        /// <summary>
        ///     The F17
        /// </summary>
        F17 = 0x80,

        /// <summary>
        ///     The F18
        /// </summary>
        F18 = 0x81,

        /// <summary>
        ///     The F19
        /// </summary>
        F19 = 130,

        /// <summary>
        ///     The f2
        /// </summary>
        F2 = 0x71,

        /// <summary>
        ///     The F20
        /// </summary>
        F20 = 0x83,

        /// <summary>
        ///     The F21
        /// </summary>
        F21 = 0x84,

        /// <summary>
        ///     The F22
        /// </summary>
        F22 = 0x85,

        /// <summary>
        ///     The F23
        /// </summary>
        F23 = 0x86,

        /// <summary>
        ///     The F24
        /// </summary>
        F24 = 0x87,

        /// <summary>
        ///     The f3
        /// </summary>
        F3 = 0x72,

        /// <summary>
        ///     The f4
        /// </summary>
        F4 = 0x73,

        /// <summary>
        ///     The f5
        /// </summary>
        F5 = 0x74,

        /// <summary>
        ///     The f6
        /// </summary>
        F6 = 0x75,

        /// <summary>
        ///     The f7
        /// </summary>
        F7 = 0x76,

        /// <summary>
        ///     The f8
        /// </summary>
        F8 = 0x77,

        /// <summary>
        ///     The f9
        /// </summary>
        F9 = 120,

        /// <summary>
        ///     The final mode
        /// </summary>
        FinalMode = 0x18,

        /// <summary>
        ///     The g
        /// </summary>
        G = 0x47,

        /// <summary>
        ///     The h
        /// </summary>
        H = 0x48,

        /// <summary>
        ///     The hanguel mode
        /// </summary>
        HanguelMode = 0x15,

        /// <summary>
        ///     The hangul mode
        /// </summary>
        HangulMode = 0x15,

        /// <summary>
        ///     The hanja mode
        /// </summary>
        HanjaMode = 0x19,

        /// <summary>
        ///     The help
        /// </summary>
        Help = 0x2f,

        /// <summary>
        ///     The home
        /// </summary>
        Home = 0x24,

        /// <summary>
        ///     The i
        /// </summary>
        I = 0x49,

        /// <summary>
        ///     The IME accept
        /// </summary>
        ImeAccept = 30,

        /// <summary>
        ///     The IME aceept
        /// </summary>
        ImeAceept = 30,

        /// <summary>
        ///     The IME convert
        /// </summary>
        ImeConvert = 0x1c,

        /// <summary>
        ///     The IME mode change
        /// </summary>
        ImeModeChange = 0x1f,

        /// <summary>
        ///     The IME nonconvert
        /// </summary>
        ImeNonconvert = 0x1d,

        /// <summary>
        ///     The insert
        /// </summary>
        Insert = 0x2d,

        /// <summary>
        ///     The j
        /// </summary>
        J = 0x4a,

        /// <summary>
        ///     The junja mode
        /// </summary>
        JunjaMode = 0x17,

        /// <summary>
        ///     The k
        /// </summary>
        K = 0x4b,

        /// <summary>
        ///     The kana mode
        /// </summary>
        KanaMode = 0x15,

        /// <summary>
        ///     The kanji mode
        /// </summary>
        KanjiMode = 0x19,

        /// <summary>
        ///     The key code
        /// </summary>
        KeyCode = 0xffff,

        /// <summary>
        ///     The l
        /// </summary>
        L = 0x4c,

        /// <summary>
        ///     The launch application1
        /// </summary>
        LaunchApplication1 = 0xb6,

        /// <summary>
        ///     The launch application2
        /// </summary>
        LaunchApplication2 = 0xb7,

        /// <summary>
        ///     The launch mail
        /// </summary>
        LaunchMail = 180,

        /// <summary>
        ///     The l button
        /// </summary>
        LButton = 1,

        /// <summary>
        ///     The l control key
        /// </summary>
        LControlKey = 0xa2,

        /// <summary>
        ///     The left
        /// </summary>
        Left = 0x25,

        /// <summary>
        ///     The line feed
        /// </summary>
        LineFeed = 10,

        /// <summary>
        ///     The l menu
        /// </summary>
        LMenu = 0xa4,

        /// <summary>
        ///     The l shift key
        /// </summary>
        LShiftKey = 160,

        /// <summary>
        ///     The l win
        /// </summary>
        LWin = 0x5b,

        /// <summary>
        ///     The m
        /// </summary>
        M = 0x4d,

        /// <summary>
        ///     The m button
        /// </summary>
        MButton = 4,

        /// <summary>
        ///     The media next track
        /// </summary>
        MediaNextTrack = 0xb0,

        /// <summary>
        ///     The media play pause
        /// </summary>
        MediaPlayPause = 0xb3,

        /// <summary>
        ///     The media previous track
        /// </summary>
        MediaPreviousTrack = 0xb1,

        /// <summary>
        ///     The media stop
        /// </summary>
        MediaStop = 0xb2,

        /// <summary>
        ///     The menu
        /// </summary>
        Menu = 0x12,

        /// <summary>
        ///     The modifiers
        /// </summary>
        Modifiers = -65536,

        /// <summary>
        ///     The multiply
        /// </summary>
        Multiply = 0x6a,

        /// <summary>
        ///     The n
        /// </summary>
        N = 0x4e,

        /// <summary>
        ///     The next
        /// </summary>
        Next = 0x22,

        /// <summary>
        ///     The no name
        /// </summary>
        NoName = 0xfc,

        /// <summary>
        ///     The none
        /// </summary>
        None = 0,

        /// <summary>
        ///     The number lock
        /// </summary>
        NumLock = 0x90,

        /// <summary>
        ///     The number pad0
        /// </summary>
        NumPad0 = 0x60,

        /// <summary>
        ///     The number pad1
        /// </summary>
        NumPad1 = 0x61,

        /// <summary>
        ///     The number pad2
        /// </summary>
        NumPad2 = 0x62,

        /// <summary>
        ///     The number pad3
        /// </summary>
        NumPad3 = 0x63,

        /// <summary>
        ///     The number pad4
        /// </summary>
        NumPad4 = 100,

        /// <summary>
        ///     The number pad5
        /// </summary>
        NumPad5 = 0x65,

        /// <summary>
        ///     The number pad6
        /// </summary>
        NumPad6 = 0x66,

        /// <summary>
        ///     The number pad7
        /// </summary>
        NumPad7 = 0x67,

        /// <summary>
        ///     The number pad8
        /// </summary>
        NumPad8 = 0x68,

        /// <summary>
        ///     The number pad9
        /// </summary>
        NumPad9 = 0x69,

        /// <summary>
        ///     The o
        /// </summary>
        O = 0x4f,

        /// <summary>
        ///     The oem1
        /// </summary>
        Oem1 = 0xba,

        /// <summary>
        ///     The oem102
        /// </summary>
        Oem102 = 0xe2,

        /// <summary>
        ///     The oem2
        /// </summary>
        Oem2 = 0xbf,

        /// <summary>
        ///     The oem3
        /// </summary>
        Oem3 = 0xc0,

        /// <summary>
        ///     The oem4
        /// </summary>
        Oem4 = 0xdb,

        /// <summary>
        ///     The oem5
        /// </summary>
        Oem5 = 220,

        /// <summary>
        ///     The oem6
        /// </summary>
        Oem6 = 0xdd,

        /// <summary>
        ///     The oem7
        /// </summary>
        Oem7 = 0xde,

        /// <summary>
        ///     The oem8
        /// </summary>
        Oem8 = 0xdf,

        /// <summary>
        ///     The oem backslash
        /// </summary>
        OemBackslash = 0xe2,

        /// <summary>
        ///     The oem clear
        /// </summary>
        OemClear = 0xfe,

        /// <summary>
        ///     The oem close brackets
        /// </summary>
        OemCloseBrackets = 0xdd,

        /// <summary>
        ///     The oemcomma
        /// </summary>
        Oemcomma = 0xbc,

        /// <summary>
        ///     The oem minus
        /// </summary>
        OemMinus = 0xbd,

        /// <summary>
        ///     The oem open brackets
        /// </summary>
        OemOpenBrackets = 0xdb,

        /// <summary>
        ///     The oem period
        /// </summary>
        OemPeriod = 190,

        /// <summary>
        ///     The oem pipe
        /// </summary>
        OemPipe = 220,

        /// <summary>
        ///     The oemplus
        /// </summary>
        Oemplus = 0xbb,

        /// <summary>
        ///     The oem question
        /// </summary>
        OemQuestion = 0xbf,

        /// <summary>
        ///     The oem quotes
        /// </summary>
        OemQuotes = 0xde,

        /// <summary>
        ///     The oem semicolon
        /// </summary>
        OemSemicolon = 0xba,

        /// <summary>
        ///     The oemtilde
        /// </summary>
        Oemtilde = 0xc0,

        /// <summary>
        ///     The p
        /// </summary>
        P = 80,

        /// <summary>
        ///     The pa1
        /// </summary>
        Pa1 = 0xfd,

        /// <summary>
        ///     The packet
        /// </summary>
        Packet = 0xe7,

        /// <summary>
        ///     The page down
        /// </summary>
        PageDown = 0x22,

        /// <summary>
        ///     The page up
        /// </summary>
        PageUp = 0x21,

        /// <summary>
        ///     The pause
        /// </summary>
        Pause = 0x13,

        /// <summary>
        ///     The play
        /// </summary>
        Play = 250,

        /// <summary>
        ///     The print
        /// </summary>
        Print = 0x2a,

        /// <summary>
        ///     The print screen
        /// </summary>
        PrintScreen = 0x2c,

        /// <summary>
        ///     The prior
        /// </summary>
        Prior = 0x21,

        /// <summary>
        ///     The process key
        /// </summary>
        ProcessKey = 0xe5,

        /// <summary>
        ///     The q
        /// </summary>
        Q = 0x51,

        /// <summary>
        ///     The r
        /// </summary>
        R = 0x52,

        /// <summary>
        ///     The r button
        /// </summary>
        RButton = 2,

        /// <summary>
        ///     The r control key
        /// </summary>
        RControlKey = 0xa3,

        /// <summary>
        ///     The return
        /// </summary>
        Return = 13,

        /// <summary>
        ///     The right
        /// </summary>
        Right = 0x27,

        /// <summary>
        ///     The r menu
        /// </summary>
        RMenu = 0xa5,

        /// <summary>
        ///     The r shift key
        /// </summary>
        RShiftKey = 0xa1,

        /// <summary>
        ///     The r win
        /// </summary>
        RWin = 0x5c,

        /// <summary>
        ///     The s
        /// </summary>
        S = 0x53,

        /// <summary>
        ///     The scroll
        /// </summary>
        Scroll = 0x91,

        /// <summary>
        ///     The select
        /// </summary>
        Select = 0x29,

        /// <summary>
        ///     The select media
        /// </summary>
        SelectMedia = 0xb5,

        /// <summary>
        ///     The separator
        /// </summary>
        Separator = 0x6c,

        /// <summary>
        ///     The shift
        /// </summary>
        Shift = 0x10000,

        /// <summary>
        ///     The shift key
        /// </summary>
        ShiftKey = 0x10,

        /// <summary>
        ///     The sleep
        /// </summary>
        Sleep = 0x5f,

        /// <summary>
        ///     The snapshot
        /// </summary>
        Snapshot = 0x2c,

        /// <summary>
        ///     The space
        /// </summary>
        Space = 0x20,

        /// <summary>
        ///     The subtract
        /// </summary>
        Subtract = 0x6d,

        /// <summary>
        ///     The t
        /// </summary>
        T = 0x54,

        /// <summary>
        ///     The tab
        /// </summary>
        Tab = 9,

        /// <summary>
        ///     The u
        /// </summary>
        U = 0x55,

        /// <summary>
        ///     Up
        /// </summary>
        Up = 0x26,

        /// <summary>
        ///     The v
        /// </summary>
        V = 0x56,

        /// <summary>
        ///     The volume down
        /// </summary>
        VolumeDown = 0xae,

        /// <summary>
        ///     The volume mute
        /// </summary>
        VolumeMute = 0xad,

        /// <summary>
        ///     The volume up
        /// </summary>
        VolumeUp = 0xaf,

        /// <summary>
        ///     The w
        /// </summary>
        W = 0x57,

        /// <summary>
        ///     The x
        /// </summary>
        X = 0x58,

        /// <summary>
        ///     The x button1
        /// </summary>
        XButton1 = 5,

        /// <summary>
        ///     The x button2
        /// </summary>
        XButton2 = 6,

        /// <summary>
        ///     The y
        /// </summary>
        Y = 0x59,

        /// <summary>
        ///     The z
        /// </summary>
        Z = 90,

        /// <summary>
        ///     The zoom
        /// </summary>
        Zoom = 0xfb
    }
}