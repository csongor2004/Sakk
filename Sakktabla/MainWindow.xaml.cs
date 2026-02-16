using Sakk;
using Sakk.Pieces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Sakktabla
{
    public partial class MainWindow : Window
    {
        private Board board = new Board();
        private Button[,] buttons = new Button[8, 8];
        private string selectedPos = null;
        private DispatcherTimer gameTimer;
        private TimeSpan whiteTime = TimeSpan.FromMinutes(10);
        private TimeSpan blackTime = TimeSpan.FromMinutes(10);
        private bool isBotMode;
        private string currentPlayer = "White";

        
        public MainWindow()
        {
            InitializeComponent();
            
            isBotMode = false;
            InitUIBoard();
            SetupTimer();
        }
        public MainWindow(bool isBot)
        {
            InitializeComponent();
            isBotMode = isBot;
            InitUIBoard();
            SetupTimer();
        }

        private void SetupTimer()
        {
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (currentPlayer == "White")
            {
                whiteTime = whiteTime.Subtract(TimeSpan.FromSeconds(1));
                WhiteTimeLabel.Text = $"Világos: {whiteTime:mm\\:ss}";
            }
            else
            {
                blackTime = blackTime.Subtract(TimeSpan.FromSeconds(1));
                BlackTimeLabel.Text = $"Sötét: {blackTime:mm\\:ss}";
            }

            if (whiteTime.TotalSeconds <= 0 || blackTime.TotalSeconds <= 0)
            {
                gameTimer.Stop();
                MessageBox.Show("Lejárt az idő! A játszma véget ért.");
            }
        }

        private void InitUIBoard()
        {
            BoardDisplay.Children.Clear();
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Button btn = new Button { FontSize = 24, FontWeight = FontWeights.Bold };
                    btn.Background = (r + c) % 2 == 0 ? Brushes.AntiqueWhite : Brushes.Gray;
                    btn.Tag = $"{(char)('a' + c)}{8 - r}";
                    btn.Click += Square_Click;
                    buttons[r, c] = btn;
                    BoardDisplay.Children.Add(btn);
                }
            }
            RefreshUI();
        }

        private void Square_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string pos = btn.Tag.ToString();

            if (selectedPos == null)
            {
                selectedPos = pos;
                HighlightLegalMoves(pos);
                btn.BorderBrush = Brushes.Gold;
                btn.BorderThickness = new Thickness(4);
            }
            else
            {
                if (board.MovePiece(selectedPos, pos, currentPlayer))
                {
                    CheckForPromotion(pos);
                    RefreshUI();
                    currentPlayer = (currentPlayer == "White") ? "Black" : "White";

                    if (isBotMode && currentPlayer == "Black")
                    {
                        var ai = new ChessAI(board, "Black");
                        var aiMove = ai.GetBestMove();
                        if (!string.IsNullOrEmpty(aiMove.from))
                        {
                            board.MovePiece(aiMove.from, aiMove.to, "Black");
                            currentPlayer = "White";
                        }
                    }
                }
                selectedPos = null;
                RefreshUI();
                CheckGameState();
            }
        }

        private void CheckForPromotion(string pos)
        {
            int r = 8 - (pos[1] - '0');
            int c = pos[0] - 'a';
            if (board.grid[r, c] is Pawn && (r == 0 || r == 7))
            {
                PromotionDialog dialog = new PromotionDialog { Owner = this };
                if (dialog.ShowDialog() == true)
                {
                    board.PromotePawn(r, c, dialog.SelectedPiece);
                }
            }
        }

        private void HighlightLegalMoves(string fromPos)
        {
            int c1 = fromPos[0] - 'a', r1 = 8 - (fromPos[1] - '0');
            Piece p = board.grid[r1, c1];
            if (p == null) return;

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (p.IsValidMove(r1, c1, r, c) && board.IsPathClear(r1, c1, r, c))
                    {
                        if (board.grid[r, c]?.Color != p.Color)
                            buttons[r, c].Background = Brushes.LightGreen;
                    }
                }
            }
        }

        private void RefreshUI()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    var p = board.grid[r, c];
                    buttons[r, c].Background = (r + c) % 2 == 0 ? Brushes.AntiqueWhite : Brushes.Gray;
                    buttons[r, c].BorderThickness = new Thickness(1);
                    buttons[r, c].BorderBrush = Brushes.Black;

                    if (p != null)
                    {
                        string imgName = $"{p.Color.ToLower()}{p.GetType().Name.ToLower()}.png";
                        try
                        {
                            buttons[r, c].Content = new Image
                            {
                                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri($"pack://application:,,,/Images/{imgName}"))
                            };
                        }
                        catch { buttons[r, c].Content = p.GetType().Name[0].ToString(); }
                    }
                    else
                    {
                        buttons[r, c].Content = null;
                    }
                }
            }
        }

        private void CheckGameState()
        {
            bool inCheck = board.IsInCheck(currentPlayer);
            bool canMove = board.HasLegalMoves(currentPlayer);

            if (inCheck && !canMove) MessageBox.Show($"SAKK-MATT! {currentPlayer} vesztett.");
            else if (!inCheck && !canMove) MessageBox.Show("PATT! Döntetlen.");

            TurnInfo.Text = inCheck ? $"SAKK! ({currentPlayer})" : $"{currentPlayer} jön";
        }

        private void Resign_Click(object sender, RoutedEventArgs e) { MessageBox.Show($"{currentPlayer} feladta."); this.Close(); }
        private void Draw_Click(object sender, RoutedEventArgs e) { MessageBox.Show("Döntetlen ajánlat elküldve."); }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            board = new Board();
            whiteTime = blackTime = TimeSpan.FromMinutes(10);
            currentPlayer = "White"; 
            TurnInfo.Text = "Világos jön";
            selectedPos = null;
            RefreshUI();
        }
    }
}