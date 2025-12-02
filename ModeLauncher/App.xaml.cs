using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ModeLauncher
{
    public partial class App : Application
    {
        private string LogPath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ModeLauncher",
                "error.log"
            );

        protected override void OnStartup(StartupEventArgs e)
        {
            // Global exception hooks
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogException("DispatcherUnhandledException", e.Exception);
            e.Handled = true;

            MessageBox.Show(
                "An unexpected error occurred. Details were written to error.log.",
                "ModeLauncher",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown();
        }

        private void CurrentDomain_UnhandledException(object? sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                LogException("CurrentDomain_UnhandledException", ex);
            else
                LogRaw("CurrentDomain_UnhandledException", e.ExceptionObject?.ToString() ?? "Unknown");

            // Can't show UI reliably here; just log and let it crash.
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            LogException("TaskScheduler_UnobservedTaskException", e.Exception);
            e.SetObserved();
        }

        private void LogException(string source, Exception ex)
        {
            try
            {
                var folder = Path.GetDirectoryName(LogPath)!;
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                using var sw = new StreamWriter(LogPath, append: true);
                sw.WriteLine("====================================================");
                sw.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {source}");
                sw.WriteLine(ex.ToString());
                sw.WriteLine();
            }
            catch
            {
                // Last resort: ignore logging failure
            }
        }

        private void LogRaw(string source, string message)
        {
            try
            {
                var folder = Path.GetDirectoryName(LogPath)!;
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                using var sw = new StreamWriter(LogPath, append: true);
                sw.WriteLine("====================================================");
                sw.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {source}");
                sw.WriteLine(message);
                sw.WriteLine();
            }
            catch
            {
                // ignore
            }
        }
    }
}
