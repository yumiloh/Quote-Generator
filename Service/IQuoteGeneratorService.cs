using System;
using Quote_Generator.Model;

namespace Quote_Generator.Service
{
    public interface IQuoteGeneratorService
{
    GeneratePicOutputModel GeneratePic(GeneratePicInputModel pictureInput);
    ShareOutputModel SharePic(ShareInputModel shareInput);

}
}