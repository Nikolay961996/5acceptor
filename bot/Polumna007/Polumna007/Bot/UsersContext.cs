using System.Collections.Concurrent;

namespace Polumna007.Bot;

public class UsersContext
{
    private readonly ConcurrentDictionary<long, UserInfo> _userInfos = new();

    public UserInfo GetUserInfo(long id)
    {
        if (_userInfos.TryGetValue(id, out var value))
            return value;

        var userInfo = new UserInfo();
        _userInfos.TryAdd(id, userInfo);
        return userInfo;
    }
}
