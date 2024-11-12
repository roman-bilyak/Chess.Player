using HtmlAgilityPack;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace Chess.Player.Data;

internal class ChessResultsDataFetcher : IChessDataFetcher, IDisposable
{
    private const string BaseUrl = "https://chess-results.com";
    private readonly HttpClient _httpClient = new();
    private readonly IChessDataNormalizer _chessDataNormalizer;

    private static readonly char[] ApostropheCharacters = { '\'', '’', '`', '"', '*' };

    public ChessResultsDataFetcher
    (
        IChessDataNormalizer chessDataNormalizer
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataNormalizer);

        _chessDataNormalizer = chessDataNormalizer;
    }

    public async Task<PlayerTournamentList> GetPlayerTournamentListAsync(string lastName, string firstName, CancellationToken cancellationToken)
    {
        string searchUrl = $"{BaseUrl}/SpielerSuche.aspx?lan=1";
        HttpResponseMessage response = await _httpClient.GetAsync(searchUrl, cancellationToken);
        response.EnsureSuccessStatusCode();

        HtmlDocument htmlDocument = new();
        string htmlContent = HttpUtility.HtmlDecode(await response.Content.ReadAsStringAsync(cancellationToken));
        htmlDocument.LoadHtml(htmlContent);

        Dictionary<string, string?> postData = new()
            {
                { "__EVENTARGUMENT", htmlDocument.DocumentNode.SelectSingleNode("//input[@name='__EVENTARGUMENT']")?.GetAttributeValue("value", "") },
                { "__EVENTTARGET", htmlDocument.DocumentNode.SelectSingleNode("//input[@name='__EVENTTARGET']")?.GetAttributeValue("value", "") },
                { "__EVENTVALIDATION", htmlDocument.DocumentNode.SelectSingleNode("//input[@name='__EVENTVALIDATION']")?.GetAttributeValue("value", "") },
                { "__LASTFOCUS", htmlDocument.DocumentNode.SelectSingleNode("//input[@name='__LASTFOCUS']")?.GetAttributeValue("value", "") },
                { "__VIEWSTATE", htmlDocument.DocumentNode.SelectSingleNode("//input[@name='__VIEWSTATE']")?.GetAttributeValue("value", "") },
                { "__VIEWSTATEGENERATOR", htmlDocument.DocumentNode.SelectSingleNode("//input[@name='__VIEWSTATEGENERATOR']")?.GetAttributeValue("value", "") },

                { "ctl00$P1$cb_suchen", "Search" },
                { "ctl00$P1$combo_Sort", "2" },
                { "ctl00$P1$txt_FED", "" },
                { "ctl00$P1$txt_GJahr", "" },
                { "ctl00$P1$txt_bis_tag", "" },
                { "ctl00$P1$txt_fideID", "" },
                { "ctl00$P1$txt_ident", "" },
                { "ctl00$P1$txt_min_elo", "" },
                { "ctl00$P1$txt_nachname", lastName.Split(ApostropheCharacters).FirstOrDefault() ?? string.Empty},
                { "ctl00$P1$txt_verein", "" },
                { "ctl00$P1$txt_von_tag", "" },
                { "ctl00$P1$txt_vorname", firstName.Split(ApostropheCharacters).FirstOrDefault() ?? string.Empty},
            };
        response = await _httpClient.PostAsync(searchUrl, new FormUrlEncodedContent(postData), cancellationToken);
        response.EnsureSuccessStatusCode();

        htmlContent = HttpUtility.HtmlDecode(await response.Content.ReadAsStringAsync(cancellationToken));
        htmlDocument.LoadHtml(htmlContent);

        PlayerTournamentList playerTournamentList = new();

        var searchResultNodes = htmlDocument.DocumentNode.SelectNodes("//table[@class='CRs2']/tr")?.Skip(1) ?? new HtmlNodeCollection(null);
        foreach (HtmlNode node in searchResultNodes)
        {
            string? name = node.SelectSingleNode("td[1]/a")?.InnerText?.Trim()?.Replace(ApostropheCharacters);
            if (!name?.StartsWith($"{lastName}, {firstName}".Replace(ApostropheCharacters), StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                continue;
            }

            string? federation = node.SelectSingleNode("td[5]")?.InnerText?.Trim();
            if (federation == "RUS")
            {
                continue;
            }

            int? tournamentId = null;
            int? playerNo = null;

            string? playerUrl = node.SelectSingleNode("td[1]/a")?.GetAttributeValue("href", "");
            if (playerUrl != null)
            {
                Match match = Regex.Match(playerUrl, @"tnr(\d+)\.aspx");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int tnr))
                {
                    tournamentId = tnr;
                }

                match = Regex.Match(playerUrl, @"snr=(\d+)");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int snr))
                {
                    playerNo = snr;
                }
            }

            string? endDateStr = node.SelectSingleNode("td[7]")?.InnerText?.Trim();
            if (tournamentId.HasValue && playerNo.HasValue && DateTime.TryParse(endDateStr, out DateTime endDate))
            {
                playerTournamentList.Items.Add(new PlayerTournament(tournamentId.Value, playerNo.Value, endDate));
            }
        }
        return playerTournamentList;
    }

    public async Task<TournamentInfo> GetTournamentInfoAsync(int tournamentId, CancellationToken cancellationToken)
    {
        string tournamentInfoUrl = $"{BaseUrl}/tnr{tournamentId}.aspx?lan=1&art=1&turdet=YES&zeilen=99999";
        HttpResponseMessage response = await _httpClient.GetAsync(tournamentInfoUrl, cancellationToken);
        response.EnsureSuccessStatusCode();

        HtmlDocument htmlDocument = new();
        string htmlContent = HttpUtility.HtmlDecode(await response.Content.ReadAsStringAsync(cancellationToken));
        htmlDocument.LoadHtml(htmlContent);

        Dictionary<string, string?> postData = new()
            {
                { "__EVENTARGUMENT", htmlDocument.DocumentNode.SelectSingleNode("//input[@name='__EVENTARGUMENT']")?.GetAttributeValue("value", "") },
                { "__EVENTTARGET", htmlDocument.DocumentNode.SelectSingleNode("//input[@name='__EVENTTARGET']")?.GetAttributeValue("value", "") },
                { "__EVENTVALIDATION", htmlDocument.DocumentNode.SelectSingleNode("//input[@name='__EVENTVALIDATION']")?.GetAttributeValue("value", "") },
                { "__LASTFOCUS", htmlDocument.DocumentNode.SelectSingleNode("//input[@name='__LASTFOCUS']")?.GetAttributeValue("value", "") },
                { "__VIEWSTATE", htmlDocument.DocumentNode.SelectSingleNode("//input[@name='__VIEWSTATE']")?.GetAttributeValue("value", "") },
                { "__VIEWSTATEGENERATOR", htmlDocument.DocumentNode.SelectSingleNode("//input[@name='__VIEWSTATEGENERATOR']")?.GetAttributeValue("value", "") },

                { "cb_alleDetails", "Show tournament details" }
        };

        response = await _httpClient.PostAsync(tournamentInfoUrl, new FormUrlEncodedContent(postData), cancellationToken);
        response.EnsureSuccessStatusCode();

        htmlContent = HttpUtility.HtmlDecode(await response.Content.ReadAsStringAsync(cancellationToken));
        htmlDocument.LoadHtml(htmlContent);

        TournamentInfo tournament = new()
        {
            Id = tournamentId,
            Name = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='defaultDialog'][1]/h2")?.InnerText
        };

        HtmlNodeCollection fieldNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='defaultDialog'][1]//td[@class='CR0'][1]/table[1]/tr") ?? new HtmlNodeCollection(null);
        foreach (var fieldNode in fieldNodes)
        {
            string? fieldName = fieldNode.SelectSingleNode("td[1]")?.InnerText?.Trim();
            string? fieldValue = fieldNode.SelectSingleNode("td[2]")?.InnerText?.Trim();

            switch (fieldName)
            {
                case "Organizer(s)":
                    tournament.Organizers = fieldValue;
                    break;
                case "Federation":
                    tournament.Federation = fieldValue;
                    break;
                case "Tournament director":
                    tournament.TournamentDirector = fieldValue;
                    break;
                case "Chief Arbiter":
                    tournament.ChiefArbiter = fieldValue;
                    break;
                case "Deputy Chief Arbiter":
                    tournament.DeputyChiefArbiter = fieldValue;
                    break;
                case "Arbiter":
                    tournament.Arbiter = fieldValue;
                    break;
                case "Time control":
                    tournament.TimeControl = fieldValue;
                    break;
                case "Time control (Blitz)":
                    tournament.TimeControl = fieldValue;
                    tournament.TimeControlType = TimeControlType.Blitz;
                    break;
                case "Time control (Rapid)":
                    tournament.TimeControl = fieldValue;
                    tournament.TimeControlType = TimeControlType.Rapid;
                    break;
                case "Time control (Standard)":
                    tournament.TimeControl = fieldValue;
                    tournament.TimeControlType = TimeControlType.Standart;
                    break;
                case "Location":
                    tournament.Location = fieldValue;
                    break;
                case "Number of rounds":
                    tournament.NumberOfRounds = int.TryParse(fieldValue, out int numberOfRounds) ? numberOfRounds : null;
                    break;
                case "Tournament type":
                    tournament.TournamentType = fieldValue;
                    break;
                case "Rating calculation":
                    tournament.RatingCalculation = fieldValue;
                    break;
                case "Date":
                    string[] dateParts = fieldValue?.Split(new string[] { " to " }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                    tournament.StartDate = DateTime.TryParse(dateParts.FirstOrDefault(), out DateTime startDate) ? startDate : null;
                    tournament.EndDate = DateTime.TryParse(dateParts.LastOrDefault(), out DateTime endDate) ? endDate : null;
                    break;
                case "Rating":
                    tournament.Rating = int.TryParse(fieldValue, out int rating) ? rating : null;
                    break;
            }
        }
        if (tournament.EndDate is null)
        {
            string? lastUpdateContainer = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='defaultDialog'][1]//td[@class='CR0'][1]/p")?.InnerText;
            if (lastUpdateContainer is not null)
            {
                Match match = Regex.Match(lastUpdateContainer, @"(\d{2}.\d{2}.\d{4} \d{2}:\d{2}:\d{2})");
                if (match.Success)
                {
                    string lastUpdateString = match.Groups[1].Value;
                    if (DateTime.TryParseExact(lastUpdateString, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lastUpdate))
                    {
                        tournament.EndDate = lastUpdate;
                    }
                }
            }
        }

        var playerRows = htmlDocument.DocumentNode.SelectNodes("//div[@class='defaultDialog'][3]/table[@class='CRs1']/tr") ?? new HtmlNodeCollection(null);

        List<string> playerHeaders = playerRows.FirstOrDefault()?.SelectNodes("th")?.Select(x => x.InnerText.Trim()).ToList() ?? new List<string>();
        int rankIndex = playerHeaders.IndexOf("Rk.");
        int noIndex = playerHeaders.IndexOf("SNo");
        int nameIndex = playerHeaders.IndexOf("Name");
        int ratingIndex = playerHeaders.IndexOf("Rtg");
        int clubCityIndex = playerHeaders.IndexOf("Club/City");
        int pointsIndex = playerHeaders.IndexOf("Pts.");
        int tb1Index = playerHeaders.IndexOf("TB1");
        int tb2Index = playerHeaders.IndexOf("TB2");
        int tb3Index = playerHeaders.IndexOf("TB3");

        foreach (HtmlNode playerRow in playerRows.Skip(1))
        {
            tournament.Players.Add(new PlayerScoreInfo
            {
                Rank = int.TryParse(playerRow.SelectSingleNode($"td[{rankIndex + 1}]")?.InnerText?.Trim(), out int round) ? round : null,
                No = int.TryParse(playerRow.SelectSingleNode($"td[{noIndex + 1}]")?.InnerText?.Trim(), out int no) ? no : null,
                Name = _chessDataNormalizer.NormalizeName(playerRow.SelectSingleNode($"td[{nameIndex + 1}]/a")?.InnerText?.Trim() ?? playerRow.SelectSingleNode($"td[{nameIndex + 1}]")?.InnerText?.Trim()),
                Rating = int.TryParse(playerRow.SelectSingleNode($"td[{ratingIndex + 1}]")?.InnerText?.Trim(), out int rating) ? rating : null,
                ClubCity = playerRow.SelectSingleNode($"td[{clubCityIndex + 1}]")?.InnerText?.Trim(),
                Points = double.TryParse(playerRow.SelectSingleNode($"td[{pointsIndex + 1}]")?.InnerText?.Trim(), NumberStyles.Number, new CultureInfo("de-DE"), out double points) ? points : null,
                TB1 = double.TryParse(playerRow.SelectSingleNode($"td[{tb1Index + 1}]")?.InnerText?.Trim(), NumberStyles.Number, new CultureInfo("de-DE"), out double tb1) ? tb1 : null,
                TB2 = double.TryParse(playerRow.SelectSingleNode($"td[{tb2Index + 1}]")?.InnerText?.Trim(), NumberStyles.Number, new CultureInfo("de-DE"), out double tb2) ? tb2 : null,
                TB3 = double.TryParse(playerRow.SelectSingleNode($"td[{tb3Index + 1}]")?.InnerText?.Trim(), NumberStyles.Number, new CultureInfo("de-DE"), out double tb3) ? tb3 : null,
            });
        }
        tournament.IsTeamTournament = htmlDocument.DocumentNode.SelectNodes("//a[text() = 'Team-Starting rank']")?.Any() ?? false;
        return tournament;
    }

    public async Task<PlayerInfo> GetPlayerInfoAsync(int tournamentId, int playerNo, CancellationToken cancellationToken)
    {
        string playerInfoUrl = $"{BaseUrl}/tnr{tournamentId}.aspx?lan=1&art=9&snr={playerNo}";
        HttpResponseMessage response = await _httpClient.GetAsync(playerInfoUrl, cancellationToken);
        response.EnsureSuccessStatusCode();

        HtmlDocument htmlDocument = new();
        string htmlContent = HttpUtility.HtmlDecode(await response.Content.ReadAsStringAsync(cancellationToken));
        htmlDocument.LoadHtml(htmlContent);

        PlayerInfo player = new();

        HtmlNodeCollection fieldNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='defaultDialog']/table[@class='CRs1'][1]/tr") ?? new HtmlNodeCollection(null);
        foreach (var fieldNode in fieldNodes)
        {
            string? fieldName = fieldNode.SelectSingleNode("td[1]")?.InnerText?.Trim();
            string? fieldValue = fieldNode.SelectSingleNode("td[2]")?.InnerText?.Trim();

            switch (fieldName)
            {
                case "Name":
                    player.Name = fieldValue;
                    break;
                case "Title":
                    player.Title = NormalizeTitle(fieldValue);
                    break;
                case "Starting rank":
                    player.No = int.TryParse(fieldValue, out int no) ? no : null; ;
                    break;
                case "Rating":
                    player.Rating = int.TryParse(fieldValue, out int rating) ? rating : null;
                    break;
                case "Rating national":
                    player.RatingNational = int.TryParse(fieldValue, out int ratingNational) ? ratingNational : null;
                    break;
                case "Rating international":
                    player.RatingInternational = int.TryParse(fieldValue, out int ratingInternational) ? ratingInternational : null;
                    break;
                case "Performance rating":
                    player.PerformanceRating = int.TryParse(fieldValue, out int performanceRating) ? performanceRating : null;
                    break;
                case "FIDE rtg +/-":
                    player.FideRtg = double.TryParse(fieldValue, NumberStyles.Number, new CultureInfo("de-DE"), out double fideRtg) ? fideRtg : null;
                    break;
                case "Points":
                    player.Points = double.TryParse(fieldValue, NumberStyles.Number, new CultureInfo("de-DE"), out double points) ? points : null;
                    break;
                case "Rank":
                    player.Rank = int.TryParse(fieldValue, out int rank) ? rank : null;
                    break;
                case "Federation":
                    player.Federation = fieldValue;
                    break;
                case "Club/City":
                    player.ClubCity = fieldValue;
                    break;
                case "Ident-Number":
                    player.IdentNumber = fieldValue;
                    break;
                case "Fide-ID":
                    player.FideId = fieldValue;
                    break;
                case "Year of birth":
                    player.YearOfBirth = int.TryParse(fieldValue, out int year) ? year : null;
                    break;
            }
        }

        player.Title ??= GetTitle(player.Name) ?? GetTitle(player.Rating);
        player.Name = _chessDataNormalizer.NormalizeName(player.Name);

        var gameRows = htmlDocument.DocumentNode.SelectNodes("//div[@class='defaultDialog']/table[@class='CRs1'][2]/tr") ?? new HtmlNodeCollection(null);

        List<string> gameHeaders = gameRows.FirstOrDefault()?.SelectNodes("th")?.Select(x => x.InnerText.Trim()).ToList() ?? new List<string>();
        int roundIndex = gameHeaders.IndexOf("Rd.");
        int boardIndex = gameHeaders.IndexOf("Bo.");
        int noIndex = gameHeaders.IndexOf("SNo");
        int nameIndex = gameHeaders.IndexOf("Name");
        int ratingIndex = gameHeaders.IndexOf("Rtg");
        int clubCityIndex = gameHeaders.IndexOf("Club/City");
        int resultIndex = gameHeaders.IndexOf("Res.");

        foreach (HtmlNode gameRow in gameRows.Skip(1))
        {
            HtmlNode resultNode = gameRow.SelectSingleNode($"td[{resultIndex + 1}]/table/tr[1]");

            player.Games.Add(new GameInfo
            {
                Round = int.TryParse(gameRow.SelectSingleNode($"td[{roundIndex + 1}]")?.InnerText?.Trim(), out int round) ? round : null,
                Board = int.TryParse(gameRow.SelectSingleNode($"td[{boardIndex + 1}]")?.InnerText?.Trim(), out int board) ? board : null,
                No = int.TryParse(gameRow.SelectSingleNode($"td[{noIndex + 1}]")?.InnerText?.Trim(), out int no) ? no : null,
                Name = _chessDataNormalizer.NormalizeName(gameRow.SelectSingleNode($"td[{nameIndex + 1}]/a")?.InnerText?.Trim() ?? gameRow.SelectSingleNode($"td[{nameIndex + 1}]")?.InnerText),
                Rating = int.TryParse(gameRow.SelectSingleNode($"td[{ratingIndex + 1}]")?.InnerText?.Trim(), out int rating) ? rating : null,
                ClubCity = gameRow.SelectSingleNode($"td[{clubCityIndex + 1}]")?.InnerText,
                IsWhite = resultNode?.SelectSingleNode("td[1]/div[@class='FarbewT']") is not null,
                Result = GetResult(resultNode?.SelectSingleNode("td[2]")?.InnerText?.Trim())
            });
        }

        return player;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    #region helper methods

    private static string? NormalizeTitle(string? title)
    {
        return title?.ToUpperInvariant() switch
        {
            "МС" => "NM",
            "КМС" => "CM",
            "1" or "1Р." or "І" or "I" or "1900" => "I",
            "2" or "2Р." or "II" or "ІІ" or "1800" => "II",
            "3" or "3Р." or "III" or "ІІІ" or "1700" => "III",
            "4" or "4Р." or "IV" or "ІV" or "1600" => "IV",
            "БР" or "Б/Р" or "ПОЧ" or "1500" => null,
            _ => title,
        };
    }
    private static string? GetTitle(string? name)
    {
        string? title = name?.Split(" ").Skip(2).LastOrDefault();
        return NormalizeTitle(title);
    }

    private static string? GetTitle(int? rating)
    {
        return rating switch
        {
            1900 => "I",
            1800 => "II",
            1700 => "III",
            1600 => "IV",
            1500 => null,
            _ => null,
        };
    }

    private static double? GetResult(string? result)
    {
        return result switch
        {
            "1" => 1,
            "½" => 0.5,
            "0" => 0,
            _ => null,
        };
    }

    #endregion
}