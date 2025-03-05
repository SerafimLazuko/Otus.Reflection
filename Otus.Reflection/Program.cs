// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Otus.Reflection;
using System.Diagnostics;

var profile = UserProfile.GetUserProfile();

const int iterations = 1000000;
var stopwatch = new Stopwatch();

//GenericSerializer serialization time
stopwatch.Start();
for (int i = 0; i < iterations; i++)
{
    var csv = GenericSerializer<UserProfile>.Serialize(profile);
}
stopwatch.Stop();
var genericSerializationTime = stopwatch.Elapsed.TotalMilliseconds / iterations;

//GenericSerializer deserialization time
var serializedCsv = GenericSerializer<UserProfile>.Serialize(profile);
stopwatch.Restart();
for (int i = 0; i < iterations; i++)
{
    var deserializedProfile = GenericSerializer<UserProfile>.Deserialize(serializedCsv);
}
stopwatch.Stop();
var genericDeserializationTime = stopwatch.Elapsed.TotalMilliseconds / iterations;

//Newtonsoft.Json serialization time
stopwatch.Restart();
for (int i = 0; i < iterations; i++)
{
    var json = JsonConvert.SerializeObject(profile);
}
stopwatch.Stop();
var newtonsoftSerializationTime = stopwatch.Elapsed.TotalMilliseconds / iterations;

//Newtonsoft.Json deserialization time
var serializedJson = JsonConvert.SerializeObject(profile);
stopwatch.Restart();
for (int i = 0; i < iterations; i++)
{
    var deserializedProfile = JsonConvert.DeserializeObject<UserProfile>(serializedJson);
}
stopwatch.Stop();
var newtonsoftDeserializationTime = stopwatch.Elapsed.TotalMilliseconds / iterations;

Console.WriteLine($"Average GenericSerializer serialization time: {genericSerializationTime} ms");
Console.WriteLine($"Average GenericSerializer deserialization time: {genericDeserializationTime} ms");
Console.WriteLine($"Average Newtonsoft.Json serialization time: {newtonsoftSerializationTime} ms");
Console.WriteLine($"Average Newtonsoft.Json deserialization time: {newtonsoftDeserializationTime} ms");

Console.ReadKey();