from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from typing import ClassVar as _ClassVar, Optional as _Optional

DESCRIPTOR: _descriptor.FileDescriptor

class FileRequest(_message.Message):
    __slots__ = ("fileName", "fileData")
    FILENAME_FIELD_NUMBER: _ClassVar[int]
    FILEDATA_FIELD_NUMBER: _ClassVar[int]
    fileName: str
    fileData: bytes
    def __init__(self, fileName: _Optional[str] = ..., fileData: _Optional[bytes] = ...) -> None: ...

class FileResponse(_message.Message):
    __slots__ = ("fileName", "fileData")
    FILENAME_FIELD_NUMBER: _ClassVar[int]
    FILEDATA_FIELD_NUMBER: _ClassVar[int]
    fileName: str
    fileData: bytes
    def __init__(self, fileName: _Optional[str] = ..., fileData: _Optional[bytes] = ...) -> None: ...
