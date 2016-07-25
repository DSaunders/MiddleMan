﻿namespace MiddleMan.Tests.Fakes.Message.Subscribers
{
    using System.Threading.Tasks;
    using Messages;
    using MiddleMan.Message;

    public class MessageParentSubscriber : IMessageSubscriber<MessageParent>
    {
        public async Task OnMessageReceived(MessageParent message)
        {
            message.Subscribers.Add("MessageParent");
        }
    }
}