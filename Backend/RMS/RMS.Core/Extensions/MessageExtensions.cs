using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Extensions
{
    public static class MessageExtensions
    {
        #region private variable
        private static string _isNotValid = "is not valid!";
        private static string _couldNotGet = $"Could not get {0}";
        private static string _couldNotBeNull = $"{0} could not be null";
        private static string _couldNotFoundBySpec = $"Could not found {0} by {1}";
        private static string _existTitle = "Afe title already exists, try another!";
        private static string _couldNotBeNullOrEmpty = $"List of {_couldNotBeNull} or empty";
        private static string _dbHasNothingChange = "Zero of objects written to the underlying database.";
        private static string _errorOccurredDuringDeleteProcess = "Error occurred during the delete process";
        private static string _couldNotFoundBySingleParam = $"Could not found {0} in systems by {1} with value {2}";
        private static string _shouldBeInheritedISoftDelete = $"{0} should be inherited ISoftDelete to use this method";
        private static string _couldNotBeLessThanOrEqualToZero = $"{0} {_isNotValid} Could not be less than or equal to zero!";
        #endregion

        /// <summary>
        /// Build message when parameter type int is not valid.
        /// Message: {param} is not valid! Could not be less than or equal to zero!
        /// </summary>
        /// <param name="param">name of parameter</param>
        /// <returns>string as message</returns>
        public static string CouldNotBeLessThanOrEqualToZero(string param)
            => string.Format(_couldNotBeLessThanOrEqualToZero, param);

        /// <summary>
        /// Build message when could not found entity by single param
        /// Message: Could not found {entity} in systems by {param} with value {value}
        /// </summary>
        /// <param name="entity">Name of the entity</param>
        /// <param name="param">Name of the param</param>
        /// <param name="value">Value of the param</param>
        /// <returns>string as message</returns>
        public static string CouldNotFoundBySingleParam(string entity, string param, string value)
            => string.Format(_couldNotFoundBySingleParam, entity, param, value);

        /// <summary>
        /// Build message when system has error during delete
        /// Message: Error occurred during the delete process
        /// </summary>
        /// <returns>string as message</returns>
        public static string ErrorWhenDelete()
            => _errorOccurredDuringDeleteProcess;

        /// <summary>
        /// Build message when could not get object succeed
        /// Message: Could not get {name}
        /// </summary>
        /// <param name="name">Name of the object</param>
        /// <returns>string as message</returns>
        public static string CouldNotGet(string name)
            => string.Format(_couldNotGet, name);

        /// <summary>
        /// Build message when no thing change on database
        /// Messagge: Zero of objects written to the underlying database.
        /// </summary>
        /// <returns>string as message</returns>
        public static string DbHasNothingChange()
            => _dbHasNothingChange;

        /// <summary>
        /// Build message when could not found entity by dynamic filter
        /// Message: Could not found {type} by {spec}
        /// </summary>
        /// <param name="type">Type of entity</param>
        /// <param name="spec">Value of dynamic filter</param>
        /// <returns>string as message</returns>
        public static string CouldNotFoundBySpec(string type, string spec)
            => string.Format(_couldNotFoundBySpec, type, spec);

        /// <summary>
        /// Build message when object is null
        /// Message: {name} could not be null
        /// </summary>
        /// <param name="name">Type of object</param>
        /// <returns>string as message</returns>
        public static string CouldNotBeNull(string name)
            => string.Format(_couldNotBeNull, name);

        /// <summary>
        /// Build message when list of object is null or empty
        /// Message: List of {name} could not be null or empty
        /// </summary>
        /// <param name="name">Type of object</param>
        /// <returns>string as message</returns>
        public static string CouldNotBeNullOrEmpty(string name)
            => string.Format(_couldNotBeNullOrEmpty, name);

        /// <summary>
        /// Build message when try soft delete but the entity not inherited ISoftDelete
        /// Message: {name} should be inherited ISoftDelete to use this method
        /// </summary>
        /// <param name="name">Type of object</param>
        /// <returns>string as message</returns>
        public static string ShouldBeInheritedISoftDelete(string name)
            => string.Format(_shouldBeInheritedISoftDelete, name);

        /// <summary>
        /// Build message when the title already exist 
        /// </summary>
        /// <returns>string as message</returns>
        public static string ExistTitle()
            => _existTitle;
    }
}
