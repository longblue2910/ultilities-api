using System.Globalization;

namespace Utilities.API.Services;

public static class CurrencyConverterService
{
    public static string NumberToWordsVietnamese(double number)
    {
        if (number == 0)
            return "không đồng";

        if (number < 0)
            return "âm " + NumberToWordsVietnamese(Math.Abs(number));

        string[] units = { "", "nghìn", "triệu", "tỷ" };
        string[] digits = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };

        string words = "";
        int unitIndex = 0;

        while (number > 0)
        {
            int part = (int)(number % 1000);
            if (part > 0)
            {
                string partWords = ThreeDigitNumberToWords(part);
                if (unitIndex > 0)
                    partWords += " " + units[unitIndex];
                words = partWords + " " + words;
            }
            number /= 1000;
            unitIndex++;
        }

        return words.Trim() + " đồng";
    }

    public static string ThreeDigitNumberToWords(int number)
    {
        string[] digits = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };

        int hundred = number / 100;
        int ten = (number % 100) / 10;
        int unitDigit = number % 10;

        string words = "";

        if (hundred > 0)
        {
            words += digits[hundred] + " trăm";
            if (ten == 0 && unitDigit > 0)
            {
                words += " lẻ";
            }
        }

        if (ten > 1)
        {
            words += " " + digits[ten] + " mươi";
            if (unitDigit == 1)
            {
                words += " mốt";
            }
        }
        else if (ten == 1)
        {
            words += " mười";
        }

        if (unitDigit > 1 || (unitDigit == 1 && ten == 0))
        {
            words += " " + digits[unitDigit];
        }
        else if (unitDigit == 1 && ten > 1)
        {
            words += " mốt";
        }
        else if (unitDigit == 5 && ten > 0)
        {
            words += " lăm";
        }
        else if (unitDigit > 0)
        {
            words += " " + digits[unitDigit];
        }

        return words.Trim();
    }


    public static string CapitalizeFirstLetter(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        TextInfo textInfo = new CultureInfo("vi-VN", false).TextInfo;
        return textInfo.ToTitleCase(input);
    }


}
