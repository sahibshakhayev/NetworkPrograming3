using HTTP_Clone_Server;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;

var ip = IPAddress.Loopback;
var port = 27001;

var client = new TcpClient();
client.Connect(ip, port);
var stream = client.GetStream();
var br = new BinaryReader(stream);
var bw = new BinaryWriter(stream);
Command command = null;
string responce = null;

while (true)
{
    Console.WriteLine("Write command or HELP: ");
    var str = Console.ReadLine().ToUpper();
    if (str == "HELP")
    {
        Console.WriteLine();
        Console.WriteLine("Command list:");
        Console.WriteLine(Command.GET);
        Console.WriteLine($"{Command.POST} <Manufacter> <Model> <Year>");
        Console.WriteLine($"{Command.PUT} <Id> ?<Manufacter> ?<Model> ?<Year>");
        Console.WriteLine($"{Command.DELETE} <Id>");
        Console.WriteLine($"HELP");
        Console.ReadKey();
        Console.Clear();
        continue;
    }
    var input = str.Split(' ');
    switch (input[0])
    {
        case Command.GET:
            command = new Command { HTTPMethod = input[0] };
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            var carList = JsonSerializer.Deserialize<List<Car>>(responce);
            foreach (var car_item in carList)
            {
                Console.WriteLine(car_item);
            }
            Console.ReadKey();
            Console.Clear();
            break;

        case Command.POST:
            var car = new Car() { Manufacter = input[1], Model = input.Length > 2 ? input[2] : null, Year = input.Length > 3 ? int.Parse(input[3]) : null };
            command = new Command { HTTPMethod = input[0], Value = car };
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            var answer = JsonSerializer.Deserialize<string>(responce);
            Console.WriteLine(answer);
            Console.ReadKey();
            Console.Clear();
            break;
        case Command.PUT:
            Car u_car = new Car() { Id = int.Parse(input[1]), Manufacter = input[2], Model = input.Length > 3 ? input[3] : null, Year= input.Length > 4 ? int.Parse(input[4]) : null };
            command = new Command { HTTPMethod = input[0], Value = u_car};
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            var answer2 = JsonSerializer.Deserialize<string>(responce);
            Console.WriteLine(answer2);
            Console.ReadKey();
            Console.Clear();
            break;
        case Command.DELETE:

            var d_car = new Car() {Id = int.Parse(input[1]) };
            command = new Command { HTTPMethod = input[0], Value = d_car};
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            var answer3 = JsonSerializer.Deserialize<string>(responce);
            Console.WriteLine(answer3);
            Console.ReadKey();
            Console.Clear();
            break;

    }


}
