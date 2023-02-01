using System;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

public class DataAccess
{
    public static bool IsNullOrEmpty(object value)
    {
        try
        {
            if (value == null || value is DBNull)
            {
                return true;
            }
            if (value.GetType().Equals(Type.GetType("System.String")))
            {
                return string.IsNullOrEmpty(value.ToString());
            }
            if (value.GetType().Equals(Type.GetType("System.Boolean")))
            {
                return !bool.Parse(value.ToString());
            }
            if (value.GetType().Equals(Type.GetType("System.Byte")))
            {
                return byte.Parse(value.ToString()) == 0;
            }
            if (value.GetType().Equals(Type.GetType("System.SByte")))
            {
                return sbyte.Parse(value.ToString()) == sbyte.MinValue;
            }
            if (value.GetType().Equals(Type.GetType("System.Int16")))
            {
                return short.Parse(value.ToString()) == short.MinValue;
            }
            if (value.GetType().Equals(Type.GetType("System.Int32")))
            {
                return int.Parse(value.ToString()) == int.MinValue;
            }
            if (value.GetType().Equals(Type.GetType("System.Int64")))
            {
                return long.Parse(value.ToString()) == long.MinValue;
            }
            if (value.GetType().Equals(Type.GetType("System.UInt16")))
            {
                return ushort.Parse(value.ToString()) == 0;
            }
            if (value.GetType().Equals(Type.GetType("System.UInt32")))
            {
                return uint.Parse(value.ToString()) == 0;
            }
            if (value.GetType().Equals(Type.GetType("System.UInt64")))
            {
                return ulong.Parse(value.ToString()) == 0;
            }
            if (value.GetType().Equals(Type.GetType("System.Single")))
            {
                return float.Parse(value.ToString()) == float.MinValue;
            }
            if (value.GetType().Equals(Type.GetType("System.Double")))
            {
                return double.Parse(value.ToString()) == double.MinValue;
            }
            if (value.GetType().Equals(Type.GetType("System.Decimal")))
            {
                return decimal.Parse(value.ToString()) == decimal.MinValue;
            }
            if (value.GetType().Equals(Type.GetType("System.DateTime")))
            {
                return DateTime.Parse(value.ToString()) == DateTime.MinValue;
            }
            if (value.GetType().Equals(Type.GetType("System.Char")))
            {
                return char.Parse(value.ToString()) == '\0';
            }
            if (value.GetType().Equals(Type.GetType("System.Guid")))
            {
                return (Guid)value == Guid.Empty;
            }
        }
        catch
        {
            return false;
        }
        return false;
    }

    public static void CorrectSql(DbParameter dataParameter, object value)
    {
        try
        {
            if (value is byte[])
            {
                byte[] array = (byte[])value;
                if (array == null || array.Length == 0)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value is ushort)
            {
                if ((ushort)value == 0)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value is uint)
            {
                if ((uint)value == 0)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value is ulong)
            {
                ulong num = (ulong)value;
                if (num == 0)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value is short)
            {
                short num2 = (short)value;
                if (num2 == short.MinValue)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value is int)
            {
                int num3 = (int)value;
                if (num3 == int.MinValue)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value is long)
            {
                long num4 = (long)value;
                if (num4 == long.MinValue)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value is float)
            {
                float num5 = (float)value;
                if (num5 == float.MinValue)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value is double)
            {
                double num6 = (double)value;
                if (num6 == double.MinValue)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value is decimal)
            {
                decimal num7 = (decimal)value;
                if (num7 == decimal.MinValue)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value is Guid)
            {
                Guid guid = (Guid)value;
                if (guid == Guid.Empty)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value is DateTime)
            {
                DateTime dateTime = (DateTime)value;
                if (dateTime == DateTime.MinValue)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value is string)
            {
                string text = (string)value;
                if (text == string.Empty)
                {
                    dataParameter.Value = DBNull.Value;
                }
                else
                {
                    dataParameter.Value = value;
                }
            }
            else if (value == null)
            {
                dataParameter.Value = DBNull.Value;
            }
            else
            {
                dataParameter.Value = value;
            }
        }
        catch
        {
            throw;
        }
    }

