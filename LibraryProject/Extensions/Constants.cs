namespace LibraryProject
{
    public enum Cbr8CcapParams
    {
        InterfacesList = 3001,

        FreeRunDuration = 3019,

        UpPortRead = 3000,
        UpPortWrite = 3100,

        CenterFreqRead = 3008,
        CenterFreqWrite = 3108,

        FreqSpanRead = 3009,
        FreqSpanWrite = 3109,

        DestIdxRead = 3024,
        DestIdxWrite = 3124,

        InitCaptureRead = 3225,
        InitCaptureWrite = 3325,
    }

    public static class Constants
    {
        public static readonly string CiscoProtocol = "CISCO CBR-8 CCAP UTSC";
    }
}
