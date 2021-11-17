using System;
using System.Collections.Generic;
using System.IO;
using ZCCUtils.Infrastructure.Serializer;

namespace ZCCUtils.Repository
{
    public class FileSystemRepository<T> : IRepository<T> where T:class,new()
    {
        public string DataDirectory { get; private set; }
        protected ISerializer Serializer { get; set; }

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
            throw new NotImplementedException();
        }

        public virtual void Insert(T instance)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> Select(Func<T, bool> func)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(T instance)
        {
            throw new NotImplementedException();
        }

        public void Write(T instance)
        {
            try
            {
                string fileName = Path.Combine(DataDirectory, typeof(T).Name);
                string serializeObject = Serializer.Serialize<T>(instance, true);
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
    }
}