using System;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace ConsoleShellCommand
{
    public static class ShellHelper
    {
        /// <summary>
        /// Linux shell cmd
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static string Bash(this string cmd, bool pWaitFlag = true)
        {
            if (Environment.OSVersion.Platform.ToString().StartsWith("Win", StringComparison.OrdinalIgnoreCase))
            {
                return "";
            }
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            if (pWaitFlag)
            {
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return result;
            }
            return null;
        }

        /// <summary>
        /// Linux 下執行 script 的呼叫方式
        /// </summary>
        /// <param name="scriptFile"></param>
        /// <returns></returns>
        public static Process BashScript(this string scriptFile)
        {
            if (string.IsNullOrWhiteSpace(scriptFile)) return null;
            if (!scriptFile.EndsWith(".sh", StringComparison.CurrentCultureIgnoreCase))
                scriptFile += ".sh";
            if (!System.IO.File.Exists(scriptFile))
                return null;
            var process = CreateNewProcess("bash", scriptFile, false);
            process.Start();
            return process;
        }

        public static Task<int> Bash(this string cmd)
        {
            var source = new TaskCompletionSource<int>();
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var process = CreateNewProcess("/bin/bash", $"-c \"{escapedArgs}\"");
            process.Exited += (sender, args) =>
            {
                if (process.ExitCode == 0)
                {
                    source.SetResult(0);
                }
                else
                {
                    source.SetException(new Exception($"Command `{cmd}` failed with exit code `{process.ExitCode}`"));
                }
                process.Dispose();
            };
            try
            {
                process.Start();
            }
            catch (Exception e)
            {
                source.SetException(e);
            }
            return source.Task;
        }

        /// <summary>
        /// 產生 script 的 Process，目前僅提供 Linux 下的 bash shell
        /// </summary>
        /// <param name="shellType"></param>
        /// <param name="arguments"></param>
        /// <param name="enableRaisingEvents"></param>
        /// <returns></returns>
        private static Process CreateNewProcess(string shellType = "bash", string arguments = "", bool enableRaisingEvents = true)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = enableRaisingEvents
            };
            if (!string.IsNullOrWhiteSpace(shellType))
            {
                process.StartInfo.FileName = shellType;
            }
            if (!string.IsNullOrWhiteSpace(arguments))
            {
                process.StartInfo.Arguments = arguments;
            }
            return process;
        }

        /// <summary>
        /// TODO: trace if this has been retrieved
        /// This only works in linux, remark for testing
        /// </summary>
        /// <returns></returns>
        // public static string CPU_Serial() => @"cat /proc/cpuinfo | perl -n -e '/^Serial\s*:\s([0-9a-f]{16})$/ && print ""$1\n""'".Bash();
        public static string CPU_Serial()
        {
            return "TestingCpuId";
        }

        //public static string WiFi_Ip() => @"ip addr show wlan0 | grep -o 'inet [0-9]\+\.[0-9]\+\.[0-9]\+\.[0-9]\+' | grep -o [0-9].*".Bash();
        //public static string wlan_ip() => @"ip addr show wlan0 | grep -o 'inet [0-9]\+\.[0-9]\+\.[0-9]\+\.[0-9]\+' | grep -o [0-9].*".Bash();
        //public static string eth_ip() => @"ip addr show eth0 | grep -o 'inet [0-9]\+\.[0-9]\+\.[0-9]\+\.[0-9]\+' | grep -o [0-9].*".Bash();

        //public static string eth_mac_address() => @"cat /sys/class/net/eth0/address".Bash();
        //public static string wlan_mac_address() => @"cat /sys/class/net/wlan0/address".Bash();
        public static bool ping(string ipAddress)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;
            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 3000;
            PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);
            if (reply.Status == IPStatus.Success) return true;
            return false;
        }

    }
    public class ShellResult
    {
        public Process ShellProcess { get; set; }

    }
    public class BashShell
    {

        private Process _shellProcess = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "/bin/bash",
                RedirectStandardInput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            }
        };
        public Process Process
        {
            get
            {
                return _shellProcess;
            }
        }
        public Process Execute(string pCommand)
        {
            var _argument = pCommand.Replace("\"", "\\\"");
            _shellProcess.StartInfo.Arguments = _argument;
            _shellProcess.Start();
            return _shellProcess;
        }

    }
}
