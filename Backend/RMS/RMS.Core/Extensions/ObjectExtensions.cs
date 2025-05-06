using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Check if the Object is null or not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">value of object</param>
        /// <returns>Returns true if the object is null otherwise returns false</returns>
        public static bool IsNull<T>(this T obj) where T : class
            => obj == null;

        /// <summary>
        /// Check if the Object is not null or not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">value of object</param>
        /// <returns>Returns true if the object is not null otherwise returns false</returns>
        public static bool IsNotNull<T>(this T obj) where T : class
            => obj != null;

        /// <summary>
        /// Check if the list of Object is null, empty or not 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects">value of object</param>
        /// <returns>Return true if the list of Object is null or empty; otherwise return false</returns>
        public static bool IsNullOrEmpty<T>(this List<T> objects) where T : class
            => objects == null || objects.Count == 0;
    }
}
