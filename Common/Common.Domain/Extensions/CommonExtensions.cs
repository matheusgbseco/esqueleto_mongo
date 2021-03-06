using Common.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static class CommonExtensions
{

    #region Defaults
    public static bool IsNotDefault(this bool value)
    {
        return value != default(bool);
    }
    public static bool IsNotDefault(this bool? value)
    {
        return value != default(bool?);
    }
    public static bool IsNotDefault(this decimal value)
    {
        return value != default(decimal);
    }
    public static bool IsNotDefault(this decimal? value)
    {
        return value != default(decimal?);
    }
    public static bool IsNotDefault(this DateTime value)
    {
        return value != default(DateTime);
    }
    public static bool IsNotDefault(this DateTime? value)
    {
        return value.IsNotNull() && value.Value != default(DateTime);
    }
    public static bool IsNotDefault(this byte value)
    {
        return !IsDefault(value);
    }
    public static bool IsNotDefault(this int value)
    {
        return !IsDefault(value);
    }
    public static bool IsNotDefault(this byte[] values)
    {
        return !IsDefault(values);
    }
    public static bool IsNotDefault(this int[] values)
    {
        return !IsDefault(values);
    }
    public static bool IsDefault(this byte[] values)
    {
        return values == default(byte[]);
    }
    public static bool IsDefault(this int[] values)
    {
        return values == default(int[]);
    }
    public static bool IsNotDefault(this byte? value)
    {
        return !IsDefault(value);
    }
    public static bool IsNotDefault(this int? value)
    {
        return !IsDefault(value);
    }
    public static bool IsDefault(this byte value)
    {
        return value == default(byte);
    }
    public static bool IsDefault(this int value)
    {
        return value == default(int);
    }
    public static bool IsDefault(this byte? value)
    {
        return value == default(byte?);
    }
    public static bool IsDefault(this int? value)
    {
        return value == default(int?);
    }
    public static bool IsNotNull(this object obj)
    {
        return obj != null;
    }
    #endregion

    #region Nulls

    public static bool IsNullOrEmpaty<T>(this IEnumerable<T> obj)
    {
        return obj.IsNull() || obj.Count() == 0;

    }
    public static bool IsNull(this object obj)
    {
        return obj == null;
    }
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }
    public static bool IsNotNullOrEmpty(this string value)
    {
        return !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
    }

    #endregion

    #region Ready

    public static bool IsReady(this int value)
    {
        return IsSent(value) && value > 0;
    }
    public static bool IsReady(this short value)
    {
        return IsSent(value) && value > 0;
    }

    public static bool IsReady(this Int64 value)
    {
        return IsSent(value) && value > 0;
    }

    public static bool IsReady(this Int64? value)
    {
        return IsSent(value) && value > 0;
    }

    #endregion

    #region Sents
    public static bool IsSent(this Guid? value)
    {
        return value != null && value != default(Guid?) && value != default(Guid);
    }
    public static bool IsSent(this Guid value)
    {
        return value != null && value != default(Guid);
    }
    public static bool IsSent(this short value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this Int64 value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this Int64? value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this Int16? value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this byte[] value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this byte? value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this byte value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this int value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this int[] values)
    {
        return IsNotDefault(values) && values.Length > 0;
    }
    public static bool IsSent(this decimal value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this decimal? value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this string value)
    {
        return IsNotNullOrEmpty(value);
    }
    public static bool IsSent(this float? value)
    {
        return value.IsNotNull();
    }
    public static bool IsSent(this float value)
    {
        return value.IsNotNull();
    }
    public static bool IsSent(this int? value)
    {
        return value.IsNotNull();
    }
    public static bool IsSent(this bool value)
    {
        return value.IsNotDefault();
    }
    public static bool IsSent(this bool? value)
    {
        return value.IsNotDefault();
    }
    public static bool IsSent(this DateTime value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this DateTime? value)
    {
        return value.IsNotNull();
    }
    #endregion

    #region IsNotSent
    public static bool IsNotSent(this string value)
    {
        return !value.IsSent() || value == " ";
    }
    #endregion

    #region Anys
    public static bool IsAny<T>(this ICollection<T> obj)
    {
        return obj.IsNotNull() && obj.Count() > 0;
    }
    public static bool IsAny<T>(this IEnumerable<T> obj)
    {
        return obj.IsNotNull() && obj.Count() > 0;
    }
    public static bool IsNotAny<T>(this ICollection<T> obj)
    {
        return !obj.IsAny();
    }
    public static bool IsNotAny<T>(this IEnumerable<T> obj)
    {
        return !obj.IsAny();
    }
    #endregion

    #region Comparative
    public static bool IsEqualZero(this int value)
    {
        return value == 0;
    }
    public static bool IsEqualZero(this decimal value)
    {
        return value == 0;
    }
    public static bool IsMoreThanZero(this decimal value)
    {
        return value > 0;
    }
    public static bool IsMoreThanZero(this int value)
    {
        return value > 0;
    }
    public static bool isNegative(this decimal value)
    {
        return value < 0;
    }
    public static bool isNegative(this int value)
    {
        return value < 0;
    }
    public static bool isPositive(this decimal value)
    {
        return value > 0;
    }
    public static bool isPositive(this int value)
    {
        return value > 0;
    }

    #endregion

    #region Convert
    public static string ToCurrency(this decimal value)
    {
        return string.Format("{0:N2}", value);
    }
    public static string ToCurrency(this decimal? value)
    {
        return string.Format("{0:N2}", value);
    }
    public static string ToPercentage(this decimal instance)
    {
        return (instance / 100).ToString("p");
    }
    public static string ToPercentage(this decimal? instance)
    {
        if (instance.IsNull())
            return "";
        else
            return ToPercentage(instance.Value);
    }
    public static DateTime ToDate(this string value)
    {
        try
        {
            DateTime result;
            DateTime.TryParse(value, out result);
            return result;
        }
        catch
        {
            return new DateTime();
        }
    }
    public static int ToInt(this string zipCode)
    {
        try
        {
            return Convert.ToInt32(zipCode);
        }
        catch
        {
            return 0;
        }
    }
    public static int ToInt(this decimal valor)
    {
        try
        {
            return Convert.ToInt32(valor);
        }
        catch
        {
            return 0;
        }
    }
    public static int ToInt(this decimal? valor)
    {
        try
        {
            return Convert.ToInt32(valor.Value);
        }
        catch
        {
            return 0;
        }
    }
    public static string ToUpperCase(this string str)
    {
        if (str == null)
            return null;

        if (str == string.Empty)
            return string.Empty;

        return str.ToUpper();


    }
    #endregion

    #region Types
    public static bool IsNumber(this string value)
    {
        return _IsNumber(value);
    }
    public static bool IsNumber(this object value)
    {
        return _IsNumber(value);
    }
    private static bool _IsNumber(object value)
    {
        if (value.IsNotNull() && value.ToString() == "-") return false;
        var valueString = Convert.ToString(value);
        var regex = new Regex(@"^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$");

        if (valueString.IsNullOrEmpaty())
            return false;

        if (regex.IsMatch(valueString.Replace(",", ".")))
            return !(valueString.Length > 29);

        return false;
    }
    public static bool IsEmail(this string value)
    {
        var regex = new Regex(@"[\w-]+@([\w-]+\.)+[\w-]+");
        return regex.IsMatch(value);
    }
    public static bool IsDigit(this string value)
    {
        foreach (var item in value)
        {
            if (!char.IsDigit(item))
                return false;
        }

        return true;

    }
    public static bool IsDate(this object value)
    {
        DateTime date;
        Decimal dec;

        var isDecimal = decimal.TryParse(Convert.ToString(value), out dec);

        if (!isDecimal)
            return DateTime.TryParse(Convert.ToString(value), out date);

        return false;
    }
    public static bool IsDate(this string value)
    {
        DateTime date;
        return DateTime.TryParse(value, out date);
    }
    #endregion

    #region Others
    public static string Right(this string value, int numeroCaracteres)
    {
        return value.Substring(value.Length - numeroCaracteres, numeroCaracteres);
    }

    public static T returnNotNull<T>(this T model)
    {
        var newInstance = Activator.CreateInstance(typeof(T));
        return model.IsNotNull() ? model : (T)newInstance;

    }

    public static string GenerateRandomCode(this string s, int lenght = 5)
    {
        Random r = new Random();

        for (int j = 0; j < lenght; j++)
        {
            int i = r.Next(3);
            int ch;
            switch (i)
            {
                case 1:
                    ch = r.Next(0, 9);
                    s = s + ch.ToString();
                    break;
                case 2:
                    ch = r.Next(65, 90);
                    s = s + Convert.ToChar(ch).ToString();
                    break;
                case 3:
                    ch = r.Next(97, 122);
                    s = s + Convert.ToChar(ch).ToString();
                    break;
                default:
                    ch = r.Next(97, 122);
                    s = s + Convert.ToChar(ch).ToString();
                    break;
            }
            r.NextDouble();
            r.Next(100, 1999);
        }
        return s;
    }

    public static string EncryptString(this string text, string keyString)
    {
        byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(text + "_" + keyString);
        string encrypted = Convert.ToBase64String(b);
        return encrypted;
    }

    public static string DecryptString(this string encrString, string keyString)
    {
        byte[] b;
        string decrypted;
        try
        {
            b = Convert.FromBase64String(encrString);
            decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
        }
        catch
        {
            decrypted = "";
        }
        return decrypted.Replace("_" + keyString, "");
    }

    public static bool IsStrongPassword(this string pass)
    {
        var temLetra = Regex.IsMatch(pass, @"([A-Za-z])");
        var temNumero = Regex.IsMatch(pass, @"\d");
        var temEspecial = Regex.IsMatch(pass, @"[!@#$%^&*()_+\-=\[\]{};':" + @"\|,.<>\/?]+");
        return temLetra && temNumero && temEspecial && pass.Length >= 6;
    }

    public static string FormataDocumento(this string _cpf_cnpj)
    {
        if (_cpf_cnpj.IsNotSent()) return string.Empty;
        if (_cpf_cnpj.Length == 11)
            return Regex.Replace(_cpf_cnpj, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");
        else
            return Regex.Replace(_cpf_cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");
    }

    #endregion

}

