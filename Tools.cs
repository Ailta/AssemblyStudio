using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace AssemblyStudio {
    partial class MainWindow {
        private int GetCaretPosition(RichTextBox richTextBox) {
            TextPointer caretPos = richTextBox.CaretPosition;
            TextPointer lineStartPos = caretPos.GetLineStartPosition(0);

            // Calculate the line number by counting the number of lines from the start of the document
            int lineNumber = 0;
            TextPointer position = richTextBox.Document.ContentStart;
            while (position != null && position.CompareTo(lineStartPos) < 0) {
                if ((position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart) && (position.Parent is Paragraph)) {
                    lineNumber++;
                }
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
            return lineNumber;
        }

        private SolidColorBrush ConvertHexToSolidColorBrush(string hex) {
            hex = hex.Replace("#", string.Empty);

            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));

            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(a, r, g, b));

            return brush;
        }

        private double GetLineHeightInPixels(RichTextBox richTextBox) {
            FormattedText formattedText = new FormattedText(
                "Hello World!",
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(richTextBox.FontFamily, richTextBox.FontStyle, richTextBox.FontWeight, richTextBox.FontStretch),
                richTextBox.FontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            double lineHeight = formattedText.Height;

            return lineHeight;
        }

        /*private bool windowMaximized = false;
        private double[] sizeAndLocationSave = new double[4];
        private void WindowActions(object sender, RoutedEventArgs e) {
            if (sender == ExitButton) { this.Close(); } else if (sender == MaximizeButton) {
                switch (windowMaximized) {
                    case true:
                        this.WindowState = WindowState.Normal;
                        this.Width = sizeAndLocationSave[0];
                        this.Height = sizeAndLocationSave[1];
                        this.Left = sizeAndLocationSave[2];
                        this.Top = sizeAndLocationSave[3];
                        break;
                    default:
                        sizeAndLocationSave[0] = this.Width;
                        sizeAndLocationSave[1] = this.Height;
                        sizeAndLocationSave[2] = this.Left;
                        sizeAndLocationSave[3] = this.Top;
                        this.WindowState = WindowState.Normal;
                        this.Width = SystemParameters.WorkArea.Width;
                        this.Height = SystemParameters.WorkArea.Height;
                        this.Left = 0;
                        this.Top = 0;
                        break;
                }
                windowMaximized = !windowMaximized;
            } else if (sender == MinimizeButton) { this.WindowState = WindowState.Minimized; }
        }*/

        private void DragWindow(object sender, MouseButtonEventArgs e) {
            bool dragWindow = (e.ButtonState == MouseButtonState.Pressed) ? true : false;
            if (dragWindow) { this.DragMove(); }
        }
    }
}
