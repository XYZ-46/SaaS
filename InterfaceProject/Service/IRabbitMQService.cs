﻿using AppConfiguration;

namespace InterfaceProject.Service
{
    public interface IRabbitMQService 
    {
        bool IsSameMessageConfig(MessageRabbitMQConfig messageConfig);
        void SetupQueue(MessageRabbitMQConfig messageConfig);
        void PushMessageIntoQueue(byte[] message);
        void PushMessageIntoQueue(string message);
        void PushMessageIntoQueue<T>(T message);
    }
}
