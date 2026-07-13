using System.Runtime.Serialization;

namespace Revision_of_Data_Seeding.CustomExcptions
{
    public class InvalidPersonIdException : Exception
    {
        public InvalidPersonIdException()
        {
        }

        public InvalidPersonIdException(string? message) : base(message)
        {
        }

        public InvalidPersonIdException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        //protected InvalidPersonIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //}
    }
}
