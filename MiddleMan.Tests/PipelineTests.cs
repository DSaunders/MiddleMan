﻿namespace MiddleMan.Tests
{
    using System.Threading.Tasks;
    using Exceptions;
    using Fakes;
    using Fakes.Pipeline;
    using Fakes.Pipeline.EmptyPipeline;
    using Fakes.Pipeline.MultiplePipelines;
    using Fakes.Pipeline.NoPipelines;
    using Should;
    using Xunit;

    public class PipelineTests
    {
        private readonly IBroker _broker;

        public PipelineTests()
        {
            _broker = FakeIoCSetup.GetBroker();
        }

        [Fact]
        public void Runs_Pipeline_In_Order()
        {
            // Given
            var message = new FooBarPipelineMessage();
            
            // When
            _broker.RunPipeline(message);

            // Then
            message.TasksRun[0].ShouldEqual("Foo");
            message.TasksRun[1].ShouldEqual("Bar");
            message.TasksRun.Count.ShouldEqual(2);
        }

        [Fact]
        public void Throws_When_Muptiple_Pipelines_Are_Registered_For_A_Message()
        {
            // Given
            var message = new MultiplePipelineMessage();

            // When
            var ex = Record.Exception(() => _broker.RunPipeline(message));

            // Then
            ex.Message.ShouldEqual("2 Pipelines found for MultiplePipelineMessage");
            ex.ShouldBeType<MultiplePipelinesException>();
        }

        [Fact]
        public void Does_Not_Throw_When_No_Pipelines_Are_Registered_For_A_Message()
        {
            // Given
            var message = new NoPipelinesMessage();

            // When
            var ex = Record.Exception(() => _broker.RunPipeline(message));

            // Then
            ex.ShouldBeNull();
        }

        [Fact]
        public void Does_Not_Throw_When_Pipeline_Is_Empty()
        {
            // Given
            var message = new EmptyPipelineMessage();

            // When
            var ex = Record.Exception(() => _broker.RunPipeline(message));

            // Then
            ex.ShouldBeNull();
        }

        [Fact]
        public async Task Runs_Pipeline_Async()
        {
            // Given
            var message = new FooBarPipelineMessage();

            // When
            await _broker.RunPipelineAsync(message);

            // Then
            message.TasksRun[0].ShouldEqual("Foo");
            message.TasksRun[1].ShouldEqual("Bar");
            message.TasksRun.Count.ShouldEqual(2);
        }

        [Fact]
        public void Throws_When_Muptiple_Async_Pipelines_Are_Registered_For_A_Message()
        {
            // Given
            var message = new MultiplePipelineMessage();

            // When
            var ex = Record.Exception(() => _broker.RunPipelineAsync(message).Wait());

            // Then
            ex.InnerException.Message.ShouldEqual("2 Async Pipelines found for MultiplePipelineMessage");
            ex.InnerException.ShouldBeType<MultiplePipelinesException>();
        }

        [Fact]
        public void Does_Not_Throw_When_No_Async_Pipelines_Are_Registered_For_A_Message()
        {
            // Given
            var message = new NoPipelinesMessage();

            // When
            var ex = Record.Exception(() => _broker.RunPipelineAsync(message).Wait());

            // Then
            ex.ShouldBeNull();
        }

        [Fact]
        public void Does_Not_Throw_When_Async_Pipeline_Is_Empty()
        {
            // Given
            var message = new EmptyPipelineMessage();

            // When
            var ex = Record.Exception(() => _broker.RunPipelineAsync(message).Wait());

            // Then
            ex.ShouldBeNull();
        }
    }
}
