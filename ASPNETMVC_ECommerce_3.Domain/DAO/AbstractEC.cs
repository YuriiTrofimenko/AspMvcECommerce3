using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASPNETMVC_ECommerce_3.Domain.DAO
{
    public abstract class AbstractEC<T>
    {
        public AspNetMvcECommerce_3Entities mDb;
        public AbstractEC(AspNetMvcECommerce_3Entities _db)
        {
            mDb = _db;
        }

        public T Save(T _entityForSave)
        {
            //T parrent = Find((int)(_parrent.GetType().GetProperty("id").GetValue(_parrent)));
            int userId =
                (int)(_entityForSave.GetType().GetProperty("id").GetValue(_entityForSave));
            if (userId != 0)
            {
                T entityFromDb = Find(userId);
                Type type = typeof(T);
                foreach (var prop in type.GetProperties())
                {
                    prop.SetValue(entityFromDb, prop.GetValue(_entityForSave));
                }
                _entityForSave = entityFromDb;
            }
            else
            {
                PropertyInfo dbSetInfo = mDb.GetType().GetProperty(propNameCreator(typeof(T).Name));
                Object o = dbSetInfo.GetValue(mDb, null);
                MethodInfo add = o.GetType().GetMethod("Add");
                add.Invoke(o, new object[] { _entityForSave });
            }
            mDb.SaveChanges();
            return _entityForSave;
        }

        public T Find(int _id)
        {
            PropertyInfo dbSetInfo = mDb.GetType().GetProperty(propNameCreator(typeof(T).Name));
            Object o = dbSetInfo.GetValue(mDb, null);
            MethodInfo find = o.GetType().GetMethod("Find");
            T result = (T)find.Invoke(o, new object[] { new object[] { _id } });
            return result;
        }

        public T Remove(T _parrent)
        {
            PropertyInfo temp = mDb.GetType().GetProperty(propNameCreator(typeof(T).Name));
            Object o = temp.GetValue(mDb, null);
            MethodInfo remove = o.GetType().GetMethod("Remove");
            //FormatterServices.GetUninitializedObject(temp.GetType())
            //Object o = Activator.CreateInstance(temp.GetType());
            T result = (T)remove.Invoke(o, new object[] { _parrent });
            mDb.SaveChanges();
            return result;

            /*return mDb.Roles.Remove(_parrent);*/
        }

        private string propNameCreator(string _name)
        {

            if (Regex.IsMatch(_name, "[A-z]{1,}[y]$"))
            {
                _name = _name.Remove(_name.Length - 1) + "ies";
            }
            else
            {
                _name = _name + "s";
            }
            return _name;
        }
    }
}
