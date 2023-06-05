using System;

namespace DotEngine.BB
{
    public class BlackboardException : Exception
    {
        public BlackboardException(string message) : base(message)
        { }
    }

    public class BlackboardKeyRepeatedException : BlackboardException
    {
        public BlackboardKeyRepeatedException(object key)
            : base($"The key({key}) has been added into blackboard")
        { }
    }

    public class BlackboardKeyNotFoundException : BlackboardException
    {
        public BlackboardKeyNotFoundException(object key)
            : base($"The key({key}) is not found in blackboard")
        {

        }
    }
}
