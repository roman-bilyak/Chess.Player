namespace Chess.Player.Data;

public static class ProgressHelper
{
    public const double Start = 1;
    public const double Finish = 99;

    public static double GetProgress(int index, int count)
    {
        return index * (Finish - Start) / count + Start;
    }
}