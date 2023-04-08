using HTTP_Clone_Server;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;

var ip = IPAddress.Loopback;
var port = 27001;
List<Car> cars = new List<Car>() { };

var listener = new TcpListener(ip, port);

listener.Start();

while (true)
{
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    var br = new BinaryReader(stream);
    var bw = new BinaryWriter(stream);

    while (true)
    {
        var input = br.ReadString();
        Console.WriteLine(input);
        var command = JsonSerializer.Deserialize<Command>(input);

        if (command == null) continue;

        Console.WriteLine(command.HTTPMethod);
        switch (command.HTTPMethod)
        {
            case Command.GET:
                GetMethod(bw);
                break;
            case Command.POST:
                PostMethod(bw, command.Value);
                break;

            case Command.PUT:
                  PutMethod(bw, command.Value,command.Value.Id);


                break;
            case Command.DELETE:
                DeleteMethod(bw, command.Value.Id);
                break;
            default:
                break;
        }

    }
}


void GetMethod(BinaryWriter bw)
{
    var JSONCars = JsonSerializer.Serialize(cars);
    bw.Write(JSONCars);
    
}

void PostMethod(BinaryWriter bw, Car car)
{
    cars.Add(car);
    bw.Write(JsonSerializer.Serialize(new String($"Car Added!")));

}

void PutMethod(BinaryWriter bw, Car car, int id)
{
    foreach (var car_item in cars)
    {
        if (car_item.Id == id)
        {
            cars[cars.IndexOf(car_item)] = car;
            bw.Write(JsonSerializer.Serialize(new String($"Car Updated!")));
            break;
        }
    }
    
}

void DeleteMethod(BinaryWriter bw, int id) {
    foreach (var car_item in cars)
    {
        if (car_item.Id == id)
        {
            cars.Remove(car_item);
            bw.Write(JsonSerializer.Serialize(new String($"Car Deteted!")));
            break;
        }
    }

}
