using System.Net.Sockets;
using System.Text;
using DiscountCodeServer.Messages;

while (true)
{
    Console.WriteLine("\n--- DISCOUNT CLIENT ---");
    Console.WriteLine("1. Generate Discount Codes");
    Console.WriteLine("2. Use Discount Code");
    Console.WriteLine("0. Exit");
    Console.Write("Choose option: ");
    var input = Console.ReadLine();

    if (input == "0") break;

    using TcpClient client = new TcpClient("127.0.0.1", 5555);
    using NetworkStream stream = client.GetStream();
    using var writer = new BinaryWriter(stream, Encoding.ASCII, leaveOpen: true);
    using var reader = new BinaryReader(stream, Encoding.ASCII, leaveOpen: true);

    switch (input)
    {
        case "1":
            Console.Write("How many codes? ");
            int count = int.Parse(Console.ReadLine() ?? "5");

            Console.Write("Code length (7 or 8): ");
            int length = int.Parse(Console.ReadLine() ?? "8");

            writer.Write((byte)0x01); // Generate
            writer.Write(count);
            writer.Write(length);
            writer.Flush();

            try
            {
                var response = GenerateResponse.FromStream(reader);
                Console.WriteLine($"[CLIENT] Code generation {(response.Result ? "Success ✅" : "Failed ❌")}");
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine("[CLIENT] Error: Unexpected end of stream from server.");
            }
            break;

        case "2":
            Console.Write("Enter code to use: ");
            string codeToUse = Console.ReadLine() ?? "";

            writer.Write((byte)0x02); // UseCode
            writer.Write(codeToUse);
            writer.Flush();

            byte successByte = reader.ReadByte();
            bool success = successByte == 1;
            Console.WriteLine($"[CLIENT] UseCode result: {(success ? "Success ✅" : "Failed ❌")}");
            break;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}
