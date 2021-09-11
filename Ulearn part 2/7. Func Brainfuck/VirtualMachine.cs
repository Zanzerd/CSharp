using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }
		
		private Dictionary<char, Action<IVirtualMachine>> dictOfCommands;

		public VirtualMachine(string program, int memorySize)
		{
			Memory = new byte[memorySize];
			Instructions = program;
			MemoryPointer = 0;
			InstructionPointer = 0;
			dictOfCommands = new Dictionary<char, Action<IVirtualMachine>>();
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			if (!dictOfCommands.ContainsKey(symbol))
				dictOfCommands[symbol] = execute;
			else
            {
				dictOfCommands.Remove(symbol);
				dictOfCommands[symbol] = execute;
            }
			return;
		}

		public void Run()
		{
			while (InstructionPointer < Instructions.Length)
            {
				var instruction = Instructions[InstructionPointer];
				if (dictOfCommands.ContainsKey(instruction))
                {
					dictOfCommands[instruction](this);
                }
				InstructionPointer++;
            }
			return;
		}
	}
}