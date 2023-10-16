namespace Chess.Player.Data
{
    public class SearchProgressEventArgs : EventArgs
    {
        public int ProgressPercentage { get; private set; }

        public SearchProgressEventArgs(int progressPercentage)
        {
            ProgressPercentage = progressPercentage;
        }
    }
}
