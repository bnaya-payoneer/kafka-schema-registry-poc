﻿using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: understand the Message.Key

namespace KafkaX;
public static class KafkaProduceExtensions
{
    public static async Task<DeliveryResult<TValue>> ProduceXAsync<TValue>(
        this IProducer<Null, byte[]> producer,
        string topic,
        Message<Null, TValue> message,
        CancellationToken cancellationToken = default(CancellationToken))
        where TValue : IKafkaIdentifier
    {
        // TODO: fetch the schema (scemaKey, scemaVersion)
        // TODO: var item = Avro.Serialize(schema, buffer)
        //var message = new Message<Null, string>
        //{
        //    Value = item,
        //    //Headers =
        //};
        //var response = await producer.ProduceAsync(topic, message, cancellationToken);
        //return response;
    }
}
