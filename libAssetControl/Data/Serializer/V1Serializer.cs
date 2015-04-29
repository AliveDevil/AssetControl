using Newtonsoft.Json;

namespace libAssetControl.Data.Serializer
{
	internal class V1Serializer : ISerializer
	{
		public void Load(AssetStore store, JsonReader reader)
		{
			while (reader.Read())
			{
			}
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

		private static void WriteUsers(AssetStore store, JsonWriter writer)
		{
			using (new JsonObject(writer, "users"))
			using (new JsonArray(writer))
			{
				foreach (var user in store.Users)
				{
					WriteUser(writer, user);
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
	}
}
