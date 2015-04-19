namespace libAssetControl.Data
{
	public sealed class User : StoreItem
	{
		public string Name { get; set; }
		public string Password { get; set; }

		public User(AssetStore store, string name, string password)
			: base(store)
		{
			Name = name;
			Password = password;
		}
	}
}
