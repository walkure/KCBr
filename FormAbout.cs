using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
//using System.Drawing;
using System.Runtime;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace KCB2
{
    public partial class FormAbout : Form
    {
        public FormAbout(string IEVersion)
        {
            InitializeComponent();

            System.Diagnostics.FileVersionInfo ver =
               System.Diagnostics.FileVersionInfo.GetVersionInfo(
               System.Reflection.Assembly.GetExecutingAssembly().Location);
            labelName.Text = ver.FileDescription;
            labelRevision.Text = ver.ProductVersion;
            labelDescription.Text = ver.Comments;
            labelCopyright.Text = ver.LegalCopyright;
            labelIEVersion.Text = "Internet Explorer " + IEVersion;

            if (System.Environment.Is64BitProcess)
                labelProcessMode.Text = "Running as 64bit process";
            else
                if(System.Environment.Is64BitOperatingSystem)
                    labelProcessMode.Text = "Running as 32bit(WOW64 subsystem) process";
                else
                    labelProcessMode.Text = "Running as 32bit process";

            labelGCMode.Text = GetGCMode();
            labelBuildDate.Text = "Build: "+GetBuildDateTime(Assembly.GetExecutingAssembly()).ToString();

        }

        string GetGCMode()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("GC: {0} ", GCSettings.IsServerGC ? "Server" : "Workstation");

            switch (GCSettings.LatencyMode)
            {
                case GCLatencyMode.Batch:
                    sb.Append("Batch");
                    break;
                case GCLatencyMode.Interactive:
                    sb.Append("Interactive");
                    break;
                case GCLatencyMode.LowLatency:
                    sb.Append("LowLatency");
                    break;
                default:
                    sb.AppendFormat("Unknown({0})", (int)GCSettings.LatencyMode);
                    break;
            }

            return sb.ToString();
        }

        /*
         * http://stackoverflow.com/questions/1600962/displaying-the-build-date
         */

#pragma warning disable 0649
        struct _IMAGE_FILE_HEADER
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        };
#pragma warning restore 0649


        static DateTime GetBuildDateTime(Assembly assembly)
        {
            if (File.Exists(assembly.Location))
            {
                var buffer = new byte[Math.Max(Marshal.SizeOf(typeof(_IMAGE_FILE_HEADER)), 4)];
                using (var fileStream = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read))
                {
                    fileStream.Position = 0x3C;
                    fileStream.Read(buffer, 0, 4);
                    fileStream.Position = BitConverter.ToUInt32(buffer, 0); // COFF header offset
                    fileStream.Read(buffer, 0, 4); // "PE\0\0"
                    fileStream.Read(buffer, 0, buffer.Length);
                }
                var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                try
                {
                    var coffHeader = (_IMAGE_FILE_HEADER)Marshal.PtrToStructure(pinnedBuffer.AddrOfPinnedObject(), typeof(_IMAGE_FILE_HEADER));

                    return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1) 
                        + new TimeSpan(coffHeader.TimeDateStamp * TimeSpan.TicksPerSecond));
                }
                finally
                {
                    pinnedBuffer.Free();
                }
            }
            return new DateTime();
        }
    }
}
