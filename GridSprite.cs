using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoMansEngine
{
    class GridSprite
    {
        public char[,] point;
        private int width, height;

        public GridSprite(int width, int height)
        {
            point = new char[width, height];
            this.width = width;
            this.height = height;

            for (int y = 0; y < 20; y++) for (int x = 0; x < width; x++) point[x, y] = ' ';
        }

        public string[] convertToStringArray()
        {
            string[] buffer = new string[20];

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < width; x++) buffer[y] += point[x, y]; 
            }

            return buffer;
        }

        public void draw(string[] img, int imgX, int imgY)
        {
            int ximg = 0, yimg = 0;
            for (int y = imgY; y < imgY + img.Length; y++)
            {
                ximg = 0;
                for (int x = imgX; x < imgX + img[0].Length; x++)
                {
                    point[x, y] = img[yimg][ximg];
                    ximg++;
                }
                yimg++;
            }
        }
    }
}
