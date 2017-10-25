using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaddleGame
{
    public class GameField : IEquatable<GameField>
    {
        private GameSet[] _sets;

        public string ID { get; private set; }

        public PaddlePlayer[] Players { get; private set; }

        public PaddlePlayer Winner { get; private set; }

        public Dictionary<string, int> CurrentScore { get; private set; }

        /// <summary>
        /// Creates a gamefield for playing
        /// </summary>
        /// <param name="pVisitingPlayer">Visiting player</param>
        /// <param name="pHomePlayer">Home Player</param>
        public GameField(PaddlePlayer pVisitingPlayer, PaddlePlayer pHomePlayer)
        {
            Players = new PaddlePlayer[] { pVisitingPlayer, pHomePlayer };

            CurrentScore = new Dictionary<string, int>();
        }

        /// <summary>
        /// Start any given game
        /// </summary>
        /// <param name="pGameID">Game ID which identifies each game in the tournament</param>
        public void Start(string pGameID)
        {
            ID = pGameID;

            _sets = new GameSet[] 
            { 
                new GameSet(SetName.Set1, ID, StatusName.Playing), 
                new GameSet(SetName.Set2, ID),
                new GameSet(SetName.Set3, ID) 
            };

            Console.WriteLine("Game ID [{0}] has been started at [{1}].", ID, DateTime.Now.ToString("HH:mm:ss"));
        }

        /// <summary>
        /// Action of score for the actual game
        /// </summary>
        /// <param name="pPlayer">Player who scores in the game</param>
        public void SetScore(PaddlePlayer pPlayer)
        {
            //Only able to score at playing sets.

            foreach (var set in _sets)
            {
                if (set.ScoreStatus == StatusName.Playing || set.ScoreStatus == StatusName.Deuce)
                {
                    if (set.Score(pPlayer))
                    {
                        Console.WriteLine("Player [{0}] has succesfully scored.", pPlayer.Licence);

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Progression of the game once the score is set
        /// </summary>
        public void Progress()
        {
            //In case some set is finished or winner has to be accounted on memory.

            Array.ForEach(_sets, set =>
            {
                if (set.ScoreStatus == StatusName.Finished)
                {
                    Console.WriteLine("Player [{0}][{1}] has won current set.", set.Winner.Licence, set.Winner.Name);

                    if (CurrentScore.ContainsKey(set.Winner.Licence))
                    {
                        CurrentScore[set.Winner.Licence] = CurrentScore[set.Winner.Licence] + 1;
                    }
                    else
                    {
                        CurrentScore.Add(set.Winner.Licence, 1);
                    }

                    //Set is accounted and finally close to evaluate.

                    set.ScoreStatus = StatusName.Close;

                    if (IsGameWinner())
                    {
                        Console.WriteLine("Game winner is decided for game [{0}] [{1}][{2}]", ID, Winner.Licence, Winner.Name);
                    }
                    else
                    {
                        StartNextSet();

                        Console.WriteLine("Game progresses on new set for scoring at game ID [{0}]", ID);
                    }
                }
            });
        }

        /// <summary>
        /// Get the current set which is playing at the moment
        /// </summary>
        /// <returns>Get the current set details</returns>
        public GameSet GetCurrentSet()
        {
            return _sets.FirstOrDefault(a => a.ScoreStatus == StatusName.Playing || a.ScoreStatus == StatusName.Deuce);
        }

        /// <summary>
        /// Find given player is visiting or home in the game
        /// </summary>
        /// <param name="pPlayer">Player to intent to find</param>
        /// <returns>Value 0 is visiting and 1 is home</returns>
        public int FindPlayer(PaddlePlayer pPlayer)
        {
            if (Players.Contains(pPlayer))
            {
                return Array.IndexOf(Players, pPlayer);
            }
            else
            {
                return -1;
            }
        }

        private bool IsGameWinner()
        {
            if (CurrentScore.Count > 0)
            {
                foreach (var final in CurrentScore)
                {
                    if (final.Value == 2)
                    {
                        Winner = Players.FirstOrDefault(a => a.Licence.Equals(final.Key));

                        return true;
                    }
                } 
            }

            return false;
        }

        private void StartNextSet()
        {
            for (int i = 0; i < _sets.Length; i++)
            {
                if (_sets[i].ScoreStatus == StatusName.Scheduled)
                {
                    _sets[i].ScoreStatus = StatusName.Playing;

                    break;
                }
            }
        }

        public bool Equals(GameField pPlayer)
        {
            return pPlayer.Players.First().Licence.Equals(Players.First().Licence) &&
                pPlayer.Players.Last().Licence.Equals(Players.Last().Licence);
        }
    }
}
