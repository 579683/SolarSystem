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

namespace WpfSolarSystem
{
    public partial class MainWindow : Window
    {

        List<SpaceObject> solarSystem = new List<SpaceObject>();
        List<SpaceObject> solarSystemReal = new List<SpaceObject>();

        double days = 0;
        double SunScale = 500;
        double z = 1;
        double speed = 0.5;

        public MainWindow()
        {
            InitializeComponent();
            getSolarSystem tempSol = new getSolarSystem();
            solarSystem = tempSol.getSolar();
            solarSystemReal = tempSol.getRealSolar();

            makeLabelButton();
            makeCircleButton();
            makeZoomButton();
            makeCircles();
            makeSpeedButton();

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = new TimeSpan(100000) //sekk
            };
            timer.Tick += t_Tick;
            timer.Start();
        }

        private void makeSpeedButton()
        {
            Button fasterButton = new Button()
            {
                Content = "faster"
            };
            Button slowerButton = new Button()
            {
                Content = "slower"
            };
            Canvas.SetLeft(fasterButton, 0);
            Canvas.SetTop(fasterButton, 50);
            myCanvas.Children.Add(fasterButton);
            Canvas.SetLeft(slowerButton, 50);
            Canvas.SetTop(slowerButton, 50);
            myCanvas.Children.Add(slowerButton);
            fasterButton.Click += new RoutedEventHandler(faster);
            slowerButton.Click += new RoutedEventHandler(slower);


        }

        private void faster(object sender, RoutedEventArgs e)
        {
            speed = speed + 2;
        }

        private void slower(object sender, RoutedEventArgs e)
        {
            speed = speed - 2;
        }

        private void makeZoomButton()
        {
            Button Plusbutton = new Button()
            {
                Content = " zoom in "
            };
            Button minusButton = new Button()
            {
                Content = " zoom out "
            };
            Canvas.SetLeft(Plusbutton, 0);
            Canvas.SetTop(Plusbutton, 30);
            Canvas.SetLeft(minusButton, 50);
            Canvas.SetTop(minusButton, 30);
            Plusbutton.Click += new RoutedEventHandler(zoomInn);
            minusButton.Click += new RoutedEventHandler(zoomOut);
            myCanvas.Children.Add(Plusbutton);
            myCanvas.Children.Add(minusButton);
        }

