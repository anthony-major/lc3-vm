public class Computer
{
    public Processor CPU { get; private set; }
    public Memory RAM { get; private set; }

    public bool Running { get; set; } = true;

    public Computer(string imageFilePath = "", ushort memorySize = ushort.MaxValue)
    {
        CPU = new Processor();

        if (imageFilePath == "")
        {
            RAM = new Memory(memorySize);
        }
        else
        {
            RAM = Util.ReadImage(imageFilePath, memorySize);
        }

        CPU.WriteRegister(Processor.Register.COND, (ushort)Processor.Flag.ZRO);
        CPU.WriteRegister(Processor.Register.PC, 0x3000);
    }

    public void Start()
    {
        while (Running)
        {
            ushort instruction = RAM.Read(CPU.ReadRegister(Processor.Register.PC));
            CPU.IncreaseProgramCounter();

            ushort opcode = Util.ExtractBits(instruction, 4, 12);

            switch (opcode)
            {
                case (ushort)Opcode.BR:
                    Instructions.BR(this, instruction);
                    break;
                case (ushort)Opcode.ADD:
                    Instructions.ADD(this, instruction);
                    break;
                case (ushort)Opcode.LD:
                    Instructions.LD(this, instruction);
                    break;
                case (ushort)Opcode.ST:
                    Instructions.ST(this, instruction);
                    break;
                case (ushort)Opcode.JSR:
                    Instructions.JSR(this, instruction);
                    break;
                case (ushort)Opcode.AND:
                    Instructions.AND(this, instruction);
                    break;
                case (ushort)Opcode.LDR:
                    Instructions.LDR(this, instruction);
                    break;
                case (ushort)Opcode.STR:
                    Instructions.STR(this, instruction);
                    break;
                case (ushort)Opcode.RTI:
                    // Unused
                    break;
                case (ushort)Opcode.NOT:
                    Instructions.NOT(this, instruction);
                    break;
                case (ushort)Opcode.LDI:
                    Instructions.LDI(this, instruction);
                    break;
                case (ushort)Opcode.STI:
                    Instructions.STI(this, instruction);
                    break;
                case (ushort)Opcode.JMP:
                    Instructions.JMP(this, instruction);
                    break;
                case (ushort)Opcode.RES:
                    // Unused
                    break;
                case (ushort)Opcode.LEA:
                    Instructions.LEA(this, instruction);
                    break;
                case (ushort)Opcode.TRAP:
                    Instructions.TRAP(this, instruction);
                    break;
                default:
                    throw new ArgumentException("Invalid opcode");
            }
        }
    }
}