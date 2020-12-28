using MailKit.Net.Smtp;
using MimeKit;
using Quote_Generator.Service.Model;
using System;
using System.Drawing;
using System.IO;

namespace Quote_Generator.Service
{
    public class QuoteGeneratorService : IQuoteGeneratorService
    {
        private const string SENDER_EMAIL = "janedoe.orsted@gmail.com";
        private string getImagePath(string imagePath)
        {
            string targetImagePath = Path.GetDirectoryName(imagePath);
            targetImagePath += "\\";
            targetImagePath += Path.GetFileNameWithoutExtension(imagePath) + "_withquote";
            targetImagePath += Path.GetExtension(imagePath);
            return targetImagePath;
        }
        public GeneratePicOutputModel GeneratePic(GeneratePicInputModel inputModel)
        {
            GeneratePicOutputModel outputModel = new GeneratePicOutputModel();
            Image img = Image.FromFile(inputModel.InputPicturePath, true);

            outputModel.OutputPicturePath = getImagePath(inputModel.InputPicturePath);

            Point quoteStartingPoint = new Point(inputModel.XCoordinate, inputModel.YCoordinate);
            Font quoteFont = new Font("Times New Roman", inputModel.FontSize, FontStyle.Bold, GraphicsUnit.Pixel);

            SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0, 0));
            Pen blackpen = new Pen(Color.Black, 1.0F);
            SolidBrush fillRectangleBrush = new SolidBrush(Color.FromArgb(180, 255, 255, 255));

            Graphics graphics = Graphics.FromImage(img);

            SizeF quoteSize = new SizeF();
            quoteSize = graphics.MeasureString(inputModel.Quote, quoteFont);

            Rectangle rect = new Rectangle(inputModel.XCoordinate, inputModel.YCoordinate, Convert.ToInt32(quoteSize.Width), Convert.ToInt32(quoteSize.Height));

            graphics.DrawRectangle(blackpen, rect);
            graphics.FillRectangle(fillRectangleBrush, rect);
            graphics.DrawString(inputModel.Quote, quoteFont, brush, rect);

            graphics.Dispose();
            brush.Dispose();
            blackpen.Dispose();
            fillRectangleBrush.Dispose();
            quoteFont.Dispose();

            img.Save(outputModel.OutputPicturePath);
            img.Dispose();

            return outputModel;
        }
        public ShareOutputModel SharePic(ShareInputModel shareInput)
        {
            ShareOutputModel shareOutputModel = new ShareOutputModel();
            GeneratePicOutputModel outputModel = new GeneratePicOutputModel();
            MimeMessage email = generateEmail(shareInput);
            try
            {
                sendEmail(email);
                shareOutputModel.IsSuccessful = true;
            }
            catch (Exception)
            {
                shareOutputModel.IsSuccessful = false;
            }            
            return shareOutputModel;
        } 
        public MimeMessage generateEmail(ShareInputModel shareInput)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(SENDER_EMAIL));
            email.To.Add(MailboxAddress.Parse(shareInput.ReceiverEmail));
            email.Subject = shareInput.EmailSubject;

            var builder = new BodyBuilder();
            builder.TextBody = shareInput.EmailBody;
            builder.Attachments.Add(shareInput.OutputPicturePath);

            email.Body = builder.ToMessageBody();
            return email;
        }
        public static void sendEmail(MimeMessage email)
        {
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 465, true);
            smtp.Authenticate("janedoe.orsted@gmail.com", "Test987*");
            smtp.AuthenticationMechanisms.Remove("XOAUTH");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}