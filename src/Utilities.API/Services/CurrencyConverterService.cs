using System.Globalization;

namespace Utilities.API.Services;

public static class CurrencyConverterService
{
    public static string NumberToWordsVietnamese(ulong number)
    {
        if (number == 0)
            return "không đồng";

        string[] units = { "", "nghìn", "triệu", "tỷ", "nghìn tỷ", "triệu tỷ", "tỷ tỷ" };
        string[] digits = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };

        string words = "";
        int unitIndex = 0;

        while (number > 0)
        {
            ulong part = number % 1000;
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

    private static string ThreeDigitNumberToWords(ulong number)
    {
        string[] digits = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };

        string words = "";

        if (number > 99)
        {
            words += digits[number / 100] + " trăm ";
            number %= 100;
        }

        if (number > 19)
        {
            words += digits[number / 10] + " mươi ";
            number %= 10;
            if (number > 0)
            {
                if (number == 1)
                    words += "mốt";
                else if (number == 5)
                    words += "lăm";
                else
                    words += digits[number];
            }
        }
        else if (number > 0)
        {
            if (number < 10)
                words += digits[number];
            else if (number < 20)
                words += "mười " + digits[number - 10];
        }

        return words.Trim();
    }

    public static string ThreeDigitNumberToWords(long number)
    {
        string[] digits = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };

        long hundred = number / 100;
        long ten = (number % 100) / 10;
        long unitDigit = number % 10;

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


    public static string CapitalizeLetter(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        TextInfo textInfo = new CultureInfo("vi-VN", false).TextInfo;
        return textInfo.ToTitleCase(input);
    }

    public static string CapitalizeFirstLetter(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }

}
