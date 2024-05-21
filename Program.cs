using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingLINQEventsExample
{
    class NumberProcessor
    {
        public void ProcessNumber(int number)
        {
            // acts as if the operation is loading
            var random = new Random();
            Thread.Sleep(random.Next(1000, 5000));
            Console.WriteLine($"Processed number: {number}");
        }
    }

    class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }

    class Downloader
    {
        public int Progress { get; set; }
        public event Action<int> ProgressChanged;

        public void DownloadFile()
        {
            for (int i = 0; i <= 100; i += 10)
            {
                Progress = i;
                ProgressChanged?.Invoke(i); // Raise the event
                Thread.Sleep(500); // intervals between download
            }
        }
    }

    class Program
    {
        static async Task Main()
        {
            // multi-threading by threading different numbers
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            var processor = new NumberProcessor();

            await Task.WhenAll(numbers.Select(number => Task.Run(() => processor.ProcessNumber(number))));

            // linq
            //processes products listed and turns themm into uppercase letters
            var products = new List<Product>
            {
                new Product { Name = "Apple", Price = 1.99 },
                new Product { Name = "Banana", Price = 0.79 },
                new Product { Name = "Orange", Price = 2.49 }
            };

            var filteredProducts = products.Where(p => p.Price >= 2.00)
                                           .Select(p => p.Name.ToUpper());

            Console.WriteLine("Products Filtered:");
            foreach (var productName in filteredProducts)
            {
                Console.WriteLine(productName);
            }

            // events is used for download progress
            var downloader = new Downloader();
            downloader.ProgressChanged += progress =>
            {
                Console.WriteLine($"Download progress: {progress}%");
            };

            Console.WriteLine("Starting download...");
            downloader.DownloadFile();

            Console.WriteLine("Download completed!");
        }
    }
}
