using OpenDMS.Domain.Entities.Settings;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenDMS.Core.Utilities;


public static class ObjectExtensions
{
    public static string Parse(this string Value, object ObjectToParse, string Prefix, string TagStart = "{{", string TagStop = "}}" )
    {if (ObjectToParse == null) return "";
        var p = Expression.Parameter(ObjectToParse.GetType(), Prefix);
        return Regex.Replace(Value, TagStart + @"(?<exp>[^}]+)" + TagStop, match =>
        {
            try
            {
                var v = match.Groups["exp"].Value;
                if (v.StartsWith(Prefix))
                {
                    var e = System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(new[] { p }, null, v);
                    return (e.Compile().DynamicInvoke(ObjectToParse) ?? "").ToString();
                }
                else
                {
                    return TagStart + v + TagStop;
                }
            }
            catch
            { 
                return TagStart + match.Groups["exp"].Value + TagStop;
            }
        });

        //if (!Prefix.EndsWith("."))
        //    Prefix += ".";



        //var p = Expression.Parameter(@object.GetType(), @object.GetType().Name);
        //int start = (@TagStart + Prefix).Length;
        //int stop = (@TagStop).Length;
        //return Regex.Replace(Value, @TagStart + Prefix + @"(.+?)"+TagStop,
        //match => {
        //    try
        //    {
        //        var m = match.Value.Substring(start, match.Value.Length - stop - start);
        //        var e = System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(new[] { p }, null, m); //.Groups[1].Value);
        //        return (e.Compile().DynamicInvoke(@object) ?? "").ToString();
        //    }
        //    catch (Exception ex) {
        //        return "";
        //    }
        //});
    }



    private static List<LookupTable> InternalParseClass(Type T, string Prefix = "")
    {
        List<LookupTable> Properties = new List<LookupTable>();

        foreach (var P in T.GetProperties())
        {
            string d = "";
            try
            {
                d = P.GetCustomAttribute<DescriptionAttribute>()?.Description;
            }
            catch
            {

            };
            Properties.Add(new LookupTable { Id = "{{"+ Prefix + P.Name +"}}", TableId = P.PropertyType.ToString(), Description = d });
            if (P.PropertyType.IsArray)
            {
                if (!((Prefix + ".").Contains(P.Name + "[].")))
                    Properties.AddRange(InternalParseClass(P.ReflectedType, Prefix + P.Name + "[]."));
            }
            else
            if (P.PropertyType.IsClass && !P.PropertyType.IsValueType && !(P.PropertyType.FullName == "System.String"))
            {
                if (!((Prefix + ".").Contains(P.Name + ".")))
                    Properties.AddRange(InternalParseClass(P.PropertyType, Prefix + P.Name + "."));
            }
        }

        return Properties;
    }

    public static string GetProperties(params Tuple<Type, string>[] types)
    {
        List<LookupTable> Properties = new();
        foreach (var T in types)
            Properties.AddRange (InternalParseClass(T.Item1, T.Item2 + "."));
        return System.Text.Json.JsonSerializer.Serialize(Properties);
    }


