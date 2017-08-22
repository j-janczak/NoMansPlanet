using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoMansEngine
{
    class Render
    {
        private int width, height, dashBoardHeight, startX, startY;
        public RenderItem[,] renderItem;
        public char[,] dashBoardItem;
        public bool dashBoardMap = false;
        private Hero hero;
        private string[] buffer;
        private string[] bufferBoard;

        public Render(int width, int height, int startX, int startY)
        {
            this.width = width;
            this.height = height;
            buffer = new string[height];

            this.startX = startX;
            this.startY = startY;

            dashBoardHeight = 8;

            bufferBoard = new string[dashBoardHeight];

            dashBoardItem = new char[width, dashBoardHeight];
        }

        public void connectWith(RenderItem[,] renderItem) { this.renderItem = renderItem; }
        public void connectCameraWith(Hero hero) { this.hero = hero; }

        public void drawOnConsole(string text, int x, int y, ConsoleColor color)
        {
            ConsoleColor oldFore = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(text);
            Console.ForegroundColor = oldFore;
        }

        public void drawOnConsole(string text, int x, int y)
        {
            Console.SetCursorPosition(x + startX, y + startY);
            Console.Write(text);
        }

        public void drawBorder()
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int y = 0; y <= height + 10; y++)
            {
                Console.SetCursorPosition(startX, startY + y);
                for (int x = 0; x <= width + 1; x++) Console.Write(Convert.ToChar(9608));
            }

            for (int y = 0; y <= height - 1; y++)
            {
                Console.SetCursorPosition(startX + 1, startY + y + 1);
                for (int x = 0; x <= width - 1; x++) Console.Write(' ');
            }

            for (int y = 0; y <= 7; y++)
            {
                Console.SetCursorPosition(startX + 1, startY + height + y + 2);
                for (int x = 0; x <= width - 1; x++) Console.Write(' ');
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void draw(string img, int x, int y)
        {
            if (!(y - height / 2 + height / 2 >= height))
            {
                if (y - height / 2 + height / 2 >= 0)
                {
                    for (int i = 0; i < img.Length; i++)
                    {
                        if (x + i - hero.x + width / 2 >= width) break;
                        if (x + i - hero.x + width / 2 >= 0)
                        {
                            if (img[i] == ' ') continue;

                            if (Convert.ToInt32(x + i - hero.x + width / 2) >= width) break;
                            renderItem[Convert.ToInt32(x + i - hero.x + width / 2), y].mark = img[i];
                        }
                    }
                }
            }
        }

        public void draw(string[] img, int imgX, int imgY)
        {
            if (imgY + img.Length < 0) return;
            if (imgY >= height) return;

            if (imgX + img[0].Length < hero.getIntX() - width / 2) return;
            if (imgX > hero.getIntX() + width / 2) return;

            for (int y = 0; y < img.Length; y++)
            {
                if (imgY + y >= height) break;
                if (imgY + y < 0) continue;

                if (imgX < hero.getIntX() - width / 2)
                {
                    for (int x = Math.Abs((hero.getIntX() - width / 2) - imgX); x < img[y].Length; x++)
                    {
                        if (imgX + x - hero.getIntX() + width / 2 >= width) break;
                        if (imgX + x - hero.getIntX() + width / 2 < 0) continue;
                        if (img[y][x] == ' ') continue;

                        if (imgX + x - hero.getIntX() + width / 2 >= width) break;
                        renderItem[imgX + x - hero.getIntX() + width / 2, imgY + y].mark = img[y][x];
                    }
                }
                else
                {
                    for (int x = 0; x < img[y].Length; x++)
                    {
                        if (imgX + x - hero.getIntX() + width / 2 >= width) break;
                        if (imgX + x - hero.getIntX() + width / 2 < 0) continue;
                        if (img[y][x] == ' ') continue;

                        if (imgX + x - hero.getIntX() + width / 2 >= width) break;
                        renderItem[imgX + x - hero.getIntX() + width / 2, imgY + y].mark = img[y][x];
                    }
                }
            }
        }

        public void render()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    buffer[y] += renderItem[x, y].mark;
                }
            }
            for (int y = 0; y < height; y++)
            {
                Console.SetCursorPosition(startX + 1, startY + y + 1);
                Console.Write(buffer[y]);
            }
            for (int y = 0; y < height; y++) buffer[y] = "";
        }

        public void clear() { for (int y = 0; y < height; y++) for (int x = 0; x <= width - 1; x++) renderItem[x, y].mark = ' '; }

        public void drawOnDashboard(string text, int x, int y)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (i + x > width - 1) break;
                dashBoardItem[i + x, y] = text[i];
            }
        }

        public void drawOnDashboard(string[] text, int x, int y)
        {
            for (int iy = 0; iy < text.Length; iy++)
            {
                for (int ix = 0; ix < text[0].Length; ix++)
                {
                    dashBoardItem[ix + x, iy + y] = text[iy][ix];
                }
            }
        }

        public void renderDashboard()
        {
            for (int y = 0; y < 8; y++)
            {
                Console.SetCursorPosition(startX + 1, startY + height + 2 + y);
                for (int x = 0; x < width; x++)
                {
                    bufferBoard[y] += dashBoardItem[x, y];
                }
            }
            for (int y = 0; y < 8; y++)
            {
                Console.SetCursorPosition(startX + 1, startY + height + 2 + y);
                Console.Write(bufferBoard[y]);
            }
            for (int y = 0; y < 8; y++) bufferBoard[y] = "";
        }

        public void clearDashboard() { for (int y = 0; y < 8; y++) for (int x = 0; x < width; x++) dashBoardItem[x, y] = ' '; }
    }
}