using Microsoft.Win32;
using Quote_Generator.Service;
using Quote_Generator.Service.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;

namespace Quote_Generator_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        GeneratePicInputModel inputModel = new GeneratePicInputModel();
        IQuoteGeneratorService generateQuote = new QuoteGeneratorService();
        GeneratePicOutputModel outputModel = new GeneratePicOutputModel();
        ShareOutputModel shareOutputModel = new ShareOutputModel();
        ShareInputModel shareInputModel = new ShareInputModel();
        string previousPicturePath = "";
        List<string> sortedQuotesList = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Browse_Button_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpeg;*.png; *.jpg)|*.jpeg;*.png;*jpg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.ShowDialog();
            PicturePath.Text = openFileDialog.FileName;
            try
            {
                inputModel.InputPicturePath = openFileDialog.FileName;
                Image img = Image.FromFile(inputModel.InputPicturePath, true);
                int width = img.Width;
                int height = img.Height;
                img.Dispose();
                XCoordinateTextBlock.Text = ($"X-Coordinate (max width: {width}): ");
                YCoordinateTextBlock.Text = ($"Y-Coordinate (max height: {height}): ");
                XCoordinateInput.IsEnabled = true;
                YCoordinateInput.IsEnabled = true;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Please select a picture.");
            }            
        }

        private void Preview_Button_Click(object sender, RoutedEventArgs e)
        {
            string invalidCoordinates = "", invalidFontSize = "", invalidQuote = "", invalidPicture = "";
            if (string.IsNullOrEmpty(inputModel.InputPicturePath))
            {
                invalidPicture = "Please select a picture.";
            }
            else
            {
                invalidCoordinates = ValidateCoordinates();
            }

            invalidFontSize = ValidateFontSize();

            if (string.IsNullOrEmpty(InputQuoteText.Text))
            {
                invalidQuote = "Please input a quote.";
            }
            else
            {
                inputModel.Quote = InputQuoteText.Text;
            }

            if (!(String.IsNullOrEmpty(invalidPicture)) || !(String.IsNullOrEmpty(invalidCoordinates)) || !(String.IsNullOrEmpty(invalidFontSize)) || (!(String.IsNullOrEmpty(invalidQuote))))
            {
                MessageBox.Show(invalidPicture + "\n" + invalidQuote + "\n" + invalidCoordinates + "\n" + invalidFontSize);
            }
            else
            {
                toContinue.IsEnabled = true;
                ToConfirm.IsEnabled = true;

                if (File.Exists(previousPicturePath))
                {
                    File.Delete(previousPicturePath);
                }
                previousPicturePath = outputModel.OutputPicturePath;

                outputModel = generateQuote.GeneratePic(inputModel);
                OpenPic(outputModel.OutputPicturePath);
            }
        }
        public static void OpenPic(string filepath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"C:\Program Files\Internet Explorer\iexplore.exe"; // C:\WINDOWS\system32\mspaint.exe
            startInfo.Arguments = filepath;
            Process.Start(startInfo);
        }

        private void ToConfirm_Click(object sender, RoutedEventArgs e)
        {
            string selection = toContinue.Text;
            if (selection == "Yes")
            {
                EmailAddressInput.IsEnabled = true;
                EmailHeaderInput.IsEnabled = true;
                EmailBodyInput.IsEnabled = true;
                ShareViaEmail.IsEnabled = true;
                InputQuoteText.IsEnabled = false;
                XCoordinateInput.IsEnabled = false;
                YCoordinateInput.IsEnabled = false;
                FontSize.IsEnabled = false;
                toContinue.IsEnabled = false;
                ToConfirm.IsEnabled = false;
                Browse.IsEnabled = false;
                Preview.IsEnabled = false;
            }
            else
            {
                File.Delete(outputModel.OutputPicturePath);
                EmailAddressInput.IsEnabled = false;
                EmailHeaderInput.IsEnabled = false;
                EmailBodyInput.IsEnabled = false;
                toContinue.IsEnabled = false;
                ToConfirm.IsEnabled = false;
            }
        }
        private void ShareViaEmail_ButtonClicked(object sender, RoutedEventArgs e)
        {
            ShareViaEmail.IsEnabled = false;
            ((MainWindow)System.Windows.Application.Current.MainWindow).UpdateLayout();

            string invalidEmail = "", invalidEmailHeader = "", invalidEmailBody = "";

            if (string.IsNullOrEmpty(EmailAddressInput.Text))
            {
                invalidEmail = "Please enter your email address.";                
            }
            else
            {
                try
                {
                    shareInputModel.ReceiverEmail = EmailAddressInput.Text;
                }
                catch (InvalidDataException)
                {
                    invalidEmail = ("Please enter a valid email.");
                }
            }

            if (string.IsNullOrEmpty(EmailHeaderInput.Text))
            {
                invalidEmailHeader = "Please enter the email subject.";                
            }
            else
            {
                shareInputModel.EmailSubject = EmailHeaderInput.Text;
            }
            if (string.IsNullOrEmpty(EmailBodyInput.Text))
            {
                invalidEmailBody = "Please enter the email body.";                
            }
            else
            {
                shareInputModel.EmailBody = EmailBodyInput.Text;
            }

            if (!(String.IsNullOrEmpty(invalidEmail)) || !(String.IsNullOrEmpty(invalidEmailHeader)) || !(String.IsNullOrEmpty(invalidEmailBody)))
            {
                MessageBox.Show(invalidEmail + "\n" + invalidEmailHeader + "\n" + invalidEmailBody);
                ShareViaEmail.IsEnabled = true;
            }
            else
            {
                shareInputModel.OutputPicturePath = outputModel.OutputPicturePath;
                shareOutputModel = generateQuote.SharePic(shareInputModel);
                if (shareOutputModel.IsSuccessful)
                {
                    MessageBox.Show("Email sent!");
                    ShareViaEmail.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Email failed to send...");
                    ShareViaEmail.IsEnabled = true;
                }
            }
        }
        public string ValidateCoordinates()
        {
            Image img = Image.FromFile(inputModel.InputPicturePath, true);
            int width = img.Width;
            int height = img.Height;
            img.Dispose();

            string invalidCoordinates = "";
            try
            {
                inputModel.SetXCoordinate(XCoordinateInput.Text, width);
            }
            catch (InvalidOperationException)
            {
                invalidCoordinates = "Invalid coordinates. Enter a valid numerical value for x-coordinate and y-coordinate. ";
            }
            catch (InvalidDataException)
            {
                invalidCoordinates = "Invalid coordinates. Enter a valid numerical value for x-coordinate and y-coordinate. ";
            }

            try
            {
                inputModel.SetYCoordinate(YCoordinateInput.Text, height);
            }
            catch (InvalidOperationException)
            {
                invalidCoordinates = "Invalid coordinates. Enter a valid numerical value for x-coordinate and y-coordinate. ";
            }
            catch (InvalidDataException)
            {
                invalidCoordinates = "Invalid coordinates. Enter a valid numerical value for x-coordinate and y-coordinate. ";
            }

            return invalidCoordinates;
        }
        public string ValidateFontSize()
        {
            string invalidFontSize = "";

            try
            {
                inputModel.SetFontSize(FontSize.Text);
            }
            catch (InvalidOperationException)
            {
                invalidFontSize = "Invalid font size. Enter numerical value between 20 - 40. ";
            }
            catch (InvalidDataException)
            {
                invalidFontSize = "Invalid font size. Enter numerical value between 20 - 40. ";
            }
            return invalidFontSize;
        }

        private void Quote_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InputQuoteText.Text = (string)QuotesCombobox.SelectedItem;
        }
        public List<string> sortedQuotes()
        {
            string filePath = @".\QuotesWithoutSource.txt";
            List<string> quoteList = File.ReadLines(filePath).ToList();
            List<string> sortedQuotes = quoteList.OrderBy(q => q).ThenBy(q => q.Length).ToList();
            return sortedQuotes;
        }

        private void InputQuote_LostFocus(object sender, RoutedEventArgs e)
        {
            //List<string> sortedQuotesList = sortedQuotes();
            string quoteEntered = InputQuoteText.Text;
            List<string> searchedList = sortedQuotesList.Where(q => q.Contains(quoteEntered)).ToList();
            QuotesCombobox.ItemsSource = searchedList;            
        }
        private void quote_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int inputQuoteLength, remainingQuoteLength;
            inputQuoteLength = InputQuoteText.Text.Length;
            remainingQuoteLength = (65 - inputQuoteLength) ;            
            CharactersRemaining.Content = ( remainingQuoteLength.ToString() + " Characters Remaining");
        }        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sortedQuotesList = sortedQuotes();
            QuotesCombobox.ItemsSource = sortedQuotesList;
        }
    }
}