    private static string InternalGetJson(Type T, int deep)
    {
        string Properties = "";
        //            if (deep < 5)
        {

            bool first = true;
            foreach (var P in T.GetProperties())
            {
                var pname = P.PropertyType.FullName.ToLower();
                var _Properties = "";
                if (P.PropertyType.IsArray && deep < 5)
                {
                    var p = InternalGetJson(P.ReflectedType, deep + 1);
                    if (!string.IsNullOrEmpty(p))
                    {
                        _Properties += "\n" + ("").PadLeft(deep * 4, ' ') + "[\n";
                        _Properties += p;
                        _Properties += "\n" + ("").PadLeft(deep * 4, ' ') + "]";
                    }
                }
                else
                if ((pname == "system.string"))
                    _Properties += "\"testo\"";
                else
                if ((pname.Contains("system.int")))
                    _Properties += "0";
                else
                if ((pname.Contains("system.datetime")))
                    _Properties += "\"1999-12-31T23:59:59\"";
                else
                if ((pname.Contains("system.date")))
                    _Properties += "\"1999-12-31\"";
                else
                if ((pname.Contains("system.bool")))
                    _Properties += "false";
                else
                    if (P.PropertyType.IsClass && !P.PropertyType.IsValueType && deep < 5)
                {
                    var p = InternalGetJson(P.PropertyType, deep + 1);
                    if (!string.IsNullOrEmpty(p))
                    {
                        _Properties += "\n" + ("").PadLeft(deep * 4, ' ') + "{\n";
                        _Properties += p;
                        _Properties += "\n" + ("").PadLeft(deep * 4, ' ') + "}";
                    }
                }
                if (!String.IsNullOrEmpty(_Properties))
                {
                    if (!first) Properties += ", \n";
                    first = false;
                    Properties += ("").PadLeft(deep * 4, ' ') + "\"" + P.Name + "\" : " + _Properties;

                }
            }
        }

        return Properties;
    }
    public static string GetJson(params Tuple<Type, string>[] types)
    {
        string Properties = "{ \n";
        bool first = true;
        foreach (var T in types)
        {
            if (!first) Properties += ",\n";
            Properties += "\"" + T.Item2 + "\" : \n    {\n";
            //var json = JsonSerializer.Serialize(Activator.CreateInstance(T.Item1));
            Properties += InternalGetJson(T.Item1, 2);
            Properties += "\n}";
            first = false;
        }
        Properties += "\n}";
        //        string Properties = "{ \n" + InternalGetJson(T, 1) + "\n}";
        return Properties;
    }




