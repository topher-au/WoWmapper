using Binarysharp.MemoryManagement.Core.Marshaling;
using Binarysharp.MemoryManagement.Core.Marshaling.Interfaces;
using Binarysharp.MemoryManagement.Core.Native.Enums;
using Binarysharp.MemoryManagement.Core.Native.Objects;
using Binarysharp.MemoryManagement.Core.Native.Structs;
using Binarysharp.MemoryManagement.Core.Threads;
using Binarysharp.MemoryManagement.Core.Threads.Enums;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThreadState = System.Diagnostics.ThreadState;

namespace Binarysharp.MemoryManagement.Models.Threads
{
    /// <summary>
    ///     Class repesenting a thread in the remote process.
    /// </summary>
    public class RemoteThread : IDisposable, IEquatable<RemoteThread>
    {
        #region Fields, Private Properties

        /// <summary>
        ///     The parameter passed to the thread when it was created.
        /// </summary>
        private readonly IMarshalledValue _parameter;

        /// <summary>
        ///     The task involved in cleaning the parameter memory when the <see cref="RemoteThread" /> object is collected.
        /// </summary>
        private readonly Task _parameterCleaner;

        /// <summary>
        ///     The reference of the <see cref="MemoryManagement.MemorySharp" /> object.
        /// </summary>
        protected readonly MemorySharp MemorySharp;

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoteThread" /> class.
        /// </summary>
        /// <param name="memorySharp">The reference of the <see cref="MemoryManagement.MemorySharp" /> object.</param>
        /// <param name="thread">The native <see cref="ProcessThread" /> object.</param>
        internal RemoteThread(MemorySharp memorySharp, ProcessThread thread)
        {
            // Save the parameters
            MemorySharp = memorySharp;
            Native = thread;
            // Save the thread id
            Id = thread.Id;
            // Open the thread
            Handle = ThreadCore.OpenThread(ThreadAccessFlags.AllAccess, Id);
            // Initialize the TEB
            Teb = new ManagedTeb(MemorySharp.Process, ManagedTeb.FindTeb(Handle));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoteThread" /> class.
        /// </summary>
        /// <param name="memorySharp">The reference of the <see cref="MemoryManagement.MemorySharp" /> object.</param>
        /// <param name="thread">The native <see cref="ProcessThread" /> object.</param>
        /// <param name="parameter">The parameter passed to the thread when it was created.</param>
        internal RemoteThread(MemorySharp memorySharp, ProcessThread thread, IMarshalledValue parameter = null)
            : this(memorySharp, thread)
        {
            // Save the parameter
            _parameter = parameter;
            // Create the task
            _parameterCleaner = new Task(() =>
                                         {
                                             Join();
                                             _parameter.Dispose();
                                         });
        }

        /// <summary>
        ///     Frees resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~RemoteThread()
        {
            Dispose();
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Gets or sets the full context of the thread.
        ///     If the thread is not already suspended, performs a <see cref="Suspend" /> and <see cref="Resume" /> call on the
        ///     thread.
        /// </summary>
        public ThreadContext Context
        {
            get
            {
                // Check if the thread is alive
                if (IsAlive)
                {
                    // Check if the thread is already suspended
                    var isSuspended = IsSuspended;
                    try
                    {
                        // Suspend the thread if it wasn't
                        if (!isSuspended)
                            Suspend();
                        // Get the context
                        return ThreadCore.GetThreadContext(Handle,
                            ThreadContextFlags.All | ThreadContextFlags.FloatingPoint |
                            ThreadContextFlags.DebugRegisters | ThreadContextFlags.ExtendedRegisters);
                    }
                    finally
                    {
                        // Resume the thread if it wasn't suspended
                        if (!isSuspended)
                            Resume();
                    }
                }
                // The thread is closed, cannot set the context
                throw new ThreadStateException($"Couldn't set the context of the thread #{Id} because it is terminated.");
            }
            set
            {
                // Check if the thread is alive
                if (!IsAlive) return;

                // Check if the thread is already suspended
                var isSuspended = IsSuspended;
                try
                {
                    // Suspend the thread if it wasn't
                    if (!isSuspended)
                        Suspend();
                    // Set the context
                    ThreadCore.SetThreadContext(Handle, value);
                }
                finally
                {
                    // Resume the thread if it wasn't suspended
                    if (!isSuspended)
                        Resume();
                }
            }
        }

        /// <summary>
        ///     The remote thread handle opened with all rights.
        /// </summary>
        public SafeMemoryHandle Handle { get; }

        /// <summary>
        ///     Gets the unique identifier of the thread.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets if the thread is alive.
        /// </summary>
        public bool IsAlive => !IsTerminated;

        /// <summary>
        ///     Gets if the thread is the main one in the remote process.
        /// </summary>
        public bool IsMainThread => this == MemorySharp.Factories.ThreadFactory.MainThread;

        /// <summary>
        ///     Gets if the thread is suspended.
        /// </summary>
        public bool IsSuspended
        {
            get
            {
                // Refresh the thread info
                Refresh();
                // Return if the thread is suspended
                return Native != null && Native.ThreadState == ThreadState.Wait &&
                       Native.WaitReason == ThreadWaitReason.Suspended;
            }
        }

        /// <summary>
        ///     Gets if the thread is terminated.
        /// </summary>
        public bool IsTerminated
        {
            get
            {
                // Refresh the thread info
                Refresh();
                // Check if the thread is terminated
                return Native == null;
            }
        }

        /// <summary>
        ///     The native <see cref="ProcessThread" /> object corresponding to this thread.
        /// </summary>
        public ProcessThread Native { get; private set; }

        /// <summary>
        ///     The Thread Environment Block of the thread.
        /// </summary>
        public ManagedTeb Teb { get; private set; }

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Releases all resources used by the <see cref="RemoteThread" /> object.
        /// </summary>
        public virtual void Dispose()
        {
            // Close the thread handle
            Handle.Close();
            // Avoid the finalizer
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        public bool Equals(RemoteThread other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || (Id == other.Id && MemorySharp.Equals(other.MemorySharp));
        }

        #endregion Interface Implementations

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RemoteThread)obj);
        }

