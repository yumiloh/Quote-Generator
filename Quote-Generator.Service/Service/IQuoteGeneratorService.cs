using Quote_Generator.Service.Model;

namespace Quote_Generator.Service
{
    public interface IQuoteGeneratorService
    {
        GeneratePicOutputModel GeneratePic(GeneratePicInputModel inputModel);
        ShareOutputModel SharePic(ShareInputModel shareInput);
    }
}