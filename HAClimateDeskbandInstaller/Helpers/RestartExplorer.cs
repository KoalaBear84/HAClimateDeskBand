using System;
using System.Collections.Generic;
using System.Diagnostics;
using Com = System.Runtime.InteropServices.ComTypes;

namespace HAClimateDeskbandInstaller.Helpers
{
    class RestartExplorer : Win32Api
    {
        public event Action<string> ReportProgress;
        public event Action<uint> ReportPercentage;
        public void Execute() => Execute(() => { });
        public void Execute(Action action)
        {
            string key = Guid.NewGuid().ToString();
            int res = RmStartSession(out IntPtr handle, 0, key);
            
            if (res == 0)
            {
                ReportProgress?.Invoke($"Restart Manager session created with ID {key}");

                RM_UNIQUE_PROCESS[] processes = GetProcesses("explorer");
                res = RmRegisterResources(
                    handle,
                    0, null,
                    (uint)processes.Length, processes,
                    0, null
                );

                if (res == 0)
                {
                    ReportProgress?.Invoke("Successfully registered resources.");

                    res = RmShutdown(handle, RM_SHUTDOWN_TYPE.RmForceShutdown, (percent) => ReportPercentage?.Invoke(percent));

                    if (res == 0)
                    {
                        ReportProgress?.Invoke("Applications stopped successfully.");
                        action();

                        res = RmRestart(handle, 0, (percent) => ReportPercentage?.Invoke(percent));

                        if (res == 0)
                        {
                            ReportProgress?.Invoke("Applications restarted successfully.");
                        }
                    }
                }
                
                res = RmEndSession(handle);
                
                if (res == 0)
                {
                    ReportProgress?.Invoke("Restart Manager session ended.");
                }
            }
        }

        private RM_UNIQUE_PROCESS[] GetProcesses(string name)
        {
            List<RM_UNIQUE_PROCESS> processes = new List<RM_UNIQUE_PROCESS>();
            
            foreach (Process process in Process.GetProcessesByName(name))
            {
                RM_UNIQUE_PROCESS rp = new RM_UNIQUE_PROCESS
                {
                    dwProcessId = process.Id
                };

                GetProcessTimes(process.Handle, out Com.FILETIME creationTime, out Com.FILETIME exitTime, out Com.FILETIME kernelTime, out Com.FILETIME userTime);
                rp.ProcessStartTime = creationTime;
                processes.Add(rp);
            }
            
            return processes.ToArray();
        }
    }
}
