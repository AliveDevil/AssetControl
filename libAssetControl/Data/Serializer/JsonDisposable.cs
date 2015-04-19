using System;
using Newtonsoft.Json;

namespace libAssetControl.Data.Serializer
{
	internal abstract class JsonDisposable : IDisposable
	{
		protected JsonWriter Writer { get; private set; }

		public JsonDisposable(JsonWriter writer)
		{
			this.Writer = writer;
		}
		public JsonDisposable(JsonWriter writer, string name)
			: this(writer)
		{
			this.Writer.WritePropertyName(name);
		}

		public abstract void Dispose();
	}
}
