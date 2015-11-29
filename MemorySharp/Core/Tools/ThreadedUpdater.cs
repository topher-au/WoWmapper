using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using System;
using System.Threading;

namespace Binarysharp.MemoryManagement.Core.Tools
{
    /// <summary>
    ///     Class to provide an effective way to use task to run events and methods at a specified interval.
    ///     <remarks>All Credits to this class is to Zat@unknowncheats.</remarks>
    ///     .
    /// </summary>
    public class ThreadedUpdater : INamedElement
    {
        #region Public Delegates/Events

        /// <summary>
        ///     An event that Occurs repeatedly at the Interval for this Instance.
        /// </summary>
        public event EventHandler<DeltaEventArgs> OnUpdate;

        #endregion Public Delegates/Events

        #region Fields, Private Properties

        private long BeginTime { get; set; }
        private long FpsTick { get; set; }
        private long LastTick { get; set; }
        private Thread WorkThread { get; set; }

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThreadedUpdater" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="updateRateMs">The rate the the <code>OnUpdate</code> event is fired in milaseconds.</param>
        public ThreadedUpdater(string name, int updateRateMs)
        {
            Name = name;
            Interval = updateRateMs;
        }

        /// <summary>
        ///     Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage
        ///     collection.
        /// </summary>
        ~ThreadedUpdater()
        {
            Dispose();
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Gets or sets the update interval [in milaseconds].
        /// </summary>
        /// <value>The updates per second.</value>
        public int Interval { get; set; }

        /// <summary>
        ///     Gets the frame rate as frames per second.
        /// </summary>
        /// <value>The frame rate.</value>
        public int FrameRate { get; private set; }

        /// <summary>
        ///     Gets the last frames per second rate.
        /// </summary>
        /// <value>The last frames per second rate.</value>
        public int LastFrameRate { get; private set; }

        /// <summary>
        ///     Gets the total tick count since the threaded updater was enabled.
        /// </summary>
        /// <value>The tick count.</value>
        public int TickCount { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the threaded updater is enabled.
        /// </summary>
        /// <value><c>true</c> if the threaded updater is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled { get; private set; }

        /// <summary>
        ///     The unique name that represents this instance.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        ///     Gets a value indicating whether the updateris disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        //
        /// <summary>
        ///     Gets a value indicating whether the element must be disposed when the Garbage Collector collects the object. The
        ///     default value is true.
        /// </summary>
        public bool MustBeDisposed { get; set; } = true;

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Enables the updater.
        /// </summary>
        public void Enable()
        {
            if (WorkThread != null)
            {
                Disable();
            }
            IsEnabled = true;
            BeginTime = DateTime.Now.Ticks;
            WorkThread = new Thread(WorkerLoop)
            {
                IsBackground = true,
                Priority = ThreadPriority.Highest
            };
            WorkThread.Start();
        }

        /// <summary>
        ///     Disables the updater.
        /// </summary>
        public void Disable()
        {
            IsEnabled = false;
            if (WorkThread == null)
            {
                return;
            }
            if (WorkThread.ThreadState == ThreadState.Running)
            {
                WorkThread.Abort();
            }
            WorkThread = null;
        }

        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            Disable();
            IsDisposed = true;
        }

        #endregion Interface Implementations

        /// <summary>
        ///     Calculates the average frames per second of the updater instance.
        /// </summary>
        /// <returns>The average frames per second.</returns>
        public int GetAverageFps()
        {
            return (int)(TickCount / GetRuntime().TotalSeconds);
        }

        /// <summary>
        ///     Gets the total run time since the updater was started as a <see cref="TimeSpan" />
        /// </summary>
        /// <returns>The total time running as a<see cref="TimeSpan" />.</returns>
        public TimeSpan GetRuntime()
        {
            return new TimeSpan(DateTime.Now.Ticks - BeginTime);
        }

        /// <summary>
        ///     Handles the <see cref="E:UpdateEvent" /> event.
        /// </summary>
        /// <param name="e">The <see cref="DeltaEventArgs" /> instance containing the event data.</param>
        public virtual void OnUpdateEvent(DeltaEventArgs e)
        {
            OnUpdate?.Invoke(this, e);
        }

        private void CalculateFramesPerSecond()
        {
            if (DateTime.Now.Ticks - FpsTick >= TimeSpan.TicksPerSecond)
            {
                LastFrameRate = FrameRate;
                FrameRate = 0;
                FpsTick = DateTime.Now.Ticks;
            }
            FrameRate++;
        }

        /// <summary>
        ///     The method the <see cref="WorkThread" /> will be created with.
        /// </summary>
        private void WorkerLoop()
        {
            LastTick = DateTime.Now.Ticks;
            while (IsEnabled)
            {
                CalculateFramesPerSecond();
                var elapsedSeconds = new TimeSpan(DateTime.Now.Ticks - LastTick).TotalSeconds;
                OnUpdateEvent(new DeltaEventArgs(elapsedSeconds));
                TickCount++;
                LastTick = DateTime.Now.Ticks;
                Thread.Sleep(Interval);
            }
        }

        /// <summary>
        ///     Event args for the <see cref="ThreadedUpdater" /> class events.
        /// </summary>
        public class DeltaEventArgs : EventArgs
        {
            #region Constructors, Destructors

            /// <summary>
            ///     Initializes a new <see cref="DeltaEventArgs" /> instance.
            /// </summary>
            /// <param name="secondsElapsed">Seconds elapsed.</param>
            public DeltaEventArgs(double secondsElapsed)
            {
                SecondsElapsed = secondsElapsed;
            }

            #endregion Constructors, Destructors

            #region Public Properties, Indexers

            /// <summary>
            ///     Seconds elapsed.
            /// </summary>
            public double SecondsElapsed { get; private set; }

            #endregion Public Properties, Indexers
        }
    }
}