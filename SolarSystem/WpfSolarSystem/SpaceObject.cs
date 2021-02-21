using System;
using System.Collections.Generic;

namespace SpaceSim
{
    public class SpaceObject
    {
        public String name;
        public double orbitalRadius;
        public double orbitalPeriod;
        public double objectRadius;
        public double rotationPeriod;
        public bool isPlanet;
        public String color;

        public SpaceObject Parent;
        public List<SpaceObject> Children = new List<SpaceObject>();

        public SpaceObject(String name, double orbitalRadius, double orbitalPeriod, double objectRadius, double rotationPeriod, String color)
        {
            this.name = name;
            this.orbitalRadius = orbitalRadius;
            this.orbitalPeriod = orbitalPeriod;
            this.objectRadius = objectRadius;
            this.rotationPeriod = rotationPeriod;
            this.color = color;
        }
        public virtual void Draw()
        {
            Console.WriteLine(name);

            if (Parent != null)
            {
                Console.Write("Orbital Radius: " + orbitalRadius + "*10^6 km ");
                Console.WriteLine("around the " + Parent.name);
                Console.WriteLine("Orbital Period: " + orbitalPeriod + " days");
                Console.WriteLine("Rotation Period: " + rotationPeriod + " days");
            }

            Console.WriteLine("Object Radius: " + objectRadius + " km");
        }

        public void setParent(SpaceObject parent)
        {
            this.Parent = parent;
        }

        public void setChild(SpaceObject child)
        {
            this.Children.Add(child);
        }


        //Calculator method
        public Tuple<double, double> calculatePos(double time)
        {
            double x;
            double y;

            if (orbitalRadius != 0)
            {
                double rad = 2 * Math.PI * (time / orbitalPeriod);

                x = orbitalRadius * Math.Cos(rad);
                y = orbitalRadius * Math.Sin(rad);

                if (Parent != null)
                {
                    Tuple<double, double> parent = Parent.calculatePos(time);
                    x += parent.Item1;
                    y += parent.Item2;
                }
            }
            else
            {
                x = 0;
                y = 0;
            }
            return Tuple.Create(x, y);
        }
    }
    public class Star : SpaceObject
    {
        public Star(String name, double orbitalRadius, double orbitalPeriod, double objectRadius, double rotationPeriod, String color) : base(name, orbitalRadius, orbitalPeriod, objectRadius, rotationPeriod, color) { }
        public override void Draw()
        {
            Console.Write("Star : ");
            base.Draw();
        }
    }

    public class Planet : SpaceObject
    {
        public Planet(String name, double orbitalRadius, double orbitalPeriod, double objectRadius, double rotationPeriod, String color) : base(name, orbitalRadius, orbitalPeriod, objectRadius, rotationPeriod, color)
        {
            isPlanet = true;
        }

        public override void Draw()
        {
            Console.Write("Planet: ");
            base.Draw();
        }
    }

    public class Moon : SpaceObject
    {
        public Moon(String name, double orbitalRadius, double orbitalPeriod, double objectRadius, double rotationPeriod, String color) : base(name, orbitalRadius, orbitalPeriod, objectRadius, rotationPeriod, color) { }

        public override void Draw()
        {
            Console.Write("Moon : ");
            base.Draw();
        }
    }

    public class Comet : SpaceObject
    {
        public Comet(String name, double orbitalRadius, double orbitalPeriod, double objectRadius, double rotationPeriod, String color) : base(name, orbitalRadius, orbitalPeriod, objectRadius, rotationPeriod, color) { }

        public override void Draw()
        {
            Console.Write("Comet : ");
            base.Draw();

        }

    }

    public class Asteroid : SpaceObject
    {
        public Asteroid(String name, double orbitalRadius, double orbitalPeriod, double objectRadius, double rotationPeriod, String color) : base(name, orbitalRadius, orbitalPeriod, objectRadius, rotationPeriod, color) { }

        public override void Draw()
        {
            Console.Write("Asteroid: ");
            base.Draw();
        }
    }

    public class Dwarfplanet : SpaceObject
    {
        public Dwarfplanet(String name, double orbitalRadius, double orbitalPeriod, double objectRadius, double rotationPeriod, String color) : base(name, orbitalRadius, orbitalPeriod, objectRadius, rotationPeriod, color) { }

        public override void Draw()
        {
            Console.Write("Dwarf planet: ");
            base.Draw();
        }
    }
}