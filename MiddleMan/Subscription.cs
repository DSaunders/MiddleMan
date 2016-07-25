namespace MiddleMan
{
    using System;
    using Message;

    internal class Subscription<T> : ISubscription where T : IMessage
    {
        public Type Type { get; protected set; }
        public Action<T> Action { get; }

        public Subscription(Action<T> action)
        {
            Action = action;
            Type = typeof(T);
        }
    }
}