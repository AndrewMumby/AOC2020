using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    enum Player
    {
        Player1 = 0,
        Player2 = 1,    
    }
    class Day22
    {
        public static string A(string input)
        {
            CrabCards game = new CrabCards(input);
            game.PlayUntilWinner();
            return game.HighestScore().ToString();
        }

        public static string B(string input)
        {
            /*
            List<int> a = new List<int>();
            List<int> b = new List<int>();
            a.Add(5);
            a.Add(2);
            a.Add(1);
            a.Add(10);
            b.Add(5);
            b.Add(2);
            b.Add(1);
            b.Add(10);
            Console.WriteLine(a.GetHashCode().ToString());
            Console.WriteLine(b.GetHashCode().ToString());
            Console.WriteLine((a == b).ToString());
            Console.WriteLine(a.Equals(b).ToString());
            HashSet<List<int>> set = new HashSet<List<int>>();
            set.Add(a);
            Console.WriteLine(set.Contains(b).ToString());
            */
            RecursiveCrabCards game = new RecursiveCrabCards(input);
            game.FindWinner();
            return game.HighestScore().ToString();
        }
    }

    internal class CrabCards
    {
         Queue<int> player1;
         Queue<int> player2;

        public CrabCards(string input)
        {
            FillHands(input);
        }

        private  void FillHands(string input)
        {
            string[] playerParts = input.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.None);
            player1 = FillHand(playerParts[0]);
            player2 = FillHand(playerParts[1]);
        }

        private Queue<int> FillHand(string handString)
        {
            Queue<int> playerHand = new Queue<int>();
            string[] lines = handString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 1; i < lines.Length; i++)
            {
                playerHand.Enqueue(Convert.ToInt32(lines[i]));
            }
            return playerHand;
        }

        internal void PlayUntilWinner()
        {
            while (player1.Count > 0 && player2.Count > 0)
            {
                int player1Number = player1.Dequeue();
                int player2Number = player2.Dequeue();
                if (player1Number > player2Number)
                {
                    player1.Enqueue(player1Number);
                    player1.Enqueue(player2Number);
                }
                else
                {
                    player2.Enqueue(player2Number);
                    player2.Enqueue(player1Number);
                }
            }
        }

        internal int HighestScore()
        {
            Queue<int> winningHand;
            if (player1.Count > player2.Count)
            {
                winningHand = player1;
            }
            else
            {
                winningHand = player2;
            }
            int total = 0;
            while (winningHand.Count > 0)
            {
                int length = winningHand.Count;
                int card = winningHand.Dequeue();
                total = total + length * card;
            }
            return total;
        }

    }

    internal class RecursiveCrabCards
    {
        List<Queue<int>> playerHands;
        HashSet<string> previousHands;

        public RecursiveCrabCards(string input)
        {
            FillHands(input);
            previousHands = new HashSet<string>();
        }

        public RecursiveCrabCards(List<Queue<int>> hands)
        {
            playerHands = hands;
            previousHands = new HashSet<string>();
        }

        private void FillHands(string input)
        {
            playerHands = new List<Queue<int>>();
            string[] playerParts = input.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.None);

            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                playerHands.Add(FillHand(playerParts[(int)player]));
            }
        }

        private Queue<int> FillHand(string handString)
        {
            Queue<int> playerHand = new Queue<int>();
            string[] lines = handString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 1; i < lines.Length; i++)
            {
                playerHand.Enqueue(Convert.ToInt32(lines[i]));
            }
            return playerHand;
        }

        internal Player FindWinner()
        {
            while (playerHands[0].Count > 0 && playerHands[1].Count > 0)
            {
                if (CheckAndStoreHand())
                {
                    //Console.WriteLine("Repeated hands! Player 1 wins game!");
                    return Player.Player1;
                }
                int player1Card = playerHands[0].Dequeue();
                int player2Card = playerHands[1].Dequeue();
                //Console.WriteLine("Player 1: " + player1Card + ". Player 2: " + player2Card);
                Player winner;
                if (player1Card <= playerHands[0].Count && player2Card <= playerHands[1].Count)
                {
                    // Play sub game
                    // Copy the card decks
                    //Console.WriteLine("Playing Subgame!");

                    List<Queue<int>> newHands = new List<Queue<int>>();
                    newHands.Add(CopyQueue(player1Card, playerHands[0]));
                    newHands.Add(CopyQueue(player2Card, playerHands[1]));
                    RecursiveCrabCards subGame = new RecursiveCrabCards(newHands);
                    winner = subGame.FindWinner();
                }
                else
                {
                    // Highest card wins
                    if (player1Card > player2Card)
                    {
                        //Console.WriteLine("Player1 wins draw");
                        winner = Player.Player1;
                    }
                    else
                    {
                        //Console.WriteLine("Player2 wins draw");
                        winner = Player.Player2;
                    }
                }
                if (winner == Player.Player1)
                {
                    playerHands[0].Enqueue(player1Card);
                    playerHands[0].Enqueue(player2Card);
                }
                else
                {
                    playerHands[1].Enqueue(player2Card);
                    playerHands[1].Enqueue(player1Card);
                }
            }
            if (playerHands[0].Count == 0)
            {
                //Console.WriteLine("Player 2 wins game!");
                return Player.Player2;
            }
            else
            {
                //Console.WriteLine("Player 1 wins game!");
                return Player.Player1;
            }

        }

        private Queue<int> CopyQueue(int player1Card, Queue<int> queue)
        {
            List<int> list = new List<int>(queue);
            Queue<int> newQueue = new Queue<int>();
            for (int i = 0; i < player1Card; i++)
            {
                newQueue.Enqueue(list[i]);
            }
            return newQueue;
        }

        private bool CheckAndStoreHand()
        {
            // Convert hand to string
            StringBuilder handStringBuilder = new StringBuilder();
            for (int p = 0; p < 2; p++)
            {
                for(int i = 0; i < playerHands[p].Count; i++)
                {
                    List<int> listHand = playerHands[p].ToList();
                    handStringBuilder.Append(listHand[i]);
                }
                handStringBuilder.Append("/");
            }
            string handString = handStringBuilder.ToString();
            // Check to see if string is present
            if (previousHands.Contains(handString))
            {
                return true;
            }
            else
            {
                // Store string
                previousHands.Add(handString);
                return false;
            }
        }


        internal int HighestScore()
        {
            int bestScore = 0;
            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                int total = 0;
                while (playerHands[(int)player].Count > 0)
                {
                    int length = playerHands[(int)player].Count;
                    int card = playerHands[(int)player].Dequeue();
                    total = total + length * card;
                }
                bestScore = Math.Max(bestScore, total);
            }

            return bestScore;
        }


    }
}
