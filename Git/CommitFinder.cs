using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace GitSpy.Git
{
    public class CommitFinder
    {
        private string path;
        private DateTime since;

        public CommitFinder(string path, DateTime since)
        {
            this.path = path;
            this.since = since;
        }

        private string BuildCommand()
        {
            string sinceDate = since.Year
                + "-" + (since.Month < 10 ? "0" + since.Month.ToString() : since.Month.ToString())
                + "-" + (since.Day < 10 ? "0" + since.Day.ToString() : since.Day.ToString())
                + " " + (since.Hour < 10 ? "0" + since.Hour.ToString() : since.Hour.ToString())
                + ":" + (since.Minute < 10 ? "0" + since.Minute.ToString() : since.Minute.ToString())
                + ":" + (since.Second < 10 ? "0" + since.Second.ToString() : since.Second.ToString());

            return string.Format(" --git-dir={0}/.git --work-tree={1} log --name-status --since=\"{2}\"", path.Replace("\\", "/"), path.Replace("\\", "/"), sinceDate);
        }

        private string Execute()
        {
            string command = BuildCommand();

            // Start the child process.
            Process p = new Process();

            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = @"C:\Program Files (x86)\Git\bin\git.exe";
            p.StartInfo.Arguments = command;
            p.Start();

            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return output;
        }

        public List<Commit> Find()
        {
            CommitParser parser = new CommitParser();

            string data = Execute();

            return parser.Parse(data);
        }
    }
}
