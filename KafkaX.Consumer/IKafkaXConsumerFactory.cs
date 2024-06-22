namespace KafkaX;

public interface IKafkaXConsumerFactory
{
    IKafkaXConsumer Subscribe(string topic);
}
