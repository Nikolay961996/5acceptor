from concurrent import futures
from evrasaSender import evraseSend
from markdown_pdf import MarkdownPdf

import logging
import grpc
import FileTransfer_pb2
import FileTransfer_pb2_grpc

class FileTransferServicer(FileTransfer_pb2_grpc.FileTransferServiceServicer):
    def TransferFile(self, request, context):
        print("Получен файл: " + request.fileName)

        file_content = request.fileData.decode('utf-8') # Если файл текстовый
        processed_content = evraseSend(file_content, request.fileName)
        # pdf = MarkdownPdf(toc_level=2)
        # pdf.add_section(Section(processed_content, toc=False))
        # fileName = request.fileName + ".pdf";
        # pdf.save(fileName)
        # with open(fileName, 'r', encoding='utf-8') as code_snippet:
        #        code_snippet_content = code_snippet.read()

        return FileTransfer_pb2.FileResponse(fileName=request.fileName, fileData=processed_content.encode('utf-8'))


def serve():
    port = "50051"
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    FileTransfer_pb2_grpc.add_FileTransferServiceServicer_to_server(FileTransferServicer(), server)
    server.add_insecure_port("[::]:" + port)
    server.start()
    print("Server started, listening on " + port)
    server.wait_for_termination()
