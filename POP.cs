using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AVC;

namespace AVI
{
    class POP : IAsseblerVirtualModule
    {
        private Dictionary<string, byte> BaitCodeList;
        private Dictionary<string, byte> RegisterCodes;
        private Dictionary<string, int> RegisterSizes;
        private Dictionary<string, string> Flags;
        private List<byte> POPR(string to)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["popr"]);
            result.Add(RegisterCodes[to]);
            return result;
        }
        private List<byte> POPM(string to)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["popm"]);
            result.Add(RegisterCodes[to.Substring(1, to.Length - 2)]);
            return result;
        }
        public byte[] Compile(string instruction)
        {
            string[] args = Commands.GetArguments(instruction);
            List<byte> binaryinst = null;
            byte rubbish;
            if (args[0][0] == '[' && args[0][args[0].Length - 1] == ']') binaryinst = POPM(args[0]);
            else if (RegisterCodes.TryGetValue(args[0], out rubbish)) binaryinst = POPR(args[0]);
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
            return instruction == "pop";
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
            return baitCode == BaitCodeList["popr"] ||
                baitCode == BaitCodeList["popm"];
        }
    }
}