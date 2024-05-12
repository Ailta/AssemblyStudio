using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace src {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();

        }

        List<Button> lineIndexes = new List<Button>();
        private void TextEditor_SelectionChanged(object sender, RoutedEventArgs e) {
            int[] caretPos = GetCaretPosition(TextEditor);
            int amountOfLines = GetTextLines(TextEditor).Length;

            LineIndexerGrid.Children.Clear();
            TextEditorHelperGrid.Children.Clear();
            lineIndexes.Clear();

            for (int i = 0; i < amountOfLines; i++) {
                double lineHeight = GetLineHeightInPixels(TextEditor, i);
                LineIndexerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(lineHeight, GridUnitType.Pixel) });
                TextEditorHelperGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(lineHeight, GridUnitType.Pixel) });

                double[] size = { 80, lineHeight };
                Thickness margin = new Thickness(0, 0, 0, 0);
                Thickness padding = new Thickness(0, 0, 20, 0);
                SolidColorBrush fontColor = ConvertHexToSolidColorBrush("#FFD8D8D8");
                TextBlock lineIndexText = GetTextBlock($"{lineHeight}  {i}", size, padding, margin, fontColor);
                Grid.SetRow(lineIndexText, i);
                
                Grid grid = new Grid();
                grid.BorderBrush = ConvertHexToSolidColorBrush("#FF606060");
                grid.BorderThickness = new Thickness(1);
                Grid.SetRow(grid, i);

                LineIndexerGrid.Children.Add(lineIndexText);
                TextEditorHelperGrid.Children.Add(grid);
            }
        }

        private double GetLineHeightInPixels(RichEditBox rEB, int line) {
            string text;
            rEB.Document.GetText(TextGetOptions.None, out text);
            int charIndex = 0;
            int lineIndex = 0;

            for (int i = 0; i < text.Length; i++) {
                char character = text[i];
                charIndex++;
                
                if (character == 13) {
                    lineIndex++;
                }

                if (charIndex == line) {
                    break;
                }
            }

            ITextRange textRange = rEB.Document.GetRange(lineIndex, lineIndex+1);
            int hit = 0;
            Rect size;
            textRange.GetRect(PointOptions.None, out size, out hit);

#if DEBUG
            //Debug.WriteLine(size.Height);
#endif

            return size.Height;
        }

        private int[] GetCaretPosition(RichEditBox rEB) {
            int lineIndex = 0;
            int columnIndex = 0;

            int caretPosition = rEB.Document.Selection.StartPosition;

            ITextRange test = rEB.Document.GetRange(0, caretPosition);
            for (int i = 0; i < test.Text.Length; i++) {
                char character = test.Text[i];
                columnIndex++;
                if (character == 13) { lineIndex++; columnIndex = 0; }
            }

#if DEBUG
            //Debug.WriteLine($"x: {columnIndex} y: {lineIndex}");
#endif

            return new int[] { columnIndex, lineIndex };
        }

        private string[] GetTextLines(RichEditBox rEB) {
            string text;
            rEB.Document.GetText(TextGetOptions.None, out text);
            string[] lines = { };
            string currentLine = "";

            for (int i = 0; i < text.Length; i++) {
                char character = text[i];

                if (character == 13) {
                    Array.Resize(ref lines, lines.Length + 1);
                    lines[lines.Length - 1] = currentLine;
                    currentLine = "";
                    continue;
                }
                currentLine += character;
            }

#if DEBUG
            //Debug.WriteLine(lines.Length);
#endif

            return lines;
        }

        private TextBlock GetTextBlock(string text, double[] size, Thickness padding, Thickness margin, SolidColorBrush fontColor) {
            TextBlock textBlock = new TextBlock();

            textBlock.Text = text;
            textBlock.Height = size[1];
            textBlock.Width = size[0];
            textBlock.Margin = margin;
            textBlock.Padding = padding;
            textBlock.Foreground = fontColor;
            textBlock.TextAlignment = TextAlignment.Right;

            return textBlock;
        }

        public SolidColorBrush ConvertHexToSolidColorBrush(string hex) {
            hex = hex.Replace("#", string.Empty);

            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));

            SolidColorBrush brush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));

            return brush;
        }

        private void TextEditor_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e) {
            Debug.WriteLine("test");
        }
    }
}
