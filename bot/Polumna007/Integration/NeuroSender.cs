﻿using FileTransfer;
using Google.Protobuf;
using Grpc.Net.Client;
using static FileTransfer.FileTransferService;

namespace Integration;

public class NeuroSender : IDisposable
{
    private const string _host = "http://localhost:50051";
    private readonly GrpcChannel _channel;
    private readonly FileTransferServiceClient _client;

    public NeuroSender()
    {
        _channel = GrpcChannel.ForAddress(_host);
        _client = new FileTransferServiceClient(_channel);
    }

    public void Dispose()
    {
        _channel.Dispose();
    }

    public async Task Send(string sourcePath,  string destinationPath)
    {
        var request = new FileRequest
        {
            FileName = Path.GetFileName(sourcePath),
            FileData = ByteString.CopyFrom(File.ReadAllBytes(sourcePath))
        };
        try
        {
            var reply = await _client.TransferFileAsync(request);
            File.WriteAllBytes(destinationPath, reply.FileData.ToByteArray());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка отправки запроса на питон: {ex.Message}");
        }
    }
}