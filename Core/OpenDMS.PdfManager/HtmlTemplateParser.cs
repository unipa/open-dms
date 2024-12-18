using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp.Processing.Processors.Filters;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static iText.Svg.SvgConstants;

namespace OpenDMS.PdfManager
{
    public static class HtmlTemplateParser
    {
        public static string Parse (string dataString, string variables)
        {
            Parser(ref dataString, variables, (dataString, key, value, varType) =>
            {
               // Console.WriteLine("PARSER: varname=" + key + " - varvalue=" + value);

                //TODO: sostituire il .Replace con una classe TemplateManager
                //per gestire differenti tipologie di files
                if (!String.IsNullOrEmpty(key))
                {
                    if (varType == "Array")
                    {
                        var count = JArray.Parse(value).Count;
                        GenerateArrayContent(ref dataString, key, count);
                        key = key + ".length";
                    }
                    if (dataString.IndexOf("{{date:" + key) >= 0)
                    {
                        DateTime d = DateTime.MinValue;
                        var date = "__/__/____";
                        if (DateTime.TryParse(value, out d))
                            date = d.ToString("dd/MM/yyyy");
                        dataString = dataString.Replace("{{date:" + key + "}}", date);
                    }
                    else
                    {
                        if (dataString.IndexOf("{{" + key) >= 0)
                            dataString = dataString.Replace("{{" + key + "}}", value);
                    }
                    SetModel(ref dataString, key, "==", value);
                    SetModel(ref dataString, key, "!=", value);
                    //                            dataString = SetModel(dataString, key, "", value);
                    //dataString = ParseTag(dataString, "visibleFor", key, "==", value);
                    //dataString = ParseTag(dataString, "visibleFor", key, "!=", value);
                    //dataString = ParseTag(dataString, "visibleFor", key, "", value);
                    //dataString = ParseTag(dataString, "hiddenFor", key, "==", value, false);
                    //dataString = ParseTag(dataString, "hiddenFor", key, "!=", value, false);
                    //dataString = ParseTag(dataString, "hiddenFor", key, "", value, false);

                    ParseTag(ref dataString, "show-if", key, "==", value);
                    ParseTag(ref dataString, "show-if", key, "!=", value);
                    ParseTag(ref dataString, "show-if", key, "", value);
                    ParseTag(ref dataString, "hide-if", key, "==", value, false);
                    ParseTag(ref dataString, "hide-if", key, "!=", value, false);
                    ParseTag(ref dataString, "hide-if", key, "", value, false);
                }
                return dataString;
            });
            // tolgo tutti i for rimantenti non associati ad alcun campo
            //ds = ParseTag(ds, "for", "", "", "DUMMY", false);
            dataString = dataString.Replace("<for ", "<for style='display:none' ");
            dataString = dataString.Replace("<hide-if ", "<hide-if style='display:none' ");
            dataString = dataString.Replace("<show-if ", "<show-if style='display:none' ");
            return dataString;
        }



