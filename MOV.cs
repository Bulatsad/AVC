using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AVC;

namespace AVI
{
    class MOV : IAsseblerVirtualModule
    {
        private Dictionary<string, byte> BaitCodeList;
        private Dictionary<string, byte> RegisterCodes;
        private Dictionary<string, int> RegisterSizes;
        private Dictionary<string, string> Flags;
        private List<byte> MOVRR(string to, string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["movrr"]);
            result.Add(RegisterCodes[to]);
            result.Add(RegisterCodes[from]);
            return result;
        }
        private List<byte> MOVRM(string to, string from) //адрес памяти берется из регистра 
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["movrm"]);
            result.Add(RegisterCodes[to]);
            result.Add(RegisterCodes[from.Substring(1, from.Length - 2)]);
            return result;
        }
        private List<byte> MOVRC(string to, string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["movrc"]);
            result.Add(RegisterCodes[to]);
            result.AddRange(Commands.ConvertToByte(Convert.ToInt64(from), RegisterSizes[to]));
            return result;
        }
        private List<byte> MOVMR(string to, string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["movmr"]);
            result.Add(RegisterCodes[to.Substring(1, to.Length - 2)]);
            result.Add(RegisterCodes[from]);
            return result;
        }
        private List<byte> MOVMC(string to, string from)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["movmc"]);
            result.Add(RegisterCodes[to.Substring(1, to.Length - 2)]);
            result.AddRange(Commands.ConvertToByte(Convert.ToInt64(from), RegisterSizes[to.Substring(1, to.Length - 2)]));
            return result;
        }
        public byte[] Compile(string instruction)
        {
            string[] args = Commands.GetArguments(instruction);
            List<byte> binaryinst = null;
            byte rubbish;
            if (args[0][0] == '[' && args[0][args[0].Length - 1] == ']' && RegisterCodes.TryGetValue(args[1], out rubbish)) binaryinst = MOVMR(args[0], args[1]);
            else if (args[0][0] == '[' && args[0][args[0].Length - 1] == ']') binaryinst = MOVMC(args[0], args[1]);
            else if (RegisterCodes.TryGetValue(args[1], out rubbish)) binaryinst = MOVRR(args[0], args[1]);
            else if (args[1][0] == '[' && args[1][args[1].Length - 1] == ']') binaryinst = MOVRM(args[0], args[1]);
            else binaryinst = MOVRC(args[0], args[1]);
            return binaryinst.ToArray();
        }

        public void Execute<RegisteTypeName, RAMTypeName>(out RegisteTypeName Registers, out RAMTypeName RAM)
        {
            throw new NotImplementedException();
        }

        public bool IsRealised(string instruction)
        {
            return instruction == "mov";
        }

        public void Init(Dictionary<string, byte> RegisterCodes_, Dictionary<string, int> RegisterSizes_, Dictionary<string, byte> BaitCodeList_, Dictionary<string, string> Flags_)
        {
            RegisterCodes = RegisterCodes_;
            RegisterSizes = RegisterSizes_;
            BaitCodeList = BaitCodeList_;
            Flags = Flags_;
        }

        public void InitLink(string pointer, int address)
        {
            throw new NotImplementedException();
        }

        public void Link(Dictionary<string, int> PointerList, List<byte> Binary)
        {
            throw new NotImplementedException();
        }

        public bool IsLinkable()
        {
            return false;
        }
        public bool IsExecutable(byte baitCode)
        {
            return baitCode == BaitCodeList["movrr"] ||
                baitCode == BaitCodeList["movrm"] ||
                baitCode == BaitCodeList["movrc"] ||
                baitCode == BaitCodeList["movmr"];
        }
    }
}