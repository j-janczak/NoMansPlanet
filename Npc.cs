using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoMansEngine
{
    class Npc
    {
        public char img = '♦';
        public int x, y;
        public int direction;
        public int speed = 2;

        private int rand;

        Hero hero;
        Random random = new Random();

        public Npc(Hero hero)
        {
            this.hero = hero;
            respawn();
        }

        public void show(Render render)
        {
            switch (direction)
            {
                case 1:
                    x -= speed;
                    break;
                case 2:
                    x += speed;
                    break;
                case 3:
                    x -= speed;
                    y--;
                    break;
                case 4:
                    x += speed;
                    y++;
                    break;
            }

            if (x < hero.getIntX() - 300) respawn();
            if (x > hero.getIntX() + 300) respawn();
            if (y > hero.getIntY() + 300) respawn();
            if (y < hero.getIntY() - 300) respawn();

            render.draw(img.ToString(), x, y);
        }

        void respawn()
        {
            rand = random.Next(0, 2);
            if (rand == 0) x = hero.getIntX() - 31;
            if (rand == 1) x = hero.getIntX() + 31;

            y = random.Next(1, 18);

            direction = random.Next(1, 5);
        }
    }
}
