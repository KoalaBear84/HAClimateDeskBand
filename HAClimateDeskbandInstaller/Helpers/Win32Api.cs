using System;
using System.Runtime.InteropServices;
using Com = System.Runtime.InteropServices.ComTypes;

namespace HAClimateDeskbandInstaller.Helpers
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RM_UNIQUE_PROCESS
    {
        public int dwProcessId;
        public Com.FILETIME ProcessStartTime;
    }

    [Flags]
    internal enum RM_SHUTDOWN_TYPE : uint
    {
        RmForceShutdown = 0x1,
        RmShutdownOnlyRegistered = 0x10
    }

    internal delegate void RM_WRITE_STATUS_CALLBACK(uint nPercentComplete);

    internal class Win32Api
    {
        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
        protected static extern int RmStartSession(out IntPtr pSessionHandle, int dwSessionFlags, string strSessionKey);

        [DllImport("rstrtmgr.dll")]
        protected static extern int RmEndSession(IntPtr pSessionHandle);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
        protected static extern int RmRegisterResources(IntPtr pSessionHandle, uint nFiles, string[] rgsFilenames, uint nApplications, RM_UNIQUE_PROCESS[] rgApplications, uint nServices, string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll")]
        protected static extern int RmShutdown(IntPtr pSessionHandle, RM_SHUTDOWN_TYPE lActionFlags, RM_WRITE_STATUS_CALLBACK fnStatus);

        [DllImport("rstrtmgr.dll")]
        protected static extern int RmRestart(IntPtr pSessionHandle, int dwRestartFlags, RM_WRITE_STATUS_CALLBACK fnStatus);

        [DllImport("kernel32.dll")]
        protected static extern bool GetProcessTimes(IntPtr hProcess, out Com.FILETIME lpCreationTime, out Com.FILETIME lpExitTime, out Com.FILETIME lpKernelTime, out Com.FILETIME lpUserTime);
    }
}
