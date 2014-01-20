using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitSpy.Git
{
    /// <summary>
    /// A repository which can be monitored by GitSpy.
    /// </summary>
    public class Repository
    {
        /// <summary>
        /// The name of this repository.
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// Gets or sets the name of this repository.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The path of this repository.
        /// </summary>
        private string path = string.Empty;

        /// <summary>
        /// Gets or sets the path of this repository.
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        /// <summary>
        /// The time of the last commit of this repository.
        /// </summary>
        private DateTime lastCommitDate = DateTime.MinValue;

        /// <summary>
        /// Gets or sets the time of the last commit of this repository.
        /// </summary>
        public DateTime LastCommitDate
        {
            get { return lastCommitDate; }
            set { lastCommitDate = value; }
        }
    }
}
