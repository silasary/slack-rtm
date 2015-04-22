using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SlackRTM.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SlackRTM
{
    class SlackJsonConverter : JsonConverter
    {
        private Slack slack;

        public SlackJsonConverter(Slack slack)
        {
            // TODO: Complete member initialization
            this.slack = slack;
        }
        public override bool CanConvert(Type objectType)
        {
            if (objectType.Assembly == Assembly.GetExecutingAssembly())
                return true;
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(Event))
                existingValue= GenerateEvent(ref reader);
            if (existingValue == null)
                existingValue = Activator.CreateInstance(objectType, true);
            serializer.ContractResolver = new TrueCamelCasePropertyNamesContractResolver();
            serializer.Populate(reader, existingValue);
            if (existingValue.GetType().GetProperty("SlackInstance") != null)
                existingValue.GetType().GetProperty("SlackInstance").SetValue(existingValue, slack, null);

            return existingValue;
        }

        private static object GenerateEvent(ref JsonReader reader)
        {
            Event generatedObject;
            JToken obj = JObject.ReadFrom(reader);
            reader = obj.CreateReader();
            if (obj["ok"] != null)
                generatedObject = new Ack();
            else
            switch (obj["type"].ToString())
            {
                case "hello":
                    generatedObject = new Hello();
                    break;
                case "message":
                    generatedObject = new Message();
                    break;
                default:
                    return new UnknownEvent(obj as JObject);
            }
            foreach (JProperty key in obj.Children())
            {
                var name = key.Name.FromUnderscoreLower();
                var prop = generatedObject.GetType().GetProperties().FirstOrDefault(n => n.Name == name);
                if (prop != null && prop.CanWrite && !string.IsNullOrEmpty(key.Value.ToString()))
                {
                    prop.SetValue(generatedObject, key.Value.ToObject(prop.PropertyType), null);
                }
            }
            return generatedObject;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            JObject jo = new JObject();
            
            foreach (PropertyInfo prop in value.GetType().GetProperties())
            {
                if (prop.CanRead)
                {
                    object propValue = prop.GetValue(value, null);
                    if (propValue != null)
                    {
                        jo.Add(prop.Name.ToUnderscoreLower(), JToken.FromObject(propValue, serializer));
                    }
                }
            }
            jo.WriteTo(writer);
        }

        public class TrueCamelCasePropertyNamesContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                if (char.IsUpper(propertyName[0]))
                    return propertyName.ToUnderscoreLower();
                else
                    return propertyName.FromUnderscoreLower();
            }
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                return base.CreateProperty(member, memberSerialization);
            }
        }

    }
}
