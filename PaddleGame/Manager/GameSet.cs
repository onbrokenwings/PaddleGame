using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleGame
{
    public class GameSet
    {
        public SetName Name { get; set; }

        private int[] _scoreBoard;
        private bool[] _advsDeuce;
        
        public PaddlePlayer Winner { get; private set; }

        public StatusName ScoreStatus { get; set; }

        private string GameID { get; set; }

        public GameSet(SetName pCurrent, string pGameID)
        {
            Name = pCurrent;

            _scoreBoard = new int[] { 0, 0 };
            _advsDeuce = new bool[] { false, false };

            ScoreStatus = StatusName.Scheduled;

            GameID = pGameID;

            Winner = null;
        }

        public GameSet(SetName pCurrent, string pGameID, StatusName pStatus) : this(pCurrent, pGameID)
        {
            ScoreStatus = pStatus;
        }

        public bool Score(PaddlePlayer pPlayer)
        {
            //Get the actual game involved for this set

            var game = Tournament.Instance.GetGame(GameID);

            if (game == null) throw new ArgumentException("Game wasn't found at this moment");

            var pos = game.FindPlayer(pPlayer);

            if (pos == -1)
            {
                throw new ArgumentException("Player isn't currently playing in the game.");
            }
            else
            {
                if (Winner == null)
                {
                    if (_scoreBoard[pos] == GameCommon.PointsToWin)
                    {
                        //Check opponent score and decide the victory of the set

                        int rival = (pos == 1) ? _scoreBoard[pos - 1] : _scoreBoard[pos + 1];

                        if (_scoreBoard[pos] > rival || (ScoreStatus == StatusName.Deuce && _advsDeuce[pos] == true))
                        {
                            ScoreStatus = StatusName.Finished;

                            Winner = game.Players[pos];
                        }
                        else if (_scoreBoard[pos] == rival && _advsDeuce.All(a => a == false))
                        {
                            ScoreStatus = StatusName.Deuce;

                            _advsDeuce[pos] = true;
                        }
                        else if (_advsDeuce.Any(a => a == true))
                        {
                            _advsDeuce = new bool[] { false, false };
                        }
                    }
                    else
                    {
                        for (int i = 0; i < GameCommon.PointsValue.Length; i++)
                        {
                            if (GameCommon.PointsValue[i] > _scoreBoard[pos])
                            {
                                _scoreBoard[pos] = GameCommon.PointsValue[i];

                                //keep increment 'till score is topped and no winner decided

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public int GetScore(PaddlePlayer pPlayer)
        {
            //Get the actual game involved for this set

            var game = Tournament.Instance.GetGame(GameID);

            if (game == null) throw new ArgumentException("Game wasn't found at this moment");

            var pos = game.FindPlayer(pPlayer);

            return _scoreBoard[pos];
        }
    }
}
