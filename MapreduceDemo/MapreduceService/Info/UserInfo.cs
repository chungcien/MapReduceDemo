using System.Collections.Generic;

namespace WS_FingerPrinter
{
    internal class UserInfo
    {
        public int Flags { get; set; }
        public string EnrollNumber { get; set; }
        public string Name { get; set; }
        public int FingerIndex { get; set; }
        public string TmpData { get; set; }
        public string Password { get; set; }
        public int Privilege { get; set; }
        public bool Enabled { get; set; }
        public string CardNumber { get; set; }
        public int TmpLength { get; set; }
        public string FingerPrinterSerial { set; get; }
    }
}
