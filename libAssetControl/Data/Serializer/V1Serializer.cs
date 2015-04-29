using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace libAssetControl.Data.Serializer
{
	internal class V1Serializer : ISerializer
	{
		public void Load(AssetStore store, JsonReader reader)
		{
			List<User> users = new List<User>();
			List<Project> projects = new List<Project>();

			// read user-collection.
			ReadUsers(store, reader, users);

			// read project-collection.
			ReadProjects(store, reader, users, projects);

			/*
			 * At this stage everything has been processed.
			 * Now add every single item to the store.
			 */
			foreach (var user in users)
				store.Users.Add(user);
			foreach (var project in projects)
				store.Projects.Add(project);
		}

		public void Save(AssetStore store, JsonWriter writer)
		{
			using (new JsonObject(writer))
			{
				writer.Property("version", "1");
				WriteUsers(store, writer);
				WriteProjects(store, writer);
			}
		}

		private static void ExpectedToken(JsonReader reader, JsonToken expectedToken)
		{
			if (reader.TokenType != expectedToken) throw new Exception();
		}

		private static void ReadBranch(JsonReader reader, Project project)
		{
			ExpectedToken(reader, JsonToken.StartObject);
			ReadExpectedProperty(reader, "id");
			Guid branchId = new Guid(reader.ReadAsString());
			ReadExpectedProperty(reader, "name");
			string branchName = reader.ReadAsString();
			ReadExpectedToken(reader, JsonToken.EndObject);
			project.Branches.Add(new Branch(project, branchId, branchName));
		}

		private static void ReadChange(JsonReader reader, Project project, List<Change> changes)
		{
			ExpectedToken(reader, JsonToken.StartObject);
			ReadExpectedToken(reader, JsonToken.EndObject);
			Change change = new Change(project, null);
			changes.Add(change);
			project.Changes.Add(change);
		}

		private static void ReadCommit(JsonReader reader, Project project, Dictionary<Guid, Guid> parentMapping)
		{
			List<Change> changes = new List<Change>();
			ExpectedToken(reader, JsonToken.StartObject);
			ReadExpectedProperty(reader, "id");
			Guid commitId = new Guid(reader.ReadAsString());

			ReadExpectedProperty(reader, "parent");
			if (!reader.Read()) throw new Exception();
			if (reader.Value != null)
			{
				Guid parentId = new Guid((string)reader.Value);
				parentMapping.Add(commitId, parentId);
			}

			ReadExpectedProperty(reader, "branch");
			Guid branchId = new Guid(reader.ReadAsString());
			Branch branch = project.Branches.SingleOrDefault(b => b.Id == branchId);
			if (branch == null) throw new Exception();

			// skip changes
			ReadCommitChanges(reader, project, changes);

			ReadExpectedToken(reader, JsonToken.EndObject);
			Commit commit = new Commit(project, branch, null, commitId);
			project.Commits.Add(commit);
			foreach (var change in changes) change.Commit = commit;
		}

		private static void ReadCommitChanges(JsonReader reader, Project project, List<Change> changes)
		{
			ReadExpectedProperty(reader, "changes");
			ReadExpectedToken(reader, JsonToken.StartArray);
			while (reader.Read() && reader.TokenType != JsonToken.EndArray)
			{
				ReadChange(reader, project, changes);
			}
		}

		private static void ReadExpectedProperty(JsonReader reader, string name)
		{
			ReadExpectedToken(reader, JsonToken.PropertyName);
			if ((string)reader.Value != name) throw new Exception();
		}

		private static void ReadExpectedToken(JsonReader reader, JsonToken token)
		{
			if (!reader.Read()) throw new Exception();
			if (reader.TokenType != token) throw new Exception();
		}

		private static void ReadProject(AssetStore store, JsonReader reader, List<User> users, List<Project> projects)
		{
			ExpectedToken(reader, JsonToken.StartObject);
			ReadExpectedProperty(reader, "name");
			Project project = new Project(store, reader.ReadAsString());

			// read project-user-collection.
			ReadProjectUsers(reader, users, project);

			// read project-branch-collection.
			ReadProjectBranches(reader, project);

			// read project-branch-collection.
			ReadProjectCommits(reader, project);

			ReadExpectedToken(reader, JsonToken.EndObject);
			projects.Add(project);
		}

		private static void ReadProjectBranches(JsonReader reader, Project project)
		{
			ReadExpectedProperty(reader, "branches");
			ReadExpectedToken(reader, JsonToken.StartArray);
			while (reader.Read() && reader.TokenType != JsonToken.EndArray)
			{
				ReadBranch(reader, project);
			}
		}

		private static void ReadProjectCommits(JsonReader reader, Project project)
		{
			ReadExpectedProperty(reader, "commits");
			ReadExpectedToken(reader, JsonToken.StartArray);
			Dictionary<Guid, Guid> parentMapping = new Dictionary<Guid, Guid>();
			while (reader.Read() && reader.TokenType != JsonToken.EndArray)
			{
				ReadCommit(reader, project, parentMapping);
			}
			foreach (var map in parentMapping)
				project.Commit(map.Key).Parent = project.Commit(map.Value);
		}

		private static void ReadProjects(AssetStore store, JsonReader reader, List<User> users, List<Project> projects)
		{
			ReadExpectedProperty(reader, "projects");
			ReadExpectedToken(reader, JsonToken.StartArray);
			while (reader.Read() && reader.TokenType != JsonToken.EndArray)
			{
				ReadProject(store, reader, users, projects);
			}
		}

		private static void ReadProjectUsers(JsonReader reader, List<User> users, Project project)
		{
			ReadExpectedProperty(reader, "users");
			ReadExpectedToken(reader, JsonToken.StartArray);
			while (reader.Read() && reader.TokenType != JsonToken.EndArray)
			{
				ReadUsername(reader, users, project);
			}
		}

		private static void ReadUser(AssetStore store, JsonReader reader, List<User> users)
		{
			ExpectedToken(reader, JsonToken.StartObject);
			ReadExpectedProperty(reader, "name");
			string name = reader.ReadAsString();
			ReadExpectedProperty(reader, "password");
			string password = reader.ReadAsString();
			ReadExpectedToken(reader, JsonToken.EndObject);
			users.Add(new User(store, name, password));
		}

		private static void ReadUsername(JsonReader reader, List<User> users, Project project)
		{
			string username = (string)reader.Value;
			User user = users.SingleOrDefault(u => u.Name.Equals(username, StringComparison.InvariantCultureIgnoreCase));
			if (user == null) throw new Exception();
			project.Users.Add(user);
		}

		private static void ReadUsers(AssetStore store, JsonReader reader, List<User> users)
		{
			ReadExpectedProperty(reader, "users");
			ReadExpectedToken(reader, JsonToken.StartArray);
			while (reader.Read() && reader.TokenType != JsonToken.EndArray)
			{
				ReadUser(store, reader, users);
			}
		}

		private static void WriteCommit(JsonWriter writer, Commit commit)
		{
			using (new JsonObject(writer))
			{
				writer.Property("id", commit.Id);
				writer.Property("parent");
				if (commit.Parent != null)
					writer.Value(commit.Parent.Id);
				else
					writer.Null();
				writer.Property("branch", commit.Branch.Id);
				WriteCommitChanges(writer, commit);
			}
		}

		private static void WriteCommitChanges(JsonWriter writer, Commit commit)
		{
			using (new JsonArray(writer, "changes"))
			{
				foreach (var change in commit.Changes)
				{
					using (new JsonObject(writer))
					{
					}
				}
			}
		}

		private static void WriteProject(JsonWriter writer, Project project)
		{
			using (new JsonObject(writer))
			{
				writer.Property("name", project.Name);
				WriteProjectUsers(writer, project);
				WriteProjectBranches(writer, project);
				WriteProjectCommits(writer, project);
			}
		}

		private static void WriteProjectBranches(JsonWriter writer, Project project)
		{
			using (new JsonArray(writer, "branches"))
			{
				foreach (var branch in project.Branches)
				{
					using (new JsonObject(writer))
					{
						writer.Property("id", branch.Id);
						writer.Property("name", branch.Name);
					}
				}
			}
		}

		private static void WriteProjectCommits(JsonWriter writer, Project project)
		{
			using (new JsonArray(writer, "commits"))
			{
				foreach (var commit in project.Commits)
				{
					WriteCommit(writer, commit);
				}
			}
		}

		private static void WriteProjects(AssetStore store, JsonWriter writer)
		{
			using (new JsonArray(writer, "projects"))
			{
				foreach (var project in store.Projects)
				{
					WriteProject(writer, project);
				}
			}
		}

		private static void WriteProjectUsers(JsonWriter writer, Project project)
		{
			using (new JsonArray(writer, "users"))
			{
				foreach (var user in project.Users)
				{
					writer.Value(user.Name);
				}
			}
		}

		private static void WriteUser(JsonWriter writer, User user)
		{
			using (new JsonObject(writer))
			{
				writer.Property("name", user.Name);
				writer.Property("password", user.Password);
			}
		}

		private static void WriteUsers(AssetStore store, JsonWriter writer)
		{
			using (new JsonArray(writer, "users"))
			{
				foreach (var user in store.Users)
				{
					WriteUser(writer, user);
				}
			}
		}
	}
}
