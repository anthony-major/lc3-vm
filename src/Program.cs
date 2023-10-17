if (args.Length < 1)
{
    return;
}

Computer lc3 = new Computer(args[0]);
lc3.Start();

// lc3.CPU.WriteRegister(Processor.Register.R0, 42);
// lc3.CPU.WriteRegister(Processor.Register.R1, 1);

// lc3.CPU.WriteRegister(Processor.Register.COND, (ushort)Processor.Flag.POS);
// lc3.CPU.WriteRegister(Processor.Register.PC, 0);

// ushort instruction = 0b0111_000_001_000001;

// Instructions.STR(lc3, instruction);

// lc3.CPU.Dump();
// lc3.RAM.Dump("memdump.txt");