        /// <summary>
        ///     Gets the termination status of the thread.
        /// </summary>
        public T GetExitCode<T>()
        {
            // Get the exit code of the thread (can be nullable)
            var ret = ThreadCore.GetExitCodeThread(Handle);
            // Return the exit code or the default value of T if there's no exit code
            return ret.HasValue ? SafeMarshal<T>.PtrToObject(MemorySharp.Handle, ret.Value) : default(T);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ MemorySharp.GetHashCode();
        }

        /// <summary>
        ///     Gets the linear address of a specified segment.
        /// </summary>
        /// <param name="segment">The segment to get.</param>
        /// <returns>A <see cref="IntPtr" /> pointer corresponding to the linear address of the segment.</returns>
        public IntPtr GetRealSegmentAddress(SegmentRegisters segment)
        {
            // Get a selector entry for the segment
            LdtEntry entry;
            switch (segment)
            {
                case SegmentRegisters.Cs:
                    entry = ThreadCore.GetThreadSelectorEntry(Handle, Context.SegCs);
                    break;

                case SegmentRegisters.Ds:
                    entry = ThreadCore.GetThreadSelectorEntry(Handle, Context.SegDs);
                    break;

                case SegmentRegisters.Es:
                    entry = ThreadCore.GetThreadSelectorEntry(Handle, Context.SegEs);
                    break;

                case SegmentRegisters.Fs:
                    entry = ThreadCore.GetThreadSelectorEntry(Handle, Context.SegFs);
                    break;

                case SegmentRegisters.Gs:
                    entry = ThreadCore.GetThreadSelectorEntry(Handle, Context.SegGs);
                    break;

                case SegmentRegisters.Ss:
                    entry = ThreadCore.GetThreadSelectorEntry(Handle, Context.SegSs);
                    break;

                default:
                    throw new InvalidEnumArgumentException("segment");
            }
            // Compute the linear address
            return new IntPtr(entry.BaseLow | (entry.BaseMid << 16) | (entry.BaseHi << 24));
        }

        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(RemoteThread left, RemoteThread right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(RemoteThread left, RemoteThread right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Discards any information about this thread that has been cached inside the process component.
        /// </summary>
        public void Refresh()
        {
            if (Native == null)
                return;
            // Refresh the process info
            MemorySharp.Process.Refresh();
            // Get new info about the thread
            Native = MemorySharp.Factories.ThreadFactory.NativeThreads.FirstOrDefault(t => t.Id == Native.Id);
        }

        /// <summary>
        ///     Blocks the calling thread until the thread terminates.
        /// </summary>
        public void Join()
        {
            ThreadCore.WaitForSingleObject(Handle);
        }

        /// <summary>
        ///     Blocks the calling thread until a thread terminates or the specified time elapses.
        /// </summary>
        /// <param name="time">The timeout.</param>
        /// <returns>The return value is a flag that indicates if the thread terminated or if the time elapsed.</returns>
        public WaitValues Join(TimeSpan time)
        {
            return ThreadCore.WaitForSingleObject(Handle, time);
        }

        /// <summary>
        ///     Resumes a thread that has been suspended.
        /// </summary>
        public void Resume()
        {
            // Check if the thread is still alive
            if (!IsAlive) return;

            // Start the thread
            ThreadCore.ResumeThread(Handle);

            // Start a task to clean the memory used by the parameter if we created the thread
            if (_parameter != null && !_parameterCleaner.IsCompleted)
                _parameterCleaner.Start();
        }

        /// <summary>
        ///     Either suspends the thread, or if the thread is already suspended, has no effect.
        /// </summary>
        /// <returns>A new instance of the <see cref="FrozenThread" /> class. If this object is disposed, the thread is resumed.</returns>
        public FrozenThread Suspend()
        {
            if (!IsAlive) return null;
            ThreadCore.SuspendThread(Handle);
            return new FrozenThread(this);
        }

        /// <summary>
        ///     Terminates the thread.
        /// </summary>
        /// <param name="exitCode">The exit code of the thread to close.</param>
        public void Terminate(int exitCode = 0)
        {
            if (IsAlive)
                ThreadCore.TerminateThread(Handle, exitCode);
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"Id = {Id} IsAlive = {IsAlive} IsMainThread = {IsMainThread}";
        }
    }
}