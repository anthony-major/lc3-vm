public static class Instructions
{
    public static void BR(Computer computer, ushort instruction)
    {
        ushort negative = Util.ExtractBits(instruction, 1, 11);
        ushort zero = Util.ExtractBits(instruction, 1, 10);
        ushort positive = Util.ExtractBits(instruction, 1, 9);

        if
        (
            (negative == 1 && computer.CPU.ReadRegister(Processor.Register.COND) == (ushort)Processor.Flag.NEG) ||
            (zero == 1 && computer.CPU.ReadRegister(Processor.Register.COND) == (ushort)Processor.Flag.ZRO) ||
            (positive == 1 && computer.CPU.ReadRegister(Processor.Register.COND) == (ushort)Processor.Flag.POS)
        )
        {
            ushort programCounterOffset = Util.ExtractBits(instruction, 9);
            programCounterOffset = Util.SignExtend(programCounterOffset, 9);
            ushort programCounter = computer.CPU.ReadRegister(Processor.Register.PC);

            computer.CPU.WriteRegister(Processor.Register.PC, (ushort)(programCounter + programCounterOffset));
        }
    }

    public static void ADD(Computer computer, ushort instruction)
    {
        ushort destinationRegister = Util.ExtractBits(instruction, 3, 9);
        ushort sourceRegisterOne = Util.ExtractBits(instruction, 3, 6);
        ushort immediateMode = Util.ExtractBits(instruction, 1, 5);

        ushort firstValue = computer.CPU.ReadRegister(sourceRegisterOne);
        ushort secondValue;

        if (immediateMode == 1)
        {
            ushort immediateValue = Util.ExtractBits(instruction, 5);
            secondValue = Util.SignExtend(immediateValue, 5);
        }
        else
        {
            ushort sourceRegisterTwo = Util.ExtractBits(instruction, 3);
            secondValue = computer.CPU.ReadRegister(sourceRegisterTwo);
        }

        computer.CPU.WriteRegister(destinationRegister, (ushort)(firstValue + secondValue));
        computer.CPU.UpdateFlags(destinationRegister);
    }

    public static void LD(Computer computer, ushort instruction)
    {
        ushort destinationRegister = Util.ExtractBits(instruction, 3, 9);
        ushort addressOffset = Util.ExtractBits(instruction, 9);
        addressOffset = Util.SignExtend(addressOffset, 9);
        ushort programCounter = computer.CPU.ReadRegister(Processor.Register.PC);

        ushort dataAddress = (ushort)(programCounter + addressOffset);

        ushort data = computer.RAM.Read(dataAddress);

        computer.CPU.WriteRegister(destinationRegister, data);
        computer.CPU.UpdateFlags(destinationRegister);
    }

    public static void ST(Computer computer, ushort instruction)
    {
        ushort sourceRegister = Util.ExtractBits(instruction, 3, 9);
        ushort sourceData = computer.CPU.ReadRegister(sourceRegister);
        ushort storeAddressOffset = Util.ExtractBits(instruction, 9);
        storeAddressOffset = Util.SignExtend(storeAddressOffset, 9);
        ushort programCounter = computer.CPU.ReadRegister(Processor.Register.PC);

        ushort storeAddress = (ushort)(programCounter + storeAddressOffset);

        computer.RAM.Write(storeAddress, sourceData);
    }

    public static void JSR(Computer computer, ushort instruction)
    {
        ushort programCounter = computer.CPU.ReadRegister(Processor.Register.PC);
        ushort offsetMode = Util.ExtractBits(instruction, 1, 11);
        ushort jumpAddress;

        computer.CPU.WriteRegister(Processor.Register.R7, programCounter);

        if (offsetMode == 1)
        {
            ushort jumpOffset = Util.ExtractBits(instruction, 11);
            jumpOffset = Util.SignExtend(jumpOffset, 11);

            jumpAddress = (ushort)(programCounter + jumpOffset);
        }
        else
        {
            ushort baseRegister = Util.ExtractBits(instruction, 3, 6);

            jumpAddress = computer.CPU.ReadRegister(baseRegister);
        }

        computer.CPU.WriteRegister(Processor.Register.PC, jumpAddress);
    }

    public static void AND(Computer computer, ushort instruction)
    {
        ushort destinationRegister = Util.ExtractBits(instruction, 3, 9);
        ushort sourceRegisterOne = Util.ExtractBits(instruction, 3, 6);
        ushort immediateMode = Util.ExtractBits(instruction, 1, 5);

        ushort firstValue = computer.CPU.ReadRegister(sourceRegisterOne);
        ushort secondValue;

        if (immediateMode == 1)
        {
            ushort immediateValue = Util.ExtractBits(instruction, 5);
            secondValue = Util.SignExtend(immediateValue, 5);
        }
        else
        {
            ushort sourceRegisterTwo = Util.ExtractBits(instruction, 3);
            secondValue = computer.CPU.ReadRegister(sourceRegisterTwo);
        }

        computer.CPU.WriteRegister(destinationRegister, (ushort)(firstValue & secondValue));
        computer.CPU.UpdateFlags(destinationRegister);
    }

    public static void LDR(Computer computer, ushort instruction)
    {
        ushort destinationRegister = Util.ExtractBits(instruction, 3, 9);
        ushort baseRegister = Util.ExtractBits(instruction, 3, 6);
        ushort baseAddress = computer.CPU.ReadRegister(baseRegister);
        ushort addressOffset = Util.ExtractBits(instruction, 6);
        addressOffset = Util.SignExtend(addressOffset, 6);

        ushort dataAddress = (ushort)(baseAddress + addressOffset);

        ushort data = computer.RAM.Read(dataAddress);

        computer.CPU.WriteRegister(destinationRegister, data);
        computer.CPU.UpdateFlags(destinationRegister);
    }

    public static void STR(Computer computer, ushort instruction)
    {
        ushort sourceRegister = Util.ExtractBits(instruction, 3, 9);
        ushort sourceData = computer.CPU.ReadRegister(sourceRegister);
        ushort baseRegister = Util.ExtractBits(instruction, 3, 6);
        ushort baseData = computer.CPU.ReadRegister(baseRegister);
        ushort addressOffset = Util.ExtractBits(instruction, 6);
        addressOffset = Util.SignExtend(addressOffset, 6);

        ushort storeAddress = (ushort)(baseData + addressOffset);

        computer.RAM.Write(storeAddress, sourceData);
    }

    public static void NOT(Computer computer, ushort instruction)
    {
        ushort destinationRegister = Util.ExtractBits(instruction, 3, 9);
        ushort sourceRegister = Util.ExtractBits(instruction, 3, 6);
        ushort sourceData = computer.CPU.ReadRegister(sourceRegister);

        ushort bitwiseComplement = (ushort)(~sourceData);

        computer.CPU.WriteRegister(destinationRegister, bitwiseComplement);
        computer.CPU.UpdateFlags(destinationRegister);
    }

    public static void LDI(Computer computer, ushort instruction)
    {
        ushort destinationRegister = Util.ExtractBits(instruction, 3, 9);
        ushort programCounterOffset = Util.ExtractBits(instruction, 9);
        ushort dataAddressAddress = (ushort)(computer.CPU.ReadRegister(Processor.Register.PC) + Util.SignExtend(programCounterOffset, 9));
        ushort dataAddress = computer.RAM.Read(dataAddressAddress);
        ushort data = computer.RAM.Read(dataAddress);

        computer.CPU.WriteRegister(destinationRegister, data);
        computer.CPU.UpdateFlags(destinationRegister);
    }

    public static void STI(Computer computer, ushort instruction)
    {
        ushort sourceRegister = Util.ExtractBits(instruction, 3, 9);
        ushort sourceData = computer.CPU.ReadRegister(sourceRegister);
        ushort addressOffset = Util.ExtractBits(instruction, 9);
        addressOffset = Util.SignExtend(addressOffset, 9);
        ushort programCounter = computer.CPU.ReadRegister(Processor.Register.PC);

        ushort storeAddressAddress = (ushort)(programCounter + addressOffset);
        ushort storeAddress = computer.RAM.Read(storeAddressAddress);

        computer.RAM.Write(storeAddress, sourceData);
    }

    public static void JMP(Computer computer, ushort instruction)
    {
        ushort baseRegister = Util.ExtractBits(instruction, 3, 6);
        ushort jumpAddress = computer.CPU.ReadRegister(baseRegister);

        computer.CPU.WriteRegister(Processor.Register.PC, jumpAddress);
    }

    public static void LEA(Computer computer, ushort instruction)
    {
        ushort destinationRegister = Util.ExtractBits(instruction, 3, 9);
        ushort addressOffset = Util.ExtractBits(instruction, 9);
        addressOffset = Util.SignExtend(addressOffset, 9);
        ushort programCounter = computer.CPU.ReadRegister(Processor.Register.PC);

        ushort loadAddress = (ushort)(programCounter + addressOffset);

        computer.CPU.WriteRegister(destinationRegister, loadAddress);
        computer.CPU.UpdateFlags(destinationRegister);
    }

    public static void TRAP(Computer computer, ushort instruction)
    {
        ushort trapAddress = Util.ExtractBits(instruction, 8);

        switch (trapAddress)
        {
            case (ushort)TrapCode.GETC:
                {
                    ushort character = (ushort)Console.ReadKey(true).KeyChar;

                    computer.CPU.WriteRegister(Processor.Register.R0, character);
                }
                break;
            case (ushort)TrapCode.OUT:
                {
                    ushort character = computer.CPU.ReadRegister(Processor.Register.R0);
                    character = Util.ExtractBits(character, 8);

                    Console.Write((char)character);
                }
                break;
            case (ushort)TrapCode.PUTS:
                {
                    ushort startAddress = computer.CPU.ReadRegister(Processor.Register.R0);
                    ushort characterData = computer.RAM.Read(startAddress);

                    while (characterData != 0x0000)
                    {
                        Console.Write((char)characterData);
                        characterData = computer.RAM.Read(++startAddress);
                    }
                }
                break;
            case (ushort)TrapCode.IN:
                {
                    Console.Write(">");

                    ushort character = (ushort)Console.ReadKey().KeyChar;

                    computer.CPU.WriteRegister(Processor.Register.R0, character);
                }
                break;
            case (ushort)TrapCode.PUTSP:
                {
                    ushort startAddress = computer.CPU.ReadRegister(Processor.Register.R0);
                    ushort characterData = computer.RAM.Read(startAddress);

                    while (characterData != 0x0000)
                    {
                        ushort characterOne = Util.ExtractBits(characterData, 8);
                        ushort characterTwo = Util.ExtractBits(characterData, 8, 8);

                        Console.Write((char)characterOne);
                        if (characterTwo != 0x00)
                        {
                            Console.Write((char)characterTwo);
                        }

                        characterData = computer.RAM.Read(++startAddress);
                    }
                }
                break;
            case (ushort)TrapCode.HALT:
                {
                    Console.Write("HALT");
                    computer.Running = false;
                }
                break;
        }
    }
}