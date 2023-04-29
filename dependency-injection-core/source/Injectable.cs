using System.Reflection;

namespace DependencyInjectionCore
{
    public class Injectable
    {
        public Injectable()
        {
            ForceInject();
        }

        public void ForceInject()
        {
            var type = GetType();
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<InjectAttribute>();
                if (attribute == null) continue;
                var fieldType = field.FieldType;
                if(field.GetValue(this) == null) field.SetValue(this, DIContainer.Get(fieldType));
            }
        }
    }
}
