namespace dl.wm.suite.interprocess.api.Commanding.PackageCheckers
{
    public sealed class WmPackageChecker : IWmPackageChecker
    {
        private byte[] _wmPackage;

        private WmPackageChecker()
        {
        }

        public static WmPackageChecker Checker { get; } = new WmPackageChecker();
        public void Check(byte[] package, byte commandCode)
        {
        }

        public bool IsValidPackage(byte[] package)
        {
            return true;
        }
    }
}
