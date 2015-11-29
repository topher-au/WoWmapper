using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using Binarysharp.MemoryManagement.Core.Marshaling.Objects;
using Binarysharp.MemoryManagement.Core.Native.Enums;
using Binarysharp.MemoryManagement.Core.Threads;
using Binarysharp.MemoryManagement.Models.Threads;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Binarysharp.MemoryManagement.Factorys
{
    /// <summary>
    ///     Class providing tools for manipulating threads.
    /// </summary>
    public class ThreadFactory : IFactory
    {
        #region Fields, Private Properties

        /// <summary>
        ///     The reference of the <see cref="MemorySharp" /> object.
        /// </summary>
        protected readonly MemorySharp MemorySharp;

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThreadFactory" /> class.
        /// </summary>
        /// <param name="memorySharp">The reference of the <see cref="MemorySharp" /> object.</param>
        internal ThreadFactory(MemorySharp memorySharp)
        {
            // Save the parameter
            MemorySharp = memorySharp;
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Gets the main thread of the remote process.
        /// </summary>
        public RemoteThread MainThread
        {
            get
            {
                return new RemoteThread(MemorySharp,
                    NativeThreads.Aggregate((current, next) => next.StartTime < current.StartTime ? next : current));
            }
        }

        /// <summary>
        ///     Gets the native threads from the remote process.
        /// </summary>
        internal IEnumerable<ProcessThread> NativeThreads
        {
            get
            {
                // Refresh the process info
                MemorySharp.Process.Refresh();
                // Enumerates all threads
                return MemorySharp.Process.Threads.Cast<ProcessThread>();
            }
        }

        /// <summary>
        ///     Gets the threads from the remote process.
        /// </summary>
        public IEnumerable<RemoteThread> RemoteThreads
        {
            get { return NativeThreads.Select(t => new RemoteThread(MemorySharp, t)); }
        }

        /// <summary>
        ///     Gets the thread corresponding to an id.
        /// </summary>
        /// <param name="threadId">The unique identifier of the thread to get.</param>
        /// <returns>A new instance of a <see cref="RemoteThread" /> class.</returns>
        public RemoteThread this[int threadId]
        {
            get { return new RemoteThread(MemorySharp, NativeThreads.First(t => t.Id == threadId)); }
        }

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Releases all resources used by the <see cref="ThreadFactory" /> object.
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose... yet
        }

        #endregion Interface Implementations

        /// <summary>
        ///     Creates a thread that runs in the remote process.
        /// </summary>
        /// <param name="address">
        ///     A pointer to the application-defined function to be executed by the thread and represents
        ///     the starting address of the thread in the remote process.
        /// </param>
        /// <param name="parameter">A variable to be passed to the thread function.</param>
        /// <param name="isStarted">Sets if the thread must be started just after being created.</param>
        /// <returns>A new instance of the <see cref="RemoteThread" /> class.</returns>
        public RemoteThread Create(IntPtr address, dynamic parameter, bool isStarted = true)
        {
            // Marshal the parameter
            var marshalledParameter = MarshalValue.Marshal(MemorySharp.Handle, parameter);

            //Create the thread
            var ret = ThreadCore.NtQueryInformationThread(
                ThreadCore.CreateRemoteThread(MemorySharp.SafeHandle, address, marshalledParameter.Reference,
                    ThreadCreationFlags.Suspended));

            // Find the managed object corresponding to this thread
            var result = new RemoteThread(MemorySharp,
                MemorySharp.Factories.ThreadFactory.NativeThreads.First(t => t.Id == ret.ThreadId), marshalledParameter);

            // If the thread must be started
            if (isStarted)
                result.Resume();
            return result;
        }

        /// <summary>
        ///     Creates a thread that runs in the remote process.
        /// </summary>
        /// <param name="address">
        ///     A pointer to the application-defined function to be executed by the thread and represents
        ///     the starting address of the thread in the remote process.
        /// </param>
        /// <param name="isStarted">Sets if the thread must be started just after being created.</param>
        /// <returns>A new instance of the <see cref="RemoteThread" /> class.</returns>
        public RemoteThread Create(IntPtr address, bool isStarted = true)
        {
            //Create the thread
            var ret = ThreadCore.NtQueryInformationThread(
                ThreadCore.CreateRemoteThread(MemorySharp.SafeHandle, address, IntPtr.Zero,
                    ThreadCreationFlags.Suspended));

            // Find the managed object corresponding to this thread
            var result = new RemoteThread(MemorySharp,
                MemorySharp.Factories.ThreadFactory.NativeThreads.First(t => t.Id == ret.ThreadId));

            // If the thread must be started
            if (isStarted)
                result.Resume();
            return result;
        }

        /// <summary>
        ///     Creates a thread in the remote process and blocks the calling thread until the thread terminates.
        /// </summary>
        /// <param name="address">
        ///     A pointer to the application-defined function to be executed by the thread and represents
        ///     the starting address of the thread in the remote process.
        /// </param>
        /// <param name="parameter">A variable to be passed to the thread function.</param>
        /// <returns>A new instance of the <see cref="RemoteThread" /> class.</returns>
        public RemoteThread CreateAndJoin(IntPtr address, dynamic parameter)
        {
            // Create the thread
            var ret = Create(address, parameter);
            // Wait the end of the thread
            ret.Join();
            // Return the thread
            return ret;
        }

        /// <summary>
        ///     Creates a thread in the remote process and blocks the calling thread until the thread terminates.
        /// </summary>
        /// <param name="address">
        ///     A pointer to the application-defined function to be executed by the thread and represents
        ///     the starting address of the thread in the remote process.
        /// </param>
        /// <returns>A new instance of the <see cref="RemoteThread" /> class.</returns>
        public RemoteThread CreateAndJoin(IntPtr address)
        {
            // Create the thread
            var ret = Create(address);
            // Wait the end of the thread
            ret.Join();
            // Return the thread
            return ret;
        }

        /// <summary>
        ///     Gets a thread by its id in the remote process.
        /// </summary>
        /// <param name="id">The id of the thread.</param>
        /// <returns>A new instance of the <see cref="RemoteThread" /> class.</returns>
        public RemoteThread GetThreadById(int id)
        {
            return new RemoteThread(MemorySharp, NativeThreads.First(t => t.Id == id));
        }

        /// <summary>
        ///     Resumes all threads.
        /// </summary>
        public void ResumeAll()
        {
            foreach (var thread in RemoteThreads)
            {
                thread.Resume();
            }
        }

        /// <summary>
        ///     Suspends all threads.
        /// </summary>
        public void SuspendAll()
        {
            foreach (var thread in RemoteThreads)
            {
                thread.Suspend();
            }
        }
    }
}