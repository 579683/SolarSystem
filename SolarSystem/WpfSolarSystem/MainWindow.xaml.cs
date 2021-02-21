using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SpaceSim;

namespace SolarSystem
{
    public partial class MainWindow : Window
    {

        List<SpaceObject> solarSystem = new List<SpaceObject>();
        List<SpaceObject> solarSystemReal = new List<SpaceObject>();

        double days = 0;
        double SunScale = 500;
        double scaled = 1;
        double speed = 0.5;

        public MainWindow()
        {

            InitializeComponent();
            program tempSol = new program();
            solarSystem = tempSol.fakeSolarSystem();
            solarSystemReal = tempSol.realSolarSystem();

            makeCircles();
            labelButton();
            zoomInButton();
            zoomOutButton();
            Grid.MouseLeftButtonDown += speedUp;
            Grid.MouseRightButtonDown += slowDown;

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = new TimeSpan(150000)
            };
            timer.Tick += t_Tick;
            timer.Start();
        }

        public Tuple<double, double> pos(SpaceObject planet, double tid)
        {
            var temp = planet.calculatePos(tid);
            return temp;
        }

        private void t_Tick(object sender, EventArgs e)
        {
            ClearCanvasSpaceObj();
            days = days + speed;
            makeSolarSystem();
            makeLabelDays();
        }

        private void speedUp(Object obj, RoutedEventArgs e)
        {
            ClearCanvasInfo();
            speed = speed + 1;
        }

        private void slowDown(Object obj, RoutedEventArgs e)
        {
            if (speed > 0.5)
            {
                ClearCanvasInfo();
                speed = speed - 1;
            }
            else
                ClearCanvasInfo();
                speed = 0.5;
        }

        private Label makeLabel(SpaceObject obj)
        {
            Label label = new Label()
            {
                Content = obj.name,
                Foreground = Brushes.White
            };
            return label;
        }

