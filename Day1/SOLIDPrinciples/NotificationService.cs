using System;
using System.Collections.Generic;

namespace NotificationService
{

    public interface INotificationService
    {
        void Send(string to, string message, string channelName);
    }


    public class EmailNotificationChannel : INotificationService
    {
        public void Send(string to, string message, string channelName)
        {
            Console.WriteLine($"[{channelName}] Sending Email to {to}: {message}");
        }
    }


    public class SmsNotificationChannel : INotificationService
    {
        public void Send(string to, string message, string channelName)
        {
            Console.WriteLine($"[{channelName}] Sending SMS to {to}: {message}");
        }
    }


    public class PushNotificationChannel : INotificationService
    {
        public void Send(string to, string message, string channelName)
        {
            Console.WriteLine($"[{channelName}] Sending Push Notification to {to}: {message}");
        }
    }


    public class NotificationService
    {
        private readonly Dictionary<string, INotificationService> _channels;

        public NotificationService()
        {
            _channels = new Dictionary<string, INotificationService>();
        }

        public void RegisterChannel(string channelName, INotificationService channel)
        {
            _channels[channelName] = channel;
        }

        public void SendNotification(string channelName, string to, string message)
        {
            if (_channels.ContainsKey(channelName))
            {
                _channels[channelName].Send(to, message, channelName);
            }
            else
            {
                throw new Exception("Notification channel not found");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var notificationService = new NotificationService();

            // Register channels
            notificationService.RegisterChannel("email", new EmailNotificationChannel());
            notificationService.RegisterChannel("sms", new SmsNotificationChannel());
            notificationService.RegisterChannel("push", new PushNotificationChannel());

            while (true)
            {
                Console.WriteLine("Select Notification Channel:");
                Console.WriteLine("1. Email");
                Console.WriteLine("2. SMS");
                Console.WriteLine("3. Push Notification");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                if (choice == "4")
                {
                    break;
                }

                string channelName = choice switch
                {
                    "1" => "email",
                    "2" => "sms",
                    "3" => "push",
                    _ => null
                };

                if (channelName == null)
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                    continue;
                }

                Console.Write("Enter recipient: ");
                var to = Console.ReadLine();

                Console.Write("Enter message: ");
                var message = Console.ReadLine();

                try
                {
                    notificationService.SendNotification(channelName, to, message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
