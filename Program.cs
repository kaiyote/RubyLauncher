using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RubyLauncher
{
    class Program
    {
        /************* CHANGE appName TO BE THE NAME OF YOUR MAIN RUBY SCRIPT *********/
        const string appName = "script.rbw";
        /************* CHANGE logFile TO BE THE NAME OF THE FILE YOU WANT TO USE FOR LOGGING *********/
        const string logFile = "log.txt";

        static void Main(string[] args)
        {
            if (args.Length > 1)
                using (StreamWriter writer = new StreamWriter(logFile))
                    writer.WriteLine("Error: \nPlease combine all arguments into one string (\"arg1 'arg2' 'arg3'\")");
            else
                Run(appName + (args.Length > 0 ? " " + args[0] : ""));
        }

        static void Run(string arguments)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                FileName = "cmd.exe",
                Arguments = "/C ruby " + arguments,
                WorkingDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).FullName
            };
            string output;
            string error;

            process.StartInfo = startInfo;
            process.Start();

            try
            {
                using (StreamReader procOut = process.StandardOutput)
                {
                    output = procOut.ReadToEnd();
                }
                using (StreamReader procErr = process.StandardError)
                {
                    error = procErr.ReadToEnd();
                }

                using (StreamWriter writer = new StreamWriter(logFile))
                {
                    writer.WriteLine("Output: \n" + output);
                    writer.WriteLine("Error: \n" + error);
                }
            }
            catch (Exception e) 
            {
                using (StreamWriter writer = new StreamWriter(logFile))
                {
                    writer.WriteLine("Attempting to execute:\n" + startInfo.FileName + " " + startInfo.Arguments);
                    writer.WriteLine("Error: \nAn error has occurred.  Please check what you are passing on the command line and the name of the script specified in the source.");
                }
            }
        }
    }
}
