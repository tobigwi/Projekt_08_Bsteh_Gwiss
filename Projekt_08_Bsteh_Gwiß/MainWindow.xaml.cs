using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Projekt_08_Bsteh_Gwiß
{
    public partial class MainWindow : Window
    {
        private int currentBet;
        private int budgetp1 = 1000;
        private int budgetp2 = 1000;
        private List<int> Rolls = new List<int>();
        private List<Bet> BetsP1 = new List<Bet>();
        private List<Bet> BetsP2 = new List<Bet>();
        private List<string> UsedButtonsP1 = new List<string>();
        private List<string> UsedButtonsP2 = new List<string>();
        private Button lastClickedButton;
        private int lastBetAmount;
        private string lastbuttontype;
        private bool isPlayer1 = true; // Flag to track current player

        public MainWindow()
        {
            InitializeComponent();
            InitializeBets();
            DataContext = this;
            UpdateBudgetDisplay();
        }

        private void InitializeBets()
        {
            currentBet = 0;
        }

        private void UpdateBudgetDisplay()
        {
            p1.Text = $"P1: {budgetp1}€";
            p2.Text = $"P2: {budgetp2}€";
        }

        // Klasse zur Repräsentation einer Wette
        public class Bet
        {
            public int Amount { get; set; }
            public string BetType { get; set; }
            public string BetValue { get; set; }

            public Bet(int amount, string betType, string betValue)
            {
                Amount = amount;
                BetType = betType;
                BetValue = betValue;
            }
        }


        // Alle Wetten verstecken, letzte Rollen und Wetten anzeigen
        private void HideBets()
        {
            bet10.Visibility = Visibility.Hidden;
            bet10.IsEnabled = false;
            bet50.Visibility = Visibility.Hidden;
            bet50.IsEnabled = false;
            bet100.Visibility = Visibility.Hidden;
            bet100.IsEnabled = false;
            bet200.Visibility = Visibility.Hidden;
            bet200.IsEnabled = false;
            bet500.Visibility = Visibility.Hidden;
            bet500.IsEnabled = false;
            last_rolls.Visibility = Visibility.Visible;
            last_bets.Visibility = Visibility.Visible;
            switchToP1.Visibility = Visibility.Visible;
            switchToP2.Visibility = Visibility.Visible;
            switchToP1.IsEnabled = true;
            switchToP2.IsEnabled = true;
        }

        // Alle Wetten anzeigen, letzte Rollen und Wetten verstecken
        private void ShowBets()
        {
            bet10.Visibility = Visibility.Visible;
            bet10.IsEnabled = true;
            bet50.Visibility = Visibility.Visible;
            bet50.IsEnabled = true;
            bet100.Visibility = Visibility.Visible;
            bet100.IsEnabled = true;
            bet200.Visibility = Visibility.Visible;
            bet200.IsEnabled = true;
            bet500.Visibility = Visibility.Visible;
            bet500.IsEnabled = true;
            last_rolls.Visibility = Visibility.Hidden;
            last_bets.Visibility = Visibility.Hidden;
            switchToP1.Visibility = Visibility.Hidden;
            switchToP2.Visibility = Visibility.Hidden;
            switchToP1.IsEnabled = false;
            switchToP2.IsEnabled = false;
        }

        // ListBoxen aktualisieren
        private void UpdateListBoxes()
        {
            last_bets.Items.Clear();
        }

        // Wette platzieren und Budget aktualisieren
        private void PlaceBet(int amount, string betType, string betValue)
        {
            if (isPlayer1)
            {
                if (budgetp1 < amount)
                {
                    MessageBox.Show("P1: Not enough budget to place this bet.");
                    return;
                }

                currentBet = amount;
                budgetp1 -= amount;
                p1.Text = $"P1: {budgetp1}€";
                BetsP1.Add(new Bet(amount, betType, betValue));
                UsedButtonsP1.Add(lastClickedButton.Name);
            }
            else
            {
                if (budgetp2 < amount)
                {
                    MessageBox.Show("P2: Not enough budget to place this bet.");
                    return;
                }

                currentBet = amount;
                budgetp2 -= amount;
                p2.Text = $"P2: {budgetp2}€";
                BetsP2.Add(new Bet(amount, betType, betValue));
                UsedButtonsP2.Add(lastClickedButton.Name);
            }

            lastBetAmount = amount;
            last_bets.Items.Add($"{betType} ({betValue}) ({lastBetAmount}€)");
            HideBets();
        }


        private void btn_bet10Click(object sender, RoutedEventArgs e)
        {
            PlaceBet(10, lastbuttontype, lastClickedButton.Content.ToString());
        }

        private void btn_bet50Click(object sender, RoutedEventArgs e)
        {
            PlaceBet(50, lastbuttontype, lastClickedButton.Content.ToString());
        }

        private void btn_bet100Click(object sender, RoutedEventArgs e)
        {
            PlaceBet(100, lastbuttontype, lastClickedButton.Content.ToString());
        }

        private void btn_bet200Click(object sender, RoutedEventArgs e)
        {
            PlaceBet(200, lastbuttontype, lastClickedButton.Content.ToString());
        }

        private void btn_bet500Click(object sender, RoutedEventArgs e)
        {
            PlaceBet(500, lastbuttontype, lastClickedButton.Content.ToString());
        }
        private void SwitchToP1_Click(object sender, RoutedEventArgs e)
        {
            isPlayer1 = true;
            MessageBox.Show("Switched to Player 1");
        }

        private void SwitchToP2_Click(object sender, RoutedEventArgs e)
        {
            isPlayer1 = false;
            MessageBox.Show("Switched to Player 2");
        }
        // Rotation des Roulette-Rads starten
        private void StartRotation_Click(object sender, RoutedEventArgs e)
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Please place a bet first.");
                return;
            }

            DisableButtons();

            Random random = new Random();
            int randomNumber = random.Next(0, 37);

            Rolls.Add(randomNumber);
            last_rolls.Items.Add(randomNumber);

            double rotationAngle = CalculateRotationAngle(randomNumber);
            RotateImage(TimeSpan.FromSeconds(10), rotationAngle);

            string result = GetRouletteResult(randomNumber);
            MessageBox.Show($"The ball landed on {result}.");

            CheckAndCalculateWinnings(randomNumber);

            currentBet = 0;
            last_bets.Items.Clear();
            BetsP1.Clear();
            BetsP2.Clear();
            UsedButtonsP1.Clear();
            UsedButtonsP2.Clear();
            UpdateListBoxes();

            EnableButtons();
        }


        // Bild drehen
        private void RotateImage(TimeSpan duration, double angle)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                To = angle,
                Duration = duration,
                FillBehavior = FillBehavior.Stop
            };
            animation.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut };

            image.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotateTransform = new RotateTransform();
            image.RenderTransform = rotateTransform;

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
        }


        private void DisableButtons()
        {
            // Deaktiviere jeden Button einzeln
            g0.IsEnabled = false;
            r3.IsEnabled = false;
            b6.IsEnabled = false;
            r9.IsEnabled = false;
            r12.IsEnabled = false;
            b15.IsEnabled = false;
            r18.IsEnabled = false;
            r21.IsEnabled = false;
            r24.IsEnabled = false;
            r27.IsEnabled = false;
            r30.IsEnabled = false;
            b33.IsEnabled = false;
            r36.IsEnabled = false;
            g1b2v1.IsEnabled = false;
            r2.IsEnabled = false;
            r5.IsEnabled = false;
            b8.IsEnabled = false;
            r11.IsEnabled = false;
            r14.IsEnabled = false;
            b17.IsEnabled = false;
            b20.IsEnabled = false;
            r23.IsEnabled = false;
            b26.IsEnabled = false;
            b29.IsEnabled = false;
            r32.IsEnabled = false;
            b35.IsEnabled = false;
            g1b2v2.IsEnabled = false;
            r1.IsEnabled = false;
            b4.IsEnabled = false;
            r7.IsEnabled = false;
            b10.IsEnabled = false;
            b13.IsEnabled = false;
            r16.IsEnabled = false;
            r19.IsEnabled = false;
            b22.IsEnabled = false;
            r25.IsEnabled = false;
            b28.IsEnabled = false;
            b31.IsEnabled = false;
            r34.IsEnabled = false;
            g1b2v3.IsEnabled = false;
            g1b12.IsEnabled = false;
            g13b24.IsEnabled = false;
            g25b36.IsEnabled = false;
            g1b18.IsEnabled = false;
            even.IsEnabled = false;
            r.IsEnabled = false;
            b.IsEnabled = false;
            odd.IsEnabled = false;
            g19b36.IsEnabled = false;
            bet10.IsEnabled = false;
            bet50.IsEnabled = false;
            bet100.IsEnabled = false;
            bet200.IsEnabled = false;
            bet500.IsEnabled = false;
            start.IsEnabled = false;
        }

        private void EnableButtons()
        {
            // Aktiviere jeden Button einzeln
            g0.IsEnabled = true;
            r3.IsEnabled = true;
            b6.IsEnabled = true;
            r9.IsEnabled = true;
            r12.IsEnabled = true;
            b15.IsEnabled = true;
            r18.IsEnabled = true;
            r21.IsEnabled = true;
            r24.IsEnabled = true;
            r27.IsEnabled = true;
            r30.IsEnabled = true;
            b33.IsEnabled = true;
            r36.IsEnabled = true;
            g1b2v1.IsEnabled = true;
            r2.IsEnabled = true;
            r5.IsEnabled = true;
            b8.IsEnabled = true;
            r11.IsEnabled = true;
            r14.IsEnabled = true;
            b17.IsEnabled = true;
            b20.IsEnabled = true;
            r23.IsEnabled = true;
            b26.IsEnabled = true;
            b29.IsEnabled = true;
            r32.IsEnabled = true;
            b35.IsEnabled = true;
            g1b2v2.IsEnabled = true;
            r1.IsEnabled = true;
            b4.IsEnabled = true;
            r7.IsEnabled = true;
            b10.IsEnabled = true;
            b13.IsEnabled = true;
            r16.IsEnabled = true;
            r19.IsEnabled = true;
            b22.IsEnabled = true;
            r25.IsEnabled = true;
            b28.IsEnabled = true;
            b31.IsEnabled = true;
            r34.IsEnabled = true;
            g1b2v3.IsEnabled = true;
            g1b12.IsEnabled = true;
            g13b24.IsEnabled = true;
            g25b36.IsEnabled = true;
            g1b18.IsEnabled = true;
            even.IsEnabled = true;
            r.IsEnabled = true;
            b.IsEnabled = true;
            odd.IsEnabled = true;
            g19b36.IsEnabled = true;
            bet10.IsEnabled = true;
            bet50.IsEnabled = true;
            bet100.IsEnabled = true;
            bet200.IsEnabled = true;
            bet500.IsEnabled = true;
            start.IsEnabled = true;
        }

        // Berechnung des Rotationswinkels basierend auf der Zufallszahl
        private double CalculateRotationAngle(int randomNumber)
        {
            double rotationAngle = 0;

            switch (randomNumber)
            {
                case 0:
                    rotationAngle = 720.0;
                    break;
                case 32:
                    rotationAngle = 710.27027;
                    break;
                case 15:
                    rotationAngle = 700.540;
                    break;
                case 19:
                    rotationAngle = 690.81081;
                    break;
                case 4:
                    rotationAngle = 681.081081;
                    break;
                case 21:
                    rotationAngle = 671.351351;
                    break;
                case 2:
                    rotationAngle = 661.621621;
                    break;
                case 25:
                    rotationAngle = 651.891891;
                    break;
                case 17:
                    rotationAngle = 642.162162;
                    break;
                case 34:
                    rotationAngle = 632.432432;
                    break;
                case 6:
                    rotationAngle = 622.702703;
                    break;
                case 27:
                    rotationAngle = 612.972973;
                    break;
                case 13:
                    rotationAngle = 603.243243;
                    break;
                case 36:
                    rotationAngle = 593.513514;
                    break;
                case 11:
                    rotationAngle = 583.783784;
                    break;
                case 30:
                    rotationAngle = 574.054054;
                    break;
                case 8:
                    rotationAngle = 564.324324;
                    break;
                case 23:
                    rotationAngle = 554.594595;
                    break;
                case 10:
                    rotationAngle = 544.864865;
                    break;
                case 5:
                    rotationAngle = 535.135135;
                    break;
                case 24:
                    rotationAngle = 525.405405;
                    break;
                case 16:
                    rotationAngle = 515.675676;
                    break;
                case 33:
                    rotationAngle = 505.945946;
                    break;
                case 1:
                    rotationAngle = 496.216216;
                    break;
                case 20:
                    rotationAngle = 486.486486;
                    break;
                case 14:
                    rotationAngle = 476.756757;
                    break;
                case 31:
                    rotationAngle = 467.027027;
                    break;
                case 9:
                    rotationAngle = 457.297297;
                    break;
                case 22:
                    rotationAngle = 447.567568;
                    break;
                case 18:
                    rotationAngle = 437.837838;
                    break;
                case 29:
                    rotationAngle = 428.108108;
                    break;
                case 7:
                    rotationAngle = 418.378378;
                    break;
                case 28:
                    rotationAngle = 408.648648;
                    break;
                case 12:
                    rotationAngle = 398.918919;
                    break;
                case 35:
                    rotationAngle = 389.189189;
                    break;
                case 3:
                    rotationAngle = 379.459459;
                    break;
                case 26:
                    rotationAngle = 369.729729;
                    break;
            }

            return rotationAngle;
        }

        // Ermittlung des Ergebnisses basierend auf der Zufallszahl


        // Überprüfung der Gewinne und Berechnung des Gewinns
        private void CheckAndCalculateWinnings(int rolledNumber)
        {
            string rolledColor = GetColor(rolledNumber);
            int winningsP1 = CalculateWinnings(BetsP1, rolledNumber, rolledColor);
            int winningsP2 = CalculateWinnings(BetsP2, rolledNumber, rolledColor);

            if (winningsP1 > 0)
            {
                MessageBox.Show($"P1 wins {winningsP1}€!");
            }

            if (winningsP2 > 0)
            {
                MessageBox.Show($"P2 wins {winningsP2}€!");
            }

            budgetp1 += winningsP1;
            budgetp2 += winningsP2;
            UpdateBudgetDisplay();
        }

        private string GetRouletteResult(int number)
        {
            string color = GetColor(number);
            return $"{number} ({color})";
        }

        private string GetColor(int number)
        {
            if (number == 0)
                return "Green";
            if (new[] { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 }.Contains(number))
                return "Red";
            else
                return "Black";
        }

        private int CalculateWinnings(List<Bet> bets, int rolledNumber, string rolledColor)
        {
            int winnings = 0;

            foreach (var bet in bets)
            {
                bool win = false;

                switch (bet.BetType)
                {
                    case "Number":
                        if (Convert.ToInt32(bet.BetValue) == rolledNumber)
                        {
                            win = true;
                            winnings += bet.Amount * 35; // Payout for a single number bet is 35:1
                        }
                        break;
                    case "Red":
                        if (GetColor(rolledNumber)== "Red") {
                            win = true;
                            winnings += bet.Amount * 2; // Payout for a color bet is 2:1
                        }
                        break;
                    case "Black":
                        if (GetColor(rolledNumber) == "Black")
                        {
                            win = true;
                            winnings += bet.Amount * 2; // Payout for a color bet is 2:1
                        }
                        break;
                    case "Even":
                        if (rolledNumber % 2 == 0)
                        {
                            win = true;
                            winnings += bet.Amount * 2; // Payout for an even bet is 2:1
                        }
                        break;
                    case "Odd":
                        if (rolledNumber % 2 != 0)
                        {
                            win = true;
                            winnings += bet.Amount * 2; // Payout for an odd bet is 2:1
                        }
                        break;
                    case "1to18":
                        if (rolledNumber >= 1 && rolledNumber <= 18)
                        {
                            win = true;
                            winnings += bet.Amount * 2; // Payout for 1-18 bet is 2:1
                        }
                        break;
                    case "19to36":
                        if (rolledNumber >= 19 && rolledNumber <= 36)
                        {
                            win = true;
                            winnings += bet.Amount * 2; // Payout for 19-36 bet is 2:1
                        }
                        break;
                    case "EveryThird1to36":
                        if (rolledNumber % 3 == 0)
                        {
                            win = true;
                            winnings += bet.Amount * 3; // Payout for every third number from 1 to 36 bet is 3:1
                        }
                        break;
                    case "EveryThird2to36":
                        if ((rolledNumber + 1) % 3 == 0)
                        {
                            win = true;
                            winnings += bet.Amount * 3; // Payout for every third number from 2 to 36 bet is 3:1
                        }
                        break;
                    case "EveryThird3to36":
                        if ((rolledNumber + 2) % 3 == 0)
                        {
                            win = true;
                            winnings += bet.Amount * 3; // Payout for every third number from 3 to 36 bet is 3:1
                        }
                        break;
                    case "First3rd":
                        if (rolledNumber > 0 && rolledNumber < 13)
                        {
                            win = true;
                            winnings += bet.Amount * 3; // Payout for first third is 3:1
                        }
                        break;
                    case "Second3rd":
                        if (rolledNumber > 12 && rolledNumber < 25)
                        {
                            win = true;
                            winnings += bet.Amount * 3; // Payout for second third is 3:1
                        }
                        break;
                    case "Third3rd":
                        if (rolledNumber > 24 && rolledNumber < 37)
                        {
                            win = true;
                            winnings += bet.Amount * 3; // Payout for third third is 3:1
                        }
                        break;
                }

                if (win)
                {
                    MessageBox.Show($"Congratulations! You won {winnings}€ on your {bet.BetType} bet ({bet.BetValue}).");
                }
            }

            return winnings;
        }



        private void g1b12_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "First3rd";
            ShowBets();
        }

        private void g13b24_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "Second3rd";
            ShowBets();
        }

        private void g25b36_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "Third3rd";
            ShowBets();

        }

        private void g1b2v1_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "EveryThird1to36";
            ShowBets();
        }

        private void g1b2v2_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "EveryThird2to36";
            ShowBets();
        }

        private void g1b2v3_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "EveryThird3to36";
            ShowBets();
        }

        private void g1b18_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "1to18";
            ShowBets();
        }

        private void even_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "Even";
            ShowBets();
        }

        private void odd_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "Odd";
            ShowBets();
        }

        private void g19b36_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "19to36";
            ShowBets();
        }
        private void btn37_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "Number";
            ShowBets();

        }
        private void r_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "Red";
            ShowBets();
        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            lastClickedButton = sender as Button; // Speichern des zuletzt geklickten Buttons
            lastbuttontype = "Black";
            ShowBets();
        }

    }
}

