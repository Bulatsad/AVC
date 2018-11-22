using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AVC;

namespace AVI
{
    class SUB : IAsseblerVirtualModule
    {
        private Dictionary<string, byte> BaitCodeList;
        private Dictionary<string, byte> RegisterCodes;
        private Dictionary<string, int> RegisterSizes;
        private Dictionary<string, string> Flags;
        private List<byte> SUBRR(string to, string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["subrr"]);
            result.Add(RegisterCodes[to]);
            result.Add(RegisterCodes[from]);
            return result;
        }
        private List<byte> SUBRM(string to, string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["subrm"]);
            result.Add(RegisterCodes[to]);
            result.Add(RegisterCodes[from.Substring(1, from.Length - 2)]);
            return result;
        }
        private List<byte> SUBRC(string to, string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["subrc"]);
            result.Add(RegisterCodes[to]);
            result.AddRange(Commands.ConvertToByte(Convert.ToInt64(from), RegisterSizes[to]));
            return result;
        }

        public byte[] Compile(string instruction)
        {
            string[] args = Commands.GetArguments(instruction);
            List<byte> binaryinst = null;
            byte rubbish;
            if (RegisterCodes.TryGetValue(args[1], out rubbish)) binaryinst = SUBRR(args[0], args[1]);
            else if (args[1][0] == '[' && args[1][args[1].Length - 1] == ']') binaryinst = SUBRM(args[0], args[1]);
            else binaryinst = SUBRC(args[0], args[1]);
            return binaryinst.ToArray();
        }

        public void Execute<RegisteTypeName, RAMTypeName>(out RegisteTypeName Registers, out RAMTypeName RAM)
        {
            throw new NotImplementedException();
        }

        public void Init(Dictionary<string, byte> RegisterCodes_, Dictionary<string, int> RegisterSizes_, Dictionary<string, byte> BaitCodeList_, Dictionary<string, string> Flags_)
        {
            RegisterCodes = RegisterCodes_;
            RegisterSizes = RegisterSizes_;
            BaitCodeList = BaitCodeList_;
            Flags = Flags_;
        }

        public bool IsRealised(string instruction)
        {
            return instruction == "sub";
        }
        
    }
}