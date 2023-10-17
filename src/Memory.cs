public class Memory
{
    enum IO
    {
        KBSR = 0xFE00,
        KBDR = 0xFE02
    }

    private ushort[] memory;

    public Memory(ushort size)
    {
        memory = new ushort[size];
    }

    public ushort Read(ushort address)
    {
        if (address < 0 || address >= memory.Length)
        {
            throw new ArgumentOutOfRangeException("Invalid address");
        }

        if (address == (ushort)IO.KBSR)
        {
            if (Console.KeyAvailable)
            {
                memory[(ushort)IO.KBSR] = (1 << 15);
                memory[(ushort)IO.KBDR] = (ushort)Console.ReadKey(true).KeyChar;
            }
            else
            {
                memory[(ushort)IO.KBSR] = 0;
            }
        }

        return memory[address];
    }

    public void Write(ushort address, ushort data)
    {
        if (address < 0 || address >= memory.Length)
        {
            throw new ArgumentOutOfRangeException("Invalid address");
        }

        memory[address] = data;
    }

    public void Dump(string dumpFilePath = "")
    {
        List<string> dump = new List<string>();

        for (ushort memoryAddress = 0; memoryAddress < memory.Length; ++memoryAddress)
        {
            dump.Add($"{memoryAddress:x} - {memory[memoryAddress]:x}");
        }

        if (dumpFilePath == "")
        {
            foreach (var line in dump)
            {
                Console.WriteLine(line);
            }
        }
        else
        {
            File.WriteAllLines(dumpFilePath, dump);
        }
    }
}