using Azure.Messaging.ServiceBus;
using System.Text.Json;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Services
{
    public class WorkflowProcessorService
    {
        private readonly string _connectionString;

        public WorkflowProcessorService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task ProcessWorkflowMessagesAsync()
        {
            var client = new ServiceBusClient(_connectionString);
            var receiver = client.CreateReceiver("workflow-queue");

            var messages = await receiver.ReceiveMessagesAsync(maxMessages: 10);

            foreach (var message in messages)
            {
                try
                {
                    var body = message.Body.ToString();
                    var workflowMessage = JsonSerializer.Deserialize<WorkflowMessage>(body);

                    switch (workflowMessage.WorkflowType)
                    {
                        case "OrderProcessing":
                            var orderProcessingData = JsonSerializer.Deserialize<OrderProcessingDto>(workflowMessage.Payload);
                            await ProcessOrder(orderProcessingData.OrderId);
                            break;

                        case "OrderCancelation":
                            var orderCancelationData = JsonSerializer.Deserialize<OrderCancelationDto>(workflowMessage.Payload);
                            await CancelOrder(orderCancelationData.OrderId);
                            break;

                        case "InvoiceProcessing":
                            var invoiceProcessingData = JsonSerializer.Deserialize<InvoiceProcessingDto>(workflowMessage.Payload);
                            await GenerateInvoice(invoiceProcessingData.OrderId);
                            break;

                        default:
                            throw new InvalidOperationException("Unknown workflow type.");
                    }

                    await receiver.CompleteMessageAsync(message); // Confirmă procesarea mesajului
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    await receiver.AbandonMessageAsync(message); // Marchează mesajul ca neprocesat
                }
            }

            await receiver.CloseAsync();
        }

        private Task ProcessOrder(Guid orderId)
        {
            // Adaugă logica pentru procesarea comenzii
            Console.WriteLine($"Processing order: {orderId}");
            return Task.CompletedTask;
        }

        private Task CancelOrder(Guid orderId)
        {
            // Adaugă logica pentru anularea comenzii
            Console.WriteLine($"Canceling order: {orderId}");
            return Task.CompletedTask;
        }

        private Task GenerateInvoice(Guid orderId)
        {
            // Adaugă logica pentru generarea facturii
            Console.WriteLine($"Generating invoice for order: {orderId}");
            return Task.CompletedTask;
        }
    }
}
