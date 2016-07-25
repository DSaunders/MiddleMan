namespace MiddleMan.Tests
{
    using Exceptions;
    using Pipeline;
    using Should;
    using Xunit;

    public partial class MessageBrokerTests
    {
        [Fact]
        public async void Runs_Pipeline_In_Order()
        {
            // Given
            var message = new PipelineMessage();
            _broker.ConstructPipeline<PipelineMessage>(p =>
            {
                p.Add<PipelineTaskFoo>();
                p.Add<PipelineTaskBar>();
            });

            // When
            await _broker.RunPipelineAsync(message);

            // Then
            message.TasksRun[0].ShouldEqual("Foo");
            message.TasksRun[1].ShouldEqual("Bar");
            message.TasksRun.Count.ShouldEqual(2);
        }

        [Fact]
        public void Throws_When_A_Pipeline_Is_Already_Registered_For_A_Message()
        {
            // Given
            _broker.ConstructPipeline<PipelineMessage>(p =>
            {
                p.Add<PipelineTaskFoo>();
                p.Add<PipelineTaskBar>();
            });
            
            // When
            var ex = Record.Exception(() => _broker.ConstructPipeline<PipelineMessage>(p => { }));

            // Then
            ex.Message.ShouldEqual("A pipeline already exists to handle this PipelineMessage type");
            ex.ShouldBeType<MultiplePipelinesException>();
        }
    }
}
