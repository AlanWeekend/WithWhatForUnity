using Newtonsoft.Json;

namespace WithWhat.Infrastructure.Serializer
{
    public class SerializerJson : ISerializer
    {
        public static readonly SerializerJson Instance = new SerializerJson();

        public T Deserialize<T>(string str) where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        public string Serialize<T>(T obj, bool readableOutput = false) where T : class, new()
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}