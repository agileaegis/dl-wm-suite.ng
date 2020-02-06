using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.Utils;

namespace ws.simulator
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
            UserLookAndFeel.Default.SetSkinStyle("Metropolis");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
