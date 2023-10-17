public class Processor
{
    public enum Register
    {
        R0 = 0,
        R1,
        R2,
        R3,
        R4,
        R5,
        R6,
        R7,
        PC,
        COND
    }

    public enum Flag
    {
        POS = 1 << 0,
        ZRO = 1 << 1,
        NEG = 1 << 2
    }

    private static int RegisterCount = Enum.GetNames(typeof(Register)).Length;

    private ushort[] registers = new ushort[RegisterCount];

    public ushort ReadRegister(ushort register)
    {
        CheckRegisterOutOfBounds(register);

        return registers[register];
    }

    public ushort ReadRegister(Register register)
    {
        return ReadRegister((ushort)register);
    }

    public void WriteRegister(ushort register, ushort data)
    {
        CheckRegisterOutOfBounds(register);

        registers[register] = data;
    }

    public void WriteRegister(Register register, ushort data)
    {
        WriteRegister((ushort)register, data);
    }

    public void IncreaseProgramCounter()
    {
        registers[(ushort)Register.PC]++;
    }

    public void UpdateFlags(ushort register)
    {
        CheckRegisterOutOfBounds(register);

        if (registers[register] == 0)
        {
            registers[(ushort)Register.COND] = (ushort)Flag.ZRO;
        }
        else if ((registers[register] >> 15) == 1)
        {
            registers[(ushort)Register.COND] = (ushort)Flag.NEG;
        }
        else
        {
            registers[(ushort)Register.COND] = (ushort)Flag.POS;
        }
    }

    public void UpdateFlags(Register register)
    {
        UpdateFlags((ushort)register);
    }

    public void Dump(string dumpFilePath = "")
    {
        List<string> dump = new List<string>();

        string[] registerNames = Enum.GetNames(typeof(Processor.Register));

        for (var register = 0; register < registerNames.Length; ++register)
        {
            dump.Add($"{registerNames[register]} - {registers[register]}");
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

    private void CheckRegisterOutOfBounds(ushort register)
    {
        if (register < 0 || register >= RegisterCount)
        {
            throw new ArgumentOutOfRangeException("Invalid register");
        }
    }
}