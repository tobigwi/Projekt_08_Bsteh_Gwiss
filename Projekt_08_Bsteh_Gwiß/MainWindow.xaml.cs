using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Projekt_08_Bsteh_Gwiß
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btn37_Click(object sender, RoutedEventArgs e)
        {
            bet10.Visibility = Visibility.Visible; //Der Button wird sichtbar gemacht
            bet10.IsEnabled = true; //Der Button wird enabled (verwendbar gemacht)
            bet50.Visibility = Visibility.Visible; //Der Button wird sichtbar gemacht
            bet50.IsEnabled = true; //Der Button wird enabled (verwendbar gemacht)
            bet100.Visibility = Visibility.Visible; //Der Button wird sichtbar gemacht
            bet100.IsEnabled = true; //Der Button wird enabled (verwendbar gemacht)
            bet200.Visibility = Visibility.Visible; //Der Button wird sichtbar gemacht
            bet200.IsEnabled = true; //Der Button wird enabled (verwendbar gemacht)
            bet500.Visibility = Visibility.Visible; //Der Button wird sichtbar gemacht
            bet500.IsEnabled = true; //Der Button wird enabled (verwendbar gemacht)
        }
        private void btn_betClick(object sender, RoutedEventArgs e)
        {
            bet10.Visibility = Visibility.Hidden; //Der Button wird unsichtbar gemacht
            bet10.IsEnabled = false; //Der Button wird disabled (nicht mehr verwendbar gemacht)
            bet50.Visibility = Visibility.Hidden; //Der Button wird unsichtbar gemacht
            bet50.IsEnabled = false; //Der Button wird disabled (nicht mehr verwendbar gemacht)
            bet100.Visibility = Visibility.Hidden; //Der Button wird unsichtbar gemacht
            bet100.IsEnabled = false; //Der Button wird disabled (nicht mehr verwendbar gemacht)
            bet200.Visibility = Visibility.Hidden; //Der Button wird unsichtbar gemacht
            bet200.IsEnabled = false; //Der Button wird disabled (nicht mehr verwendbar gemacht)
            bet500.Visibility = Visibility.Hidden; //Der Button wird unsichtbar gemacht
            bet500.IsEnabled = false; //Der Button wird disabled (nicht mehr verwendbar gemacht)

        }
        private void StartRotation_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 37);

            // Gradzahl berechnen, um die sich das Rad drehen muss, um die gewünschte Nummer anzuzeigen
            double rotationAngle = CalculateRotationAngle(randomNumber);

            // Rotation starten
            RotateImage(TimeSpan.FromSeconds(30), 2880 + rotationAngle);
        }

        private void RotateImage(TimeSpan duration, double angle)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                To = angle,
                Duration = duration,
                FillBehavior = FillBehavior.Stop // Hält die Animation am Ende
            };

            // Füge ein Easing-Funktion hinzu, um die Geschwindigkeit der Rotation allmählich zu verlangsamen
            animation.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut };

            image.RenderTransformOrigin = new Point(0.5, 0.5); // Setze den Mittelpunkt der Drehung auf das Zentrum des Bildes
            RotateTransform rotateTransform = new RotateTransform(); // Erstelle eine neue RotateTransform
            image.RenderTransform = rotateTransform; // Weise das neue RotateTransform dem RenderTransform des Bildes zu

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
        }
        private double CalculateRotationAngle(int targetNumber)
        {
            double degreePerNumber = 360.0 / 37; // 37 ist die Gesamtanzahl der Nummern auf dem Roulette-Rad (0 bis 36)

            // Gradzahl für die Rotation, um die gewünschte Nummer anzuzeigen
            double rotationAngle = targetNumber * degreePerNumber;

            return rotationAngle;
        }
    }
}