using System;

namespace SpaceSim
{
    public class SpaceObject {
        public string name { get; set; }
        protected double orbitalRadius;
        protected int orbitalPeriod;
        public int objRadius { get; }
        protected double rotationPeriod;
        public string objColor { get; }


        public SpaceObject Parent { get; set; }
        public SpaceObject(string name, double orbitalRadius, int orbitalPeriod, int objRadius, double rotationPeriod, string color) {
            this.name = name;
            this.orbitalRadius = orbitalRadius;
            this.orbitalPeriod = orbitalPeriod;
            this.objRadius = objRadius;
            this.rotationPeriod = rotationPeriod;
            this.objColor = color;
        }

        public virtual void Draw() {
            Console.WriteLine(name);
            Console.WriteLine("Orbital radius: " + orbitalRadius);
            Console.WriteLine("Orbital period: " + orbitalPeriod);
            Console.WriteLine("Object radius: " + objRadius);
            Console.WriteLine("Rotational period: " + rotationPeriod);
            Console.WriteLine("Object Color" + objColor);

            if (Parent != null)
                Console.WriteLine("Orbit around: " + Parent.name);
            else
                Console.WriteLine("Orbit around: Nothing");
        }

        public Tuple<double, double> CalculatePos(double time)
        {
            double x;
            double y;

            if(orbitalRadius != 0)
            {
                double radius = 2 * Math.PI * (time/orbitalPeriod);

                x = orbitalRadius * Math.Cos(radius);
                y = orbitalRadius * Math.Sin(radius);

                if(Parent != null)
                {
                    Tuple<double, double> par = Parent.CalculatePos(time);
                    x += par.Item1;
                    y += par.Item2;
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
    public class Star : SpaceObject {
        public Star(string name, double orbitalRadius, int orbitalPeriod, int objRadius, double rotationPeriod, string color) : base(name, orbitalRadius, orbitalPeriod, objRadius, rotationPeriod, color) { }
        public override void Draw() {
            Console.Write("Star : "); 
            base.Draw();
        }
    }
    public class Planet : SpaceObject {
        public Planet(string name, double orbitalRadius, int orbitalPeriod, int objRadius, double rotationPeriod, string color) : base(name, orbitalRadius, orbitalPeriod, objRadius, rotationPeriod, color) { }
        public override void Draw() {
            Console.Write("Planet : "); 
            base.Draw();
        }
    }
    public class Moon : SpaceObject {
        public Moon(string name, double orbitalRadius, int orbitalPeriod, int objRadius, double rotationPeriod, string color) : base(name, orbitalRadius, orbitalPeriod, objRadius, rotationPeriod, color) { }
        public override void Draw() {
            Console.Write("Moon : ");
            base.Draw();
        }
    }

    public class Asteroid : SpaceObject
    {
        public Asteroid(string name, double orbitalRadius, int orbitalPeriod, int objRadius, double rotationPeriod, string color) : base(name, orbitalRadius, orbitalPeriod, objRadius, rotationPeriod, color) { }

        public override void Draw()
        {
            Console.WriteLine("Asteroid : ");
            base.Draw();
        }
    }

    public class AsteroidBelt : SpaceObject
    {
        public AsteroidBelt(string name, double orbitalRadius, int orbitalPeriod, int objRadius, double rotationPeriod, string color) : base(name, orbitalRadius, orbitalPeriod, objRadius, rotationPeriod, color) { }

        public override void Draw()
        {
            Console.WriteLine("AsteroidBelt : ");
            base.Draw();
        }
    }

    public class Comet : SpaceObject
    {
        public Comet(string name, double orbitalRadius, int orbitalPeriod, int objRadius, double rotationPeriod, string color) : base(name, orbitalRadius, orbitalPeriod, objRadius, rotationPeriod, color) { }

        public override void Draw()
        {
            Console.WriteLine("Comet : ");
            base.Draw();
        }
    }

    public class DwarfPlanet : SpaceObject
    {
        public DwarfPlanet(string name, double orbitalRadius, int orbitalPeriod, int objRadius, double rotationPeriod, string color) : base(name, orbitalRadius, orbitalPeriod, objRadius, rotationPeriod, color) { }

        public override void Draw()
        {
            Console.WriteLine("DwarfPlanet : ");
            base.Draw();
        }
    }

}    