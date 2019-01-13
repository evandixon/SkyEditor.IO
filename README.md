# SkyEditor.IO

File and file system abstractions for file editing code libraries.

## IFileSystem Interface

This interface is based on the System.IO.File and System.IO.Directory classes, and allows interaction with both files and directories.

The benefit to using this interface as opposed to the System.IO classes is that other classes (such as a class that interacts with zip files) can implement the interface, and all code that uses this interface will be able to read from that class with no additional modification.

Such a class is not included in this library for the time being.

## BinaryFile Class

This class is an abstraction over reading a file from an arbitrary source. Thanks to the IBinaryDataAccessor interfaces, one can read subsets of the file with minimal additional allocations.

This class is thread-safe unless otherwise indicated in the XML documentation. SetLength, the method that resizes a file, is the biggest example of a method that is _not_ thread-safe.

Supported sources (in order of preference):
* Byte arrays
    * Advantages: fastest, makes use of Memory and Span methods in IBinaryDataAccessor, supports concurrency
	* Disadvantages: uses a lot of memory, can't be used with files that are 2GB in size or larger
	* Resize support: when size is less than 2GB
* Memory-mapped files
    * Advantages: comparable speed to byte arrays, supports concurrency, supports files over 2GB in size, less memory used compared to byte arrays
	* Disadvangates: no benefit from using Memory or Span methods in IBinaryDataAccessor
	* Resize support: never, must close, resize with a stream, and reopen
* Streams
    * Advantages: low memory footprint, supports files over 2GB in size 
	* Disadvantages: no benefit from using Memory or Span methods in IBinaryDataAccessor, does not support concurrency (locks are used by BinaryFile to ensure thread safety at the cost of speed)
	* Resize support: always

When opening a file from a filename, BinaryFile will choose one of these for you, possibly even changing automatically when resizing the file.

Alternatively, you can use a constructor overload to pass it a byte array, stream, or memory-mapped file directly, at the expense of an exception being thrown when attempting to resize a file inconsistent with the Resize Support listed above.