using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaddleGame
{
    public class GameField : IEquatable<GameField>
    {
        private GameSet[] Sets;

        public string ID { get; private set; }

        public PaddlePlayer[] Players { get; private set; }

        public PaddlePlayer Winner { get; private set; }

        public Dictionary<string, int> CurrentScore { get; private set; }

        public GameField(PaddlePlayer pVisiting, PaddlePlayer pHome)
        {
            Players = new PaddlePlayer[] { pVisiting, pHome };

            CurrentScore = new Dictionary<string, int>();
        }

        public void Start(string pGameID)
        {
            ID = pGameID;

            Sets = new GameSet[] 
            { 
                new GameSet(SetName.Set1, ID, StatusName.Playing), 
                new GameSet(SetName.Set2, ID),
                new GameSet(SetName.Set3, ID) 
            };

            Console.WriteLine("Game ID [{0}] has been started at [{1}].", ID, DateTime.Now.ToString("HH:mm:ss"));
        }

        public void SetScore(PaddlePlayer pPlayer)
        {
            //Only score at playing sets.

            foreach (var set in Sets)
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

        public void Progress()
        {
            //In case some set is finished or winner has to be accounted on memory.

            Array.ForEach(Sets, set =>
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

                    //Set is accounted and finally close.

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

        public GameSet GetCurrentSet()
        {
            return Sets.FirstOrDefault(a => a.ScoreStatus == StatusName.Playing || a.ScoreStatus == StatusName.Deuce);
        }

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
            for (int i = 0; i < Sets.Length; i++)
            {
                if (Sets[i].ScoreStatus == StatusName.Scheduled)
                {
                    Sets[i].ScoreStatus = StatusName.Playing;

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
