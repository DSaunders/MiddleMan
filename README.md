# MiddleMan

A Command, Query and Message passing library that aids in using the mediator pattern. Shamelessly 'inspired' by Jimmy Bogard's [MediatR](https://github.com/jbogard/MediatR) (which you should probably use instead of this!).


### Getting started
- Install via [Nuget]().
- Create your Command and Query handlers (see below)
- Take a dependency on `IMessageBroker` to start passing commands/queries

### IoC container setup

If you are not using 'auto-discover' on your IoC container, you will need to register:
- `IMessageBroker` -> `MiddleMan.MessageBroker`
- All implementations of `IHandler` (both `IQuery<T>` and `ICommmand` implement this interface)

### Queries

Queries take an Query object and return a value.

Queries are dispatched to a single handler. Multiple handlers for the same query will cause an exception to be thrown.

Create a query object that implements `IQuery<T>` (where `T` is the type that the query should return):

```csharp
public class MyQuery : IQuery<string>
{
    public string Name { get; set; }
}
```

Create a handler for the query:
```csharp
public class MyQueryHandler : IQueryHandler<MyQuery, string>
{
    public string HandleQuery(MyQuery query)
    {
        return "Hello " + query.Name;
    }
}
```

Then just call `IMessageBroker` with an instance of the query:

```csharp
var result = _broker.ProcessQuery(new MyQuery { Name = "Dave" });

Assert.Equal(result, "Hello Dave");
```

MiddleMan will discover the correct handler, pass in the Query object and return the result to the caller.

And, of course, there's also an async version:

```csharp
// Async Handler
public class MyQueryHandler : IQueryHandlerAsync<MyQuery, string>
{
    public async Task<string> HandleQueryAsync(MyQuery query)
    {
        await SomeLongRunningThing();
        return "Hello " + query.Name;
    }
}

// Await the result of the async call
var result = await _broker.ProcessQueryAsync(new MyQuery { Name = "Dave" });
```

### Commands
A Command is the same as a Query, but just doesn't return a result.

Just like Queries, only one Command handler can exist per command type.

```csharp
public class TestCommand : ICommand
{
      public string Name { get; set; }
}

public class TestCommandHandler : ICommandHandler<TestCommand>
{
    public void HandleCommand(TestCommand command)
    {
        Console.WriteLine("Hello " + command.Name);
    }
}

_broker.ProcessCommand(command);
```

And the async version:
```csharp
public class TestAsyncCommandHandler : ICommandHandlerAsync<TestCommand>
{
    public async Task HandleCommandAsync(TestCommand command)
    {
        await SomeLongRunningThing();         
    }
}

await _broker.ProcessCommandAsync(command);
```

### Messages

Unlike Commands and Queries, Messages can be dispatched to multiple subscribers.

First, define a message object:

```csharp
public class TestMessage : IMessage
{
    public string MessageText { get; }
}
```

Subscribe to the message like so:
```csharp
_broker.SubscribeToMessage<TestMessage>(m =>
{
    Console.WriteLine(m.MessageText);
});
```

Dispatch a message to all subscribers:
```csharp
 _broker.SendMessage(new TestMessage { MessageText = "Hello, World" });
```

Subscibers to a message type will also receive notification of all messages that derive from that type. So, you can create a base class for all of your messages, subscribe to that and receive all messages sent through the broker.
