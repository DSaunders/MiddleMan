# MiddleMan

A library for abstracting your code into commands, queries, messages and pipelines.

## Getting started
- Install via [Nuget]().
- Create your Command and Query handlers (see below)
- Take a dependency on `IBroker` to start passing commands/queries

## Setup

MiddleMan is designed to make configuration easier by allowing you to implement interfaces for everything and have it 'just work'. This works best when your container auto-registers everything for you, and supports multiple concrete types for a given interface.

If you need to register your dependencies manually, see 'Manual IoC container setup'.

## Queries

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

## Commands
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

## Messages

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
    public async Task OnMessageReceived(TestMessage message)
    {
        Log.Info(message.MessageText);
    }
}
```

Dispatch a message to all subscribers:
```csharp
 await _broker.SendMessageAsync(new TestMessage { MessageText = "Hello, World" });
```

Subscribers to a message type will also receive notification of all messages that derive from that type. So, you can create a base class for all of your messages, subscribe to that and receive all messages sent through the broker.

## Pipelines

A pipeline runs a sequence of actions, in order, against a given message.

Firstly, create a message to be passed along the pipeline, this should implement `IPipelineMessage`:
```csharp
public class SomePipelineMessage : IPipelineMessage
{
    public string Text { get; }
}
```

Then, create the steps of the pipeline. The simplest way to do this is to derive each task in the pipeline from `PipelineTaskBase<T>`, where `T` is the message type that the pipeline task will process.

```csharp
public class SomePipelineTask : PipelineTaskBase<SomePipelineMessage>
{
    public override void Run(SomePipelineMessage message)
    {
        message.Text += "Foo"

        // Call the next step in the pipeline.
        // Alternatively, do nothing and terminate the pipeline early
        Next(message);
    }
}
```

Finally, tell MiddleMan how to construct the pipeline for your message type, by implementing `IPipeline<T>`:
```csharp
public class SomePipeline : IPipeline<SomePipelineMessage>
{
    public void GetPipelineTasks(PipelineBuilder<SomePipelineMessage> builder)
    {
        builder.Add<SomePipelineTask>();
        builder.Add<AnotherPipelineTask>();
    }
}
```

You can then call the pipeline for your message in a single line. MiddleMan will find the correct pipeline
to handle the message you pass it.

```csharp
_broker.RunPipeline(new SomePipelineMessage());
```

If there are no pipelines registered for the type of message you pass to the broker, the message will simply be ignored.

Here's the Async version:

```csharp
public class SomePipelineTaskAsync : PipelineTaskBaseAsync<SomePipelineMessage>
{
    public override async Task Run(SomePipelineMessage message)
    {
        message.Text += "Foo"
        await Next(message);
    }
}

public class SomePipelineAsync : IPipelineAsync<SomePipelineMessage>
{
    public void GetPipelineTasks(PipelineBuilderAsync<SomePipelineMessage> builder)
    {
        builder.Add<SomePipelineTaskAsync>();
        builder.Add<SomeOtherPipelineTaskAsync>();
    }
}

await _broker.RunPipelineAsync(new SomePipelineMessage());
```

## Manual IoC container setup

If you are not using 'auto-discover' on your IoC container, you will need to register:
- `Broker` -> `MiddleMan.Broker`.
- All implementations of `IHandler` (both `IQuery<T>` and `ICommmand` implement this interface)
- All implementations of `IPipelineTask` and `IPipeline` (if you are using pipelines)
- All implementations of `IMessageSubscriber` (if you are using messages)
