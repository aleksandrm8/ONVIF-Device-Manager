using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Runtime.Remoting.Messaging;

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using utils;
using System.Runtime.Remoting;

namespace odm.hosting {
	
	static class Program {
		static string controllerUrl;

		delegate uint UnhandledExceptionHandler(IntPtr ExceptionPointers);
		[DllImport("kernel32.dll")]
		static extern UnhandledExceptionHandler SetUnhandledExceptionFilter(UnhandledExceptionHandler lpTopLevelExceptionFilter);

		//static UnhandledExceptionHandler handler = null;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			//log.WriteInfo(String.Format("arguments: {0}",String.Join(" ", args)));
			//Trace.Listeners.Add(new ConsoleTraceListener());
			//Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			//handler = ExceptionPointers => {
			//    Process.GetCurrentProcess().Kill();
			//    return 1;
			//};
			//SetUnhandledExceptionFilter(handler);
			
			CommandLineArgs commandLineArgs = null;
			try {
				commandLineArgs = CommandLineArgs.Parse(args);
				controllerUrl = commandLineArgs.GetParamAsString("controller-url");
			} catch (Exception err) {
				dbg.Break();
				log.WriteError(err);
				//log.WriteError("incorrect command line syntax, should be in format: odm-player-host.exe /server-pipe:<pipe-uri> /parent-pid:<parent process id>");
				return;
			}

			try {
				//RemotingServices.
				log.WriteInfo("connecting to controller...");
				var controller = RemotingServices.Connect(typeof(IHostController), controllerUrl) as IHostController;
				if (controller != null) {
					log.WriteInfo("sending hello to controller...");
					var act = controller.Hello();
					log.WriteInfo("executing action returned by controller...");
					act(controller);
					log.WriteInfo("sending bye to controller...");
					controller.Bye();
				} else {
					dbg.Break(); log.WriteError("failed to connect to controller...");
				}
			} catch (Exception err) {
				dbg.Break(); log.WriteError(err);
				//log.WriteInfo(err.Message);
			}
			log.WriteInfo("host process terminated....");
		}
		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e){
			dbg.Break(); log.WriteError("Unhandled exception was caught: " + e.ExceptionObject);
			Process.GetCurrentProcess().Kill();
			//Exception exp = e.ExceptionObject as Exception;
			//if (exp == null) return;
			//new ThreadExceptionDialog(exp).ShowDialog();
			//Environment.Exit(exp.GetHashCode());
		}
	}
}


