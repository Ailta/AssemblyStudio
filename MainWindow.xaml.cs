using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AssemblyStudio {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        List<Label> labelsLineIndex = new List<Label>();
        private int fontSize = 15;
        private int lineHeight = 10;

        public MainWindow() {
            InitializeComponent();
            prepareStyles();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            TextEditor.FontSize = fontSize;
            TextEditor.AppendText(" ");

            foreach (Paragraph paragraph in TextEditor.Document.Blocks) {
                paragraph.Margin = new Thickness(0);
                paragraph.Padding = new Thickness(0);
                paragraph.LineHeight = lineHeight;
            }
        }

        private int prevLine = 0;
        private void EditTextEditor(object? sender, RoutedEventArgs? e) {
            TextRange textRange = new TextRange(TextEditor.Document.ContentStart, TextEditor.Document.ContentEnd);
            string[] lines = textRange.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            if (((prevLine != lines.Length) && sender != null) || sender == null) {
                prevLine = lines.Length;
                double rowHeight = GetLineHeightInPixels(TextEditor);

                LineIndexGrid.Children.Clear();
                LineIndexGrid.RowDefinitions.Clear();
                TextEditorHelperGrid.RowDefinitions.Clear();
                labelsLineIndex.Clear();

                for (int i = 0; i < lines.Length - 1; i++) {
                    LineIndexGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(rowHeight, GridUnitType.Pixel) });
                    TextEditorHelperGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(rowHeight, GridUnitType.Pixel) });

                    Label label = createLabel($"{i}", fontSize, fontColorDarker, [0, rowHeight], new Thickness(0, 0, 10, 0), new Thickness(0));
                    Grid.SetRow(label, i);

                    LineIndexGrid.Children.Add(label);
                    labelsLineIndex.Add(label);
                }
            }
        }

        private void SelectionChanged(object? sender, RoutedEventArgs? e) {
            TextEditorHelperGrid.Children.Clear();

            int caretLinePos = GetCaretPosition(TextEditor);
            double rowHeight = GetLineHeightInPixels(TextEditor);
            TextEditorHelperGrid.Children.Add(createBorder(fontColorSubtle, rowHeight, caretLinePos));

            EditTextEditor(null, null);

            Label tmpLabel = labelsLineIndex[caretLinePos];
            tmpLabel.Foreground = fontColorLight;
            LineIndexGrid.Children[caretLinePos] = tmpLabel;
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e) {
            if (sender == LineIndexScrollViewer) {
                TextEditor.ScrollToVerticalOffset(e.VerticalOffset);
                TextEditorHelperScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
            }
            if (sender == TextEditor) {
                LineIndexScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
                TextEditorHelperScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
            }
        }
    }
}