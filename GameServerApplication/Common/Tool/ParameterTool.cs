using System;
using System.Collections.Generic;
using System.Text;
using LitJson;
namespace Common.Tool
{
    public class ParameterTool
    {
        public static T GetParameter<T>(Dictionary<byte, object> parameters, ParameterCode parameterCode, bool isObject = true)
        {
            object o = null;
            parameters.TryGetValue((byte)parameterCode, out o);
            if (isObject == false)
            {
                return (T)o;
            }
            return JsonMapper.ToObject<T>(o.ToString());
        }

        public static void AddParameter<T>(Dictionary<byte, object> parameters, ParameterCode key, T value,
            bool isObject = true)
        {
            if (isObject)
            {
                string json = JsonMapper.ToJson(value);
                parameters.Add((byte)key, json);
            }
            else
            {
                parameters.Add((byte)key, value);
            }
        }
        public static SubCode GetSubcode(Dictionary<byte, object> parameters)
        {
            return GetParameter<SubCode>(parameters, ParameterCode.SubCode, false);
        }

        public static void AddSubcode(Dictionary<byte, object> parameters, SubCode subcode)
        {
            AddParameter(parameters, ParameterCode.SubCode, subcode, false);
        }

    }
}
