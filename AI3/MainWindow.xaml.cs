using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AI3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Gameboard gameboard;
        int heuristic1, heuristic2; //0 - most moves, 1 - most points, 2 - most edges
        int algorithm1, algorithm2; //0 - minmax, 1 - alfabeta
        bool playerIsPlaying;
        int depth = 4;
        int turn = 1;
        public MainWindow()
        {
            InitializeComponent();
            gameboard = new Gameboard(this, true);
            algorithm1 = 0;
            algorithm2 = 0;
            heuristic1 = 0;
            heuristic2 = 0;
            playerIsPlaying = true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            gameboard = new Gameboard(this, true);
            heuristic1 = comboBoxHeuristic1.SelectedIndex;
            if (radioButton2.IsChecked == true)
                algorithm1 = 0;
            else algorithm1 = 1;
            if(!playerIsPlaying)
            {
                if (radioButton4.IsChecked == true)
                    algorithm2 = 0;
                else algorithm2 = 1;
                heuristic2 = comboBoxHeuristic2.SelectedIndex;
                turn = 1;
            }
        }

        private void board_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (playerIsPlaying)
            {
                Point point = e.GetPosition(board);
                int x = (int)(point.X / 32);
                int y = (int)(point.Y / 32);
                if (gameboard.IsPossible(x, y, -1))
                {
                    gameboard.Put(x, y, -1, true);
                    List<Move> moves = new List<Move>();
                    do
                    {
                        MoveAI(1, 1);
                        moves = new List<Move>();
                        for (int i = 0; i < 8; i++)
                            for (int j = 0; j < 8; j++)
                                if (gameboard.IsPossible(i, j, -1)) moves.Add(new Move(i, j));
                        if (gameboard.GetWinner() != 0)
                        {
                            string color = "";
                            if (gameboard.GetWinner() == -1) color = "Blue";
                            else color = "Red";
                            MessageBox.Show("End of the game! " + color + " wins!");
                            break;
                        }
                    }
                    while (moves.Count == 0);
                }
            }
            else
            {
                if (gameboard.GetWinner() == 0)
                {
                    if(turn == 1)
                    {
                        MoveAI(1, 1);
                        turn = 2;
                    }
                    else
                    {
                        MoveAI(-1, 2);
                        turn = 1;
                    }
                }
                else
                {
                    string color = "";
                    if (gameboard.GetWinner() == -1) color = "Blue";
                    else color = "Red";
                    MessageBox.Show("End of the game! " + color + " wins!");
                }
            }

        }

        private void MoveAI(int color, int aiNumber)
        {
            List<Move> moves = new List<Move>();
            int algorithm, heuristic;
            if (aiNumber == 1)
            {
                algorithm = algorithm1;
                heuristic = heuristic1;
            }
            else
            {
                algorithm = algorithm2;
                heuristic = heuristic2;
            }
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (gameboard.IsPossible(i, j, color)) moves.Add(new Move(i, j));
            if (moves.Count == 0) return;
            int[] movesValues = new int[moves.Count];

            Move bestMove = moves[0];
            int bestIndex = 0;

            for (int i = 0; i < movesValues.Length; i++)
            {
                Gameboard newBoard = new Gameboard(this, false, gameboard.array);
                if(algorithm == 0)
                    movesValues[i] = Algorithms.Minmax(moves[i].x, moves[i].y, true, newBoard, depth, color, heuristic);
                else
                   movesValues[i] = Algorithms.Alphabeta(moves[i].x, moves[i].y, Algorithms.minBest, Algorithms.maxBest, true, newBoard, depth, color, heuristic1);

                if (movesValues[bestIndex] < movesValues[i])
                {
                    bestMove = moves[i];
                    bestIndex = i;
                }
            }
            gameboard.Put(bestMove.x, bestMove.y, color, true);
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            //AI vs AI
            playerIsPlaying = false;
            radioButton4.Visibility = Visibility.Visible;
            radioButton5.Visibility = Visibility.Visible;
            comboBoxHeuristic2.Visibility = Visibility.Visible;
            label2.Visibility = Visibility.Visible;
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            //AI vs player
            playerIsPlaying = true;
            radioButton4.Visibility = Visibility.Hidden;
            radioButton5.Visibility = Visibility.Hidden;
            comboBoxHeuristic2.Visibility = Visibility.Hidden;
            label2.Visibility = Visibility.Hidden;
        }
    }
}
