namespace Chess.Player.Data
{
    public class SearchCriteria
    {
        public string LastName { get; private set; }

        public string FirstName { get; private set; }

        public SearchCriteria(string lastName, string firstName)
        {
            LastName = lastName;
            FirstName = firstName;
        }
    }
}