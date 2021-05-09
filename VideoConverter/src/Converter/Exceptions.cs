using System;

namespace VideoConverter
{
    public class ConversionException : Exception
    {
        public ConversionException()
        {
        }

        public ConversionException(string message)
            : base(message)
        {
        }

        public ConversionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class VideoFormatException : Exception
    {
        public VideoFormatException()
        {
        }

        public VideoFormatException(string message)
            : base(message)
        {
        }

        public VideoFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}