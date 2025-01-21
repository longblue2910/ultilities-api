using GTranslatorAPI;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

[ApiController]
[Route("api/[controller]")]
public class TranslationController : ControllerBase
{
    [HttpPost("translate-excel")]
    public async Task<IActionResult> TranslateExcel([FromForm] TranslateClass request)
    {
        if (request.File == null || request.File.Length == 0)
        {
            return BadRequest("File không hợp lệ.");
        }

        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Đọc file Excel từ request
            using var stream = new MemoryStream();
            await request.File.CopyToAsync(stream);
            stream.Position = 0;

            // Xử lý file Excel bằng EPPlus
            using var package = new ExcelPackage(stream);
            foreach (var worksheet in package.Workbook?.Worksheets)
            {
                for (int row = 1; row <= worksheet?.Dimension?.End?.Row; row++)
                {
                    for (int col = 1; col <= worksheet?.Dimension?.End?.Column; col++)
                    {
                        // Kiểm tra xem ô hiện tại có thuộc vùng merge không
                        if (worksheet.Cells[row, col].Merge)
                        {
                            var mergedCellAddress = worksheet?.MergedCells[row, col]; // Lấy địa chỉ vùng merge
                            if (worksheet.Cells[mergedCellAddress]?.Start?.Row == row &&
                                worksheet.Cells[mergedCellAddress]?.Start?.Column == col)
                            {
                                var cellValue = worksheet.Cells[row, col]?.Text; // Lấy nội dung từ ô chính
                                if (!string.IsNullOrEmpty(cellValue) && GenerateJapaneseHashCode(cellValue) > 0)
                                {
                                    string translatedText = await TranslateTextAsync(cellValue);

                                    // Ghi nội dung tiếng Anh vào cột kế tiếp
                                    worksheet.Cells[row, col].Value = $"{cellValue} | {translatedText}";
                                }
                            }
                        }
                        else
                        {
                            var cellValue = worksheet.Cells[row, col]?.Text;
                            if (!string.IsNullOrEmpty(cellValue) && GenerateJapaneseHashCode(cellValue) > 0)
                            {
                                string translatedText = await TranslateTextAsync(cellValue);

                                // Ghi nội dung tiếng Anh vào cột kế tiếp
                                worksheet.Cells[row, col].Value = $"{cellValue} | {translatedText}";
                            }
                        }
                    }
                }
            }

            // Lưu file Excel đã dịch vào MemoryStream
            var outputStream = new MemoryStream();
            package.SaveAs(outputStream);
            outputStream.Position = 0;

            // Trả file Excel đã dịch về client
            return File(outputStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "translated_file.xlsx");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi trong quá trình xử lý: {ex.Message}");
        }
    }

    private async Task<string> TranslateTextAsync(string text)
    {
        var translator = new Translator(); // Sử dụng class Translator của bạn
        var result = await translator.TranslateAsync(Languages.ja, Languages.en, text);
        return result.TranslatedText;
    }

    private bool IsJapaneseChar(char c)
    {
        // Kiểm tra xem ký tự có thuộc tiếng Nhật hay không
        return (c >= '\u3040' && c <= '\u309F') || // Hiragana
               (c >= '\u30A0' && c <= '\u30FF') || // Katakana
               (c >= '\u4E00' && c <= '\u9FFF');   // Kanji
    }

    private int GenerateJapaneseHashCode(string text)
    {
        int hashCode = 0;

        foreach (char c in text)
        {
            if (IsJapaneseChar(c))
            {
                hashCode += c; // Sử dụng mã Unicode của ký tự để tạo hash
            }
        }

        return hashCode;
    }

}

public class TranslateClass
{
    public IFormFile File { get; set; }
}
