using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoMansEngine
{
    class Planet : Sprite
    {
        public string name;
        public int seed;
        string sseed;

        public Planet(int x, int y, int[] seed, int planetNr)
        {
            this.x = x;
            this.y = y;
   
            for (int i = 0; i < seed.Length; i++) sseed = sseed + (seed[i] * seed[planetNr]).ToString();
            if (sseed.Length > 9) sseed = sseed.Substring(0, 9);
            this.seed = Int32.Parse(sseed);
        }
    }
}
