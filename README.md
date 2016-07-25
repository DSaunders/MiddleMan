# MiddleMan

A Command, Query and Message passing library that aids in using the mediator pattern. Shamelessly 'inspired' by Jimmy Bogard's [MediatR](https://github.com/jbogard/MediatR) (which you should probably use instead of this!).


### Getting started
- Install via [Nuget]().
- Create your Command and Query handlers (see below)
- Take a dependency on `IBroker` to start passing commands/queries

### IoC container setup

If you are not using 'auto-discover' on your IoC container, you will need to register:
- `Broker` -> `MiddleMan.Broker`. This should be a singleton.
- All implementations of `IHandler` (both `IQuery<T>` and `ICommmand` implement this interface)
- All implementations of `IPipelineTask` (if you are using pipelines)
- All implementations of `IMessageSubscriber` (if you are using messages)

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

Then just call `IBroker` with an instance of the query:

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

Define subscribers to messages like so:
```csharp
public class TestMessageLogger : IMessageSubscriber<TestMessage>
{
    public void OnMessageReceived(TestMessage message)
    {
        Log.Info(message.MessageText);
    }
}
```

Dispatch a message to all subscribers:
```csharp
 _broker.SendMessage(new TestMessage { MessageText = "Hello, World" });
```

Subscribers to a message type will also receive notification of all messages that derive from that type. So, you can create a base class for all of your messages, subscribe to that and receive all messages sent through the broker.

### Pipelines

A pipeline runs a sequence of actions, in order, against a given message.

Firstly, create a message to be passed along the pipeline, this should implement `IPipelineMessage`:
```csharp
public class SomePipelineMessage : IPipelineMessage
{
    public string Text { get; }
}
```

Then, define actions that can be performed on this message using `IPipelineTask<T>`, where `T` is the type of message this task should handle:
```csharp
public class PipelineTaskFoo : IPipelineTask<SomePipelineMessage>
{
    public async Task Run(SomePipelineMessage message)
    {
        message.Text += "Foo"
    }
}
```

Finally, construct a pipeline to process these items:
```csharp
_broker.ConstructPipeline<PipelineMessage>(p =>
    {
        p.Add<PipelineTaskFoo>();
        p.Add<PipelineTaskFoo>();
    });
```

You can then call the pipeline for your message in a single line. MiddleMan will find the correct pipeline
to handle the message you pass it.

If there are no pipelines registered for the type of message you pass to the broker, the message will simply be ignored.
