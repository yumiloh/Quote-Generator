namespace Quote_Generator.Service.Model
{
    public class GeneratePicOutputModel
    {
        private string outputPicturePath;
        public string OutputPicturePath
        {
            get
            {
                return outputPicturePath;
            }
            set
            {
                outputPicturePath = value;
            }
        }
    }
}