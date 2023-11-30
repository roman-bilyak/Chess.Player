namespace Chess.Player.Data;

public static class ProgressHelper
{
    public const int Start = 1;
    public const int Finish = 100;

    public static int GetProgress(int index, int count)
    {
        return index * (Finish - Start) / count + Start;
    }
}