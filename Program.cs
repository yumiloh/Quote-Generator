using System;
using Quote_Generator.Model;
using Quote_Generator.Service;

namespace Quote_Generator
{
    class Program
    {
        static IQuoteGeneratorService quoteGenerator = new QuoteGeneratorService();  //service instance

        static void Main(string[] args)

        {
            string quote = Console.ReadLine();
            string inputPicturePath = Console.ReadLine();
            int picPosition;
            bool isInteger = false;
            //bool isInteger= int.TryParse(Console.ReadLine(), out picPosition);
            
            //while loop to validate position
            while (!isInteger)
            {
                Console.WriteLine("Enter an integer:");
                isInteger = int.TryParse(Console.ReadLine(), out picPosition);

                if (isInteger==true)
                {
                    break;
                }
                
            }


            GeneratePicInputModel generatePicModel = new GeneratePicInputModel();
            generatePicModel.Quote = quote;
            generatePicModel.InputPicturePath = inputPicturePath;
            generatePicModel.PicturePosition = picPosition;

            var outputPic = quoteGenerator.GeneratePic(generatePicModel);  //quoteGenerator is an interface. outputpic is the return type which is the model. pass in model and return model
            
            string accName = Console.ReadLine();
            string email = Console.ReadLine();

            ShareInputModel shareInputModel = new ShareInputModel();      
            shareInputModel.AccountName = accName;  
            shareInputModel.Email = email;

            shareInputModel.OutputPicturePath = outputPic.OutputPicturePath;

            var shareOutput =  quoteGenerator.SharePic(shareInputModel);
            Console.WriteLine(shareOutput.IsSuccessful);

        }
    }
}




