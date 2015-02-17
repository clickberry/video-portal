// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Wrappers.Interface;

namespace Wrappers.Implementation
{
    public class ProcessAsync : IProcessAsync
    {
        private const string ProcFileName = "Ffmpeg/ffmpeg.exe";
        private readonly StringBuilder _output = new StringBuilder();
        private DataReceivedEventHandler _act;

        public async Task Start(string arguments, Action<string> processData, CancellationTokenWrapper token)
        {
            var tcs = new TaskCompletionSource<object>();

            _act = (s, e) =>
            {
                _output.AppendLine(e.Data);

                try
                {
                    processData(e.Data);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            };

            var process = new Process();

            process.Exited += (s, e) => ExitedMethod((Process)s, tcs);

            process.StartInfo = CreateStartInfo(arguments);
            process.EnableRaisingEvents = true;
            process.ErrorDataReceived += _act;
            process.OutputDataReceived += _act;

            _output.Clear();

            try
            {
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

                token.Register(() => tcs.TrySetCanceled());
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            try
            {
                await tcs.Task;
            }
            finally
            {
                TryKillProcess(process);
                process.Dispose();
            }
        }

        private ProcessStartInfo CreateStartInfo(string arguments)
        {
            return new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                FileName = ProcFileName,
                Arguments = arguments
            };
        }

        private void ExitedMethod(Process proc, TaskCompletionSource<object> tcs)
        {
            proc.ErrorDataReceived -= _act;
            proc.OutputDataReceived -= _act;
            switch (proc.ExitCode)
            {
                case 0:
                    tcs.TrySetResult(null);
                    break;
                case -1:
                    tcs.TrySetCanceled();
                    break;
                case 1:
                {
                    string message = string.Format("Incorrect ffmpeg parameters:{0}{1}{0}{2}", Environment.NewLine, proc.StartInfo.Arguments, _output);
                    tcs.TrySetException(new ArgumentException(message));
                }
                    break;
                default:
                {
                    string message = string.Format("Unknown ffmpeg error:{0}{1}{0}{2}", Environment.NewLine, proc.StartInfo.Arguments, _output);
                    tcs.TrySetException(new ApplicationException(message));
                }
                    break;
            }
        }

        private void TryKillProcess(Process process)
        {
            if (!process.HasExited)
            {
                try
                {
                    process.Kill();
                }
                catch (Win32Exception) //The process is terminating.
                {
                }
                catch (InvalidOperationException) //The process has already exited
                {
                }
            }
        }
    }
}