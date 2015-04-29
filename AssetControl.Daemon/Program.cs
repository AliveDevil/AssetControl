using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using libAssetControl.Data;
using libAssetControl.Network;

namespace AssetControl.Daemon
{
	class Program
	{
		static void Main(string[] args)
		{
			string file = ConfigurationManager.AppSettings["StoreFile"];
			FileInfo storeFile = new FileInfo(file);
			AssetStore store = new AssetStore();
			if (!storeFile.Exists)
			{
				User user = new User(store, "TestUser", "TestPasswordShouldBeSHA512OrSHA256.");
				Project project = new Project(store, "TestProject");
				Branch branch = new Branch(project, Guid.Empty, "master");
				Commit commit = new Commit(project, branch, null, Guid.Empty);
				Commit childCommit = new Commit(project, branch, commit, new Guid("00000000-0000-0000-0000-000000000001"));
				Commit childChildCommit = new Commit(project, branch, childCommit, new Guid("00000000-0000-0000-0000-000000000002"));
				Change change = new Change(project, commit);

				project.Changes.Add(change);
				project.Commits.Add(childChildCommit);
				project.Commits.Add(childCommit);
				project.Commits.Add(commit);
				project.Branches.Add(branch);
				project.Users.Add(user);

				store.Projects.Add(project);
				store.Users.Add(user);

				using (Stream fileStream = storeFile.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
					store.Save(fileStream);
			}
			store.Users.Clear();
			store.Projects.Clear();
			using (Stream fileStream = storeFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
				store.Load(fileStream);
			Host host = new Host(c => new DaemonClient(c, store), 13337);
			host.Wait();
		}
	}
}
