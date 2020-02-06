namespace dl.wm.suite.interprocess.api.Commanding.PackageCheckers
{
    public interface IWmPackageChecker
    {
        void Check(byte[] package, byte commandCode);
        bool IsValidPackage(byte[] package);
    }
}