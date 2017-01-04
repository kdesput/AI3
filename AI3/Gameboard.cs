using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;

namespace AI3
{
    public class Gameboard
    {
        public int[,] array;
        Brush color1 = Brushes.Blue;
        Brush color2 = Brushes.Red;
        bool draw; //should I draw this window?
        public MainWindow window;
        public Gameboard(MainWindow window, bool draw, int[,] array = null)
        {
            this.window = window;
            this.draw = draw;
            if(draw)
            {
                window.board.Children.Clear();
                //draw vertical lines
                for (int i = 0; i < 8; i++)
                {
                    Line line = new Line();
                    line.X1 = i * 32;
                    line.X2 = i * 32;
                    line.Y1 = 0;
                    line.Y2 = 256;
                    line.Stroke = Brushes.LightGray;
                    line.StrokeThickness = 1;
                    window.board.Children.Add(line);
                }
                //draw horizontal lines
                for (int i = 0; i < 8; i++)
                {
                    Line line = new Line();
                    line.X1 = 0;
                    line.X2 = 256;
                    line.Y1 = i * 32;
                    line.Y2 = i * 32;
                    line.Stroke = Brushes.LightGray;
                    line.StrokeThickness = 1;
                    window.board.Children.Add(line);
                }
            }
            if (array == null)
            {
                this.array = new int[8, 8];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        this.array[i, j] = 0;
                    }
                }
                Put(3, 3, -1, true);
                Put(4, 4, -1, true);
                Put(3, 4, 1, true);
                Put(4, 3, 1, true);
            }
            else this.array = (int[,])array.Clone();
                
        }

        public void Put(int x, int y, int color, bool takeFields)
        {
            Brush colorBrush;
            switch(color)
            {
                case -1:
                    colorBrush = color1;
                    break;
                case 1:
                    colorBrush = color2;
                    break;
                default:
                    return;
            }
            array[x, y] = color;

            //Draw the new ellipse
            if (draw)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Width = 32;
                ellipse.Height = 32;
                ellipse.Fill = colorBrush;
                ellipse.Margin = new System.Windows.Thickness(x * 32, y * 32, 0, 0);
                window.board.Children.Add(ellipse);
            }

            //Take opponent's fields
            if (takeFields)
            {
                bool isOpponent;
                bool isThereAnyToTake = false;
                isOpponent = false;
                //LEFT
                for (int i = x - 1; i >= 0; i--)
                {
                    if (array[i, y] == 0) i = -1; //empty
                    else if (array[i, y] == -1 * color) isOpponent = true;
                    else if (isOpponent && array[i, y] == color) isThereAnyToTake = true;
                    else if (array[i, y] == color) i = -1;
                }

                if (isThereAnyToTake)
                {
                    int i = x - 1;
                    while (array[i, y] == -1 * color && i >= 0)
                    {
                        Put(i, y, color, false);
                        i--;
                    }
                }
                //RIGHT
                isOpponent = false;
                isThereAnyToTake = false;
                for (int i = x + 1; i < 8; i++)
                {
                    if (array[i, y] == 0) i = 8; //empty
                    else if (array[i, y] == -1 * color) isOpponent = true;
                    else if (isOpponent && array[i, y] == color) isThereAnyToTake = true;
                    else if (array[i, y] == color) i = 8;
                }

                if (isThereAnyToTake)
                {
                    int i = x + 1;
                    while (array[i, y] == -1 * color && i < 8)
                    {
                        Put(i, y, color, false);
                        i++;
                    }
                }
                //UP
                isThereAnyToTake = false;
                isOpponent = false;
                for (int i = y - 1; i >= 0; i--)
                {
                    if (array[x, i] == 0) i = -1; //empty
                    else if (array[x, i] == -1 * color) isOpponent = true;
                    else if (isOpponent && array[x, i] == color) isThereAnyToTake = true;
                    else if (array[x, i] == color) i = -1;
                }

                if (isThereAnyToTake)
                {
                    int i = y - 1;
                    while (array[x, i] == -1 * color && i >= 0)
                    {
                        Put(x, i, color, false);
                        i--;
                    }
                }

                //DOWN
                isOpponent = false;
                isThereAnyToTake = false;
                for (int i = y + 1; i < 8; i++)
                {
                    if (array[x, i] == 0) i = 8; //empty
                    else if (array[x, i] == -1 * color) isOpponent = true;
                    else if (isOpponent && array[x, i] == color) isThereAnyToTake = true;
                    else if (array[x, i] == color) i = 8;
                }
                if (isThereAnyToTake)
                {
                    int i = y + 1;
                    while (array[x, i] == -1 * color && i < 8)
                    {
                        Put(x, i, color, false);
                        i++;
                    }
                }
                //UP LEFT
                isOpponent = false;
                isThereAnyToTake = false;
                for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
                {
                    if (array[i, j] == 0) i = -1; //empty
                    else if (array[i, j] == -1 * color) isOpponent = true;
                    else if (isOpponent && array[i, j] == color) isThereAnyToTake = true;
                    else if (array[i, j] == color) i = -1;
                }

                if (isThereAnyToTake)
                {
                    int i = x - 1;
                    int j = y - 1;
                    while (array[i, j] == -1 * color && i >= 0 && j >= 0)
                    {
                        Put(i, j, color, false);
                        i--;
                        j--;
                    }
                }
                //DOWN LEFT
                isOpponent = false;
                isThereAnyToTake = false;
                for (int i = x - 1, j = y + 1; i >= 0 && j < 8; i--, j++)
                {
                    if (array[i, j] == 0) i = -1; //empty
                    else if (array[i, j] == -1 * color) isOpponent = true;
                    else if (isOpponent && array[i, j] == color) isThereAnyToTake = true;
                    else if (array[i, j] == color) i = -1;
                }
                if (isThereAnyToTake)
                {
                    int i = x - 1;
                    int j = y + 1;
                    while (array[i, j] == -1 * color && i >= 0 && j < 8)
                    {
                        Put(i, j, color, false);
                        i--;
                        j++;
                    }
                }
                //DOWN RIGHT
                isOpponent = false;
                isThereAnyToTake = false;
                for (int i = x + 1, j = y + 1; i < 8 && j < 8; i++, j++)
                {
                    if (array[i, j] == 0) i = 8; //empty
                    else if (array[i, j] == -1 * color) isOpponent = true;
                    else if (isOpponent && array[i, j] == color) isThereAnyToTake = true;
                    else if (array[i, j] == color) i = 8;
                }
                if (isThereAnyToTake)
                {
                    int i = x + 1;
                    int j = y + 1;
                    while (array[i, j] == -1 * color && i < 8 && j < 8)
                    {
                        Put(i, j, color, false);
                        i++;
                        j++;
                    }
                }
                //UP RIGHT
                isOpponent = false;
                isThereAnyToTake = false;
                for (int i = x + 1, j = y - 1; i < 8 && j >= 0; i++, j--)
                {
                    if (array[i, j] == 0) i = 8; //empty
                    else if (array[i, j] == -1 * color) isOpponent = true;
                    else if (isOpponent && array[i, j] == color) isThereAnyToTake = true;
                    else if (array[i, j] == color) i = 8;
                }
                if (isThereAnyToTake)
                {
                    int i = x + 1;
                    int j = y - 1;
                    while (array[i, j] == -1 * color && i < 8 && j >= 0)
                    {
                        Put(i, j, color, false);
                        i++;
                        j--;
                    }
                }
            }
        }

        public bool IsPossible(int x, int y, int color)
        {
            if (array[x, y] != 0) return false; //THIS FIELD IS NOT EMPTY
            bool possible = false;
            bool isOpponent;
            isOpponent = false;
            //LEFT
            for(int i = x - 1; i >= 0; i--)
            {
                if (array[i, y] == 0) i = -1; //empty
                else if (array[i, y] == -1 * color) isOpponent = true;
                else if(isOpponent && array[i,y] == color) return true;
                else if (array[i, y] == color) i = -1;
            }
            //RIGHT
            isOpponent = false;
            for (int i = x + 1; i < 8; i++)
            {
                if (array[i, y] == 0) i = 8; //empty
                else if (array[i, y] == -1 * color) isOpponent = true;
                else if (isOpponent && array[i, y] == color) return true;
                else if (array[i, y] == color) i = 8;
            }
            //UP
            isOpponent = false;
            for (int i = y - 1; i >= 0; i--)
            {
                if (array[x, i] == 0) i = -1; //empty
                else if (array[x, i] == -1 * color) isOpponent = true;
                else if (isOpponent && array[x, i] == color) return true;
                else if (array[x, i] == color) i = -1;
            }
            //DOWN
            isOpponent = false;
            for (int i = y + 1; i < 8; i++)
            {
                if (array[x, i] == 0) i = 8; //empty
                else if (array[x, i] == -1 * color) isOpponent = true;
                else if (isOpponent && array[x, i] == color) return true;
                else if (array[x, i] == color) i = 8;
            }
            //UP LEFT
            isOpponent = false;
            for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (array[i, j] == 0) i = -1; //empty
                else if (array[i, j] == -1 * color) isOpponent = true;
                else if (isOpponent && array[i, j] == color) return true;
                else if (array[i, j] == color) i = -1;
            }
            //DOWN LEFT
            isOpponent = false;
            for (int i = x - 1, j = y + 1; i >= 0 && j < 8; i--, j++)
            {
                if (array[i, j] == 0) i = -1; //empty
                else if (array[i, j] == -1 * color) isOpponent = true;
                else if (isOpponent && array[i, j] == color) return true;
                else if (array[i, j] == color) i = -1;
            }
            //DOWN RIGHT
            isOpponent = false;
            for (int i = x + 1, j = y + 1; i < 8 && j < 8; i++, j++)
            {
                if (array[i, j] == 0) i = 8; //empty
                else if (array[i, j] == -1 * color) isOpponent = true;
                else if (isOpponent && array[i, j] == color) return true;
                else if (array[i, j] == color) i = 8;
            }
            //UP RIGHT
            isOpponent = false;
            for (int i = x + 1, j = y - 1; i < 8 && j >= 0; i++, j--)
            {
                if (array[i, j] == 0) i = 8; //empty
                else if (array[i, j] == -1 * color) isOpponent = true;
                else if (isOpponent && array[i, j] == color) return true;
                else if (array[i, j] == color) i = 8;
            }
            return possible;
        }

        public int GetWinner()
        {
            int winner;
            bool isFull = true;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (IsPossible(i, j, -1) || IsPossible(i, j, 1)) isFull = false;
            if (isFull)
            {
                int count1 = 0, count2 = 0;
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        if (array[i, j] == -1) count1++;
                        else count2++;
                    }
                if (count1 > count2) winner = -1;
                else winner = 1;
            }
            else winner = 0;
            return winner;
        }

    }
}