    public static void CorrectValue(object obj, PropertyInfo propertyInfo, object value)
    {
        switch (propertyInfo.PropertyType.Name.ToLower())
        {
            case "boolean":
            case "bool":
                propertyInfo.SetValue(obj, CorrectValue(value, returnValue: false), null);
                break;
            case "string":
                if (value == null || value == DBNull.Value)
                {
                    propertyInfo.SetValue(obj, string.Empty, null);
                }
                else
                {
                    propertyInfo.SetValue(obj, value, null);
                }
                break;
            case "date":
            case "datetime":
                propertyInfo.SetValue(obj, CorrectValue(value, DateTime.MinValue), null);
                break;
            case "byte[]":
                propertyInfo.SetValue(obj, CorrectValue(value, new byte[0]), null);
                break;
            case "byte":
                propertyInfo.SetValue(obj, CorrectValue(value, (byte)0, invariantCulture: true), null);
                break;
            case "uint16":
                propertyInfo.SetValue(obj, CorrectValue(value, (ushort)0, invariantCulture: true), null);
                break;
            case "uint32":
                propertyInfo.SetValue(obj, CorrectValue(value, 0u), null);
                break;
            case "uint64":
                propertyInfo.SetValue(obj, CorrectValue(value, 0u), null);
                break;
            case "int16":
                propertyInfo.SetValue(obj, CorrectValue(value, short.MinValue), null);
                break;
            case "int32":
                propertyInfo.SetValue(obj, CorrectValue(value, int.MinValue), null);
                break;
            case "int64":
                propertyInfo.SetValue(obj, CorrectValue(value, long.MinValue), null);
                break;
            case "single":
                propertyInfo.SetValue(obj, CorrectValue(value, float.MinValue), null);
                break;
            case "double":
                propertyInfo.SetValue(obj, CorrectValue(value, double.MinValue), null);
                break;
            case "decimal":
                propertyInfo.SetValue(obj, CorrectValue(value, decimal.MinValue), null);
                break;
            case "guid":
                propertyInfo.SetValue(obj, CorrectValue(value, Guid.Empty), null);
                break;
        }
    }

