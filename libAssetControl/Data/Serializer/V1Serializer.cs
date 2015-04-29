using System;
using Newtonsoft.Json;

namespace libAssetControl.Data.Serializer
{
	internal class V1Serializer : ISerializer
	{
		public void Load(AssetStore store, JsonReader reader)
		{
			while (reader.Read())
			{
				;
			}
		}

		public void Save(AssetStore store, JsonWriter writer)
		{
			using (new JsonObject(writer))
			{
				writer.Property("version", "1");
				using (new JsonObject(writer, "users"))
				using (new JsonArray(writer))
				{
					foreach (var user in store.Users)
					{
						using (new JsonObject(writer))
						{
							writer.Property("name", user.Name);
							writer.Property("password", user.Password);
						}
					}
				}
				using (new JsonArray(writer, "projects"))
				{
					foreach (var project in store.Projects)
					{
						using (new JsonObject(writer))
						{
							writer.Property("name", project.Name);
							using (new JsonArray(writer, "users"))
							{
								foreach (var user in project.Users)
								{
									writer.Value(user.Name);
								}
							}
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
							using (new JsonArray(writer, "commits"))
							{
								foreach (var commit in project.Commits)
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
								}
							}
						}
					}
				}
			}
		}
	}
}
