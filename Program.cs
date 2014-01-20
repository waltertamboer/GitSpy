using System;
using System.Windows.Forms;

namespace GitSpy
{
    /// <summary>
    /// The entry point class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The title of this program.
        /// </summary>
        public const string Title = "GitSpy";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (GitSpyApplication GitSpy = new GitSpyApplication())
            {
                Application.Run();
            }
        }
    }
}
