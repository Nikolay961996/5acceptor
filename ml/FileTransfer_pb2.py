# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# NO CHECKED-IN PROTOBUF GENCODE
# source: FileTransfer.proto
# Protobuf Python Version: 5.27.2
"""Generated protocol buffer code."""
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import runtime_version as _runtime_version
from google.protobuf import symbol_database as _symbol_database
from google.protobuf.internal import builder as _builder
_runtime_version.ValidateProtobufRuntimeVersion(
    _runtime_version.Domain.PUBLIC,
    5,
    27,
    2,
    '',
    'FileTransfer.proto'
)
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n\x12\x46ileTransfer.proto\x12\x0c\x46ileTransfer\"1\n\x0b\x46ileRequest\x12\x10\n\x08\x66ileName\x18\x01 \x01(\t\x12\x10\n\x08\x66ileData\x18\x02 \x01(\x0c\"2\n\x0c\x46ileResponse\x12\x10\n\x08\x66ileName\x18\x01 \x01(\t\x12\x10\n\x08\x66ileData\x18\x02 \x01(\x0c\x32^\n\x13\x46ileTransferService\x12G\n\x0cTransferFile\x12\x19.FileTransfer.FileRequest\x1a\x1a.FileTransfer.FileResponse\"\x00\x62\x06proto3')

_globals = globals()
_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, _globals)
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'FileTransfer_pb2', _globals)
if not _descriptor._USE_C_DESCRIPTORS:
  DESCRIPTOR._loaded_options = None
  _globals['_FILEREQUEST']._serialized_start=36
  _globals['_FILEREQUEST']._serialized_end=85
  _globals['_FILERESPONSE']._serialized_start=87
  _globals['_FILERESPONSE']._serialized_end=137
  _globals['_FILETRANSFERSERVICE']._serialized_start=139
  _globals['_FILETRANSFERSERVICE']._serialized_end=233
# @@protoc_insertion_point(module_scope)