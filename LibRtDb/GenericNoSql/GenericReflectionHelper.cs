using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

//Useful sources:
//https://stackoverflow.com/questions/39633917/c-getmethod-by-type-generic-list

namespace LibRtDb.GenericNoSql
{
    public static class GenericReflectionHelper
    {

        /// <summary>
        /// Simply Casts object to needed type
        /// </summary>
        /// <param name="Document"></param>
        /// <returns></returns>
        public static IEnumerable CastToEnumerable(dynamic Document)
        {
            return (IEnumerable)Document;
        }

        /// <summary>
        /// LiteDB only
        /// <para/>
        /// Gets Collection of appropriate type
        /// </summary>
        /// <param name="Document"></param>
        /// <returns></returns>
        public static dynamic GetDynamicCollection(object SourceClass, dynamic Document)
        {
            //search for appropriate method in LiteDB library
            var method = SourceClass.GetType().GetMethod("GetCollection", 1, new Type[] { });

            //Recover current document type, even if it's a list of such types
            Type type = Document.GetType().GetGenericArguments()[0];

            //build generic method on the fly
            var generics = method.MakeGenericMethod(type);

            //invoke new method on specific instance of DB
            dynamic collection = generics.Invoke(SourceClass, null);

            return collection;

        }

        /// <summary>
        /// For example if document is IEnumerable<string> it will return string as type
        /// </summary>
        /// <param name="Document"></param>
        /// <returns></returns>
        private static Type GetDocumentNestedType(object Document)
        {
            return Document.GetType().GetGenericArguments()[0];
        }

        /// <summary>
        /// Will fetch first occurence of IEnumerable<T> method and generate new generic method 
        /// <para/>
        /// that corresponds to Document type
        /// </summary>
        /// <param name="Document"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        public static MethodInfo GetAppropriateCollectionGenericMethod(object SourceClass, dynamic Document, string MethodName)
        {

            //get all public methods
            var publicMethods = SourceClass.GetType().GetMethods().Where(x => x.Name == MethodName && x.IsGenericMethod);

            //filter out only useful methods
            foreach (var goodMethod in publicMethods)
            {
                var methodParams = goodMethod.GetParameters();
                var firstParameterType = methodParams[0].ParameterType;
                //methods that has arguments like Ienumerable<T>, RepeatedField<T> and so on
                var hasNested = firstParameterType.GenericTypeArguments.Length > 0;
                if (hasNested == true)
                {
                    //if we found first method with that name that has as parameter an IEnumerable<T> we are ok
                    var genericTypeDef = firstParameterType.GetGenericTypeDefinition();
                    if (genericTypeDef == typeof(IEnumerable<>))
                    {
                        //Recover current document type, even if it's a list of such types
                        Type documentType = GetDocumentNestedType(Document);
                        //simply create a generic method based on Document inner Type
                        return goodMethod.MakeGenericMethod(documentType);
                    }
                }
            }

            return null;

        }

    }
}
