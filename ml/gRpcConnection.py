from concurrent import futures
from evrasaSender import evraseSend

import logging
import grpc
import FileTransfer_pb2
import FileTransfer_pb2_grpc

class FileTransferServicer(FileTransfer_pb2_grpc.FileTransferServiceServicer):
    def TransferFile(self, request, context):
        print("Получен файл: " + request.fileName)

        file_content = request.fileData.decode('utf-8') # Если файл текстовый
        processed_content = evraseSend(file_content, request.fileName)
        print(processed_content.encode('utf-8'))

        return FileTransfer_pb2.FileResponse(fileName=request.fileName, fileData=processed_content.encode('utf-8'))


def serve():
    port = "50051"
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    FileTransfer_pb2_grpc.add_FileTransferServiceServicer_to_server(FileTransferServicer(), server)
    server.add_insecure_port("[::]:" + port)
    server.start()
    print("Server started, listening on " + port)
    server.wait_for_termination()
