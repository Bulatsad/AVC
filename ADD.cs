using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AVC;

namespace AVI
{
    class ADD : IAsseblerVirtualModule
    {
        private Dictionary<string, byte> BaitCodeList;
        private Dictionary<string, byte> RegisterCodes;
        private Dictionary<string, int> RegisterSizes;
        private Dictionary<string, string> Flags;
        private List<byte> ADDRR(string to, string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["addrr"]);
            result.Add(RegisterCodes[to]);
            result.Add(RegisterCodes[from]);
            return result;
        }
        private List<byte> ADDRM(string to, string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["addrm"]);
            result.Add(RegisterCodes[to]);
            result.Add(RegisterCodes[from.Substring(1, from.Length - 2)]);
            return result;
        }
        private List<byte> ADDRC(string to, string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["addrc"]);
            result.Add(RegisterCodes[to]);
            result.AddRange(Commands.ConvertToByte(Convert.ToInt64(from), RegisterSizes[to]));
            return result;
        }

        public byte[] Compile(string instruction)
        {
            string[] args = Commands.GetArguments(instruction);
            List<byte> binaryinst = null;
            byte rubbish;
            if (RegisterCodes.TryGetValue(args[1], out rubbish)) binaryinst = ADDRR(args[0], args[1]);
            else if (args[1][0] == '[' && args[1][args[1].Length - 1] == ']') binaryinst = ADDRM(args[0], args[1]);
            else binaryinst = ADDRC(args[0], args[1]);
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
            return instruction == "add";
        }
    }
}