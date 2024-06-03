using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace YamlDotNetExtensions.CommentSerialization
{
    public class WrappedObjectModel
    {
        public static Type ApplyCommentWrappers<T>()
        {
            AssemblyBuilder ab =
                AssemblyBuilder.DefineDynamicAssembly(
                    new AssemblyName("DynamicAssembly"),
                    AssemblyBuilderAccess.Run);

            ModuleBuilder mb = ab.DefineDynamicModule("DynamicAssemblyExample");

            TypeBuilder tb = mb.DefineType(
                "MyDynamicType",
                TypeAttributes.Public);

            var getSetAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            var props = typeof(T)
                .GetProperties()
                .Where(p => p.DeclaringType == typeof(T));

            foreach(var propInfo in props) {
                Type commentWrapperType = typeof(CommentWrapper<>).MakeGenericType(propInfo.PropertyType);

                var backingFieldBuilder = tb.DefineField(
                    $"<{propInfo.Name}>k_BackingField",
                    commentWrapperType,
                    FieldAttributes.Private);

                var propertyBuilder = tb.DefineProperty(
                    propInfo.Name,
                    PropertyAttributes.None,
                    commentWrapperType,
                    null);

                var getAccessor = tb.DefineMethod(
                    $"get_{propInfo.Name}",
                    getSetAttributes,
                    commentWrapperType,
                    null);

                var setAccessor = tb.DefineMethod(
                    $"set_{propInfo.Name}",
                    getSetAttributes,
                    null,
                    [commentWrapperType]);

                var getILGenerator = getAccessor.GetILGenerator();
                getILGenerator.Emit(OpCodes.Ldarg_0);
                getILGenerator.Emit(OpCodes.Ldfld, backingFieldBuilder);
                getILGenerator.Emit(OpCodes.Ret);

                var setILGenerator = setAccessor.GetILGenerator();
                setILGenerator.Emit(OpCodes.Ldarg_0);
                setILGenerator.Emit(OpCodes.Ldarg_1);
                setILGenerator.Emit(OpCodes.Stfld, backingFieldBuilder);
                setILGenerator.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getAccessor);
                propertyBuilder.SetSetMethod(setAccessor);
            }

            return tb.CreateType();
        }
    }
}
