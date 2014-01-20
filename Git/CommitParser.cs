using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GitSpy.Git
{
    public class CommitParser
    {
        public List<Commit> Parse(string data)
        {
            List<Commit> commits = new List<Commit>();

            if (data != null && data != string.Empty)
            {
                using (StringReader reader = new StringReader(data))
                {
                    ParseData(reader, ref commits);
                }
            }

            return commits;
        }

        private void ParseData(StringReader reader, ref List<Commit> commits)
        {
            while (reader.Peek() != -1)
            {
                ParseCommit(reader, ref commits);
            }
        }

        private void ParseCommit(StringReader reader, ref List<Commit> commits)
        {
            string line = reader.ReadLine();

            if (!line.StartsWith("commit "))
            {
                throw new Exception("Invalid commit provided!");
            }

            Commit commit = new Commit();
            commit.Sha = line.Split(' ')[1];
            commits.Add(commit);

            // Parse the headers:
            ParseHeaders(reader, commit);

            // Parse the message:
            while (reader.Peek() != -1)
            {
                line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                commit.Message += line;
            }

            // Parse the modification list:
            while (reader.Peek() != -1)
            {
                line = reader.ReadLine();

                if (line.Length > 1 && Char.IsLetter(line[0]) && line[1] == '\t')
                {
                    string status = line.Split('\t')[0];
                    string file = line.Split('\t')[1];

                    commit.Files.Add(new FileStatus() { Status = status, File = file });
                }
                else
                {
                    break;
                }
            }
        }

        private void ParseHeaders(StringReader reader, Commit commit)
        {
            string line;

            do
            {
                line = reader.ReadLine();

                if (!StartsWithHeader(line))
                {
                    break;
                }

                string header = line.Split(':')[0];
                string value = string.Join(":", line.Split(':').Skip(1)).Trim();

                commit.Headers.Add(header, value);
            }
            while (reader.Peek() != -1);
        }

        private bool StartsWithHeader(string line)
        {
            if (line.Length > 0 && char.IsLetter(line[0]))
            {
                var seq = line.SkipWhile(ch => Char.IsLetter(ch) && ch != ':');
                return seq.FirstOrDefault() == ':';
            }
            return false;
        }
    }
}