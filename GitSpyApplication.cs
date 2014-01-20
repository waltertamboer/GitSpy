using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using GitSpy.Git;

namespace GitSpy
{
    /// <summary>
    /// The application class.
    /// </summary>
    public class GitSpyApplication : IDisposable
    {
        /// <summary>
        /// The lock used for threading purposes.
        /// </summary>
        private readonly object instanceLock = new object();

        /// <summary>
        /// The update timer used to do automatic updates.
        /// </summary>
        private System.Windows.Forms.Timer updateTimer = new System.Windows.Forms.Timer();

        /// <summary>
        /// The notify icon of this application.
        /// </summary>
        private NotifyIcon notifyIcon = new NotifyIcon();

        /// <summary>
        /// The queue with all the commits that should be shown.
        /// </summary>
        private Queue<Commit> logQueue = new Queue<Commit>();

        /// <summary>
        /// Whether or not the balloon tip is visible.
        /// </summary>
        private bool balloonTipVisible = false;

        /// <summary>
        /// A list with all the repositories we're monitoring.
        /// </summary>
        private List<Repository> repositories = new List<Repository>();

        /// <summary>
        /// The timer used to switch the notify icon.
        /// </summary>
        private System.Windows.Forms.Timer notifyIconTimer = new System.Windows.Forms.Timer();

        /// <summary>
        /// The amount of times that the icon should be changed.
        /// </summary>
        private int notifyIconTimerMaxFlashCount = 3;

        /// <summary>
        /// The amount of times that the icon has been changed.
        /// </summary>
        private int notifyIconTimerFlashCount = 0;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public GitSpyApplication()
        {
            LoadRepositories();
            InitializeNotifyIcon();
            InitializeUpdateTimer();
        }

        /// <summary>
        /// Initializes the notify icon.
        /// </summary>
        private void InitializeNotifyIcon()
        {
            notifyIcon.BalloonTipClicked += new EventHandler(OnBalloonTipClicked);
            notifyIcon.BalloonTipClosed += new EventHandler(OnBalloonTipClosed);
            notifyIcon.ContextMenu = InitializeNotifyIconContextMenu();
            notifyIcon.DoubleClick += new EventHandler(OnDoubleClick);
            notifyIcon.Icon = GitSpy.Properties.Resources.GitSpy;
            notifyIcon.Text = "GitSpy";
            notifyIcon.Visible = true;

            notifyIconTimer.Enabled = true;
            notifyIconTimer.Interval = 1000;
            notifyIconTimer.Tick += new EventHandler(OnTickNotifyIconTimer);
        }

        /// <summary>
        /// Initializes the context menu of the notify icon.
        /// </summary>
        private ContextMenu InitializeNotifyIconContextMenu()
        {
            MenuItem[] updateMenu = new MenuItem[this.repositories.Count];
            for (int i = 0; i < repositories.Count; ++i)
            {
                updateMenu[i] = new MenuItem(repositories[i].Name, new EventHandler(OnUpdate));
                updateMenu[i].Tag = repositories[i];
            }

            ContextMenu contextMenu = new ContextMenu();

            contextMenu.Name = "GitSpyContextMenu";
            contextMenu.MenuItems.Add("&Update", updateMenu);
            contextMenu.MenuItems.Add("&Settings", new EventHandler(OnSettings));
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add("E&xit", new EventHandler(OnExit));

            return contextMenu;
        }

        /// <summary>
        /// Initializes the update timer.
        /// </summary>
        private void InitializeUpdateTimer()
        {
            updateTimer.Enabled = true; 
            updateTimer.Interval = (int)Properties.Settings.Default.UpdateInterval * 1000 * 60; // In minutes.
            updateTimer.Tick += new EventHandler(OnUpdateTimer);
            updateTimer.Start();
        }

