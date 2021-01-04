using System;
using System.Diagnostics;

namespace Nextwin.Util
{
    public static class ProcessManager
    {
        /// <summary>
        /// 동기적으로 cmd 실행
        /// </summary>
        /// <param name="arg">명령어를 포함한 인자</param>
        /// <param name="useShellExecute">쉘을 실행시킬지 여부</param>
        /// <returns></returns>
        public static string SyncCmd(string arg, bool useShellExecute = false)
        {
            ProcessStartInfo startInfo = CreateDefaultProcessStartInfo("CMD.exe", useShellExecute);
            Process process = CreateDefaultProcess(startInfo);

            return SyncExecuteProcess(process, arg);
        }

        private static string SyncExecuteProcess(Process process, string arg)
        {
            string result = "Error";

            try
            {
                process.StandardInput.Write($"{arg}{Environment.NewLine}");
                process.StandardInput.Close();
                result = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                process.Close();
                Print.Log(result);
            }
            catch(Exception e)
            {
                Print.Log(e.ToString());
            }

            return result;
        }

        private static ProcessStartInfo CreateDefaultProcessStartInfo(string processName, bool useShellExecute)
        {
            return new ProcessStartInfo
            {
                FileName = processName,
                UseShellExecute = useShellExecute,
                RedirectStandardInput = !useShellExecute,
                RedirectStandardOutput = !useShellExecute,
                RedirectStandardError = !useShellExecute
            };
        }

        private static Process CreateDefaultProcess(ProcessStartInfo startInfo)
        {
            return new Process
            {
                EnableRaisingEvents = false,
                StartInfo = startInfo
            };
        }
    }
}
