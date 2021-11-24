using UnityEngine;
using WithWhat.Repository;

public class RepositoryTest : MonoBehaviour
{
    public class User {
        public string UserName;
        public string Password;
    }

    private FileSystemRepository<User> _fileSystemRepository;

    private void Awake()
    {
        var user = new User() { UserName = "张三", Password = "123" };
        _fileSystemRepository.Insert(user);
    }
}