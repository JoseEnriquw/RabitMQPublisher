
namespace Core.Domain.Common
{
    public static class ErrorMessage
    {
        public const string EMPTY_VALUE = "{0} cannot be empty";
        public const string MAXIMUM_LENGTH = "{0} cannot have more than {1} characters";
        public const string NULL_VALUE = "{0} cannot be null";
        public const string USER_DELETED = "The User '{0}' is deleted successfully";
        public const string INVALID_MAIL = "Email is not valid";
        public const string NOT_FOUND_GET_BY_ID = "The Id {0} of {1} was not found in the database";
        public const string MUST_BE_A_POSITIVE_NUMBER = "{0} must be greater than zero";
    }

}
