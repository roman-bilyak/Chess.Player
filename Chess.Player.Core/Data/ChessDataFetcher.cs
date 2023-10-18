﻿using HtmlAgilityPack;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace Chess.Player.Data
{
    public class ChessResultsDataFetcher : IChessDataFetcher
    {
        private const string BaseUrl = "https://chess-results.com";

        public async Task<List<PlayerTournament>?> GetPlayerTournamentsAsync(string lastName, string firstName, CancellationToken cancellationToken)
        {
            using HttpClient httpClient = new();

            string searchUrl = $"{BaseUrl}/SpielerSuche.aspx?lan=1";
            HttpResponseMessage response = await httpClient.GetAsync(searchUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

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
                    { "ctl00$P1$txt_nachname", lastName},
                    { "ctl00$P1$txt_verein", "" },
                    { "ctl00$P1$txt_von_tag", "" },
                    { "ctl00$P1$txt_vorname", firstName},
                };
            response = await httpClient.PostAsync(searchUrl, new FormUrlEncodedContent(postData), cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            htmlContent = HttpUtility.HtmlDecode(await response.Content.ReadAsStringAsync(cancellationToken));
            htmlDocument.LoadHtml(htmlContent);

            List<PlayerTournament> playerTournaments = new();

            var searchResultNodes = htmlDocument.DocumentNode.SelectNodes("//table[@class='CRs2']/tr")?.Skip(1) ?? new HtmlNodeCollection(null);
            foreach (HtmlNode node in searchResultNodes)
            {
                string? name = node.SelectSingleNode("td[1]/a")?.InnerText?.Trim();
                if (!name?.StartsWith(lastName, StringComparison.InvariantCultureIgnoreCase) ?? false)
                {
                    continue;
                }

                string? federation = node.SelectSingleNode("td[5]")?.InnerText?.Trim();
                if (federation == "RUS")
                {
                    continue;
                }

                int? tournamentId = null;
                int? startingRank = null;

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
                        startingRank = snr;
                    }
                }

                if (tournamentId.HasValue && startingRank.HasValue)
                {
                    playerTournaments.Add(new PlayerTournament(tournamentId.Value, startingRank.Value));
                }
            }
            return playerTournaments;
        }

        public async Task<TournamentInfo?> GetTournamentInfoAsync(int tournamentId, CancellationToken cancellationToken)
        {
            using HttpClient httpClient = new();

            string tournamentInfoUrl = $"{BaseUrl}/tnr{tournamentId}.aspx?lan=1&turdet=YES";
            HttpResponseMessage response = await httpClient.GetAsync(tournamentInfoUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

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

                    { "cb_alleDetails", "Show tounament details" }
                };

            response = await httpClient.PostAsync(tournamentInfoUrl, new FormUrlEncodedContent(postData));
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

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
            tournament.NumberOfPlayers = htmlDocument.DocumentNode.SelectNodes("//div[@class='defaultDialog'][3]/table[@class='CRs1']/tr[position()>1]")?.Count;
            tournament.IsTeamTournament = htmlDocument.DocumentNode.SelectNodes("//a[text() = 'Team-Starting rank']")?.Any() ?? false;
            return tournament;
        }

        public async Task<PlayerInfo?> GetPlayerInfoAsync(int tournamentId, int startingRank, CancellationToken cancellationToken)
        {
            using HttpClient httpClient = new();

            string playerInfoUrl = $"{BaseUrl}/tnr{tournamentId}.aspx?lan=1&art=9&snr={startingRank}";
            HttpResponseMessage response = await httpClient.GetAsync(playerInfoUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            HtmlDocument htmlDocument = new();
            string htmlContent = HttpUtility.HtmlDecode(await response.Content.ReadAsStringAsync(cancellationToken));
            htmlDocument.LoadHtml(htmlContent);

            PlayerInfo player = new();

            HtmlNodeCollection fieldNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='defaultDialog']/table[@class='CRs1']/tr") ?? new HtmlNodeCollection(null);
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
                        player.StartingRank = int.TryParse(fieldValue, out int staringRank) ? staringRank : null; ;
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

            return player;
        }

        #region helper methods
        private static string? NormalizeTitle(string? title)
        {
            return title switch
            {
                "МС" => "NM",
                "КМС" => "CM",
                "1" or "І" or "I" or "1900" => "I",
                "2" or "II" or "ІІ" or "1800" => "II",
                "3" or "III" or "ІІІ" or "1700" => "III",
                "4" or "IV" or "ІV" or "1600" => "IV",
                "БР" or "Б/Р" or "поч" or "1500" => null,
                _ => title,
            };
        }
        private static string? GetTitle(string? name)
        {
            string? title = name?.Split(" ").Skip(2).LastOrDefault()?.ToUpperInvariant();
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

        #endregion
    }
}