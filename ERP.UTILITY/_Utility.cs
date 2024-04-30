using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UTILITY
{
    public static class _Utility
    {
        public static List<T> ConvertDataTableToList<T>(DataTable table) where T : new()
        {
            List<T> list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T obj = new T();

                foreach (DataColumn col in table.Columns)
                {
                    PropertyInfo property = obj.GetType().GetProperty(col.ColumnName, BindingFlags.Public | BindingFlags.Instance);

                    if (property != null && row[col] != DBNull.Value)
                    {
                        property.SetValue(obj, Convert.ChangeType(row[col], property.PropertyType), null);
                    }
                }

                list.Add(obj);
            }

            return list;
        }

    }
}
