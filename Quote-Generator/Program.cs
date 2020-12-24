using Quote_Generator.Model;
using Quote_Generator.Service;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Quote_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                bool toContinue;                
                GeneratePicInputModel inputModel = new GeneratePicInputModel();
                ShareInputModel shareInputModel = new ShareInputModel();
                IQuoteGeneratorService generateQuote = new QuoteGeneratorService();
                GeneratePicOutputModel outputModel;
                ShareOutputModel shareOutputModel;

                do
                {
                    validateImagePath(ref inputModel);
                    Image img = Image.FromFile(inputModel.InputPicturePath, true);
                    int width = img.Width;
                    int height = img.Height;
                    img.Dispose();
                    userInput(width, height, ref inputModel);                    
                    outputModel = generateQuote.GeneratePic(inputModel);
                    OpenPic(outputModel.OutputPicturePath);
                    toContinue = validFinalConfirmation();
                    if (toContinue)
                    {
                        File.Delete(outputModel.OutputPicturePath);
                        Console.Clear();
                    }                                       
                } while (toContinue);

                shareInputModel.OutputPicturePath = outputModel.OutputPicturePath;

                getUserInput(ref shareInputModel);
                shareOutputModel =generateQuote.SharePic(shareInputModel);
                
                if (shareOutputModel.IsSuccessful)
                {
                    Console.WriteLine("Email sent!");
                }
                else
                {
                    Console.WriteLine("Email failed to send...");
                }                
            }
            catch (Exception)   
            {
                Console.WriteLine("An error has occurred");
                // TODO: Log exception details into log file
            }

        }

        public static void userInput(int width, int height, ref GeneratePicInputModel inputModel)//out string quote, out int xCoordinate, out int yCoordinate, out int fontSize)
        {
            Console.Write("Enter quotation here: ");
            inputModel.Quote = Console.ReadLine();
            validateFontSize(inputModel);
            validatePosition(width, height, ref inputModel);
        }

        public static void validatePosition(int width, int height, ref GeneratePicInputModel inputModel)
        {
            bool isXValid, isYValid;
            Console.Write($"Enter x-coordinate [ < {width} ]: ");

            do
            {
                try
                {
                    inputModel.SetXCoordinate(Console.ReadLine(), width);
                    isXValid = true;
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine($"Please enter a numerical value: ");
                    isXValid = false;
                }
                catch (InvalidDataException)
                {
                    Console.WriteLine($"Please enter a valid numerical value [ x < {width} ]: ");
                    isXValid = false;
                }

            } while (!isXValid);

            Console.Write($"Enter y-coordinate [ < {height} ]: ");
            do
            {
                try
                {
                    inputModel.SetYCoordinate(Console.ReadLine(), height);
                    isYValid = true;
                }
                catch (InvalidOperationException)
                {
                    isYValid = false;
                }
                catch (InvalidDataException)
                {
                    Console.WriteLine($"Please enter a valid numerical value [ x < {height}: ");
                    isYValid = false;
                }
            } while (!isYValid);
        }
        public static void validateFontSize(GeneratePicInputModel inputModel)
        {
            bool isFontSizeValid = false;
            Console.Write("Enter font size [20 - 40] : ");            
            do
            {
                try
                {
                    inputModel.SetFontSize(Console.ReadLine());
                    isFontSizeValid = true;
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Invalid input. Enter numerical value: ");
                    isFontSizeValid = false;
                }
                catch (InvalidDataException)
                {
                    Console.WriteLine("Invalid input. Enter numerical value from 20 - 40: ");
                    isFontSizeValid = false;
                }
            } while (!isFontSizeValid);
        }
        public static void validateImagePath(ref GeneratePicInputModel inputModel)
        {
            bool isPathTrue = false;
            Console.WriteLine("Input picture path/ drag picture into this window: ");

            do
            {
                try
                {
                    inputModel.InputPicturePath = Console.ReadLine();
                    isPathTrue = true;
                }
                catch (InvalidOperationException)
                {
                    isPathTrue = false;
                }

                if (!isPathTrue)
                {
                    Console.WriteLine("File not found. Please input picture path / drag picture into this window: ");
                }
            } while (!isPathTrue);
        }
        public static bool validFinalConfirmation()
        {
            bool toContinue = false;
            bool isInputValid = true;
            Console.Write("Is this the picture you want? (yes/no): ");
            do
            {
                string reply = Console.ReadLine();
                if (reply == "yes")
                {
                    toContinue = false;
                    isInputValid = true;
                }
                else if (reply == "no")
                {
                    toContinue = true;
                    isInputValid = true;
                }
                else
                {
                    Console.WriteLine("Input invalid. Is this the picture you want ? (yes / no) : ");
                    isInputValid = false;
                }
            } while (!isInputValid);
            return toContinue;
        }
        public static void OpenPic(string filepath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"C:\Program Files\Internet Explorer\iexplore.exe"; // C:\WINDOWS\system32\mspaint.exe
            startInfo.Arguments = filepath;
            Process.Start(startInfo);
        }
        public static void getUserInput(ref ShareInputModel shareInputModel)
        {
            validateReceiverEmail(shareInputModel);

            Console.WriteLine("Enter email subject: ");
            shareInputModel.EmailSubject = Console.ReadLine();
            Console.WriteLine("Enter email body: ");
            shareInputModel.EmailBody = Console.ReadLine();
        }
        public static void validateReceiverEmail(ShareInputModel shareInputModel)
        {
            bool isEmailValid = false;
            Console.WriteLine("Enter receiver email: ");
            do
            {
                try
                {
                    shareInputModel.ReceiverEmail = Console.ReadLine();
                    isEmailValid = true;
                }
                catch (InvalidDataException)
                {
                    isEmailValid = false;
                }
                if (!isEmailValid)
                {
                    Console.WriteLine("Invalid email. Please input again: ");
                }
            } while (!isEmailValid);
        }
    }
}








