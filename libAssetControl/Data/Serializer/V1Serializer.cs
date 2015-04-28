using Newtonsoft.Json;

namespace libAssetControl.Data.Serializer
{
	internal class V1Serializer : ISerializer
	{
		public void Load(AssetStore store, JsonReader reader)
		{
			while(reader.Read())
			{
				;
			}
		}

		public void Save(AssetStore store, JsonWriter writer)
		{
			using (new JsonObject(writer))
			{
				WriteProperty("version", "1", writer);
				using (new JsonObject(writer, "Users"))
				using (new JsonArray(writer))
				{
					foreach (var user in store.Users)
					{
						using (new JsonObject(writer))
						{
							WriteProperty("name", user.Name, writer);
							WriteProperty("password", user.Password, writer);
						}
					}
				}
				using (new JsonObject(writer, "Projects"))
				using (new JsonArray(writer))
				{
					foreach (var project in store.Projects)
					{
						using (new JsonObject(writer))
						{
							WriteProperty("name", project.Name, writer);
							using (new JsonArray(writer, "Users"))
							{
								foreach (var user in project.Users)
								{
									WriteProperty("user", user.Name, writer);
								}
							}
							using (new JsonArray(writer, "Branches"))
							{
								foreach (var branch in project.Branches)
								{
									using (new JsonObject(writer))
									{
										WriteProperty("id", branch.Id, writer);
										WriteProperty("name", branch.Name, writer);
									}
								}
							}
							using (new JsonArray(writer, "Commits"))
							{
								foreach (var commit in project.Commits)
								{
									using (new JsonObject(writer))
									{
										WriteProperty("id", commit.Id, writer);
										WriteProperty("parent", writer);
										if (commit.Parent != null)
											WriteValue(commit.Parent.Id, writer);
										else
											WriteNull(writer);
										WriteProperty("branch", commit.Branch.Id, writer);
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

		private void WriteNull(JsonWriter writer)
		{
			writer.WriteNull();
		}

		private void WriteNull(string name, JsonWriter writer)
		{
			WriteProperty(name, writer);
			WriteNull(writer);
		}

		private void WriteProperty(string name, JsonWriter writer)
		{
			writer.WritePropertyName(name);
		}

		private void WriteProperty<T>(string name, T value, JsonWriter writer)
		{
			WriteProperty(name, writer);
			WriteValue(value, writer);
		}

		private void WriteValue<T>(T value, JsonWriter writer)
		{
			writer.WriteValue(value);
		}
	}
}
