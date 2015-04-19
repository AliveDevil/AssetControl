namespace libAssetControl.Data.Serializer
{
	internal static class SerializerFactory
	{
		public static ISerializer ImporterForVersion(string version)
		{
			switch (version)
			{
				case "1":
					return new V1Serializer();
				default:
					return null;
			}
		}

		public static ISerializer LatestSerializer()
		{
			return new V1Serializer();
		}
	}
}
