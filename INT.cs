using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AVC;

namespace AVI
{
    class INT : IAsseblerVirtualModule
    {
        private Dictionary<string, byte> BaitCodeList;
        private Dictionary<string, byte> RegisterCodes;
        private Dictionary<string, int> RegisterSizes;
        private Dictionary<string, string> Flags;
        public byte[] Compile(string instruction)
        {
            string[] args = Commands.GetArguments(instruction);
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["int"]);
            result.Add(Convert.ToByte(Convert.ToInt32(args[0])));
            return result.ToArray();
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

        public void InitLink(string pointer, int address)
        {
            throw new NotImplementedException();
        }

        public bool IsLinkable()
        {
            return false;
        }

        public bool IsRealised(string instruction)
        {
            return instruction == "int";
        }

        public void Link(Dictionary<string, int> PointerList, List<byte> Binary)
        {
            throw new NotImplementedException();
        }
        public bool IsExecutable(byte baitCode)
        {
            return baitCode == BaitCodeList["int"];
        }
    }
}