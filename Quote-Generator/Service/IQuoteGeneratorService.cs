using Quote_Generator.Model;

namespace Quote_Generator.Service
{
    public interface IQuoteGeneratorService
    {
        GeneratePicOutputModel GeneratePic(GeneratePicInputModel inputModel);
        ShareOutputModel SharePic(ShareInputModel shareInput);
    }
}