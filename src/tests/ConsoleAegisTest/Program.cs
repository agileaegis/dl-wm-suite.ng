using System;

namespace ConsoleAegisTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Timestamp : uint32 (4):4d388f5d--> LE: 5D8F384D --> Hex to Decimal = 1569667149 Convert epoch to human-readable date and vice versa --> epoch Saturday, September 28, 2019 10:39:09 AM

            Console.WriteLine("Hello World!");
            byte[] imeiBuffer = new byte[10];

            imeiBuffer[0] = 0x4D;
            imeiBuffer[1] = 0x38;
            imeiBuffer[2] = 0x8F;
            imeiBuffer[3] = 0x5D;
            imeiBuffer[4] = 0x5D;
            imeiBuffer[5] = 0x5D;
            imeiBuffer[6] = 0x5D;
            imeiBuffer[7] = 0x5D;
            imeiBuffer[8] = 0x5D;
            imeiBuffer[9] = 0x5D;

            var dec = GetLittleEndianIntegerFromByteArray(imeiBuffer, 0);

        }

        private static int GetLittleEndianIntegerFromByteArray(byte[] data, int startIndex)
        {
            byte[] imeiBuffer = new byte[4];
            Array.Copy(data, startIndex, imeiBuffer, 0,
                4);

            return (imeiBuffer[startIndex + 3] << 24)
                   | (imeiBuffer[startIndex + 2] << 16)
                   | (imeiBuffer[startIndex + 1] << 8)
                   | imeiBuffer[startIndex];
        }
    }

}
