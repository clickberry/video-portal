// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Wrappers
{
    public class ProcessWrapper : IDisposable
    {
        //private ProcessStartInfoWrapper _processStartInfoWrapper;
        private readonly Dictionary<DataReceivedEventHandlerWrapper, DataReceivedEventHandler> _errorDataReceivedList = new Dictionary<DataReceivedEventHandlerWrapper, DataReceivedEventHandler>();
        private Process _process;

        public ProcessWrapper()
        {
            _process = new Process();
        }

        public virtual int BasePriority
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool EnableRaisingEvents
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual int ExitCode
        {
            get { return _process.ExitCode; }
        }

        public virtual DateTime ExitTime
        {
            get { throw new NotImplementedException(); }
        }

        public virtual IntPtr Handle
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int HandleCount
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool HasExited
        {
            get { return _process.HasExited; }
        }

        public virtual int Id
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string MachineName
        {
            get { throw new NotImplementedException(); }
        }

        public virtual ProcessModule MainModule
        {
            get { throw new NotImplementedException(); }
        }

        public virtual IntPtr MainWindowHandle
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string MainWindowTitle
        {
            get { throw new NotImplementedException(); }
        }

        public virtual IntPtr MaxWorkingSet
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual IntPtr MinWorkingSet
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual ProcessModuleCollection Modules
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int NonpagedSystemMemorySize
        {
            get { throw new NotImplementedException(); }
        }

        public virtual long NonpagedSystemMemorySize64
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int PagedMemorySize
        {
            get { throw new NotImplementedException(); }
        }

        public virtual long PagedMemorySize64
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int PagedSystemMemorySize
        {
            get { throw new NotImplementedException(); }
        }

        public virtual long PagedSystemMemorySize64
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int PeakPagedMemorySize
        {
            get { throw new NotImplementedException(); }
        }

        public virtual long PeakPagedMemorySize64
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int PeakVirtualMemorySize
        {
            get { throw new NotImplementedException(); }
        }

        public virtual long PeakVirtualMemorySize64
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int PeakWorkingSet
        {
            get { throw new NotImplementedException(); }
        }

        public virtual long PeakWorkingSet64
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool PriorityBoostEnabled
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual ProcessPriorityClass PriorityClass
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual int PrivateMemorySize
        {
            get { throw new NotImplementedException(); }
        }

        public virtual long PrivateMemorySize64
        {
            get { throw new NotImplementedException(); }
        }

        public virtual TimeSpan PrivilegedProcessorTime
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string ProcessName
        {
            get { throw new NotImplementedException(); }
        }

        public virtual IntPtr ProcessorAffinity
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual bool Responding
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int SessionId
        {
            get { throw new NotImplementedException(); }
        }

        public virtual StreamReader StandardError
        {
            get { return _process.StandardError; }
        }

        public virtual StreamWriter StandardInput
        {
            get { return _process.StandardInput; }
        }

        public virtual StreamReader StandardOutput
        {
            get { return _process.StandardOutput; }
        }

        public virtual ProcessStartInfo StartInfo
        {
            //get { return _processStartInfoWrapper; }
            //set
            //{
            //    _processStartInfoWrapper = value;
            //    _process.StartInfo = _processStartInfoWrapper.ProcessStartInfo;
            //}
            get { return _process.StartInfo; }
            set { _process.StartInfo = value; }
        }

        public virtual DateTime StartTime
        {
            get { throw new NotImplementedException(); }
        }

        public virtual ISynchronizeInvoke SynchronizingObject
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual ProcessThreadCollection Threads
        {
            get { throw new NotImplementedException(); }
        }

        public virtual TimeSpan TotalProcessorTime
        {
            get { throw new NotImplementedException(); }
        }

        public virtual TimeSpan UserProcessorTime
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int VirtualMemorySize
        {
            get { throw new NotImplementedException(); }
        }

        public virtual long VirtualMemorySize64
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int WorkingSet
        {
            get { throw new NotImplementedException(); }
        }

        public virtual long WorkingSet64
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     For use internal wrappers only.
        /// </summary>
        public Process Process
        {
            get { return _process; }
            set { _process = value; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            _process.Dispose();
        }

        #endregion

        public virtual event DataReceivedEventHandlerWrapper ErrorDataReceived
        {
            add
            {
                DataReceivedEventHandler method = (sender, e) => value(sender, new DataReceivedEventArgsWrapper
                {
                    DataReceivedEventArgs = e
                });
                _errorDataReceivedList.Add(value, method);
                _process.ErrorDataReceived += method;
            }
            remove
            {
                DataReceivedEventHandler method = _errorDataReceivedList[value];
                _errorDataReceivedList.Remove(value);
                _process.ErrorDataReceived -= method;
            }
        }


        public virtual event EventHandler Exited
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public virtual event DataReceivedEventHandler OutputDataReceived
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public virtual void BeginErrorReadLine()
        {
            _process.BeginErrorReadLine();
        }

        public virtual void BeginOutputReadLine()
        {
            _process.BeginOutputReadLine();
        }

        public virtual void CancelErrorRead()
        {
            throw new NotImplementedException();
        }

        public virtual void CancelOutputRead()
        {
            throw new NotImplementedException();
        }

        public virtual void Close()
        {
            _process.Close();
        }

        public virtual bool CloseMainWindow()
        {
            throw new NotImplementedException();
        }

        public virtual void Kill()
        {
            _process.Kill();
        }

        public virtual void Refresh()
        {
            throw new NotImplementedException();
        }

        public virtual bool Start()
        {
            return _process.Start();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public virtual void WaitForExit()
        {
            _process.WaitForExit();
        }

        public virtual bool WaitForExit(int milliseconds)
        {
            throw new NotImplementedException();
        }

        public virtual bool WaitForInputIdle()
        {
            throw new NotImplementedException();
        }

        public virtual bool WaitForInputIdle(int milliseconds)
        {
            throw new NotImplementedException();
        }
    }


    public delegate void DataReceivedEventHandlerWrapper(object sender, DataReceivedEventArgsWrapper e);

    public class DataReceivedEventArgsWrapper
    {
        public DataReceivedEventArgs DataReceivedEventArgs { get; set; }

        public virtual string Data
        {
            get { return DataReceivedEventArgs.Data; }
        }
    }
}