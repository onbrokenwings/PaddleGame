using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleGame
{
    class Program
    {
        //***********************************************
        //Game Requirements:
        //***********************************************

        //Several matches can be play at a time.
        //In a game of paddle, a player begins with a score of zero (0). With each success, the player earns more points.
        //The points are earned in this sequence: 0, 15, 30, 40.
        //If a player has 40 and scores again, that player wins the game as long as the other player does not also have 40 points.
        //If both players reach 40 points it is referred to as a 'deuce.'
        //Scoring during deuce give a player advantage.
        //If the other player scores again, the score returns to deuce. If a player has advantage and scores again, that player wins the game.
        //The matches are played to the best of three sets.

        //Players must be able to score points of the different games.
        //The game must be able to be completed with a winner
        //The 'deuce' case should be handled
        //After a game has been won, a winner must be determined
        //The current score of either player should be available at any point during the game.

        static void Main(string[] args)
        {
            var player1 = new PaddlePlayer { Licence = "SJ125OL", Name = "Jalen" };

            var player2 = new PaddlePlayer { Licence = "MU345UX", Name = "Damien" };

            var player3 = new PaddlePlayer { Licence = "JA687OP", Name = "Calvin" };

            var game1Guid = Tournament.Instance.StartGame(player1, player2);

            Tournament.Instance.SetScore(player1, game1Guid);

            var game2Guid = Tournament.Instance.StartGame(player1, player3);

            //Game 2 exemplifies the deuce case

            Tournament.Instance.SetScore(player1, game2Guid);

            Tournament.Instance.SetScore(player1, game2Guid);

            Tournament.Instance.SetScore(player1, game2Guid);

            Tournament.Instance.SetScore(player3, game2Guid);

            Tournament.Instance.SetScore(player3, game2Guid);

            Tournament.Instance.SetScore(player3, game2Guid);

            Tournament.Instance.SetScore(player3, game2Guid);

            Tournament.Instance.SetScore(player1, game2Guid);

            Tournament.Instance.SetScore(player3, game2Guid);

            Tournament.Instance.SetScore(player3, game2Guid);

            var score = Tournament.Instance.GetScore(player3, game2Guid);

            if (score == null)
            {
                Console.WriteLine("Player isn't playing in the actual game [{0}][{1}].", player3.Licence, game2Guid);
            }
            else
            {
                Console.WriteLine("Scoreboard for game ID [{0}] --> [{1}][{2}][{3}][{4}]", game2Guid, score.Score, score.SetsWon, 
                    score.Set.ToString(), score.Player.Licence);
            }

            Console.ReadLine();
        }
    }
}
