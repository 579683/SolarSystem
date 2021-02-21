using System;
using System.Collections.Generic;
using SpaceSim;

class Program
{
    public static void Main(string[] args)
    {

        List<SpaceObject> solarSystem = new List<SpaceObject>();

        {
            Star Sun = new Star("Sun", 0, 0, 696340, 0, "YELLOW");
            Planet Mercury = new Planet("Mercury", 57.9, 88, 2440, 59, "YELLOW");
            Planet Venus = new Planet("Venus", 108.2, 224.7, 6052, 243, "YELLOW");
            Planet Earth = new Planet("Earth", 149.6, 365.26, 6371, 1, "YELLOW");
            Planet Mars = new Planet("Mars", 227.9, 686.98, 3402, 1.025, "YELLOW");

            Moon TheMoon = new Moon("Luna aka The Moon", 0.384, 27.322, 1737.1, 27, "YELLOW");
            Moon Phobos = new Moon("Phobos", 0.000009, 0.3189, 11.25, 0.3, "YELLOW");


            Mercury.setParent(Sun);
            Venus.setParent(Sun);
            Earth.setParent(Sun);
            Mars.setParent(Sun);

            TheMoon.setParent(Earth);
            Phobos.setParent(Mars);

            Earth.setChild(TheMoon);
            Mars.setChild(Phobos);

            solarSystem.Add(Sun);
            solarSystem.Add(Mercury);
            solarSystem.Add(Venus);
            solarSystem.Add(Earth);
            solarSystem.Add(Mars);

            solarSystem.Add(TheMoon);
            solarSystem.Add(Phobos);
        };

        Console.WriteLine("Enter number of days");
        String Stime = Console.ReadLine();
        int time = Convert.ToInt32(Stime);

        bool match = false;

        Console.WriteLine("Write the name of one of these planets: Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptun.");
        String answer = Console.ReadLine();

        foreach (SpaceObject obj in solarSystem)
        {
            if (obj.name.Equals(answer))
            {
                match = true;
                obj.Draw();

                if (obj.Parent != null)
                {
                    var temp = obj.getPosition(time);
                    double x = temp.Item1;
                    double y = temp.Item2;

                    Console.WriteLine(obj.name + "'s position relative to the sun after " + time + " days in x and y direction: \n" + x + " km*10^6" + " and " + y + " km*10^6");
                    Console.WriteLine();
                }

                int storrelse = obj.Children.Count;
                if (storrelse > 0)
                {
                    Console.WriteLine("Moons:");
                    Console.WriteLine();
                    foreach (SpaceObject child in obj.Children)
                    {
                        child.Draw();

                        var temp = child.getPosition(time);
                        double x = temp.Item1;
                        double y = temp.Item2;

                        Console.WriteLine(child.name + "'s position relative to the sun after " + time + " days in x and y direction: \n " + x + " km*10^6" + " and " + y + " km*10^6");
                        Console.WriteLine();
                    }
                }
            }

        }
        if (!match)
        {
            Console.WriteLine("You did not type a planet.");
            Console.WriteLine();
            solarSystem[0].Draw();
            Console.WriteLine();
            foreach (SpaceObject planet in solarSystem)
            {
                if (planet.isPlanet)
                {
                    planet.Draw();
                    Console.WriteLine();
                }
            }
        }
        Console.ReadLine();
    }
}