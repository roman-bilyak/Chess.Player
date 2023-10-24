namespace Chess.Player.MAUI.Models;

public class PlayerGroupInfo : List<string>
{
    public PlayerGroupInfo(string name)
        : base(new List<string>() { name })
    {

    }
}