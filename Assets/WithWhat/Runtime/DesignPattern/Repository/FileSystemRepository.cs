using System;
using System.Collections.Generic;
using System.IO;
using WithWhat.Infrastructure.Serializer;

namespace WithWhat.Repository
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
        }

        public virtual void Delete(T instance)
        {
        }

        public virtual void Insert(T instance)
        {
            try
            {
                // 创建文件夹
                if (!Directory.Exists(DataDirectory))
                {
                    Directory.CreateDirectory(DataDirectory);
                }
                // 创建文件
                string fileName = Path.Combine(DataDirectory, typeof(T).Name);
                if (!File.Exists(fileName))
                {
                    FileStream fs = File.Create(fileName);
                    fs.Close();
                }
                string serializeObject = Serializer.Serialize(instance, true);
                File.WriteAllText(fileName, serializeObject);
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
                    return new List<T>() { Serializer.Deserialize<T>(serializeObject) };
                }
            }
            catch (DirectoryNotFoundException e)
            {
                return new List<T>();
            }
            catch (FileNotFoundException e)
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