using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AVC
{
    interface IAsseblerVirtualModule : ICompilable , IExecutable
    {
        void Init(
            Dictionary<string, byte> RegisterCodes_,
            Dictionary<string, int> RegisterSizes_,
            Dictionary<string, byte> BaitCodeList_,
            Dictionary<string, string> Flags_);
    }
    interface ICompilable
    {
        bool IsRealised(string instruction);
        byte[] Compile(string instruction);
    }
    interface IExecutable
    {
        void Execute<RegisteTypeName, RAMTypeName>(out RegisteTypeName Registers, out RAMTypeName RAM);
    }
}