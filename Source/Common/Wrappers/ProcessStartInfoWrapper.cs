// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security;
using System.Text;

namespace Wrappers
{
    public class ProcessStartInfoWrapper
    {
        private ProcessStartInfo _processStartInfo;

        public ProcessStartInfoWrapper()
        {
            _processStartInfo = new ProcessStartInfo();
        }

        public ProcessStartInfoWrapper(string fileName)
        {
            _processStartInfo = new ProcessStartInfo(fileName);
        }

        public ProcessStartInfoWrapper(string fileName, string arguments)
        {
            _processStartInfo = new ProcessStartInfo(fileName, arguments);
        }

        public virtual string Arguments
        {
            get { return _processStartInfo.Arguments; }
            set { _processStartInfo.Arguments = value; }
        }

        public virtual bool CreateNoWindow
        {
            get { return _processStartInfo.CreateNoWindow; }
            set { _processStartInfo.CreateNoWindow = value; }
        }

        public virtual string Domain
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual StringDictionary EnvironmentVariables
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool ErrorDialog
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual IntPtr ErrorDialogParentHandle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string FileName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual bool LoadUserProfile
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual SecureString Password
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual bool RedirectStandardError
        {
            get { return _processStartInfo.RedirectStandardError; }
            set { _processStartInfo.RedirectStandardError = value; }
        }

        public virtual bool RedirectStandardInput
        {
            get { return _processStartInfo.RedirectStandardInput; }
            set { _processStartInfo.RedirectStandardInput = value; }
        }

        public virtual bool RedirectStandardOutput
        {
            get { return _processStartInfo.RedirectStandardOutput; }
            set { _processStartInfo.RedirectStandardOutput = value; }
        }

        public virtual Encoding StandardErrorEncoding
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual Encoding StandardOutputEncoding
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        public virtual string UserName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual bool UseShellExecute
        {
            get { return _processStartInfo.UseShellExecute; }
            set { _processStartInfo.UseShellExecute = value; }
        }

        public virtual string Verb
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string Verbs
        {
            get { throw new NotImplementedException(); }
        }

        public virtual ProcessWindowStyle WindowStyle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string WorkingDirectory
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        /// <summary>
        ///     For use internal wrappers only.
        /// </summary>
        public ProcessStartInfo ProcessStartInfo
        {
            get { return _processStartInfo; }
            set { _processStartInfo = value; }
        }
    }
}