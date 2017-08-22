using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoMansEngine
{
    class Starsystem
    {
        private int[] seed;
        private Render render;
        public string name;
        public Planet[] planets;
        public int howManyPlanets;
        public string map;
        public int gWidth;
        public Sprite smallImg;

        public Starsystem(int[] seed, string name)
        {
            this.seed = seed;
            this.name = name;

            howManyPlanets = seed.Length;
            planets = new Planet[howManyPlanets];
            Random rand = new Random();

            for (int i = 0; i < howManyPlanets; i++)
            {
                planets[i] = new Planet(seed[i]*200, rand.Next(0,10), seed, i);
                planets[i].setImg(new string[]
                {
                    "     ▓▓▓▓▓▓▓▓     ",
                    "   ▓▓▓▓▓▓▓▓▓▓▓▓   ",
                    " ▓▓▓▓▓▓▓▓▓▓███▓▓▓ ",
                    " ▓▓▓▓▓▓▓▓▓█████▓▓ ",
                    "▓▓▓▓▓▓▓▓▓▓▓███▓▓▓▓",
                    "▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓",
                    " ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ",
                    " ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ",
                    "   ▓▓▓▓▓▓▓▓▓▓▓▓   ",
                    "     ▓▓▓▓▓▓▓▓     "
                });
            }

            gWidth = Convert.ToInt32(planets[planets.Length - 1].x);

            smallImg = new Sprite();
            smallImg.setImg(new string[]
            {
                " ▓▓▓ ",
                "▓▓█▓▓",
                " ▓▓▓ "
            });
        }

        public void show(Render render)
        {
            for (int i = 0; i < howManyPlanets; i++)
            {
                planets[i].show(render);
            }
        }
    }
}