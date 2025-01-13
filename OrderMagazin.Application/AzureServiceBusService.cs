using Azure.Messaging.ServiceBus;
using OrderManagement.Application.DTOs;
using System.Text.Json;

namespace OrderManagement.Application.Services
{
    public class AzureServiceBusService
    {
        private readonly string _connectionString;

        public AzureServiceBusService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SendWorkflowMessageAsync(string workflowType, object payload)
        {
            try
            {
                await using var client = new ServiceBusClient(_connectionString);
                await using var sender = client.CreateSender("workflow-queue");

                var messageBody = JsonSerializer.Serialize(new WorkflowMessage
                {
                    WorkflowType = workflowType,
                    Payload = JsonSerializer.Serialize(payload)
                });

                var message = new ServiceBusMessage(messageBody);
                Console.WriteLine($"Sending message to queue: {messageBody}");
                await sender.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                // Log the error (înlocuiește cu un sistem de logare real, dacă este necesar)
                Console.WriteLine($"Error sending message to Service Bus: {ex.Message}");
            }
        }

    }
}
