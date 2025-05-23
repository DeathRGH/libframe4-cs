using System.Runtime.InteropServices;
using System.Text;

namespace libframe4
{
    public partial class FRAME4
    {
        // console
        // packet sizes

        // send size
        private const int CMD_CONSOLE_PRINT_PACKET_SIZE = 4;
        private const int CMD_CONSOLE_NOTIFY_PACKET_SIZE = 8;
        private const int CMD_CONSOLE_FANTHRESHOLD_PACKET_SIZE = 1;

        // receive size
        private const int CONSOLE_INFO_SIZE = 332;

        // console
        // note: the disconnect command actually uses the console api to end the connection
        /// <summary>
        /// Reboot console
        /// </summary>
        public void Reboot()
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_CONSOLE_REBOOT, 0);
            IsConnected = false;
        }

        /// <summary>
        /// Print to serial port
        /// </summary>
        public void Print(string str)
        {
            CheckConnected();

            string raw = str + "\0";

            SendCMDPacket(CMDS.CMD_CONSOLE_PRINT, CMD_CONSOLE_PRINT_PACKET_SIZE, raw.Length);
            SendData(Encoding.ASCII.GetBytes(raw), raw.Length);
            CheckStatus();
        }

        /// <summary>
        /// Notify console
        /// </summary>
        public void Notify(int messageType, string message)
        {
            CheckConnected();

            string raw = message + "\0";

            SendCMDPacket(CMDS.CMD_CONSOLE_NOTIFY, CMD_CONSOLE_NOTIFY_PACKET_SIZE, messageType, raw.Length);
            SendData(Encoding.ASCII.GetBytes(raw), raw.Length);
            CheckStatus();
        }

        /// <summary>
        /// Console information
        /// </summary>
        public ConsoleInfo GetConsoleInformation()
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_CONSOLE_INFO, 0);
            CheckStatus();

            return (ConsoleInfo)GetObjectFromBytes(ReceiveData(CONSOLE_INFO_SIZE), typeof(ConsoleInfo));
        }

        /// <summary>
        /// Get the console PSID as string
        /// </summary>
        public string GetPSID()
        {
            ConsoleInfo ci = GetConsoleInformation();

            StringBuilder sb = new StringBuilder(ci.psid.Length * 2);
            foreach (byte b in ci.psid)
            {
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Set the fan threshold temperature in degrees celsius
        /// </summary>
        public void SetFanThresholdCelsius(byte temperature)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_CONSOLE_FANTHRESHOLD, CMD_CONSOLE_FANTHRESHOLD_PACKET_SIZE, temperature);
            CheckStatus();
        }

        /// <summary>
        /// Set the fan threshold temperature in degrees fahrenheit
        /// </summary>
        public void SetFanThresholdFahrenheit(byte temperature)
        {
            CheckConnected();

            byte celsius = (byte)(5.0f / 9.0f * (float)(temperature - 32));

            SendCMDPacket(CMDS.CMD_CONSOLE_FANTHRESHOLD, CMD_CONSOLE_FANTHRESHOLD_PACKET_SIZE, celsius);
            CheckStatus();
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ConsoleInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] psid;

        public int upd_version;

        public int sdk_version;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string kernelOsType;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string kernelOsRelease;

        public int kernelOsRev;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string kernelVersion;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string hardwareModel;

        public int hardwareNumCpus;
    }
}
