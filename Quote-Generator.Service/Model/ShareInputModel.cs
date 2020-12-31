using System;
using System.IO;
using System.Text.RegularExpressions;


namespace Quote_Generator.Service.Model
{
    public class ShareInputModel
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        private string receiverEmail;
        public string OutputPicturePath { get; set; }
        public string ReceiverEmail
        {   get
            {
                return receiverEmail;
            }
            set
            {
                if (regex.IsMatch(value))
                {
                    receiverEmail = value;
                }
                else
                {
                    throw new InvalidDataException();                     
                }                 
            }
        }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}