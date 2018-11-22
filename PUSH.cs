using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AVC;

namespace AVI
{
    class PUSH : IAsseblerVirtualModule
    {
        private Dictionary<string, byte> BaitCodeList;
        private Dictionary<string, byte> RegisterCodes;
        private Dictionary<string, int> RegisterSizes;
        private Dictionary<string, string> Flags;
		private List<byte>PUSHR(string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["pushr"]);
            result.Add(RegisterCodes[from]);
            return result;
        }
        private List<byte> PUSHM(string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["pushm"]);
            result.Add(RegisterCodes[from.Substring(1, from.Length - 2)]);
            return result;
        }
        private List<byte> PUSHC(string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["pushc"]);
			result.AddRange(Commands.CompleteToLenght(Commands.ConvertToByte(Convert.ToInt64(from),8),Convert.ToInt32(Flags["target_bit_depth"]) / 8));
            return result;
        }
        public byte[] Compile(string instruction)
        {
            string[] args = Commands.GetArguments(instruction);
            List<byte> binaryinst = null;
            byte rubbish;
            if (args[0][0] == '[' && args[0][args[0].Length - 1] == ']') binaryinst = PUSHM(args[0]);
            else if (RegisterCodes.TryGetValue(args[0], out rubbish)) binaryinst = PUSHR(args[0]);
            else binaryinst = PUSHC(args[0]);
            return binaryinst.ToArray();
        }

        public void Execute<RegisteTypeName, RAMTypeName>(out RegisteTypeName Registers, out RAMTypeName RAM)
        {
            throw new NotImplementedException();
        }

        public void Init(Dictionary<string, byte> RegisterCodes_, Dictionary<string, int> RegisterSizes_, Dictionary<string, byte> BaitCodeList_,Dictionary<string, string> Flags_)
        {
            RegisterCodes = RegisterCodes_;
            RegisterSizes = RegisterSizes_;
            BaitCodeList = BaitCodeList_;
            Flags = Flags_;
        }

        public bool IsRealised(string instruction)
        {
            return instruction == "push";
        }
    }
}