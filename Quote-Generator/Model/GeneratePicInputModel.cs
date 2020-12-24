using System;
using System.IO;

namespace Quote_Generator.Model 
{
    public class GeneratePicInputModel
    {
        private int xCoordinate, yCoordinate;
        private string inputPicturePath;  
        private int fontSize;
        public bool IsXValid { get; set; }
        public bool IsYValid { get; set; }
        public string Quote { get; set; }        
        public int FontSize
        {
            get
            {
                return fontSize;
            }                   
        }
        public void SetFontSize (string inputFontSize)
        {
            int localFontSize;
            if (!int.TryParse(inputFontSize, out localFontSize))
                throw new InvalidOperationException();
            if (localFontSize > 40 || localFontSize < 20)
                throw new InvalidDataException();
            else
                fontSize = localFontSize;
        }
        public string InputPicturePath
        { 
            get
            {
                return inputPicturePath;
            }
            set
            {
                if (File.Exists(value))
                {
                    inputPicturePath = value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
        public int XCoordinate
        {
            get
            {
                return xCoordinate;
            }
        }
        public void SetXCoordinate(string strXCoordinate, int width)
        {
            int coordinate;
            if (!int.TryParse(strXCoordinate, out coordinate))
                throw new InvalidOperationException();
            if (coordinate > width)
                throw new InvalidDataException();
            else
                xCoordinate = coordinate;
        }
        public int YCoordinate
        {
            get
            {
                return yCoordinate;
            }                      
        }
        public void SetYCoordinate (string strYCoordinate, int height)
        {
            int coordinate;
            if (!int.TryParse(strYCoordinate, out coordinate))
                throw new InvalidOperationException();
            if (coordinate > height)
                throw new InvalidDataException();
            else
                yCoordinate = coordinate;
        }
    }
}