using System;
using System.Collections.Generic;
using System.IO;
using ZCCUtils.Infrastructure.Serializer;

namespace ZCCUtils.Repository
{
    public class FileSystemRepository<T> : IRepository<T> where T : class, new()
    {
        public string DataDirectory { get; set; }
        protected ISerializer Serializer { get; set; }

        public FileSystemRepository() { }

        public FileSystemRepository(string pathToDataDirectory, ISerializer serializer = null)
        {
            DataDirectory = Path.Combine(pathToDataDirectory);
            Serializer = serializer != null ? serializer : SerializerJson.Instance;
            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
            }
        }

        public virtual void Delete(T instance)
        {
        }

        public virtual void Insert(T instance)
        {
            try
            {
                string fileName = Path.Combine(DataDirectory, typeof(T).Name);
                string serializeObject = Serializer.Serialize(instance, true);
                using (StreamWriter stream = new StreamWriter(fileName))
                {
                    stream.Write(serializeObject);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public virtual IEnumerable<T> Select(Func<T, bool> func)
        {
            try
            {
                string fileName = Path.Combine(DataDirectory, typeof(T).Name);
                using (StreamReader stream = new StreamReader(fileName))
                {
                    var serializeObject = stream.ReadToEnd();
                    return Serializer.Deserialize<List<T>>(serializeObject);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                return new List<T>();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public virtual void Update(T instance)
        {
        }
    }
}