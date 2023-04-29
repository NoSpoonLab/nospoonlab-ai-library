using System.Reflection;

namespace FrameworkCore.Reflection
{
    public class Method
    {
        public MethodInfo MethodInfo { get; set; }
        public object AssociatedObject { get; set; }
    }
}