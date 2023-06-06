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

    public class BlackboardValueNotCastException : BlackboardException
    {
        public BlackboardValueNotCastException(object key, Type targetType, object value)
            : base($"The value({value.GetType().Name}) of key(${key}) cant be casted to ${targetType.Name}")
        {

        }
    }

    public class BlackboardInvokeEndlessLoopException : BlackboardException
    {
        public BlackboardInvokeEndlessLoopException(object key)
            : base($"Listener({key}) was called in circular dependencies")
        {

        }
    }
}
