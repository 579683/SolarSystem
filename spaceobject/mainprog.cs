using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SpaceSim;
using System.Collections;

class Astronomy
{
    public static void Main()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SpaceObject.txt");
        string[] lines = System.IO.File.ReadAllLines(path);

        string[][] array = lines.Select(x => x.Split(' ').ToArray()).ToArray();

        List<SpaceObject> solarSystem = new List<SpaceObject>();

        foreach(string[] line in array)
        {
            string obj = line[0];
            string name = line[1];
            int orbRad = Convert.ToInt32(line[2]);
            int orbPeriod = Convert.ToInt32(line[3]);
            int objRad = Convert.ToInt32(line[4]);
            double RotPeriod = Convert.ToDouble(line[5]);
            string color = line[6];

            switch(obj)
            {
                case "Star":
                    solarSystem.Add(new Star(name, orbRad, orbPeriod, objRad, RotPeriod, color));
                    break;
                case "Planet":
                    solarSystem.Add(new Planet(name, orbRad, orbPeriod, objRad, RotPeriod, color));
                    break;
                case "Moon":
                    solarSystem.Add(new Moon(name, orbRad, orbPeriod, objRad, RotPeriod, color));
                    break;
            }
        }

        foreach(SpaceObject obj in solarSystem)
        {
            switch(obj.name)
            {
                case "Sun":
                    break;
                case "The moon":
                    obj.Parent = solarSystem[3];
                    break;
                default:
                    obj.Parent = solarSystem[0];
                    break;
            }
        }
        Console.Write("Time: ");
        int time = Convert.ToInt32(Console.ReadLine());
        Console.Write("Planet: ");
        string planet = Console.ReadLine();

        foreach(SpaceObject obj in solarSystem)
        {
            if (planet == "" && obj.name.Equals("Sun") || obj.Parent.name.Equals("Sun"))
            {
                obj.Draw();
                Console.WriteLine(obj.CalculatePos(time) + "\n");
            }
            else if (obj.name.ToLower().Equals(planet.ToLower()) || (obj.Parent != null && obj.Parent.name.ToLower().Equals(planet.ToLower()))) {
                obj.Draw();
                Console.WriteLine(obj.CalculatePos(time) + "\n");
            }
        }
        Console.ReadLine();
    }
}