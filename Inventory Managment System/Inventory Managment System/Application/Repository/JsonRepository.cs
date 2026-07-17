using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inventory_Managment_System.Data.Repositories
{
    public class JsonRepository<T>
    {
        private readonly string _filePath;

        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };


        public JsonRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException(
                    "File path cannot be empty.",
                    nameof(filePath));
            }

            _filePath = filePath;

            EnsureFileExists();
        }

        public List<T> GetAll()
        {
            try
            {
                string json =
                    File.ReadAllText(_filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return [];
                }

                return JsonSerializer.Deserialize<List<T>>(
                           json,
                           _options)
                       ?? [];
            }
            catch (JsonException)
            {
                Console.WriteLine(
                    $"Invalid JSON in {_filePath}.");

                return [];
            }
            catch (IOException exception)
            {
                Console.WriteLine(
                    $"Could not read {_filePath}: " +
                    exception.Message);

                return [];
            }
        }

        public void SaveAll(
            IEnumerable<T> items)
        {
            ArgumentNullException.ThrowIfNull(items);

            try
            {
                string json =
                    JsonSerializer.Serialize(
                        items,
                        _options);

                File.WriteAllText(
                    _filePath,
                    json);
            }
            catch (IOException exception)
            {
                throw new InvalidOperationException(
                    $"Could not save data in {_filePath}.",
                    exception);
            }
        }

        private void EnsureFileExists()
        {
            string? directory =
                Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(
                    _filePath,
                    "[]");
            }
        }
    }
}