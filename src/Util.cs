public static class Util
{
    public static ushort SignExtend(ushort data, ushort bitCount)
    {
        bool isNegative = (data >> (bitCount - 1)) == 1;

        if (isNegative)
        {
            data |= (ushort)(0xffff << bitCount);
        }

        return data;
    }

    public static ushort ExtractBits(ushort data, ushort count, ushort start = 0)
    {
        ushort mask = (ushort)((1 << count) - 1);

        data >>= start;
        data &= mask;

        return data;
    }

    public static Memory ReadImage(string imageFilePath, ushort memorySize = ushort.MaxValue)
    {
        byte[] imageBytes = File.ReadAllBytes(imageFilePath);

        ushort origin = (ushort)((imageBytes[0] << 8) | imageBytes[1]);

        Memory memory = new Memory(memorySize);

        ushort memoryAddress;
        ushort imageIndex;
        bool isHighByte;
        for
        (
            memoryAddress = origin, imageIndex = 2, isHighByte = true;
            memoryAddress < memorySize && imageIndex < imageBytes.Length;
            ++imageIndex, isHighByte = !isHighByte
        )
        {
            ushort memoryData = memory.Read(memoryAddress);
            ushort imageData = imageBytes[imageIndex];

            if (isHighByte)
            {
                imageData <<= 8;
            }

            memoryData |= imageData;

            memory.Write(memoryAddress, memoryData);

            if (!isHighByte)
            {
                ++memoryAddress;
            }
        }

        return memory;
    }
}