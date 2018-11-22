
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AVC
{
    class Commands
    {
        public static byte[] CompleteToLenght(byte[] line, int targetLenght)
        {
            List<byte> result = new List<byte>(line);
            if (targetLenght > result.Count)
            {
                for (int i = result.Count; i < targetLenght; i++)
                {
                    byte tmp = 0;
                    result.Add(tmp);
                }
            }
            else
            {
                for (int i = result.Count; i > targetLenght; i--)
                    result.RemoveAt(result.Count - 1);

            }
            return result.ToArray();
        }
        public static byte[] ConvertToByte(long value, int size)
        {
            if (size == 1)
            {
                List<byte> tmp = new List<byte>();
                tmp.Add(Convert.ToByte(value));
                return tmp.ToArray();
            }
            if (size == 2)
                return BitConverter.GetBytes((short)value);
            if (size == 4)
                return BitConverter.GetBytes((int)value);
            if (size == 8)
                return BitConverter.GetBytes((long)value);
            return null;
        }
        public static string[] GetArguments(string instruction)
        {
            List<string> result = new List<string>();
            instruction = instruction.Substring(instruction.IndexOf(' ') + 1);
            int indexcomma;
            string nextcommand = "";
            while (true)
            {
                indexcomma = instruction.IndexOf(',');
                if (indexcomma == -1)
                    break;
                nextcommand = instruction.Substring(0, indexcomma);
                ClearCommand(ref nextcommand);
                result.Add(nextcommand);
                instruction = instruction.Substring(indexcomma + 1);
            }
            ClearCommand(ref instruction);
            result.Add(instruction);
            return result.ToArray();
        }
        public static void ClearCommand(ref string Command, char rubbish = ' ')
        {
            while (true)
            {
                int indexrub = Command.IndexOf(rubbish);
                if (indexrub == -1)
                    return;
                Command = Command.Remove(indexrub, 1);
            }

        }
    }
}