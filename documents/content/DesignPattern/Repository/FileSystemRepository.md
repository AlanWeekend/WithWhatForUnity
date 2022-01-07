---
title: "FileSystemRepository"
date: 2021-11-24T15:17:10+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/DesignPattern/Repository/FileSystemRepository.cs)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/DesignPattern/Repository/FileSystemRepository.cs)

WithWhat.DesignPattern
## 描述
文件仓储
## 用法
```C#
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
```