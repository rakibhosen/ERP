using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UTILITY
{
    public static class DynamicTypeBuilder
    {
        public static TypeBuilder BuildDynamicType(DataTable table)
        {
            // Create a dynamic assembly and module
            AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

            // Create a dynamic type
            TypeBuilder typeBuilder = moduleBuilder.DefineType("DynamicType", TypeAttributes.Public);

            // Add properties to the dynamic type based on DataTable columns
            foreach (DataColumn column in table.Columns)
            {
                string columnName = column.ColumnName;
                Type columnType = column.DataType;

                // Define field
                FieldBuilder fieldBuilder = typeBuilder.DefineField($"_{Char.ToLowerInvariant(columnName[0])}{columnName.Substring(1)}", columnType, FieldAttributes.Private);

                // Define property
                PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(columnName, PropertyAttributes.None, columnType, null);

                // Define getter
                MethodBuilder getMethodBuilder = typeBuilder.DefineMethod($"get_{columnName}", MethodAttributes.Public | MethodAttributes.SpecialName, columnType, Type.EmptyTypes);
                ILGenerator getIL = getMethodBuilder.GetILGenerator();
                getIL.Emit(OpCodes.Ldarg_0);
                getIL.Emit(OpCodes.Ldfld, fieldBuilder);
                getIL.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getMethodBuilder);

                // Define setter
                MethodBuilder setMethodBuilder = typeBuilder.DefineMethod($"set_{columnName}", MethodAttributes.Public | MethodAttributes.SpecialName, null, new Type[] { columnType });
                ILGenerator setIL = setMethodBuilder.GetILGenerator();
                setIL.Emit(OpCodes.Ldarg_0);
                setIL.Emit(OpCodes.Ldarg_1);
                setIL.Emit(OpCodes.Stfld, fieldBuilder);
                setIL.Emit(OpCodes.Ret);

                propertyBuilder.SetSetMethod(setMethodBuilder);
            }

            // Define a constructor to initialize fields
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
            ILGenerator ctorIL = constructorBuilder.GetILGenerator();
            ctorIL.Emit(OpCodes.Ret);

            return typeBuilder;
        }

    }

}
