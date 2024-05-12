using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AssemblyStudio
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

        List<Label> labelsLineIndex = new List<Label>();
        private int fontSize = 15;
        private int lineHeight = 10;
        private double[] gridSize = { 0, 0 };
        private void setTextEditor()
        {
            gridSize[0] = textEditor.ActualWidth;
            gridSize[1] = textEditor.ActualHeight;

            SolidColorBrush fontColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6D6D6"));
            double rowHeight = fontSize+(fontSize-lineHeight);
            int rows = textEditor.RowDefinitions.Count();

            // Set style to a richTextBox
            {
                richTextBox.Padding = new Thickness(0);
                richTextBox.Margin = new Thickness(0);
                richTextBox.AcceptsReturn = false;
                richTextBox.AcceptsTab = true;
                richTextBox.TextChanged += editTextEditor;
                richTextBox.FontSize = fontSize;
                richTextBox.AppendText(" ");

                foreach (Paragraph paragraph in richTextBox.Document.Blocks)
                {
                    paragraph.Margin = new Thickness(0);
                    paragraph.Padding = new Thickness(0);
                    paragraph.LineHeight = lineHeight;
                }
            }
        }

        private int prevLineCount = 0;
        private void editTextEditor(object? sender, RoutedEventArgs? e)
        {
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            string[] lines = textRange.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            if (prevLineCount != lines.Length - 1)
            {
                SolidColorBrush fontColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF666666"));
                double rowHeight = GetLineHeightInPixels(richTextBox);
                
                lineIndex.Children.Clear();
                lineIndex.RowDefinitions.Clear();
                textBox.RowDefinitions.Clear();
                labelsLineIndex.Clear();

                for (int i = 0; i < lines.Length-1; i++)
                {
                    // Add row
                    lineIndex.RowDefinitions.Add(new RowDefinition { Height = new GridLength(rowHeight, GridUnitType.Pixel) });
                    textBox.RowDefinitions.Add(new RowDefinition { Height = new GridLength(rowHeight, GridUnitType.Pixel) });

                    // Create and add labelIndex
                    var label = createLabel(fontColor, rowHeight, i);
                    lineIndex.Children.Add(label);
                    labelsLineIndex.Add(label);
                }
            }
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender == lineIndexScroll) { 
                richTextBox.ScrollToVerticalOffset(e.VerticalOffset);
                textBoxScroll.ScrollToVerticalOffset(e.VerticalOffset);
            }
            if (sender == richTextBox) { 
                lineIndexScroll.ScrollToVerticalOffset(e.VerticalOffset);
                textBoxScroll.ScrollToVerticalOffset(e.VerticalOffset);
            }
        }

        private double GetLineHeightInPixels(RichTextBox richTextBox)
        {
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

        private Label createLabel(SolidColorBrush fontColor, double rowHeight, int line)
        {
            Label label = new Label();
            {
                label.Content = line;
                label.FontSize = fontSize;
                label.Foreground = fontColor;
                label.Height = rowHeight;
                label.Padding = new Thickness(0);
                label.Margin = new Thickness(0);
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.HorizontalAlignment = HorizontalAlignment.Stretch;
                label.VerticalAlignment = VerticalAlignment.Stretch;
                label.BorderBrush = null;
                label.BorderThickness = new Thickness(1);
                label.FontFamily = new FontFamily("Consolas");
                //label.FontWeight = FontWeights.Black;

                Grid.SetRow(label, line);
            }
            return label;
        }

        private Border createBorder(SolidColorBrush fontColor, double rowHeight, int line)
        {
            Border border = new Border();
            border.Height = rowHeight;
            border.BorderThickness = new Thickness(2);
            border.BorderBrush = fontColor;

            Grid.SetRow(border, line);

            return border;
        }

        private void SelectionChanged(object? sender, RoutedEventArgs? e)
        {
            textBox.Children.Clear();

            // Get the current caret position
            TextPointer caretPos = richTextBox.CaretPosition;

            // Get the starting position of the line where the caret is positioned
            TextPointer lineStartPos = caretPos.GetLineStartPosition(0);

            // Calculate the line number by counting the number of lines from the start of the document
            int lineNumber = 0;
            TextPointer position = richTextBox.Document.ContentStart;
            while (position != null && position.CompareTo(lineStartPos) < 0)
            {
                if ((position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart) && (position.Parent is Paragraph))
                {
                    lineNumber++;
                }
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
            SolidColorBrush fontColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4D4D4D"));
            double rowHeight = GetLineHeightInPixels(richTextBox);

            textBox.Children.Add(createBorder(fontColor, rowHeight, lineNumber));

            editTextEditor(null, null);

            fontColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6D6D6"));
            Label tmpLabel = labelsLineIndex[lineNumber];
            tmpLabel.Foreground = fontColor;
            lineIndex.Children[lineNumber] = tmpLabel;
        }

        private void richTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.Key == Key.Enter)
            {
                // Create a new paragraph and insert it after the current paragraph
                Paragraph newParagraph = new Paragraph(new Run(""));
                newParagraph.Margin = new Thickness(0);
                newParagraph.Padding = new Thickness(0);
                newParagraph.LineHeight = lineHeight;

                richTextBox.CaretPosition = newParagraph.ContentStart;

                e.Handled = true;
            }*/
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setTextEditor();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Debug.WriteLine("hello?");
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Debug.WriteLine("bye?");
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e) {
            Debug.WriteLine("f");
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Debug.WriteLine("hello?");
        }
    }
}