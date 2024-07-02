using GTranslatorAPI;
using Microsoft.AspNetCore.Mvc;
using Utilities.API.Services;

namespace Utilities.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UtilityController : ControllerBase
{
    [HttpGet("covert-number-to-text/{number}")]
    public IActionResult CurrencyConverterAsync(int number)
    {
        // Origin text
        string vnText = CurrencyConverterService.NumberToWordsVietnamese(number);
        return Ok(vnText.CapitalizeFirstLetter());
    }


    [HttpGet("languages")]
    public IActionResult CurrencyConverterAsync()
    {
        List<LanguageResponse> result =
        [
            new LanguageResponse {Label = "Anh", Value = Languages.en},
            new LanguageResponse {Label = "Nhật", Value = Languages.ja}
        ];

        return Ok(result);
    }


    [HttpPost("translate")]
    public async Task<IActionResult> TranslateAsync(string text, Languages language)
    {
        //Translator
        var translator = new Translator();
        var result = await translator.TranslateAsync(Languages.vi, language, text);

        string resultText = result.TranslatedText;

        return Ok(resultText);
    }


    [HttpGet("covert-number-to-multi-text/{vnText}")]
    public async Task<IActionResult> CurrencyConverterMutilTextAsync(string vnText)
    {
        var response = new ConvertTextResponse();

        var translator = new Translator();

        var result = await translator.TranslateAsync(Languages.vi, Languages.en, vnText);
        response.EngText = result.TranslatedText;

        var resultJa = await translator.TranslateAsync(Languages.vi, Languages.ja, vnText);
        response.JaText = resultJa.TranslatedText;

        // Origin text
        return Ok(response);
    }
}

public class LanguageResponse
{
    public string Label { get; set; }
    public Languages Value { get; set; }
}


public class ConvertTextResponse
{
    public string EngText { get; set; }
    public string JaText { get; set; }

}