        //Making the whole solarsystem with their name showing as labels
        public void makeSolarSystem()
        {
            foreach (SpaceObject obj in solarSystem)
            {
                double x = pos(obj, days).Item1 + ((myCanvas.ActualWidth - ((obj.objectRadius) / SunScale)) / 2);
                double y = pos(obj, days).Item2 + ((myCanvas.ActualHeight - ((obj.objectRadius) / SunScale)) / 2);

                Ellipse ellipse = makeSpaceObject(obj.objectRadius, obj.color);

                ellipse.MouseEnter += spaceObjectInfo;
                Label label = makeLabel(obj);

                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);

                Canvas.SetLeft(label, x + 20);
                Canvas.SetTop(label, y);

                myLabel.Children.Add(label);
                myCanvas.Children.Add(ellipse);
            }
        }

        //Making the planets
        public Ellipse makeSpaceObject(double objectRadius, String color)
        {
            Ellipse ellipse = new Ellipse();
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = Color.FromArgb(0, 255, 255, 1);
            ellipse.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            ellipse.StrokeThickness = 2;
            ellipse.Stroke = Brushes.Black;
            ellipse.Width = objectRadius / SunScale;
            ellipse.Height = objectRadius / SunScale;

            return ellipse;
        }

        // Making the circles going through the planets
        public void makeCircles()
        {
            for (int i = 0; i <= 8; i++)
            {
                {
                    Ellipse ellipse = new Ellipse
                    {
                        StrokeThickness = 1,
                        Stroke = Brushes.Gray,
                        Width = solarSystem[i].orbitalRadius * 2,
                        Height = solarSystem[i].orbitalRadius * 2
                    };

                    Canvas.SetLeft(ellipse, myCanvas.ActualWidth / 2);
                    Canvas.SetTop(ellipse, myCanvas.ActualHeight / 2);
                    Grid.Children.Add(ellipse);
                }
            }
        }

        private void zoomInButton()
        {
            Button btn = new Button()
            {
                Content = "Zoom in"
            };

            Canvas.SetLeft(btn, 0);
            Canvas.SetTop(btn, 20);
            btn.Click += new RoutedEventHandler(btnClick1);
            myCanvas.Children.Add(btn);
        }

        private void zoomOutButton()
        {
            Button btn = new Button()
            {
                Content = "Zoom out"
            };

            Canvas.SetLeft(btn, 0);
            Canvas.SetTop(btn, 40);
            btn.Click += new RoutedEventHandler(btnClick2);
            myCanvas.Children.Add(btn);
        }

        private void btnClick1(object sender, RoutedEventArgs e)

        {

            var scaler = Grid.LayoutTransform as ScaleTransform;

            if (scaler == null)
            {
                scaler = new ScaleTransform(1.0, 1.0);
                Grid.LayoutTransform = scaler;
            }

            // DoubleAnimation object to drive the scaleX and scaleY properties.

            DoubleAnimation animator = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(600)),
            };

            // Toggle the scale between 1.0 and 1.5 
            if(scaler.ScaleX == 1.0)
            {
                scaled = scaled + 0.5;
                animator.To = scaled;
            }
            else
            {
                scaled = scaled + 0.5;
                animator.To = scaled;
            }

            scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
            scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
        }

        private void btnClick2(object sender, RoutedEventArgs e)

        {

            var scaler = Grid.LayoutTransform as ScaleTransform;

            if (scaler == null)
            {
                scaler = new ScaleTransform(1.0, 1.0);
                Grid.LayoutTransform = scaler;
            }

            // DoubleAnimation object to drive the scaleX and scaleY properties.

            DoubleAnimation animator = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(600)),
            };

            // Toggle the scale between 1.0 and 1.5 
            if (scaler.ScaleX > 1.0)
            {
                scaled = scaled - 0.5;
                animator.To = scaled;
            }

            scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
            scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
        }



        // Clear duplications of labels, planets, etc...
        private void ClearCanvasSpaceObj()
        {
            for (int i = myCanvas.Children.Count - 1; i >= 0; i += -1)
            {
                UIElement Child = myCanvas.Children[i];
                if (Child is Ellipse || Child is Label)
                    myCanvas.Children.Remove(Child);

            }
             
            for (int i = myLabel.Children.Count - 1; i >= 0; i += -1)
            {
                UIElement Child = myLabel.Children[i];
                if (Child is Label)
                    myLabel.Children.Remove(Child);
            }
        }

        //Method for clearing textbox from screen
        private void ClearCanvasInfo()
        {
            for (int i = myInfo.Children.Count - 1; i >= 0; i += -1)
            {
                UIElement Child = myInfo.Children[i];
                if (Child is TextBox)
                    myInfo.Children.Remove(Child);
            }
        }

        // Show textbox with info when you click planet
        private void spaceObjectInfo(Object obj, MouseEventArgs e)
        {
            ClearCanvasInfo();
            Ellipse ellipse = (Ellipse)obj;
            int i = 0;
            

            foreach(SpaceObject space in solarSystem)
            {
                if(ellipse.Height == space.objectRadius / SunScale)
                {
                    i = solarSystem.IndexOf(space);
                }
            }
    

            if(solarSystemReal[i].Parent == null)
            {
                // Gir ut verdiene til Solen
                TextBox textBox = new TextBox
                {
                    Text = solarSystemReal[i].name + "\n" + "Object Radius: " + solarSystemReal[i].objectRadius + " km"
                };

                Canvas.SetRight(textBox, 500);
                Canvas.SetTop(textBox, 0);
                myInfo.Children.Add(textBox);
            }
            else
            {

                // Gir ut verdiene til planetene
                TextBox textbox = new TextBox();

                textbox.Text = solarSystemReal[i].name + "\nOrbital Radius: " + solarSystemReal[i].orbitalRadius + " km" +
                " around the " + solarSystemReal[i].Parent.name + "\n" +
                "Orbital Period: " + solarSystemReal[i].orbitalPeriod + " days \n" +
                "Rotation Period: " + solarSystemReal[i].rotationPeriod + " days \n" +
                "Object Radius: " + solarSystemReal[i].objectRadius + " km";

                //Hvis maaner finnes, så legges det med i beskrivelsen til planetene
                if (solarSystemReal[i].Children.Count > 0)
                {
                    textbox.Text += "\n \nMoons:\n";
                    foreach (SpaceObject child in solarSystemReal[i].Children)
                    {
                        textbox.Text += child.name + "\nOrbital Radius: " + child.orbitalRadius + "*10^6 km " +
                        "around the " + child.Parent.name + "\n" +
                        "Orbital Period: " + child.orbitalPeriod + " days \n" +
                        "Rotation Period: " + child.rotationPeriod + " days \n" +
                        "Object Radius: " + child.objectRadius + " km\n";
                    }
                }
                Canvas.SetRight(textbox, 300);
                Canvas.SetTop(textbox, 0);
                myInfo.Children.Add(textbox);
       
            }
        }

        private void toggleLabel(object obj, RoutedEventArgs e)
        {
            if(myLabel.Visibility == Visibility.Collapsed)
            {
                myLabel.Visibility = Visibility.Visible;
            } 
            else
            {
               myLabel.Visibility = Visibility.Collapsed;
            }
        }

        private void labelButton()
        {
            Button btn = new Button()
            {
                Content = "Labels"
            };

            Canvas.SetTop(btn, 0);
            Canvas.SetLeft(btn, 0);
            btn.Click += new RoutedEventHandler(toggleLabel);
            myCanvas.Children.Add(btn);
        }

        // Methods shows days and years
        private void makeLabelDays()
        {
            Label label = new Label()
            {
                Content = "DAYS: " + days + ".\nYEARS: " + days / 365,
                Foreground = Brushes.White
            };

            Canvas.SetLeft(label, 300);
            Canvas.SetTop(label, ActualWidth/2);
            myCanvas.Children.Add(label);
        }
    }
}