    // DEPRECATA
    private static string _Parse(this string Value, object d,  string Prefix)
{

        if (string.IsNullOrEmpty(Value)) return Value;
        if (d == null) return Value;
        try
        {
        System.Reflection.FieldInfo[] fi = d.GetType().GetFields();
            string lPrefix = Prefix.ToLower() + ".";
            string uPrefix = Prefix.ToUpper() + ".";
        foreach (System.Reflection.FieldInfo F in fi)
        {
                string fn = F.Name.ToLower();
            try
            {
                if (F.FieldType.IsArray)
                {
                    var fv = F.GetValue(d);
                    if (fv != null)
                    for (int i = 0; i < (fv as Array).Length; i++)
                    {
                        try
                        {
                            if (fv != null)
                            {
                                if ((fv is Array) && (fv as Array).GetValue(i) != null)
                                {
                                    Value = Regex.Replace(Value, @"\{" + lPrefix + fn + i.ToString() + @"\}", (fv as Array).GetValue(i).ToString(), RegexOptions.IgnoreCase);
                                    Value = Regex.Replace(Value, @"\{" + lPrefix + fn + "[" + i.ToString() + "]" + @"\}", (fv as Array).GetValue(i).ToString(), RegexOptions.IgnoreCase);
                                }
                                else
                                {
                                    Value = Regex.Replace(Value, @"\{" + lPrefix + fn + i.ToString() + @"\}", "", RegexOptions.IgnoreCase);
                                    Value = Regex.Replace(Value, @"\{" + lPrefix + fn + "[" + i.ToString() + "]" + @"\}", "", RegexOptions.IgnoreCase);
                                }
                            }
                            else
                            {
                                Value = Regex.Replace(Value, @"\{" + lPrefix + fn + i.ToString() + @"\}", "", RegexOptions.IgnoreCase);
                                Value = Regex.Replace(Value, @"\{" + lPrefix + fn + "[" + i.ToString() + "]" + @"\}", "", RegexOptions.IgnoreCase);
                            }

                        }
                        catch (Exception e)
                        {
                            Value = Regex.Replace(Value, @"\{" + lPrefix + fn + i.ToString() + @"\}", "", RegexOptions.IgnoreCase);
                            Value = Regex.Replace(Value, @"\{" + lPrefix + fn + "[" + i.ToString() + "]" + @"\}", "", RegexOptions.IgnoreCase);

                        }
                    }    
                }
                else
                {
                        string v = (F.GetValue(d) == null ? "" : F.GetValue(d).ToString());
                    Value = Regex.Replace(Value, @"\{" + lPrefix + fn + @"\}", v, RegexOptions.IgnoreCase);
                }
            }
            catch (Exception E)
            {
                Value = Regex.Replace(Value, @"\{" + lPrefix + fn + @"\}", "", RegexOptions.IgnoreCase);
            }
        }

        System.Reflection.PropertyInfo[] mi = d.GetType().GetProperties();
        foreach (System.Reflection.PropertyInfo P in mi)
        {
            try 
            {
                    if (P.PropertyType.IsGenericType)
                    {
                        var l = (System.Collections.IList)P.GetValue(d, null);
                        if (l != null)
                        for (int i = 0; i < l.Count; i++)
                        {
                            try
                            {
                                if (P.PropertyType.ToString() == "System.DateTime")
                                {
                                    if (l[i] != null)
                                        Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + "[" + i.ToString() + @"]\}", l[i].ToString().Substring(0, 10), RegexOptions.IgnoreCase);
                                    else
                                        Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + "[" + i.ToString() + @"]\}", "", RegexOptions.IgnoreCase);
                                }
                                else
                                {
                                    if (l[i] != null)
                                        Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + "[" + i.ToString() + @"]\}", l[i].ToString(), RegexOptions.IgnoreCase);
                                    else
                                        Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + "[" + i.ToString() + @"]\}", "", RegexOptions.IgnoreCase);
                                }
                            }
                            catch (Exception e)
                            {
                                Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + "[" + i.ToString() + @"]\}", "", RegexOptions.IgnoreCase);
                            }
                        }
                    }
                    else
               if (P.PropertyType.IsArray)
                    {
                        var pv = P.GetValue(d, null);
                        if (pv != null)

                            for (int i = 0; i < (pv as Array).Length; i++)
                        {
                            try
                            {
                                if (P.PropertyType.ToString() == "System.DateTime")
                                {
                                     if ((pv is Array) && (pv as Array).GetValue(i) != null)
                                        Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + "[" + i.ToString() + @"]\}", (pv as Array).GetValue(i).ToString().Substring(0, 10), RegexOptions.IgnoreCase);
                                    else
                                        Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + "[" + i.ToString() + @"]\}", "", RegexOptions.IgnoreCase);
                                }
                                else
                                {
                                    if ((pv is Array) && (pv as Array).GetValue(i) != null)
                                        Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + "[" + i.ToString() + @"]\}", (pv as Array).GetValue(i).ToString(), RegexOptions.IgnoreCase);
                                    else
                                        Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + "[" + i.ToString() + @"]\}", "", RegexOptions.IgnoreCase);
                                }
                            }
                            catch (Exception e)
                            {
                                 Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + "[" + i.ToString() + @"]\}", "", RegexOptions.IgnoreCase);
                            }
                        }
                    }
                    else
                    {
                        if (P.PropertyType.ToString() == "System.DateTime")
                    {
                        try
                        {
                            if (P.GetValue(d, null) != null)
                                Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + @"\}", P.GetValue(d, null).ToString().Substring(0, 10), RegexOptions.IgnoreCase);
                            else
                                Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + @"\}", "", RegexOptions.IgnoreCase);
                        }
                        catch (Exception)
                        {
                             Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + @"\}", "", RegexOptions.IgnoreCase);
                        }
                    }
                    else
                    {
                        if (P.GetValue(d, null) != null)
                            Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + @"\}", P.GetValue(d, null).ToString(), RegexOptions.IgnoreCase);
                        else
                            Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + @"\}", "", RegexOptions.IgnoreCase);

                    }
                }
            }
            catch (Exception E)
            {
                 Value = Regex.Replace(Value, @"\{" + lPrefix + P.Name + @"\}", "", RegexOptions.IgnoreCase);
            }
        }
    }
    catch (Exception e)
    {
    }
    return Value;
}



}