        public static void  Parser(ref string dataString, string variables, Func<string, string, string, string, string> Callback, string prefix = "")
        {
            if (string.IsNullOrEmpty(variables)) return; // dataString;
            foreach (var var in JObject.Parse(variables))
            {
                var vartype = var.Value.Type.ToString();
                var varvalue = var.Value.ToString();
                var varname = prefix + var.Key;
                if (varname != "Document")
                {
                    if (vartype == "Object")
                    {
                        Parser(ref dataString, varvalue, Callback, varname + ".");
                    }
                    else
                    if (vartype == "Array")
                    {
                        try
                        {
                            var array = JArray.Parse(varvalue);
                            dataString = Callback(dataString, varname, varvalue, vartype);
                            for (int i = 0; i < array.Count; i++)
                            {
                                Parser(ref dataString, array[i].ToString(), Callback, varname + "[" + i + "].");
                                Parser(ref dataString, array[i].ToString(), Callback, varname + "." + i + ".");
                            }
                            dataString = Callback(dataString, varname + ".length", array.Count.ToString(), vartype);
                            dataString = Callback(dataString, varname + ".Count", array.Count.ToString(), vartype);
                        }
                        catch { }
                    }
                    else if (vartype != "Null")
                    {
                        dataString = Callback(dataString, varname, varvalue, vartype);
                    }
                }
            }
            //return dataString;

        }
        private static void  GenerateArrayContent(ref string dataString, string key, int count)
        {
            var startToken = "<for " + key + ">";
            var i = dataString.IndexOf(startToken);
            if (i < 0)
            {
                startToken = "<for " + key + " >";
                i = dataString.IndexOf(startToken);
            }
            while (i >= 0)
            {
                int j = i + startToken.Length-1;
                var endToken = "</" + startToken.Substring(1);
                var e = dataString.IndexOf(endToken, j);
                //  <for k>123</for k>
                // 0123456789 123456789 123456789 123456789
                // 0         1         2         3
                // i = 1
                // j = 16
                // tagvalue = 1+14, 16-1-14 = 15,1 = "v"
                // e = 19
                // innerHtml = 16+1, 20-16-1 = 17, 3
                // remove = 1, 20 + 16 - 1 + 1 = 1,26 = " <visibleFor k=v>123</visibleFor k=v>"
                var innerHtml = e >= 0 ? dataString.Substring(j + 1, e - j - 1) : dataString.Substring(j + 1);

                if (e >= 0) 
                    dataString = dataString.Remove(i, e + endToken.Length - i);
                else
                    dataString = dataString.Remove(i);
                for (int index = 0; index < count; index++)
                {
                    var row = innerHtml
                        .Replace("{index}", index.ToString())
                        .Replace("{row-index}", (1+index).ToString())
                        .Replace("{count}", count.ToString())
                        .Replace("{length}", count.ToString());
                    var offset = row.Length;
                    dataString = dataString.Insert(i, row);
                    i += offset;
                }

                i = dataString.IndexOf(startToken);
            }
            //return dataString;


        }
        private static void  SetModel(ref string dataString, string key, string operatorValue, string value)
        {
            if (dataString.IndexOf("{{" + key + "}}") >= 0)
                dataString = dataString.Replace("{{" + key + "}}", value);

            var valueArray = value.Split(',');

            // Caso d'uso {{ Key == Value ? return : else }}
            var startToken = "{{" + key + operatorValue;
            int i = dataString.IndexOf(startToken);
            if (i < 0)
            {
                startToken = "{{" + key + " " + operatorValue;
                i = dataString.IndexOf(startToken);
            }
            if (i < 0)
            {
                startToken = "{{ " + key + operatorValue;
                i = dataString.IndexOf(startToken);
            }
            if (i < 0)
            {
                startToken = "{{ " + key + " " + operatorValue;
                i = dataString.IndexOf(startToken);
            }
            while (i >= 0)
            {
                var defaultValue = "";
                var elseValue = "";
                int j = dataString.IndexOf("}}", i);
                string token = dataString.Substring(i, j - i+2);
                var values = dataString.Substring(i + startToken.Length, j - i - startToken.Length);
                int question = values.IndexOf("?");
                int dot = values.IndexOf(":");
                if (dot >= 0)
                {
                    elseValue = values.Substring(dot + 1);
                    values = values.Substring(0, dot);
                }
                if (question >= 0)
                {
                    defaultValue = values.Substring(question + 1).Trim();
                    values = values.Substring(0, question).Trim();
                }
                if (string.IsNullOrEmpty(defaultValue)) defaultValue = "checked";
                var val = values;

                var ok = false;
                if (operatorValue == "==")
                    //if (!String.IsNullOrEmpty(operatorValue))
                    ok |= (valueArray.FirstOrDefault(v => v == val) != null);
                else if (operatorValue == "!=")
                    ok |= (valueArray.FirstOrDefault(v => v == val) == null);

                ok |= (string.IsNullOrEmpty(val) && string.IsNullOrEmpty(operatorValue) && !string.IsNullOrEmpty(values));


                var tokenvalue = ok ? defaultValue : elseValue;
                dataString = dataString.Replace(token, tokenvalue);
                i = dataString.IndexOf(startToken);
            }
            //return dataString;
        }
        private static void  ParseTag(ref string dataString, string tag, string key, string operatorValue, string value, bool mostra = true)
        {
            var startToken = "<" + tag + " " + key + operatorValue;
            if (string.IsNullOrEmpty(operatorValue)) startToken+=">";
            var i = dataString.IndexOf(startToken);
            if (i < 0)
            {
                startToken = "<" + tag + " " + key + " " + operatorValue;
                if (string.IsNullOrEmpty(operatorValue)) 
                    startToken += ">";
                i = dataString.IndexOf(startToken);
            }
            while (i >= 0)
            {
                var tagValue = "";
                var endToken = "</" + startToken.Substring(1);
                int j = i + startToken.Length-1;
                if (!string.IsNullOrEmpty(operatorValue))
                {
                    j = dataString.IndexOf(">", i);
                    tagValue = j >= 0 ? dataString.Substring(i + startToken.Length, j - i - startToken.Length) : dataString.Substring(i + startToken.Length);
                    endToken += tagValue + ">";
                }
                var e = dataString.IndexOf(endToken, j);
                //  <visibleFor k=v>123</visibleFor k=v>
                // 0123456789 123456789 123456789 123456789
                // 0         1         2         3
                // i = 1
                // j = 16
                // tagvalue = 1+14, 16-1-14 = 15,1 = "v"
                // e = 19
                // innerHtml = 16+1, 20-16-1 = 17, 3
                // remove = 1, 20 + 16 - 1 + 1 = 1,26 = " <visibleFor k=v>123</visibleFor k=v>"
                var innerHtml = e >= 0 ? dataString.Substring(j + 1, e - j - 1) : dataString.Substring(j + 1);
                if (e >= 0) dataString = dataString.Remove(i, e + endToken.Length - i ); else dataString = dataString.Remove(i);


                var val = tagValue;
                var valueArray = value.Split(',');
                var ok = false;
                if (operatorValue == "==")
                    //if (!String.IsNullOrEmpty(operatorValue))
                    ok |= (valueArray.FirstOrDefault(v => v == val) != null);
                else if (operatorValue == "!=")
                    ok |= (valueArray.FirstOrDefault(v => v == val) == null);

                ok |= (string.IsNullOrEmpty(tagValue) && string.IsNullOrEmpty(operatorValue) && !string.IsNullOrEmpty(value));
                //  <visibleFor k>123</visibleFor k>
                if ((mostra && ok) || (!mostra && !ok))
                    try
                    {
                        dataString = dataString.Insert(i, innerHtml);
                    }
                    catch { }
                i = dataString.IndexOf(startToken);
            }
            //return dataString;
        }

    }
}
