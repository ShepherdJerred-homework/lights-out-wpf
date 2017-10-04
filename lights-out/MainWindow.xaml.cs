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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lights_out {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private LightsOutGame game;

        public MainWindow() {
            InitializeComponent();

            game = new LightsOutGame();
            InitGame();
        }

        private void InitGame() {
            game.NewGame();
            CreateGrid();
        }

        private void CreateGrid() {
            int rectSize = (int) boardCanvas.Width / game.NumCells;
            boardCanvas.Children.Clear();
            // Create rectangles for grid
            for (int r = 0; r < game.NumCells; r++) {
                for (int c = 0; c < game.NumCells; c++) {
                    Rectangle rect = new Rectangle();
                    rect.Fill = Brushes.White;
                    rect.Width = rectSize + 1;
                    rect.Height = rect.Width + 1;
                    rect.Stroke = Brushes.Black;
                    // Store each row and col as a Point
                    rect.Tag = new Point(r, c);
                    // Register event handler
                    rect.MouseLeftButtonDown += Rect_MouseLeftButtonDown;
                    // Put the rectangle at the proper location within the canvas
                    Canvas.SetTop(rect, r * rectSize);
                    Canvas.SetLeft(rect, c * rectSize);
                    // Add the new rectangle to the canvas' children
                    boardCanvas.Children.Add(rect);
                }
            }
            UpdateGridColors();
        }

        private void Rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Rectangle rect = sender as Rectangle;
            if (rect != null) {
                var rowCol = (Point) rect.Tag;
                int row = (int) rowCol.X;
                int col = (int) rowCol.Y;
                game.MakeMove(row, col);
            }
            UpdateGridColors();
            CheckWinCondition();
        }

        private void CheckWinCondition() {
            if (game.PlayerWon()) {
                MessageBox.Show("You've Won!");
            }
        }

        private void UpdateGridColors() {
            int index = 0;

            // Set the colors of the rectangles
            for (int r = 0; r < game.NumCells; r++) {
                for (int c = 0; c < game.NumCells; c++) {
                    Rectangle rect = boardCanvas.Children[index] as Rectangle;
                    index++;
                    if (game.GetGridValue(r, c)) {
                        rect.Fill = Brushes.White;
                        rect.Stroke = Brushes.Black;
                    }
                    else {
                        rect.Fill = Brushes.Black;
                        rect.Stroke = Brushes.White;
                    }
                }
            }
        }

        private void NewGame_OnClick(object sender, RoutedEventArgs e) {
            game.NewGame();
            UpdateGridColors();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e) {
                Close();
        }

        private void About_OnClick(object sender, RoutedEventArgs e) {
            About about = new About();
            about.ShowDialog();
        }

        private void Exit_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            if (game != null) {
                e.CanExecute = game.PlayerWon();
            }
        }

        private void GridSize_OnChecked(object sender, RoutedEventArgs e) {
            MenuItem checkedMenuItem = sender as MenuItem;
            if (checkedMenuItem != null && game != null) {
                int newGridSize = int.Parse(checkedMenuItem.Header.ToString());

                game.NumCells = newGridSize;
                InitGame();

                MenuItem checkedMenuItemParent = checkedMenuItem.Parent as MenuItem;
                foreach (MenuItem menuItem in checkedMenuItemParent.Items) {
                    if (menuItem != sender) {
                        menuItem.IsChecked = false;
                    }
                }
            }
        }

        public void DragWindow(object sender, MouseButtonEventArgs args) {
            if (args.ButtonState == MouseButtonState.Pressed) {
                DragMove();
            }
        }
    }
}