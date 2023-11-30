namespace Chess.Player.Data;

public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

public class ProgressEventArgs : EventArgs
{
    public int Percentage { get; private set; }

    public ProgressEventArgs(int percentage)
    {
        Percentage = percentage;
    }
}
