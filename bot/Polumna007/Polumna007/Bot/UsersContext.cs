﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polumna007.Bot;

public class UsersContext
{
    private readonly Dictionary<long, UserInfo> _userInfos = new();

    public UserInfo GetUserInfo(long id)
    {
        if (_userInfos.TryGetValue(id, out var value))
            return value;

        var userInfo = new UserInfo();
        _userInfos.Add(id, userInfo);
        return userInfo;
    }
}