        //Zoom in method
        private void zoomInn(object sender, RoutedEventArgs e)
        {
            var scaler = Grid.LayoutTransform as ScaleTransform;

            if (scaler == null)
            {
                scaler = new ScaleTransform(z, z);
                Grid.LayoutTransform = scaler;
            }
            DoubleAnimation animator = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(600)),
            };
            if (scaler.ScaleX == z)
            {
                z = z + 0.5;
                animator.To = z;
            }
            else if (scaler.ScaleX > 1.0)
            {
                z = z + 0.5;
                animator.To = z;
            }
            scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
            scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
        }

        //Zoom out method
        private void zoomOut(object sender, RoutedEventArgs e)
        {
            var scaler = Grid.LayoutTransform as ScaleTransform;
            if (scaler == null)
            {
                scaler = new ScaleTransform(1.0, 1.0);
                Grid.LayoutTransform = scaler;
            }
            DoubleAnimation animator = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(600)),
            };
            if (scaler.ScaleX > 1.0)
            {
                z = scaler.ScaleX - 0.5;
                animator.To = z;
            }

            scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
            scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
        }

        // Show textbox with info when you click planet
        private void spaceObjectInfo(object sender, MouseButtonEventArgs e)
        {
            ClearCanvasInfo();
            Ellipse ellipse = (Ellipse)sender;
            int i = 0;
            SpaceObject temp = null;
            foreach (SpaceObject space in solarSystem)
            {
                if (ellipse.Height == space.objectRadius / SunScale)
                {
                    i = solarSystem.IndexOf(space);
                }
            }

            temp = solarSystemReal[i];

            if (temp.Parent == null)
            {
                TextBox textbox = new TextBox
                {
                    Text = temp.name + "\n" + "Object Radius: " + temp.objectRadius
                };
                Canvas.SetLeft(textbox, 0);
                Canvas.SetTop(textbox, 200);
                myInfo.Children.Add(textbox);
            }
            else
            {
                TextBox textbox = new TextBox();

                textbox.Text = temp.name + "\nOrbital Radius: " + temp.orbitalRadius + "*10^6 km " +
                "around the " + temp.Parent.name + "\n" +
                "Orbital Period: " + temp.orbitalPeriod + " days \n" +
                "Rotation Period: " + temp.rotationPeriod + " days \n" +
                "Object Radius: " + temp.objectRadius + " km";

                if (temp.Children.Count > 0)
                {
                    textbox.Text += "\n \nMoons:\n";
                    foreach (SpaceObject child in temp.Children)
                    {
                        textbox.Text += child.name + "\nOrbital Radius: " + child.orbitalRadius + "*10^6 km " +
                        "around the " + child.Parent.name + "\n" +
                        "Orbital Period: " + child.orbitalPeriod + " days \n" +
                        "Rotation Period: " + child.rotationPeriod + " days \n" +
                        "Object Radius: " + child.objectRadius + " km\n";
                    }
                }
                Canvas.SetLeft(textbox, 0);
                Canvas.SetTop(textbox, 200);
                myInfo.Children.Add(textbox);
            }
        }

        // Button to show/unshow the names of the planets
        private void makeLabelButton()
        {
            Button button = new Button()
            {
                Content = "toggle labels"
            };
            Canvas.SetLeft(button, 0);
            Canvas.SetTop(button, 0);
            button.Click += new RoutedEventHandler(toggleLabel);
            myCanvas.Children.Add(button);
        }

        // Show/unshow the names of the planets

        private void toggleLabel(object sender, RoutedEventArgs e)
        {
            if (myLabel.Visibility == Visibility.Collapsed)
            {
                myLabel.Visibility = Visibility.Visible;
            }
            else
            {
                myLabel.Visibility = Visibility.Collapsed;
            }
        }

        // Displays days and years
        private void makeLabeldays()
        {
            Label label = new Label()
            {
                Content = "Days: " + days + ".\nYears: " + days / 365.242199,
                Foreground = Brushes.White
            };
            Canvas.SetLeft(label, ActualWidth / 2);
            Canvas.SetTop(label, 50);
            myCanvas.Children.Add(label);
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
        private void ClearCanvasInfo()
        {
            for (int i = myInfo.Children.Count - 1; i >= 0; i += -1)
            {
                UIElement Child = myInfo.Children[i];
                if (Child is TextBox)
                    myInfo.Children.Remove(Child);
            }
        }

        private void t_Tick(object sender, EventArgs e)
        {
            ClearCanvasSpaceObj();
            days = days + speed;
            makeSolarSystem();
            makeLabeldays();
        }

        public void makeSolarSystem()
        {
            foreach (SpaceObject obj in solarSystem)
            {
                double x = pos(obj, days, 1).Item1 + ((myCanvas.ActualWidth - ((obj.objectRadius) / SunScale)) / 2);
                double y = pos(obj, days, 1).Item2 + ((myCanvas.ActualHeight - ((obj.objectRadius) / SunScale)) / 2);

                Ellipse ellipse = makeSpaceObject(obj.objectRadius, obj.color);

                ellipse.MouseLeftButtonDown += spaceObjectInfo;
                Label label = makeLabel(obj);

                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);

                Canvas.SetLeft(label, x + 20);
                Canvas.SetTop(label, y);

                myLabel.Children.Add(label);
                myCanvas.Children.Add(ellipse);
            }
        }

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

        private void makeCircleButton()
        {
            Button button = new Button()
            {
                Content = "toggle circles"
            };
            Canvas.SetLeft(button, 50);
            Canvas.SetTop(button, 0);
            button.Click += new RoutedEventHandler(toggleCircles);
            myCanvas.Children.Add(button);
        }

        private void toggleCircles(object sender, RoutedEventArgs e)
        {
            if (myRings.Visibility == Visibility.Collapsed)
            {
                myRings.Visibility = Visibility.Visible;
            }
            else
            {
                myRings.Visibility = Visibility.Collapsed;
            }
        }

        public Tuple<double, double> pos(SpaceObject planet, double tid, int skala)
        {
            var temp = planet.getPosition(tid);
            return temp;
        }
    }
}