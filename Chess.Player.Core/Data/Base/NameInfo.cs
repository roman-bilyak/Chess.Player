namespace Chess.Player.Data;

public class NameInfo
{
    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string FullName => $"{LastName} {FirstName}";

    public NameInfo(string lastName, string firstName)
    {
        LastName = lastName;
        FirstName = firstName;
    }
}