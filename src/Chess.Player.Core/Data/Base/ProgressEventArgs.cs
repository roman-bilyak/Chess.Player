namespace Chess.Player.Data;

public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

public class ProgressEventArgs : EventArgs
{
    public double Percentage { get; private set; }

    public ProgressEventArgs(double percentage)
    {
        Percentage = percentage;
    }
}
