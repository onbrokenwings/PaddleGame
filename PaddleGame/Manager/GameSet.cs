using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleGame
{
    public class GameSet
    {
        private int[] _scoreBoard;

        private bool[] _advsDeuce;

        public SetName Name { get; set; }

        public PaddlePlayer Winner { get; private set; }

        public StatusName ScoreStatus { get; set; }

        private string GameID { get; set; }

        /// <summary>
        /// Creates a gameset for given gamefield
        /// </summary>
        /// <param name="pCurrent">Set name to identify</param>
        /// <param name="pGameID">Game ID in the tournament</param>
        public GameSet(SetName pCurrent, string pGameID)
        {
            Name = pCurrent;

            _scoreBoard = new int[] { 0, 0 };
            _advsDeuce = new bool[] { false, false };

            ScoreStatus = StatusName.Scheduled;

            GameID = pGameID;

            Winner = null;
        }

        /// <summary>
        /// Creates a gameset and change status for given gamefield
        /// </summary>
        /// <param name="pCurrent">Set name to identify</param>
        /// <param name="pGameID">Game ID in the tournament</param>
        /// <param name="pStatus">Status of the gameset</param>
        public GameSet(SetName pCurrent, string pGameID, StatusName pStatus) : this(pCurrent, pGameID)
        {
            ScoreStatus = pStatus;
        }

        /// <summary>
        /// Action of score for the actual set
        /// </summary>
        /// <param name="pPlayer">Player who scores in the game</param>
        /// <returns>Score is succesfully set</returns>
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

                            return true;
                        }
                        else if (_scoreBoard[pos] == rival && _advsDeuce.All(a => a == false))
                        {
                            ScoreStatus = StatusName.Deuce;

                            _advsDeuce[pos] = true;

                            return true;
                        }
                        else if (ScoreStatus == StatusName.Deuce)
                        {
                            _advsDeuce = new bool[] { false, false };

                            return true;
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

        /// <summary>
        /// Get the current score of the set
        /// </summary>
        /// <param name="pPlayer">Player to access to the scoreboard</param>
        /// <returns>Current score of the set</returns>
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
