using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleGame
{
    public enum SetName
    {
        Set1,
        Set2,
        Set3
    }

    public enum StatusName
    {
        Scheduled,
        Playing,
        Deuce,
        Finished,
        Close
    }

    public class GameCommon
    {
        public static int[] PointsValue = { 15, 30, 40 };

        public static int PointsToWin = 40;
    }
}
