using System;

namespace Binarysharp.MemoryManagement.Core.Managment.Interfaces
{
    /// <summary>
    ///     Class PluginContainer.
    /// </summary>
    public class PluginContainer : IPlugin
    {
        #region Fields, Private Properties
        /// <summary>
        ///     The plugin
        /// </summary>
        internal IPlugin Plugin;
        #endregion

        #region Constructors, Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="PluginContainer" /> class.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        internal PluginContainer(IPlugin plugin)
        {
            Plugin = plugin;
        }
        #endregion

        #region Public Properties, Indexers
        /// <summary>
        ///     Gets a value indicating whether the element is disposed.
        /// </summary>
        /// <value><c>true</c> if this instance is disposed; otherwise, <c>false</c>.</value>
        public bool IsDisposed => Plugin.IsDisposed;

        /// <summary>
        ///     Gets a value indicating whether the element must be disposed when the Garbage Collector collects the object.
        /// </summary>
        /// <value><c>true</c> if [must be disposed]; otherwise, <c>false</c>.</value>
        public bool MustBeDisposed => Plugin.MustBeDisposed;

        /// <summary>
        ///     States if the element is enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled => Plugin.IsEnabled;

        /// <summary>
        ///     The name of the element.
        /// </summary>
        /// <value>The name.</value>
        public string Name => Plugin.Name;

        /// <summary>
        ///     Current version of this plugin
        /// </summary>
        /// <value>The version.</value>
        public Version Version => Plugin.Version;

        /// <summary>
        ///     The creator of this plugin
        /// </summary>
        /// <value>The author.</value>
        public string Author => Plugin.Author;

        /// <summary>
        ///     Description to display on the plugin interface
        /// </summary>
        /// <value>The description.</value>
        public string Description => Plugin.Description;
        #endregion

        #region Interface Implementations
        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Plugin.Dispose();
        }

        /// <summary>
        ///     Disables the element.
        /// </summary>
        public void Disable()
        {
            Plugin.Disable();
        }

        /// <summary>
        ///     Enables the element.
        /// </summary>
        public void Enable()
        {
            Plugin.Enable();
        }

        /// <summary>
        ///     Pulse one iteration of this instance's logic.
        /// </summary>
        public void Pulse()
        {
            Plugin.Pulse();
        }

        /// <summary>
        ///     Work to be done when the plugin is disabled by the user
        /// </summary>
        public void Disabled()
        {
            Plugin.Disabled();
        }

        /// <summary>
        ///     Work to be done when the plugin is loaded by the bot on startup
        /// </summary>
        public void Initialize()
        {
            Plugin.Initialize();
        }
        #endregion
    }
}