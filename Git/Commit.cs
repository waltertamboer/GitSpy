using System;
using System.Collections.Generic;

namespace GitSpy.Git
{
    public class Commit
    {
        public Commit()
        {
            Headers = new Dictionary<string, string>();
            Files = new List<FileStatus>();
            Message = "";
        }

        public Repository Repository { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Sha { get; set; }
        public string Message { get; set; }
        public List<FileStatus> Files { get; set; }
    }
}
