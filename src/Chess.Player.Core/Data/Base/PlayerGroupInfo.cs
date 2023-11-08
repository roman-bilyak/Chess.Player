namespace Chess.Player.Data;

public class PlayerGroupInfo : List<string>
{
    public PlayerGroupInfo()
        : base()
    {

    }

    public PlayerGroupInfo(string name)
        : base(new List<string>() { name })
    {

    }
}