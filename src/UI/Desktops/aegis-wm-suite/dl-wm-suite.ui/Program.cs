using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.Utils;
using dl.wm.presenter.Utilities;

namespace dl.wm.suite.ui
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      SkinManager.EnableFormSkins();
      BonusSkins.Register();
      AppearanceObject.DefaultFont = new Font("Segoe UI", 8.25f);
      UserLookAndFeel.Default.SetSkinStyle("Metropolis Dark");
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      //ClientSettingsSingleton.InstanceSettings().IpAddressConfigValue = "localhost";
      ClientSettingsSingleton.InstanceSettings().IpAddressConfigValue = "137.116.232.108";
      Application.Run(new Main());
    }
  }
}
