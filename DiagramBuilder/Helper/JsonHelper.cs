using ClassStructureJson;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;
using System.Reflection;
namespace DiagramBuilder.Helper
{
    public class JsonHelper
    {
        public static string GetClassStructure(Type type)
        {
            var instance = System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(type);

            var resolver = new AvoidNullContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings
            {
                ContractResolver = resolver
            };

            settings.Converters.Add(new ClassStructureSerialize());

            return JsonConvert.SerializeObject(instance, Formatting.Indented, settings);
        }

        public class AvoidNullContractResolver : DefaultContractResolver
        {
            public AvoidNullContractResolver()
            {
            }

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);

                object value = "";

                var type = property.PropertyType.GetOriginalType();

                if (type != typeof(string))
                {
                    if (type.IsArray)
                    {
                        value = Activator.CreateInstance(type, 0);
                    }
                    else
                    {
                        value = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
                    }
                }

                var t = typeof(DefaultValueProvider<>).MakeGenericType(property.PropertyType);

                var valueProvider = Activator.CreateInstance(t, value);

                property.ValueProvider = (IValueProvider)valueProvider;
                property.ObjectCreationHandling = ObjectCreationHandling.Reuse;

                return property;
            }
        }
    }
}
