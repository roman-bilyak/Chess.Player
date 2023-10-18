namespace Chess.Player.Data
{
    public class SearchCriteria
    {
        public string Name { get; private set; }

        public SearchCriteria(string name)
        {
            Name = name;
        }
    }
}