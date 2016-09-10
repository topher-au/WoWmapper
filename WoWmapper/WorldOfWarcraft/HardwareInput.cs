using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Cursor = System.Windows.Forms.Cursor;

namespace WoWmapper.Input
{
    public static class HardwareInput
    {
        /// <summary>
        ///     The set of valid MapTypes used in MapVirtualKey
        /// </summary>
        public enum MapVirtualKeyMapTypes : uint
        {
            /// <summary>
            ///     uCode is a virtual-key code and is translated into a scan code.
            ///     If it is a virtual-key code that does not distinguish between left- and
            ///     right-hand keys, the left-hand scan code is returned.
            ///     If there is no translation, the function returns 0.
            /// </summary>
            MAPVK_VK_TO_VSC = 0x00,

            /// <summary>
            ///     uCode is a scan code and is translated into a virtual-key code that
            ///     does not distinguish between left- and right-hand keys. If there is no
            ///     translation, the function returns 0.
            /// </summary>
            MAPVK_VSC_TO_VK = 0x01,

            /// <summary>
            ///     uCode is a virtual-key code and is translated into an unshifted
            ///     character value in the low-order word of the return value. Dead keys (diacritics)
            ///     are indicated by setting the top bit of the return value. If there is no
            ///     translation, the function returns 0.
            /// </summary>
            MAPVK_VK_TO_CHAR = 0x02,

            /// <summary>
            ///     Windows NT/2000/XP: uCode is a scan code and is translated into a
            ///     virtual-key code that distinguishes between left- and right-hand keys. If
            ///     there is no translation, the function returns 0.
            /// </summary>
            MAPVK_VSC_TO_VK_EX = 0x03,

            /// <summary>
            ///     Not currently documented
            /// </summary>
            MAPVK_VK_TO_VSC_EX = 0x04
        }

        /// <summary>
        ///     The MapVirtualKey function translates (maps) a virtual-key code into a scan
        ///     code or character value, or translates a scan code into a virtual-key code
        /// </summary>
        /// <param name="uCode">
        ///     [in] Specifies the virtual-key code or scan code for a key.
        ///     How this value is interpreted depends on the value of the uMapType parameter
        /// </param>
        /// <param name="uMapType">
        ///     [in] Specifies the translation to perform. The value of this
        ///     parameter depends on the value of the uCode parameter.
        /// </param>
        /// <returns>
        ///     Either a scan code, a virtual-key code, or a character value, depending on
        ///     the value of uCode and uMapType. If there is no translation, the return value is zero
        /// </returns>
        [DllImport("user32.dll")]
        private static extern int MapVirtualKey(uint uCode, MapVirtualKeyMapTypes uMapType);


        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs,
            [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,
            int cbSize);

        public static INPUT[] SynthKeyPress(Key key, bool state)
        {
            var vKey = KeyInterop.VirtualKeyFromKey(key);
            var aInput = new[]
            {
                new INPUT
                {
                    type = InputType.KEYBOARD,
                    U = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wScan = (short) MapVirtualKey((uint) vKey, MapVirtualKeyMapTypes.MAPVK_VK_TO_VSC),
                            wVk = (short) vKey,
                            dwFlags = !state ? KEYEVENTF.KEYUP : 0
                        }
                    }
                }
            };
            return aInput;
        }

        public static INPUT[] SynthMouseClick(MouseButton button, bool state)
        {
            var up = MOUSEEVENTF.LEFTUP;
            var down = MOUSEEVENTF.LEFTDOWN;

            if (button == MouseButton.Right)
            {
                up = MOUSEEVENTF.RIGHTUP;
                down = MOUSEEVENTF.RIGHTDOWN;
            }

            var aInput = new[]
            {
                new INPUT
                {
                    type = InputType.MOUSE,
                    U = new InputUnion
                    {
                        mi = new MOUSEINPUT
                        {
                            dwFlags = state ? down : up,
                            dx = Cursor.Position.X,
                            dy = Cursor.Position.Y
                        }
                    }
                }
            };
            return aInput;
        }

        public static INPUT[] SynthMouseMove(int relativeX, int relativeY)
        {
            var aInput = new[]
            {
                new INPUT()
                {
                    type = InputType.MOUSE,
                    U = new InputUnion()
                    {
                        mi = new MOUSEINPUT()
                        {
                            dwFlags = MOUSEEVENTF.MOVE,
                            dx = relativeX,
                            dy = relativeY
                        }
                    }
                }
            };
            return aInput;
        }

        public static void SendKey(Key key, bool state)
        {
            var inputs = SynthKeyPress(key, state);

            SendInput((uint) inputs.Length, inputs, INPUT.Size);
        }

        public static void SendClick(MouseButton button, bool state)
        {
            var inputs = SynthMouseClick(button, state);
            SendInput((uint) inputs.Length, inputs, INPUT.Size);
        }

        public static void MoveMouse(int relativeX, int relativeY)
        {

            var input = SynthMouseMove(relativeX, relativeY);
            SendInput((uint) input.Length, input, INPUT.Size);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            internal InputType type;
            internal InputUnion U;

            internal static int Size
            {
                get { return Marshal.SizeOf(typeof (INPUT)); }
            }
        }

        internal enum InputType : uint
        {
            MOUSE = 0,
            KEYBOARD = 1,
            HARDWARE = 2
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct InputUnion
        {
            [FieldOffset(0)] internal MOUSEINPUT mi;
            [FieldOffset(0)] internal KEYBDINPUT ki;
            [FieldOffset(0)] internal HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            internal int dx;
            internal int dy;
            internal int mouseData;
            internal MOUSEEVENTF dwFlags;
            internal uint time;
            internal UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            internal short wVk;
            internal short wScan;
            internal KEYEVENTF dwFlags;
            internal int time;
            internal UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            internal int uMsg;
            internal short wParamL;
            internal short wParamH;
        }

        [Flags]
        internal enum KEYEVENTF : uint
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            SCANCODE = 0x0008,
            UNICODE = 0x0004
        }

        [Flags]
        internal enum MOUSEEVENTF : uint
        {
            ABSOLUTE = 0x8000,
            HWHEEL = 0x01000,
            MOVE = 0x0001,
            MOVE_NOCOALESCE = 0x2000,
            LEFTDOWN = 0x0002,
            LEFTUP = 0x0004,
            RIGHTDOWN = 0x0008,
            RIGHTUP = 0x0010,
            MIDDLEDOWN = 0x0020,
            MIDDLEUP = 0x0040,
            VIRTUALDESK = 0x4000,
            WHEEL = 0x0800,
            XDOWN = 0x0080,
            XUP = 0x0100
        }
    }
}