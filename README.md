# SkyEditor.IO

File and file system abstractions for file editing code libraries.

## IFileSystem Interface

This interface is based on the System.IO.File and System.IO.Directory classes, and allows interaction with both files and directories.

The benefit to using this interface as opposed to the System.IO classes is that other classes (such as a class that interacts with zip files) can implement the interface, and all code that uses this interface will be able to read from that class with no additional modification.

Such a class is not included in this library for the time being.

## BinaryFile Class

This class is an abstraction over reading a file from an arbitrary source. Thanks to the IBinaryDataAccessor interfaces, one can read subsets of the file with minimal additional allocations.

This class will support reading from byte arrays, streams, and memory mapped files in a thread-safe manor, but for the time being, only supports arrays.

## Planned features

* BinaryFile
    * Read from streams
	* Read from memory mapped files
	* Autodetect the best way to open the file based on the available memory
* IFileSystem implementation to interact with zip files
