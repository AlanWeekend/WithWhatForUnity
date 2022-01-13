---
title: "RemoveMatch()"
date: 2021-12-10T17:06:28+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/Utils/ListUtils.cs#L15)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/Utils/ListUtils.cs#L15)

WithWhat.Utils
## 描述
删除List中符合条件的所有元素，不会产生额外的GC，只遍历一次集合 [详情](https://yongshen.me/20220113/collection-dynamic-deletion-optimization/)
## 参数
| 参数名 | 类型 | 描述 |
| - | - | - |
| Match | Predicate<T> | 匹配函数 |