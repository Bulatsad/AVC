﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AVC;

namespace AVI
{
    class CMP : IAsseblerVirtualModule , ILinkable
    {
        private Dictionary<string, byte> BaitCodeList;
        private Dictionary<string, byte> RegisterCodes;
        private Dictionary<string, int> RegisterSizes;
        private Dictionary<string, string> Flags;
        private byte[] CMPRR(string[] args)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["cmprr"]);
            result.Add(RegisterCodes[args[0]]);
            result.Add(RegisterCodes[args[1]]);
            return result.ToArray();
        }
        private byte[] CMPRM(string[] args)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["cmprm"]);
            result.Add(RegisterCodes[args[0]]);
            result.Add(RegisterCodes[args[1].Substring(1,args[1].Length -2)]);
            return result.ToArray();
        }
        private byte[] CMPRC(string[] args)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["cmprc"]);
            result.Add(RegisterCodes[args[0]]);
            result.AddRange(Commands.CompleteToLenght(Commands.ConvertToByte(Convert.ToInt64(args[1]),8),RegisterSizes[args[0]]));
            return result.ToArray();
        }
        private byte[] CMPMR(string[] args)
        {
            List<byte> result = new List<byte>();
            result.Add(BaitCodeList["cmpmr"]);
            result.Add(RegisterCodes[args[0].Substring(1, args[0].Length - 2)]);
            result.Add(RegisterCodes[args[1]]);
            return result.ToArray();
        }
        private byte[] CMPXX(string instruction,string[] args)
        {
            if (Commands.IsRegister(args[0], RegisterCodes) &&
                Commands.IsRegister(args[1], RegisterCodes))
                return CMPRR(args);
            if (Commands.IsRegister(args[0], RegisterCodes) &&
                Commands.IsConstant(args[1], RegisterCodes))
                return CMPRC(args);
            if (Commands.IsRegister(args[0], RegisterCodes) &&
                Commands.IsMemory(args[1], RegisterCodes)) 
                return CMPRM(args);
            if (Commands.IsMemory(args[0], RegisterCodes) &&
                Commands.IsRegister(args[1], RegisterCodes))
                return CMPMR(args);
            return null;
        }
       
        public byte[] Compile(string instruction)
        {
            string[] args = Commands.GetArguments(instruction);
            string command = instruction.Substring(0, instruction.IndexOf(' '));
            return CMPXX(command, args);
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
            return
                instruction == "cmp";
        }

        public void InitLink(string pointer, int address)
        {
            throw new Exception();
        }

        public void Link(Dictionary<string, int> PointerList, List<byte> Binary)
        {
            throw new Exception();
        }
        public bool IsLinkable()
        {
            return false;
        }
        public bool IsExecutable(byte baitCode)
        {
            return baitCode == BaitCodeList["cmprr"] ||
                baitCode == BaitCodeList["cmprm"] ||
                baitCode == BaitCodeList["cmprc"] ||
                baitCode == BaitCodeList["cmpmr"];
        }
    }
}