        /// <summary>
        /// Loads in all the repositories that should be checked.
        /// </summary>
        private void LoadRepositories()
        {
            if (!File.Exists(GitSpy.Properties.Settings.Default.RepositoryFile))
            {
                SaveRepositories();
            }

            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(GitSpy.Properties.Settings.Default.RepositoryFile);

                XmlNodeList nodeList = document.SelectNodes("/GitSpy/Repository");
                foreach (XmlNode node in nodeList)
                {
                    Repository repository = new Repository();

                    repository.Name = node.SelectSingleNode("Name").InnerText;
                    repository.Path = node.SelectSingleNode("Path").InnerText;

                    DateTime lastCommitDate = DateTime.MinValue;

                    if (DateTime.TryParse(node.SelectSingleNode("LastCommitDate").InnerText, out lastCommitDate))
                    {
                        repository.LastCommitDate = lastCommitDate;
                    }
                    else
                    {
                        repository.LastCommitDate = DateTime.MinValue;
                    }

                    repositories.Add(repository);
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(null, e.Message, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Saves all the repositories that should be checked.
        /// </summary>
        private void SaveRepositories()
        {
            try
            {
                XmlDocument document = new XmlDocument();

                XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "utf-8", "yes");
                document.AppendChild(declaration);

                XmlElement root = document.CreateElement("GitSpy");
                document.AppendChild(root);

                foreach (Repository repository in repositories)
                {
                    XmlElement parent = document.CreateElement("Repository");
                    root.AppendChild(parent);

                    XmlElement element = null;

                    element = document.CreateElement("Name");
                    element.InnerText = repository.Name;
                    parent.AppendChild(element);

                    element = document.CreateElement("Path");
                    element.InnerText = repository.Path;
                    parent.AppendChild(element);

                    element = document.CreateElement("LastCommitDate");
                    element.InnerText = repository.LastCommitDate.ToString();
                    parent.AppendChild(element);
                }

                document.Save(GitSpy.Properties.Settings.Default.RepositoryFile);
            }
            catch (System.Exception e)
            {
                MessageBox.Show(null, e.Message, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Called when the balloon tip is clicked.
        /// </summary>
        /// <param name="sender">The object who called this.</param>
        /// <param name="e">The event arguments.</param>
        private void OnBalloonTipClicked(object sender, EventArgs e)
        {
            balloonTipVisible = false;
        }

        /// <summary>
        /// Called when the balloon tip is closed.
        /// </summary>
        /// <param name="sender">The object who called this.</param>
        /// <param name="e">The event arguments.</param>
        private void OnBalloonTipClosed(object sender, EventArgs e)
        {
            balloonTipVisible = false;
            ShowNextToolTip();
        }

        private void OnTickNotifyIconTimer(object sender, EventArgs e)
        {
            notifyIconTimerFlashCount++;

            if (notifyIconTimerFlashCount % 2 == 0)
            {
                notifyIcon.Icon = GitSpy.Properties.Resources.GitSpy;
            }
            else
            {
                notifyIcon.Icon = GitSpy.Properties.Resources.GitSpyAlert;
            }

            if (notifyIconTimerFlashCount >= notifyIconTimerMaxFlashCount)
            {
                notifyIconTimerFlashCount = 0;
                notifyIcon.Icon = GitSpy.Properties.Resources.GitSpy;
                notifyIconTimer.Stop();
            }
        }

        /// <summary>
        /// Shows the next tooltip that's available.
        /// </summary>
        private void ShowNextToolTip()
        {
            if (!balloonTipVisible && logQueue.Count > 0)
            {
                balloonTipVisible = true;

                notifyIconTimer.Start();

                Commit gitCommit = logQueue.Dequeue();
                notifyIcon.BalloonTipText = gitCommit.Message;

                if (gitCommit.Headers.ContainsKey("Date"))
                {
                    notifyIcon.BalloonTipTitle = string.Format("{0} - {1}", gitCommit.Repository.Name, gitCommit.Headers["Date"]);
                }
                else
                {
                    notifyIcon.BalloonTipTitle = gitCommit.Repository.Name;
                }

                notifyIcon.ShowBalloonTip((int)GitSpy.Properties.Settings.Default.BalloonTipTimeout);
            }
        }

        /// <summary>
        /// Updates the log messages for the given repository.
        /// </summary>
        private void UpdateRepository(Repository repository)
        {
            CommitFinder process = new CommitFinder(repository.Path, repository.LastCommitDate);
            List<Commit> commits = process.Find();

            if (commits.Count != 0)
            {
                lock (instanceLock) // Queue isn't thread-safe, let's lock
                {
                    DateTime commitDate = DateTime.Now;

                    if (commitDate > repository.LastCommitDate)
                    {
                        repository.LastCommitDate = commitDate;

                        foreach (Commit commit in commits)
                        {
                            commit.Repository = repository;

                            logQueue.Enqueue(commit);
                        }
                    }
                }

                ShowNextToolTip();
            }
        }

        /// <summary>
        /// Updates the log messages.
        /// </summary>
        private void Update()
        {
            foreach (Repository repository in repositories)
            {
                UpdateRepository(repository);
            }
        }

        /// <summary>
        /// Called when the user double clicked on a notify icon.
        /// </summary>
        /// <param name="sender">The object who requested this.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDoubleClick(object sender, EventArgs e)
        {
            Update();
        }

        /// <summary>
        /// Called when the user requested a manual update.
        /// </summary>
        /// <param name="sender">The object who requested this.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdate(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;

            if (item == null || ((Repository)item.Tag) == null)
            {
                Update();
            }
            else
            {
                UpdateRepository((Repository)item.Tag);
            }
        }

        /// <summary>
        /// Called when the user requested the application settings.
        /// </summary>
        /// <param name="sender">The object who requested this.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSettings(object sender, EventArgs e)
        {
            using (SettingsDialog dialog = new SettingsDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.Reset();
                }
            }
        }

        /// <summary>
        /// Called when the user wants to exit the application.
        /// </summary>
        /// <param name="sender">The object who requested this.</param>
        /// <param name="e">The event arguments.</param>
        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Called when the automatic update timer kicks in.
        /// </summary>
        /// <param name="sender">The object who requested this.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTimer(object sender, EventArgs e)
        {
            Update();
        }

        #region IDisposable Members

        public void Dispose()
        {
            SaveRepositories();

            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }

        #endregion
    }
}
