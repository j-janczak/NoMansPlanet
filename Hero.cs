using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoMansEngine
{
    class Hero : Sprite
    {
        private bool speed;
        public double fuel = 100.0;
        public char img = '♦';
        public bool broken = false;
        public int hyperdriveStage = 0;
        public string direction = "right";

        public double forceX = 0.0;

        public Sprite spaceCraftRight = new Sprite();
        public Sprite spaceCraftLeft = new Sprite();

        public Hero()
        {
            spaceCraftRight.setImg(new string[]
            {
                "▒▒►    ",
                "██████►",
                " ╦  ╦  "
            });

            spaceCraftLeft.setImg(new string[]
            {
                "    ◄▒▒",
                "◄██████",
                "  ╦  ╦ "
            });
        }

        public string[] getSpaceCraftImg()
        {
            if (direction == "right")
            {
                return spaceCraftRight.img;
            }
            else return spaceCraftLeft.img;
        }

        public void calcForce()
        {
            x += forceX;

            if (forceX > 0) forceX -= forceX / 12.0;
            if (forceX < 0) forceX -= forceX / 12.0;

            if (forceX > 0.00 && forceX < 0.1) forceX = 0.0;
            if (forceX < 0.00 && forceX > -0.1) forceX = 0.0;

        }

        public void up()
        {
            if (y > 0) y--;
        }

        public void down()
        {
            if (y < 19) y++;
        }

        public void left()
        {
            if (speed && fuelCalc())
            {
                if (forceX > - 5) forceX -= 1.0;
            }
            else if (forceX > - 1) forceX -= 0.5;
            direction = "left";
        }

        public void right()
        {
            if (speed && fuelCalc())
            {
                if (forceX < 5) forceX += 1.0;
            }
            else if (forceX < 1) forceX += 0.5;
            direction = "right";
        }

        public void toggleSpeed()
        {
            if (speed) speed = false;
            else speed = true;
        }

        public bool fuelCalc()
        {
            fuel -= 0.1;
            while (fuel < 0) fuel += 0.1;
            if (fuel > 1) return true;
            else return false;
        }

        public string errorTemp()
        {
            if (x <= 100) return "|OVERHEATING|";
            else return "";
        }

        public string errorFuel()
        {
            if (fuel < 1) return "|NO FUEL|";
            else return "";
        }

        public string errorBroken()
        {
            if (broken) return "|SPACECRAFT BROKEN|";
            else return "";
        }

        public int getIntX() { return Convert.ToInt32(x); }
        public int getIntY() { return Convert.ToInt32(y); }
    }
}
