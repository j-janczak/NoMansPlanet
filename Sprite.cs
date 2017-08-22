using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoMansEngine
{
    class Sprite
    {
        public double x, y;
        protected int width, height;
        public string[] img;

        public Sprite() { }

        public Sprite(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void setImg(string[] img)
        {
            height = img.Length;
            width = img[0].Length;
            this.img = img;
        }

        public int getWidth() { return width; }
        public int getHeight() { return height; }

        public void show(Render render) { render.draw(img, Convert.ToInt32(x), Convert.ToInt32(y)); }

        public bool overlaps(Sprite sprite)
        {
            if ((x >= sprite.x && x < sprite.x + sprite.width) && (y >= sprite.y && y < sprite.y + sprite.height)) return true;
            else return false;
        }
    }
}
