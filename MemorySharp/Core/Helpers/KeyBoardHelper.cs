using Binarysharp.MemoryManagement.Core.Native;
using Binarysharp.MemoryManagement.Core.Native.Enums;
using System;
using System.Collections;
using System.Linq;

namespace Binarysharp.MemoryManagement.Core.Helpers
{
    /// <summary>
    ///     Class to help with extracting basic keyboard information.
    ///     <remarks> Credits for this class go to Zat@unknowncheats.</remarks>
    ///     .
    /// </summary>
    public class KeyBoardHelper
    {
        #region Fields, Private Properties

        /// <summary>
        ///     The ke y_ pressed
        /// </summary>
        public const int KeyPressed = 0x8000;

        private short[] AllKeys { get; }
        private Hashtable Keys { get; }
        private Hashtable PrevKeys { get; set; }

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="KeyBoardHelper" /> class.
        /// </summary>
        public KeyBoardHelper()
        {
            Keys = new Hashtable();
            PrevKeys = new Hashtable();
            var myKeys = (Keys[])Enum.GetValues(typeof(Keys));
            AllKeys = new short[myKeys.Length];
            for (var i = 0; i < AllKeys.Length; i++)
            {
                AllKeys[i] = (short)myKeys[i];
            }
            Init();
        }

        /// <summary>
        ///     Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage
        ///     collection.
        /// </summary>
        ~KeyBoardHelper()
        {
            Keys.Clear();
            PrevKeys.Clear();
        }

        #endregion Constructors, Destructors

        /// <summary>
        ///     Gets the key down.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetKeyDown(int key)
        {
            return Convert.ToBoolean(SafeNativeMethods.GetKeyState(key) & KeyPressed);
        }

        /// <summary>
        ///     Gets the key down asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetKeyDownAsync(int key)
        {
            return GetKeyDownAsync((Keys)key);
        }

        /// <summary>
        ///     Gets the key down asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetKeyDownAsync(Keys key)
        {
            return Convert.ToBoolean(SafeNativeMethods.GetAsyncKeyState(key) & KeyPressed);
        }

        /// <summary>
        ///     Initializes and fills the hashtables.
        /// </summary>
        private void Init()
        {
            foreach (var key in AllKeys.Where(key => !PrevKeys.ContainsKey(key)))
            {
                PrevKeys.Add(key, false);
                Keys.Add(key, false);
            }
        }

        /// <summary>
        ///     Updates the key-states
        /// </summary>
        public void Update()
        {
            PrevKeys = (Hashtable)Keys.Clone();
            foreach (var key in AllKeys)
            {
                Keys[key] = GetKeyDown(key);
            }
        }

        /// <summary>
        ///     Returns an array of all keys that went up since the last Update-call
        /// </summary>
        /// <returns></returns>
        public Keys[] KeysThatWentUp()
        {
            return AllKeys.Cast<Keys>().Where(key => KeyWentUp((int)key)).ToArray();
        }

        /// <summary>
        ///     Returns an array of all keys that went down since the last Update-call
        /// </summary>
        /// <returns></returns>
        public Keys[] KeysThatWentDown()
        {
            return AllKeys.Cast<Keys>().Where(KeyWentDown).ToArray();
        }

        /// <summary>
        ///     Returns an array of all keys that went are down since the last Update-call
        /// </summary>
        /// <returns></returns>
        public Keys[] KeysThatAreDown()
        {
            return AllKeys.Cast<Keys>().Where(KeyIsDown).ToArray();
        }

        /// <summary>
        ///     Returns whether the given key went up since the last Update-call
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        public bool KeyWentUp(Keys key)
        {
            return KeyWentUp((int)key);
        }

        /// <summary>
        ///     Returns whether the given key went up since the last Update-call
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        public bool KeyWentUp(int key)
        {
            if (!KeyExists(key))
                return false;
            return (bool)PrevKeys[key] && !(bool)Keys[key];
        }

        /// <summary>
        ///     Returns whether the given key went down since the last Update-call
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        public bool KeyWentDown(Keys key)
        {
            return KeyWentDown((int)key);
        }

        /// <summary>
        ///     Returns whether the given key went down since the last Update-call
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        public bool KeyWentDown(int key)
        {
            if (!KeyExists(key))
                return false;
            return !(bool)PrevKeys[key] && (bool)Keys[key];
        }

        /// <summary>
        ///     Returns whether the given key was down at time of the last Update-call
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        public bool KeyIsDown(Keys key)
        {
            return KeyIsDown((int)key);
        }

        /// <summary>
        ///     Returns whether the given key was down at time of the last Update-call
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        public bool KeyIsDown(int key)
        {
            if (!KeyExists(key))
                return false;
            return (bool)PrevKeys[key] || (bool)Keys[key];
        }

        /// <summary>
        ///     Returns whether the given key is contained in the used hashtables
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        private bool KeyExists(int key)
        {
            return (PrevKeys.ContainsKey(key) && Keys.ContainsKey(key));
        }
    }
}