    public static object CorrectValue(object objSource, object returnValue)
    {
        object obj = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return objSource;
        }
        catch
        {
            throw;
        }
    }

    public static string CorrectValue(object objSource, string returnValue, bool invariantCulture = true)
    {
        string text = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? string.Format(CultureInfo.InvariantCulture, "{0}", objSource) : objSource.ToString();
        }
        catch
        {
            throw;
        }
    }

    public static bool CorrectValue(object objSource, bool returnValue, bool invariantCulture = true)
    {
        bool flag = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            if (objSource.ToString() == "0")
            {
                return false;
            }
            if (objSource.ToString() == "1")
            {
                return true;
            }
            return invariantCulture ? Convert.ToBoolean(objSource, CultureInfo.InvariantCulture) : Convert.ToBoolean(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static byte CorrectValue(object objSource, byte returnValue, bool invariantCulture = true)
    {
        byte b = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToByte(objSource, CultureInfo.InvariantCulture) : Convert.ToByte(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static byte[] CorrectValue(object objSource, byte[] returnValue)
    {
        byte[] array = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return objSource as byte[];
        }
        catch
        {
            throw;
        }
    }

    public static sbyte CorrectValue(object objSource, sbyte returnValue, bool invariantCulture = true)
    {
        sbyte b = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToSByte(objSource, CultureInfo.InvariantCulture) : Convert.ToSByte(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static short CorrectValue(object objSource, short returnValue, bool invariantCulture = true)
    {
        short num = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToInt16(objSource, CultureInfo.InvariantCulture) : Convert.ToInt16(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static int CorrectValue(object objSource, int returnValue, bool invariantCulture = true)
    {
        int num = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToInt32(objSource, CultureInfo.InvariantCulture) : Convert.ToInt32(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static long CorrectValue(object objSource, long returnValue, bool invariantCulture = true)
    {
        long num = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToInt64(objSource, CultureInfo.InvariantCulture) : Convert.ToInt64(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static ushort CorrectValue(object objSource, ushort returnValue, bool invariantCulture = true)
    {
        ushort num = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToUInt16(objSource, CultureInfo.InvariantCulture) : Convert.ToUInt16(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static uint CorrectValue(object objSource, uint returnValue, bool invariantCulture = true)
    {
        uint num = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToUInt32(objSource, CultureInfo.InvariantCulture) : Convert.ToUInt32(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static ulong CorrectValue(object objSource, ulong returnValue, bool invariantCulture = true)
    {
        ulong num = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToUInt64(objSource, CultureInfo.InvariantCulture) : Convert.ToUInt64(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static float CorrectValue(object objSource, float returnValue, bool invariantCulture = true)
    {
        float num = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToSingle(objSource, CultureInfo.InvariantCulture) : Convert.ToSingle(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static double CorrectValue(object objSource, double returnValue, bool invariantCulture = true)
    {
        double num = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToDouble(objSource, CultureInfo.InvariantCulture) : Convert.ToDouble(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static decimal CorrectValue(object objSource, decimal returnValue, bool invariantCulture = true)
    {
        decimal num = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToDecimal(objSource, CultureInfo.InvariantCulture) : Convert.ToDecimal(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static DateTime CorrectValue(object objSource, DateTime returnValue, bool invariantCulture = true)
    {
        DateTime dateTime = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToDateTime(objSource, CultureInfo.InvariantCulture) : Convert.ToDateTime(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static char CorrectValue(object objSource, char returnValue, bool invariantCulture = true)
    {
        char c = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return invariantCulture ? Convert.ToChar(objSource, CultureInfo.InvariantCulture) : Convert.ToChar(objSource);
        }
        catch
        {
            throw;
        }
    }

    public static Guid CorrectValue(object objSource, Guid returnValue, bool invariantCulture = true)
    {
        Guid guid = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            if (objSource is Guid)
            {
                return new Guid(invariantCulture ? string.Format(CultureInfo.InvariantCulture, "{0}", objSource) : objSource.ToString());
            }
            return new Guid(invariantCulture ? string.Format(CultureInfo.InvariantCulture, "{0}", objSource) : objSource.ToString());
        }
        catch
        {
            throw;
        }
    }

    public static DateTime ParseExact(object objSource, string defaultFormat, DateTime returnValue)
    {
        DateTime dateTime = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            return DateTime.ParseExact(objSource.ToString(), defaultFormat, CultureInfo.InvariantCulture);
        }
        catch
        {
            throw;
        }
    }

    public static object GetInt32(object sourceValue, object minValue)
    {
        object value = DBNull.Value;
        try
        {
            if (sourceValue == null || sourceValue is DBNull)
            {
                return minValue;
            }
            if (string.IsNullOrEmpty(sourceValue.ToString()))
            {
                return minValue;
            }
            int result = int.MinValue;
            int.TryParse(sourceValue.ToString(), out result);
            if (result == int.MinValue)
            {
                return minValue;
            }
            return result;
        }
        catch
        {
            value = minValue;
            throw;
        }
    }

    public static object GetInt64(object sourceValue, object minValue)
    {
        object value = DBNull.Value;
        try
        {
            if (sourceValue == null || sourceValue is DBNull)
            {
                return minValue;
            }
            if (string.IsNullOrEmpty(sourceValue.ToString()))
            {
                return minValue;
            }
            long result = long.MinValue;
            long.TryParse(sourceValue.ToString(), out result);
            if (result == long.MinValue)
            {
                return minValue;
            }
            return result;
        }
        catch
        {
            value = minValue;
            throw;
        }
    }

    public static object GetString(object sourceValue, object minValue)
    {
        object value = DBNull.Value;
        try
        {
            if (sourceValue == null || sourceValue is DBNull)
            {
                return minValue;
            }
            if (string.IsNullOrEmpty(sourceValue.ToString()))
            {
                return minValue;
            }
            return sourceValue.ToString()!.Trim();
        }
        catch
        {
            value = minValue;
            throw;
        }
    }

    public static object GetUpperString(object sourceValue, object minValue)
    {
        object value = DBNull.Value;
        try
        {
            if (sourceValue == null || sourceValue is DBNull)
            {
                return minValue;
            }
            if (string.IsNullOrEmpty(sourceValue.ToString()))
            {
                return minValue;
            }
            return sourceValue.ToString()!.ToUpper().Trim();
        }
        catch
        {
            value = minValue;
            throw;
        }
    }

    public static object GetDateTime(object sourceValue, object minValue)
    {
        object value = DBNull.Value;
        try
        {
            if (sourceValue == null || sourceValue is DBNull)
            {
                return minValue;
            }
            if (string.IsNullOrEmpty(sourceValue.ToString()))
            {
                return minValue;
            }
            DateTime result = DateTime.MinValue;
            DateTime.TryParse(sourceValue.ToString(), out result);
            if (result == DateTime.MinValue)
            {
                return minValue;
            }
            return result;
        }
        catch
        {
            value = minValue;
            throw;
        }
    }

    public static bool GetValue(object objSource, bool returnValue)
    {
        bool flag = returnValue;
        try
        {
            if (objSource == null || objSource is DBNull)
            {
                return returnValue;
            }
            if (string.IsNullOrEmpty(objSource.ToString()))
            {
                return returnValue;
            }
            if (objSource.ToString() == "0")
            {
                return false;
            }
            if (objSource.ToString() == "1")
            {
                return true;
            }
            return Convert.ToBoolean(objSource);
        }
        catch
        {
            flag = returnValue;
            throw;
        }
    }

    public static decimal GetValue(object sourceValue, decimal minValue)
    {
        decimal result;
        try
        {
            if (sourceValue == null || sourceValue is DBNull)
            {
                return minValue;
            }
            if (string.IsNullOrEmpty(sourceValue.ToString()))
            {
                return minValue;
            }
            if (!decimal.TryParse(sourceValue.ToString(), out result))
            {
                return minValue;
            }
        }
        catch
        {
            result = minValue;
            throw;
        }
        return result;
    }

    public static int GetValue(object sourceValue, int minValue)
    {
        int result;
        try
        {
            if (sourceValue == null || sourceValue is DBNull)
            {
                return minValue;
            }
            if (string.IsNullOrEmpty(sourceValue.ToString()))
            {
                return minValue;
            }
            if (!int.TryParse(sourceValue.ToString(), out result))
            {
                return minValue;
            }
        }
        catch
        {
            result = minValue;
            throw;
        }
        return result;
    }
}
