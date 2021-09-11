using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', b => write((char)b.Memory[b.MemoryPointer]));
			vm.RegisterCommand('+', b => {
				if (b.Memory[b.MemoryPointer] == 255)
					b.Memory[b.MemoryPointer] = 0;
				else
					b.Memory[b.MemoryPointer]++;
			});
			vm.RegisterCommand('-', b => {
				if (b.Memory[b.MemoryPointer] == 0)
					b.Memory[b.MemoryPointer] = 255;
				else
					b.Memory[b.MemoryPointer]--;
			}) ;
			vm.RegisterCommand(',', b => b.Memory[b.MemoryPointer] = (byte)read());
			vm.RegisterCommand('>', b => {
				if (b.MemoryPointer == b.Memory.Length - 1)
					b.MemoryPointer = 0;
				else
					b.MemoryPointer++;
			});
			vm.RegisterCommand('<', b => {
				if (b.MemoryPointer == 0)
					b.MemoryPointer = b.Memory.Length - 1;
				else
					b.MemoryPointer--;
			});
			char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
			/*var alphaList = alpha.ToList();
			for (int i = 0; i < 10; i++)
				alphaList.Add(Convert.ToChar(i));
			char[] alphaFinal = alphaList.ToArray(); */
			foreach(char c in alpha) 
            {
				vm.RegisterCommand(c, b => b.Memory[b.MemoryPointer] = (byte)c);
            }
			
		}
	}
}