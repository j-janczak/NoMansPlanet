using System;
using System.Timers;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoMansEngine
{
    class GAME
    {
        public int width = 60;
        public int height = 20;

        public int startX = 2;
        public int startY = 1;

        public int planetSize = 20000;

        public int gameTime;

        private int backPos = 0;
        private int currentStarSystem = 0;
        private int selectedStarSystem;
        private int currentPlanet = 0;

        int planetSeed;

        private string gameStage = "space";

        public string key;

        Textures textures;
        Render render;
        Hero hero;
        RenderItem[,] renderItem;
        Starsystem[] starsystem;
        Npc[] npc;
        Stopwatch sw = new Stopwatch();
        Random random = new Random();
        Sprite planetTerrain = new Sprite();
        GridSprite planetGrid;

        public void start()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WindowHeight = 33;

            Console.Title = "No Man's Planet";
            Console.CursorVisible = false;

            textures = new Textures();
            render = new Render(width, height, startX, startY);
            hero = new Hero();
            npc = new Npc[5];
            renderItem = new RenderItem[width, height];
            planetGrid = new GridSprite(planetSize, 20);

            for (int y = 0; y <= height - 1; y++) for (int x = 0; x <= width - 1; x++) renderItem[x, y] = new RenderItem();
            for (int i = 0; i < 5; i++) npc[i] = new Npc(hero);

            starsystem = new Starsystem[3];
            starsystem[0] = new Starsystem(new int[] { 1, 5, 12, 19, 28, 45, 68 }, "TRAPPIST-1");
            starsystem[0].map = "Oo--o-----o-----o------o-------------o-------------------o";
            starsystem[0].planets[0].name = "b";
            starsystem[0].planets[1].name = "c";
            starsystem[0].planets[2].name = "d";
            starsystem[0].planets[3].name = "e";
            starsystem[0].planets[4].name = "f";
            starsystem[0].planets[5].name = "g";
            starsystem[0].planets[6].name = "h";

            starsystem[1] = new Starsystem(new int[] { 12, 23, 30, 42 }, "Cervantes");
            starsystem[1].map = "O---------------o--------------o--------o----------------o";
            starsystem[1].planets[0].name = "Dulcinea";
            starsystem[1].planets[1].name = "Rocinante";
            starsystem[1].planets[2].name = "Quijote";
            starsystem[1].planets[3].name = "Sancho";

            starsystem[2] = new Starsystem(new int[] { 12, 23, 30, 42 }, "Copernicus");
            starsystem[2].map = "O---------------o--------------o--------o----------------o";
            starsystem[2].planets[0].name = "Dulcinea";
            starsystem[2].planets[1].name = "Rocinante";
            starsystem[2].planets[2].name = "Quijote";
            starsystem[2].planets[3].name = "Sancho";

            render.connectWith(renderItem);
            render.connectCameraWith(hero);

            afterClear();

            hero.x = 200;
            hero.y = 10;
            backPos = Convert.ToInt32(hero.x - width / 2);

            selectedStarSystem = currentStarSystem;

            while (true)
            {
                gameTime = Convert.ToInt32(9.0 - sw.ElapsedMilliseconds);
                if (gameTime > 1) System.Threading.Thread.Sleep(gameTime);
                //Console.Write(gameTime+ "    ");
                sw.Reset();
                sw.Start();
                switch (gameStage)
                {
                    case "space":
                        if (hero.hyperdriveStage != 1) render.clear();
                        switch (hero.hyperdriveStage)
                        {
                            case 0:
                                render.draw(textures.spaceImg.img, backPos - width, 0);
                                render.draw(textures.spaceImg.img, backPos, 0);
                                render.draw(textures.spaceImg.img, backPos + width, 0);
                                textures.star.show(render);
                                starsystem[currentStarSystem].show(render);
                                for (int i = 0; i < npc.Length; i++) npc[i].show(render);
                                break; 
                            case 1:
                                render.draw(textures.spaceImg.img, backPos - width, 0);
                                render.draw(textures.spaceImg.img, backPos, 0);
                                render.draw(textures.spaceImg.img, backPos + width, 0);
                                backPos -= 1;
                                textures.hyperdriveLoop.x -= 2;
                                if (textures.hyperdriveLoop.x < hero.x - width / 2) hero.hyperdriveStage = 2;
                                break;
                            case 2:
                                textures.hyperdriveFlash.x = hero.x - width / 2;
                                textures.hyperdriveFlash.show(render);
                                textures.loop++;
                                if (textures.loop == 10)
                                {
                                    hero.hyperdriveStage = 3;
                                    textures.loop = 0;
                                }
                                break;
                            case 3:
                                render.draw(textures.hyperdriveLoop.img, backPos - width, 0);
                                render.draw(textures.hyperdriveLoop.img, backPos, 0);
                                render.draw(textures.hyperdriveLoop.img, backPos + width, 0);
                                textures.loop++;
                                backPos -= 2;
                                if (textures.loop == 250)
                                {
                                    hero.hyperdriveStage = 4;
                                    textures.loop = 0;
                                    currentStarSystem = selectedStarSystem;
                                }
                                break;
                            case 4:
                                textures.hyperdriveFlash.x = hero.x - width / 2;
                                textures.hyperdriveFlash.show(render);
                                textures.loop++;
                                if (textures.loop == 10)
                                {
                                    hero.hyperdriveStage = 0;
                                    textures.loop = 0;
                                }
                                break;
                        }
                        render.draw(hero.img.ToString(), Convert.ToInt32(hero.x), Convert.ToInt32(hero.y));
                        hero.calcForce();
                        render.render();

                        render.clearDashboard();
                        {
                            if (!render.dashBoardMap && hero.hyperdriveStage != 3)
                            {
                                if (((hero.x / starsystem[currentStarSystem].gWidth) * 56.5 + 1 < 58) && ((hero.x / starsystem[currentStarSystem].gWidth) * 56.5 + 1 > 1)) render.drawOnDashboard("▼", Convert.ToInt32((hero.x / starsystem[currentStarSystem].gWidth) * 56.5) + 1, 0);
                                render.drawOnDashboard(starsystem[currentStarSystem].map, 1, 1);
                                if (((hero.x / starsystem[currentStarSystem].gWidth) * 56.5 + 1 < 58) && ((hero.x / starsystem[currentStarSystem].gWidth) * 56.5 + 1 > 1)) render.drawOnDashboard("▲", Convert.ToInt32((hero.x / starsystem[currentStarSystem].gWidth) * 56.5) + 1, 2);
                                render.drawOnDashboard("Planetary system: " + starsystem[currentStarSystem].name, 17, 3);
                                render.drawOnDashboard("Fuel: " + Convert.ToInt32(hero.fuel) + "% " + hero.errorTemp() + hero.errorFuel() + hero.errorBroken(), 1, 5);
                                for (int i = 0; i < starsystem[currentStarSystem].howManyPlanets; i++) { if (hero.overlaps(starsystem[currentStarSystem].planets[i])) render.drawOnDashboard("Above planet: \"" + starsystem[currentStarSystem].planets[i].name + "\"", 1, 7); }
                            }
                            else if (!render.dashBoardMap)
                            {
                                render.drawOnDashboard("O--------------------------------------------------------0", 1, 1);
                                render.drawOnDashboard("♦", Convert.ToInt32((textures.loop / 250.0) * 56.5) + 1, 1);
                                render.drawOnDashboard("Travel to: " + starsystem[selectedStarSystem].name, 17, 3);
                            }
                            else
                            {
                                if (selectedStarSystem == 0) render.drawOnDashboard(">" + starsystem[0].name + "<", 0, 5); else render.drawOnDashboard(starsystem[0].name, 1, 5);
                                if (selectedStarSystem == 1) render.drawOnDashboard(">" + starsystem[1].name + "<", 24, 5); else render.drawOnDashboard(starsystem[1].name, 25, 5);
                                if (selectedStarSystem == 2) render.drawOnDashboard(">" + starsystem[2].name + "<", 48, 5); else render.drawOnDashboard(starsystem[2].name, 49, 5);

                                render.drawOnDashboard(starsystem[0].smallImg.img, 3, 1);
                                render.drawOnDashboard(starsystem[1].smallImg.img, 27, 1);
                                render.drawOnDashboard(starsystem[2].smallImg.img, 52, 1);
                            }
                        }
                        render.renderDashboard();

                        if (hero.x <= 48) hero.broken = true;
                        if (hero.x - width * 1.5 >= backPos || hero.x + width * 0.5 <= backPos) backPos = Convert.ToInt32(hero.x - width / 2);

                        break;
                    case "enteringTheAtmosphere":
                        render.clear();
                        {
                            render.draw(textures.enterPlanetAnimation.img, Convert.ToInt32(hero.x - width / 2), backPos);
                            render.draw(planetTerrain.img, 0, backPos + 350);
                            if (backPos <= -330)
                            {
                                render.draw(hero.getSpaceCraftImg(), hero.getIntX() - hero.spaceCraftRight.getWidth() / 2, 8);
                                render.draw(textures.landingDust[-330 + Math.Abs(backPos)].img, hero.getIntX() - (textures.landingDust[-330 + Math.Abs(backPos)].img[0].Length / 2), 8 + hero.spaceCraftRight.img.Length);
                            }
                            else render.draw(hero.getSpaceCraftImg(), hero.getIntX() - (hero.spaceCraftRight.getWidth() / 2) + random.Next(-1, 2), 8 + random.Next(-1, 2));
                        }
                        render.render();

                        backPos--;
                        render.clearDashboard();
                        {
                            render.drawOnDashboard("◄--------------------------------------------------------►", 1, 1);
                            render.drawOnDashboard("♦", Convert.ToInt32((Math.Abs(backPos) / 350.0) * 56.5) + 1, 1);
                            render.drawOnDashboard("Entering to: " + starsystem[currentStarSystem].planets[currentPlanet].name, 17, 3);
                        }
                        Console.BackgroundColor = ConsoleColor.Black;
                        render.renderDashboard();

                        if (backPos <= -230) Console.BackgroundColor = ConsoleColor.Blue;
                        if (backPos <= -330) System.Threading.Thread.Sleep(30);
                        if (backPos <= -350)
                        {
                            gameStage = "inPlanet";
                        }

                        break;
                    case "inPlanet":
                        Console.BackgroundColor = ConsoleColor.Blue;
                        render.clear();
                        {
                            render.draw(hero.getSpaceCraftImg(), hero.getIntX() -(hero.spaceCraftRight.getWidth() / 2), hero.getIntY());
                            render.draw(planetTerrain.img, 0, 0);
                        }
                        render.render();

                        render.clearDashboard();
                        {
                            render.drawOnDashboard("◄--------------------------------------------------------►", 1, 1);
                            render.drawOnDashboard("♦", Convert.ToInt32((Math.Abs(hero.x) / 20000.0) * 56.5) + 1, 1);
                            render.drawOnDashboard("Welcome on: " + starsystem[currentStarSystem].planets[currentPlanet].name, 1, 3);
                            render.drawOnDashboard("------------------- Fuel: " + Convert.ToInt32(hero.fuel) + "% ", 1, 4);
                            render.drawOnDashboard("Oxygen: deficiency", 1, 5);
                            render.drawOnDashboard("Life forms: unknown", 1, 6);
                        }
                        Console.BackgroundColor = ConsoleColor.Black;
                        render.renderDashboard();

                        hero.calcForce();
                        break;
                }

                if (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true).Key.ToString();
                    if (!render.dashBoardMap && hero.hyperdriveStage == 0)
                    {
                        if (key == "A") hero.left();
                        if (key == "D") hero.right();
                        if (key == "W") hero.up();
                        if (key == "S") hero.down();
                        if (key == "F1") hero.toggleSpeed();
                        if (key == "Enter")
                        {
                            for (int p = 0; p < starsystem[currentStarSystem].howManyPlanets; p++)
                            {
                                if (hero.overlaps(starsystem[currentStarSystem].planets[p]))
                                {
                                    backPos = 0;
                                    hero.x = 0;
                                    hero.y = 8;
                                    currentPlanet = p;
                                    gameStage = "enteringTheAtmosphere";
                                    generatePlanet();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (key == "A") if (selectedStarSystem > 0) selectedStarSystem--;
                        if (key == "D") if (selectedStarSystem < 2) selectedStarSystem++;
                        if (key == "Enter")
                        {
                            textures.hyperdriveLoop.x = hero.x + width / 2;
                            render.dashBoardMap = false;
                            hero.hyperdriveStage = 1;
                        }
                    }

                    if (key == "F2")
                    {
                        if (render.dashBoardMap) render.dashBoardMap = false;
                        else
                        {
                            selectedStarSystem = currentStarSystem;
                            render.dashBoardMap = true;
                        }
                    }

                    if (key == "R")
                    {
                        Console.Clear();
                        afterClear();
                    }

                    key = "";
                }
                sw.Stop();
            }

            void afterClear()
            {
                render.drawBorder();

                render.drawOnConsole("No Man's Planet by TheFlashes", width + 3, 0);
                render.drawOnConsole("Click \"H\" to close this info", width + 3, 1);
                render.drawOnConsole("F1 - Pulse Engine", width + 3, 3);
                render.drawOnConsole("F2 - starsystem map", width + 3, 4);
                render.drawOnConsole("R - Reset screen", width + 3, 5);
                render.drawOnConsole("ENTER - Enter to planet", width + 3, 6);
            }

            void generatePlanet()
            {
                Random random = new Random(starsystem[currentStarSystem].planets[currentPlanet].seed);
                for (int i = 0; i < planetSize; i++)
                {
                    planetSeed = random.Next(2, 12);
                    if (i > 0)
                    {
                        while (planetGrid.point[i - 1, 20 - planetSeed + 1] == ' ') planetSeed--;
                        while (planetGrid.point[i - 1, 20 - planetSeed - 1] == '░') planetSeed++;
                    }
                    planetGrid.point[i, 20 - planetSeed] = '▓';
                    for (int ii = 20 - planetSeed + 1; ii < 20; ii++) planetGrid.point[i, ii] = '░';
                }
                for (int x = 2; x < planetSize; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (planetGrid.point[x, y] == '▓' && planetGrid.point[x - 1, y] == ' ' && planetGrid.point[x - 1, y + 1] == '▓' && planetGrid.point[x - 2, y] == '▓')
                        {
                            planetGrid.point[x - 1, y] = '▓';
                            planetGrid.point[x - 1, y + 1] = '░';
                        }
                        if (planetGrid.point[x, y] == ' ' && planetGrid.point[x - 1, y] == '▓' && planetGrid.point[x - 2, y] == ' ')
                        {
                            planetGrid.point[x - 1, y] = ' ';
                            planetGrid.point[x - 1, y + 1] = '▓';
                        }
                        if (planetGrid.point[x, y] == ' ' && planetGrid.point[x - 1, y] == '▓' && planetGrid.point[x - 2, y] == '▓' && planetGrid.point[x - 3, y] == ' ')
                        {
                            planetGrid.point[x - 1, y] = ' ';
                            planetGrid.point[x - 2, y] = ' ';
                            planetGrid.point[x - 1, y + 1] = '▓';
                            planetGrid.point[x - 2, y + 1] = '▓';
                        }
                        if (x > 3 && planetGrid.point[x, y] == '▓' && planetGrid.point[x - 1, y] == ' ' && planetGrid.point[x - 2, y] == ' ' && planetGrid.point[x - 3, y] == '▓')
                        {
                            planetGrid.point[x - 1, y] = '▓';
                            planetGrid.point[x - 1, y + 1] = '░';
                            planetGrid.point[x - 2, y] = '▓';
                            planetGrid.point[x - 2, y + 1] = '░';
                        }
                    }
                }
                for (int x = 0; x < planetSize; x++)
                {
                    planetSeed = random.Next(0, 1000);
                    if (planetSeed == 99)
                    {
                        int ybase = 0;
                        for (int yscreen = 0; yscreen < 20; yscreen++)
                        {
                            for (int xbase = 0; xbase < textures.spaceBase.img[0].Length; xbase++)
                            {
                                if (yscreen + textures.spaceBase.img.Length >= 20) break;
                                if (planetGrid.point[x + xbase, yscreen + textures.spaceBase.img.Length] == ' ')
                                {
                                    ybase++;
                                    break;
                                }
                            }
                        }
                        planetGrid.draw(textures.spaceBase.img, x, ybase);
                    }
                }
                planetTerrain.setImg(planetGrid.convertToStringArray());
                hero.x = planetSize / 2;
            }
        }
    }
}