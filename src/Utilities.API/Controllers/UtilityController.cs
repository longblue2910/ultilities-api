using GTranslatorAPI;
using Microsoft.AspNetCore.Mvc;
using Utilities.API.Services;

namespace Utilities.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UtilityController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> CurrencyConverterAsync(int number)
    {
        CurencyConverterResponse response = new();

        // Origin text
        var vnText = CurrencyConverterService.NumberToWordsVietnamese(number);

        response.VietNameseText = vnText.CapitalizeFirstLetter();

        var translator = new Translator();

        var result = await translator.TranslateAsync(Languages.vi, Languages.en, response.VietNameseText);
        response.EnglishText = result.TranslatedText;

        var resultJa = await translator.TranslateAsync(Languages.vi, Languages.ja, response.VietNameseText);
        response.JapaneseText = resultJa.TranslatedText;

        return Ok(response);
    }


}

public class CurencyConverterResponse
{
    public string VietNameseText { get; set; } 
    public string EnglishText { get; set; }
    public string JapaneseText { get; set; }

}
