using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace AssemblyStudio
{
    partial class MainWindow
    {
        private string fontColorDarker_HEX = "#FF666666";
        private SolidColorBrush fontColorDarker = new SolidColorBrush(Color.FromRgb(255, 0, 255));
        private string fontColorSubtle_HEX = "#FF4D4D4D";
        private SolidColorBrush fontColorSubtle = new SolidColorBrush(Color.FromRgb(255, 0, 255));
        private string fontColorLight_HEX = "#FFD6D6D6";
        private SolidColorBrush fontColorLight = new SolidColorBrush(Color.FromRgb(255, 0, 255));

        private void prepareStyles() {
            fontColorDarker = ConvertHexToSolidColorBrush(fontColorDarker_HEX);
            fontColorSubtle = ConvertHexToSolidColorBrush(fontColorSubtle_HEX);
            fontColorLight = ConvertHexToSolidColorBrush(fontColorLight_HEX);
        }

        private Label createLabel(string text, int fontSize, SolidColorBrush fontColor, double[] size, Thickness padding, Thickness margin) {
            Label label = new Label();

            label.Content = text;
            label.FontSize = fontSize;
            label.Foreground = fontColor;
            label.Height = size[1];
            label.HorizontalContentAlignment = HorizontalAlignment.Right;
            label.Padding = padding;
            label.Margin = margin;
            label.BorderBrush = fontColorDarker;
            label.BorderThickness = new Thickness(0, 0, 1, 0);
            label.FontFamily = new FontFamily("Consolas");

            return label;
        }

        private Border createBorder(SolidColorBrush fontColor, double rowHeight, int line) {
            Border border = new Border();
            border.Height = rowHeight;
            border.BorderThickness = new Thickness(2);
            border.BorderBrush = fontColor;

            Grid.SetRow(border, line);

            return border;
        }
    }
}
