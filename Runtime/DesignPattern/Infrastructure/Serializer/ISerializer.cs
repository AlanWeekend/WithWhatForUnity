namespace ZCCUtils.Infrastructure.Serializer
{
    public interface ISerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ojb"></param>
        /// <param name="readableOutput"></param>
        /// <returns></returns>
        string Serialize<T>(T ojb, bool readableOutput = false) where T : class, new();

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        T Deserialize<T>(string str) where T : class, new();
    }
}