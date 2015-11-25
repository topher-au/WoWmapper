using System;
using System.Collections.Generic;
using System.Linq;
using Binarysharp.MemoryManagement.Core.Helpers;
using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using Binarysharp.MemoryManagement.Core.Windows;
using Binarysharp.MemoryManagement.Core.Windows.Objects;

namespace Binarysharp.MemoryManagement.Factorys
{
    /// <summary>
    ///     Class providing tools for manipulating windows.
    /// </summary>
    public class WindowFactory : IFactory
    {
        #region Fields, Private Properties
        /// <summary>
        ///     The reference of the <see cref="MemorySharp" /> object.
        /// </summary>
        private readonly MemorySharp _memorySharp;
        #endregion

        #region Constructors, Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowFactory" /> class.
        /// </summary>
        /// <param name="memorySharp">The reference of the <see cref="MemorySharp" /> object.</param>
        internal WindowFactory(MemorySharp memorySharp)
        {
            // Save the parameter
            _memorySharp = memorySharp;
        }
        #endregion

        #region Public Properties, Indexers
        /// <summary>
        ///     Gets the unique name for this instance.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; } = "WindowFactory-" + Randomizer.GenerateString();

        /// <summary>
        ///     Gets all the child windows that belong to the application.
        /// </summary>
        public IEnumerable<ProxyWindow> ChildWindows
        {
            get { return ChildWindowHandles.Select(handle => new ProxyWindow(_memorySharp.Process, handle)); }
        }

        /// <summary>
        ///     Gets all the child window handles that belong to the application.
        /// </summary>
        internal IEnumerable<IntPtr> ChildWindowHandles
            => WindowCore.EnumChildWindows(_memorySharp.Process.MainWindowHandle);

        /// <summary>
        ///     Gets the main window of the application.
        /// </summary>
        public ProxyWindow MainWindow => new ProxyWindow(_memorySharp.Process, MainWindowHandle);

        /// <summary>
        ///     Gets the main window handle of the application.
        /// </summary>
        public IntPtr MainWindowHandle => _memorySharp.Process.MainWindowHandle;

        /// <summary>
        ///     Gets all the windows that have the same specified title.
        /// </summary>
        /// <param name="windowTitle">The window title string.</param>
        /// <returns>A collection of <see cref="ProxyWindow" />.</returns>
        public IEnumerable<ProxyWindow> this[string windowTitle] => GetWindowsByTitle(windowTitle);

        /// <summary>
        ///     Gets all the windows that belong to the application.
        /// </summary>
        public IEnumerable<ProxyWindow> RemoteWindows
        {
            get { return WindowHandles.Select(handle => new ProxyWindow(_memorySharp.Process, handle)); }
        }

        /// <summary>
        ///     Gets all the window handles that belong to the application.
        /// </summary>
        internal IEnumerable<IntPtr> WindowHandles => new List<IntPtr>(ChildWindowHandles) {MainWindowHandle};
        #endregion

        #region Interface Implementations
        /// <summary>
        ///     Releases all resources used by the <see cref="WindowFactory" /> object.
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose... yet
        }
        #endregion

        /// <summary>
        ///     Gets all the windows that have the specified class name.
        /// </summary>
        /// <param name="className">The class name string.</param>
        /// <returns>A collection of <see cref="ProxyWindow" />.</returns>
        public IEnumerable<ProxyWindow> GetWindowsByClassName(string className)
        {
            return WindowHandles
                .Where(handle => WindowCore.GetClassName(handle) == className)
                .Select(handle => new ProxyWindow(_memorySharp.Process, handle));
        }

        /// <summary>
        ///     Gets all the windows that have the same specified title.
        /// </summary>
        /// <param name="windowTitle">The window title string.</param>
        /// <returns>A collection of <see cref="ProxyWindow" />.</returns>
        public IEnumerable<ProxyWindow> GetWindowsByTitle(string windowTitle)
        {
            return WindowHandles
                .Where(handle => WindowCore.GetWindowText(handle) == windowTitle)
                .Select(handle => new ProxyWindow(_memorySharp.Process, handle));
        }

        /// <summary>
        ///     Gets all the windows that contain the specified title.
        /// </summary>
        /// <param name="windowTitle">A part a window title string.</param>
        /// <returns>A collection of <see cref="ProxyWindow" />.</returns>
        public IEnumerable<ProxyWindow> GetWindowsByTitleContains(string windowTitle)
        {
            return WindowHandles
                .Where(handle => WindowCore.GetWindowText(handle).Contains(windowTitle))
                .Select(handle => new ProxyWindow(_memorySharp.Process, handle));
        }
    }
}