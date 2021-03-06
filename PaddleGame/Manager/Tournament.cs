﻿using System;
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

        /// <summary>
        /// Start a new game into the tournament
        /// </summary>
        /// <param name="pVisitingPlayer">Visiting player</param>
        /// <param name="pHomePlayer">Home player</param>
        /// <returns>Game ID which identifies the actual game in the tournament</returns>
        public string StartGame(PaddlePlayer pVisitingPlayer, PaddlePlayer pHomePlayer)
        {
            var game = new GameField(pVisitingPlayer, pHomePlayer);

            if (!_games.Contains(game))
            {
                var guid = Guid.NewGuid();

                var gameID = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", string.Empty); ;

                game.Start(gameID);

                _games.Add(game);

                Console.WriteLine("New game has started with ID [{0}][{1}][{2}]", game.ID, pVisitingPlayer.Licence, pHomePlayer.Licence);

                return game.ID;
            }
            else
            {
                Console.WriteLine("The game has already started.");

                return string.Empty;
            }
        }

        /// <summary>
        /// Action of scoring in a game of the tournament
        /// </summary>
        /// <param name="pPlayer">Player who scores</param>
        /// <param name="pGameID">Game ID which identifies the actual game</param>
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

        /// <summary>
        /// Get the current scoreboard for a game of the tournament
        /// </summary>
        /// <param name="pPlayer">Player to get the scoreboard</param>
        /// <param name="pGameID">Game ID which identifies the actual game</param>
        /// <returns>The current scoreboard for the game in progress</returns>
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

        /// <summary>
        /// Get the game for a given game ID of the tournament
        /// </summary>
        /// <param name="pGameID">Game ID which identifies the actual game</param>
        /// <returns>The current game set for a given game</returns>
        public GameField GetGame(string pGameID)
        {
            if (_games.Any(a => a.ID.Equals(pGameID)))
            {
                return _games.FirstOrDefault(a => a.ID.Equals(pGameID));
            }
            else
            {
                Console.WriteLine("Game wasn't found for the game ID [{0}]", pGameID);

                return null;
            }
        }
    }
}
