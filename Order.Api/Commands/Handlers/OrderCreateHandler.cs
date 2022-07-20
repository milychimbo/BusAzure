﻿using Common.Bus;
using Microsoft.Azure.ServiceBus;
using Order.Api.Commands.Impl;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using System.Linq;
using Order.Api.Events;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Order.Api.Commands.Handlers
{
    public class OrderCreateHandler : IHandler<OrderCreateCommand>
    {
        private readonly IServiceBus _serviceBus;

        public OrderCreateHandler(IServiceBus serviceBus) 
        {
            _serviceBus = serviceBus;
        }

        public async Task Execute(OrderCreateCommand command)
        {
            var client = _serviceBus.GetQueueClient("stock");

            var json = JsonSerializer.Serialize(
                command.Items.Select(x => new ProductStockEvent
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                }).ToList()
            );
            // 01. Your logic to order creation
            
            
            // 02. Azure Service Bus
           

            await client.SendAsync(
                new Message(Encoding.UTF8.GetBytes(json))
            );

            await client.CloseAsync();
        }
    }
}