
[System.Serializable]
public class AcquaintanceData
{
    public float point;
    public PlayerInfo info;

    public AcquaintanceData()
    {
        point = 0.8f;
        info = new PlayerInfo(true);
    }
}
