using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace volleyball_scout
{
    public partial class MatchAnalysisWindow : Window
    {
        private TextBox currentTextBox;
        private bool isEditing = false;

        private List<string> codes = new List<string>();

        public MatchAnalysisWindow()
        {
            InitializeComponent();
            CodesList.ItemsSource = codes;
        }

        private void Input_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.Key == Key.Enter)
            {
                codes.Insert(0, Input_1.Text);
                Input_1.Text = "";
                CodesList.Items.Refresh();
            }
        }

        private void AddCode(object sender, RoutedEventArgs e)
        {
            codes.Insert(0, Input_1.Text);
            Input_1.Text = "";
            CodesList.Items.Refresh();
        }

        private void DeleteCode(object sender, RoutedEventArgs e)
        {
            if (CodesList.SelectedItem != null)
            {
                codes.RemoveAt(CodesList.Items.IndexOf(CodesList.SelectedItem));
                CodesList.Items.Refresh();
            }
        }

      private void EditCode(object sender, RoutedEventArgs e)
{
    if (CodesList.SelectedItem != null && !isEditing)
    {
        // Storing the text to be replaced
        string previousText = CodesList.SelectedItem.ToString();

        // Create a new TextBox and add it to the Popup
        currentTextBox = new TextBox
        {
            Text = previousText,
            Width = CodesList.ActualWidth
        };

        // Get the container (ListBoxItem) of the selected item
        var selectedContainer = (UIElement)CodesList.ItemContainerGenerator.ContainerFromIndex(CodesList.SelectedIndex);

        if (selectedContainer != null)
        {
            // Calculate the screen coordinates of the selected item
            Point startPoint = selectedContainer.PointToScreen(new Point(0, 0));

            // Set the position of the Popup to overlay the selected ListBox item
            editPopup.HorizontalOffset = startPoint.X;
            editPopup.VerticalOffset = startPoint.Y;

            // Set the height of the TextBox to the ActualHeight of the ListBoxItem
            var listBoxItem = (ListBoxItem)selectedContainer;
            currentTextBox.Height = listBoxItem.ActualHeight;

            // Handle the TextBox's PreviewKeyDown event to save the edited entry when the Enter key is pressed
            currentTextBox.PreviewKeyDown += CurrentTextBox_PreviewKeyDown;

            // Add the TextBox to the Popup and show it
            editPopup.Child = currentTextBox;
            editPopup.IsOpen = true;

            // Set the focus to the new TextBox
            currentTextBox.Focus();
            isEditing = true;
        }
    }
}





        private void CurrentTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Save the edited entry and close the Popup
                string editedText = currentTextBox.Text;
                int selectedIndex = CodesList.SelectedIndex;

                if (!string.IsNullOrWhiteSpace(editedText))
                {
                    codes[selectedIndex] = editedText;
                    CodesList.Items.Refresh();
                }

                isEditing = false;
                editPopup.IsOpen = false;

                // Remove the PreviewKeyDown event handler to avoid multiple subscriptions
                currentTextBox.PreviewKeyDown -= CurrentTextBox_PreviewKeyDown;
            }
        }



        private void EditTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (isEditing)
            {
                // Save the edited entry and close the Popup
                string editedText = currentTextBox.Text;
                int selectedIndex = CodesList.SelectedIndex;

                if (!string.IsNullOrWhiteSpace(editedText))
                {
                    codes[selectedIndex] = editedText;
                    CodesList.Items.Refresh();
                }

                isEditing = false;
                editPopup.IsOpen = false;
            }
        }
    }
}
