using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleGame
{
    public class ScoreBoard
    {
        public PaddlePlayer Player { get; set; }

        public int Score { get; set; }

        public StatusName Status { get; set; }

        public SetName Set { get; set; }

        public int SetsWon { get; set; }
    }
}
