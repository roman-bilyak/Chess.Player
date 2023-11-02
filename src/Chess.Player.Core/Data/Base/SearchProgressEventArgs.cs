namespace Chess.Player.Data;

public delegate void SearchProgressEventHandler(object sender, SearchProgressEventArgs e);

public class SearchProgressEventArgs : EventArgs
{
    public int ProgressPercentage { get; private set; }

    public SearchProgressEventArgs(int progressPercentage)
    {
        ProgressPercentage = progressPercentage;
    }
}
