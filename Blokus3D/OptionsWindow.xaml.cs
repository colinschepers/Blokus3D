using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace Blokus3D
{
    public partial class OptionsWindow : Window
    {
        private List<Action> _changes = new List<Action>();

        public bool BoardChanged { get; private set; }

        public OptionsWindow()
        {
            InitializeComponent();

            SizeXComboBox.SelectedIndex = Configuration.BoardSizeX - 1;
            SizeYComboBox.SelectedIndex = Configuration.BoardSizeY - 1;
            SizeZComboBox.SelectedIndex = Configuration.BoardSizeZ - 1;
            SizeXComboBox.SelectionChanged += new SelectionChangedEventHandler(SizeXChanged);
            SizeYComboBox.SelectionChanged += new SelectionChangedEventHandler(SizeYChanged);
            SizeZComboBox.SelectionChanged += new SelectionChangedEventHandler(SizeZChanged);
            ShouldDrawLabelCheckBox.IsChecked = Configuration.ShouldDrawLabel;

            foreach (var pieceColor in Enum.GetValues(typeof(PieceColors)).Cast<PieceColors>())
            {
                for (int shapeNr = 0; shapeNr < Configuration.ShapeCount; shapeNr++)
                {
                    var piece = new Piece(pieceColor, shapeNr);
                    var optionsPiece = new OptionsPiece(piece);
                    Grid.SetRow(optionsPiece, (int)pieceColor);
                    Grid.SetColumn(optionsPiece, shapeNr);
                    PieceGrid.Children.Add(optionsPiece);
                    optionsPiece.Clicked += new OptionsPiece.ClickedEvent(PieceClicked);

                    if (Configuration.PieceSet.Contains(piece))
                    {
                        optionsPiece.Enabled = true;
                    }
                }
            }
        }

        private void SizeXChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ComboBoxItem)e.AddedItems[0];
            var newSize = int.Parse(item.Content.ToString());
            _changes.Add(new Action(delegate { Configuration.BoardSizeX = newSize; BoardChanged = true; }));
        }

        private void SizeYChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ComboBoxItem)e.AddedItems[0];
            var newSize = int.Parse(item.Content.ToString());
            _changes.Add(new Action(delegate { Configuration.BoardSizeY = newSize; BoardChanged = true; }));
        }

        private void SizeZChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ComboBoxItem)e.AddedItems[0];
            var newSize = int.Parse(item.Content.ToString());
            _changes.Add(new Action(delegate { Configuration.BoardSizeZ = newSize; BoardChanged = true; }));
        }

        private void ShouldDrawLabel(object sender, RoutedEventArgs e)
        {
            _changes.Add(new Action(delegate { Configuration.ShouldDrawLabel = true; }));
        }

        private void ShouldNotDrawLabel(object sender, RoutedEventArgs e)
        {
            _changes.Add(new Action(delegate { Configuration.ShouldDrawLabel = false; }));
        }

        private void PieceClicked(Piece piece, bool enabled)
        {
            _changes.Add(new Action(delegate
            {
                if (!enabled)
                {
                    Configuration.PieceSet.Remove(piece);
                }
                else if (!Configuration.PieceSet.Contains(piece))
                {
                    Configuration.PieceSet.Add(piece);
                }
                BoardChanged = true;
            }));
        }

        private void OKClick(object sender, RoutedEventArgs e)
        {
            if (_changes.Count > 0)
            {
                foreach (var action in _changes)
                {
                    action.Invoke();
                }
                DialogResult = true;
            }
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
