using System.Collections.Generic;

[System.Serializable]
public class PlayerInfo
{
    public string name;
    public PlayerRole role;
    public Gender gender;
    public int age;
    public MaritalStatus ms;
    public List<Interest> interests;

    public PlayerInfo(bool isNone)
    {
        if (isNone)
        {
            name = "";
            role = PlayerRole.None;
            gender = Gender.None;
            age = 0;
            ms = MaritalStatus.None;
            interests = new List<Interest>();
        }
        else
        {
            name = "Player";
            role = PlayerRole.Patient;
            gender = Gender.Male;
            age = 20;
            ms = MaritalStatus.Single;
            interests = new List<Interest>();
        }
    }

    public PlayerInfo Clone()
    {
        PlayerInfo info = new PlayerInfo(false);
        info.name = name;
        info.role = role;
        info.gender = gender;
        info.age = age;
        info.ms = ms;
        info.interests = interests;
        return info;
    }

    public object[] ToObjs()
    {
        object[] objs = new object[5 + interests.Count];

        objs[0] = name;
        objs[1] = (int)role;
        objs[2] = (int)gender;
        objs[3] = age;
        objs[4] = (int)ms;
        for (int i = 5; i < interests.Count + 5; i++)
            objs[i] = (int)interests[i - 5];

        return objs;
    }

    public static PlayerInfo Objs2Info(object[] data)
    {
        PlayerInfo info = new PlayerInfo(false);
        info.name = (string)data[0];
        info.role = (PlayerRole)(int)data[1];
        info.gender = (Gender)(int)data[2];
        info.age = (int)data[3];
        info.ms = (MaritalStatus)(int)data[4];
        info.interests = new List<Interest>();
        for (int i = 5; i < data.Length; i++)
        {
            info.interests.Add((Interest)(int)data[i]);
        }
        return info;
    }
}