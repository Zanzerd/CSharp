using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		// (который раз встречается, какая скобка), позиция в инструкции
		public static void RegisterTo(IVirtualMachine vm)
		{
			Dictionary<(char, int), (char, int)> dictOfBrackets = new Dictionary<(char, int), (char, int)>();
			Stack<int> stackOfBrackets = new Stack<int>();
			for (int i = 0; i < vm.Instructions.Length; i++)
			{
				if (vm.Instructions[i] == '[')
				{
					stackOfBrackets.Push(i);
				}
				if (vm.Instructions[i] == ']')
				{
					var temp = stackOfBrackets.Pop();
					dictOfBrackets[('[', temp)] = (']', i);
					dictOfBrackets[(']', i)] = ('[', temp);
				}
			}
			vm.RegisterCommand('[', b => { 
				if (b.Memory[b.MemoryPointer] == 0)
                {
					b.InstructionPointer = dictOfBrackets[('[', b.InstructionPointer)].Item2 - 1;
                }
			});
			vm.RegisterCommand(']', b => { 
				if (b.Memory[b.MemoryPointer] != 0)
                {
					b.InstructionPointer = dictOfBrackets[(']', b.InstructionPointer)].Item2;
                }
			});
		}
	}
}