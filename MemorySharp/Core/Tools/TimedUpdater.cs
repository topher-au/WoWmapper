using System;
using System.Threading;

namespace Binarysharp.MemoryManagement.Core.Tools
{
    /// <summary>
    ///     Class to provide an effective way to use a threaded updater to run events and methods at a specified interval.
    ///     <remarks>This class is not thread safe by default. Careful consideration is needed to prevent threading issues.</remarks>
    /// </summary>
    public class TimedUpdater<T>
    {
        #region Public Delegates/Events
        /// <summary>
        ///     An event that is raised repeatedly at the Interval[in milliseconds] for this Instance.
        /// </summary>
        public event EventHandler<T> OnUpdate;
        #endregion

        #region Fields, Private Properties
        private Timer Timer { get; }
        private TimerCallback Callback { get; }
        #endregion

        #region Constructors, Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="TimedUpdater{T}" /> class.
        /// </summary>
        /// <param name="state">The state object to use for updating data.</param>
        /// <param name="name">The unique name representing this instance.</param>
        /// <param name="updateRateMs">The rate the the <code>OnUpdate</code> event is raised in milliseconds.</param>
        public TimedUpdater(T state, string name, int updateRateMs)
        {
            Name = name;
            Interval = updateRateMs;
            State = state;
            Callback += Process;
            Timer = new Timer(Callback, State, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        ///     Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage
        ///     collection.
        /// </summary>
        ~TimedUpdater()
        {
            Dispose();
        }
        #endregion

        #region Public Properties, Indexers
        protected T State { get; }

        /// <summary>
        ///     Gets or sets the interval in milliseconds.
        /// </summary>
        /// <value>The interval in milaseconds.</value>
        public int Interval { get; set; }

        /// <summary>
        ///     States if the updater is enabled.
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        ///     Gets the unique name that represents this instance.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }
        #endregion

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Disable();
            Timer.Dispose();
        }

        /// <summary>
        ///     Enables the updater.
        /// </summary>
        public void Enable()
        {
            Timer.Change(Interval, Interval);
            IsEnabled = true;
        }

        /// <summary>
        ///     Disables the updater.
        /// </summary>
        public void Disable()
        {
            Timer.Change(int.MaxValue, Interval);
            IsEnabled = false;
        }

        /// <summary>
        ///     Processes the specified object casted to the type.
        /// </summary>
        /// <param name="e">The e.</param>
        protected virtual void Process(object e)
        {
            OnUpdate?.Invoke(this, (T) e);
        }
    }
}