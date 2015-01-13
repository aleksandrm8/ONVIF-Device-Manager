using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using utils;

namespace odm.ui {
	public partial class App {

		/// <summary>
		/// Application Entry Point.
		/// </summary>
		[STAThreadAttribute]
		public static void Main() {
			Bootstrapper.CreateConsoleForTracing();

            log.WriteInfo(string.Format("\n== Program started at {0} ==\n", DateTime.Now));

			Bootstrapper.ScanSpecialFolders();
			odm.ui.App app = new odm.ui.App();
			app.InitializeComponent();
			app.Run();
		}
	}
}
