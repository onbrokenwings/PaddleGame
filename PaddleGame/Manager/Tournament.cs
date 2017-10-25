using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaddleGame
{
    public class Tournament
    {
        private List<GameField> _games;

        private static Tournament _manager;

        public static Tournament Instance
        {
            get { return _manager ?? (_manager = new Tournament()); }
        }

        private Tournament()
        {
            _games = new List<GameField>();
        }

        public string StartGame(PaddlePlayer pVisiting, PaddlePlayer pHome)
        {
            var game = new GameField(pVisiting, pHome);

            if (!_games.Contains(game))
            {
                var guid = Guid.NewGuid();

                var gameID = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", string.Empty); ;

                game.Start(gameID);

                _games.Add(game);

                Console.WriteLine("New game has started with ID [{0}][{1}][{2}]", game.ID, pVisiting.Licence, pHome.Licence);

                return game.ID;
            }
            else
            {
                Console.WriteLine("The game has already started.");

                return string.Empty;
            }
        }

        public void SetScore(PaddlePlayer pPlayer, string pGameID)
        {
            try
            {
                if (_games.Count > 0)
                {
                    var game = _games.FirstOrDefault(a => a.ID.Equals(pGameID));

                    if (game != null)
                    {
                        game.SetScore(pPlayer);

                        Console.WriteLine("Player [{0}] has scored at game ID [{1}]", pPlayer.Licence, game.ID);
                    }
                    else
                    {
                        Console.WriteLine("Game with ID [{0}] no longer exists.", pGameID);
                    }

                    game.Progress();
                }
                else
                {
                    Console.WriteLine("No games are scheduled.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in order to set a score: {0}", ex.Message);
            }
        }

        public ScoreBoard GetScore(PaddlePlayer pPlayer, string pGameID)
        {
            var game = GetGame(pGameID);

            if (game != null && game.Players.Contains(pPlayer))
            {
                //Get the current playing set

                var current = game.GetCurrentSet();

                //Get total sets won

                var CountSets = 0;

                game.CurrentScore.TryGetValue(pPlayer.Licence, out CountSets);

                return new ScoreBoard
                {
                    Player = pPlayer,
                    Set = current.Name,
                    SetsWon = CountSets,
                    Status = current.ScoreStatus,
                    Score = current.GetScore(pPlayer)
                };
            }
            else
            {
                Console.WriteLine("No scoreboard has been found for the details");

                return null;
            }
        }

        public GameField GetGame(string pGameID)
        {
            return _games.FirstOrDefault(a => a.ID.Equals(pGameID));
        }
    }
}
