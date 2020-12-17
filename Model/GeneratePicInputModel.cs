using System;

namespace Quote_Generator.Model 
{
    public class GeneratePicInputModel
    {
        private int picturePosition;
        public string Quote { get; set; }
        public string InputPicturePath { get; set; }
        public int PicturePosition
        {
            get
            {
                return picturePosition;
            }
            set
            {
                if (picturePosition < 1 || picturePosition > 9)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    picturePosition = value;
                }

            }
        } //enum or integer. validate user input

    }


}