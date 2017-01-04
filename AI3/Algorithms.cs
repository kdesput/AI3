using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI3
{

    public static class Algorithms
    {
        public static int minBest = 0;
        public static int maxBest = 60;
        public static int Minmax(int x, int y, bool maxP, Gameboard board, int deep, int color, int heuristic)
        {
            List<Move> moves = GetMoves(x, y, board, color);

            if (moves.Count() == 0 || deep == 0)
            {
                switch (heuristic)
                {
                    case 0: //mostMoves
                        return moves.Count();
                    case 1: //mostPoints
                        int points = 0;
                        for (int i = 0; i < 8; i++)
                            for (int j = 0; j < 8; j++)
                                if (board.array[i, j] == color) points++;
                        return points;
                    case 2: //mostEdges
                        return boardValuesEdges[x, y];
                }
            }

            if (maxP)
            {
                int best = minBest;
                foreach (Move m in moves)
                {
                    Gameboard newboard = new Gameboard(board.window, false, board.array);
                    int val = Minmax(m.x, m.y, false, newboard, deep - 1, color * -1, heuristic);
                    best = Math.Max(best, val);
                }
                return best;
            }
            else
            {
                int best = maxBest;
                foreach (Move m in moves)
                {
                    Gameboard newboard = new Gameboard(board.window, false, board.array);
                    int val = Minmax(m.x, m.y, true, newboard, deep - 1, color * -1, heuristic);
                    best = Math.Min(best, val);
                }
                return best;
            }
        }

        public static int Alphabeta(int x, int y, int alpha, int beta, bool maxP, Gameboard board, int deep, int color, int heuristic)
        {
            List<Move> moves = GetMoves(x, y, board, color);

            if (moves.Count() == 0 || deep == 0)
            {
                switch (heuristic)
                {
                    case 0: //mostMoves
                        return moves.Count();
                    case 1: //mostPoints
                        int points = 0;
                        for (int i = 0; i < 8; i++)
                            for (int j = 0; j < 8; j++)
                                if (board.array[i, j] == color) points++;
                        return points;
                    case 2: //mostEdges
                        return boardValuesEdges[x, y];
                }
            }

            if (maxP)
            {
                int val = minBest;
                foreach (Move m in moves)
                {
                    Gameboard newboard = new Gameboard(board.window, false, board.array);
                    val = Math.Max(val, Alphabeta(m.x, m.y, alpha, beta, false, newboard, deep - 1, color * -1, heuristic));
                    alpha = Math.Max(alpha, val);
                    if (beta <= alpha)
                        break;
                }
                return val;
            }
            else
            {
                int val = maxBest;
                foreach (Move m in moves)
                {
                    Gameboard newboard = new Gameboard(board.window, false, board.array);
                    val = Math.Min(val, Minmax(m.x, m.y, true, newboard, deep - 1, color * -1, heuristic));
                    beta = Math.Min(beta, val);
                    if (beta <= alpha)
                        break;
                }
                return val;
            }
        }





        public static List<Move> GetMoves(int x, int y, Gameboard board, int color)
        {
            board.Put(x, y, color, true);
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (board.IsPossible(i, j, color * -1))
                        moves.Add(new Move(i, j));
                }
            return moves;
        }

        private static int[,] boardValuesEdges = new int[8, 8]{
            {50, 50, 50, 50, 50, 50, 50, 50},
            {50, 10, 10, 10, 10, 10, 10, 50},
            {50, 10,  1, 1, 1, 1,  10, 50},
            {50, 10,  1, 0, 0, 1,  10, 50},
            {50, 10,  1, 0, 0, 1,  10, 50},
            {50, 10,  1, 1, 1, 1,  10, 50},
            {50, 10, 10, 10, 10, 10, 10, 50},
            {50, 50, 50, 50, 50, 50, 50, 50}};
    }
}
