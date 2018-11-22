using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AVC
{
    class AVComplier
    {
        private static Dictionary<string, byte> BaitCodeList;
        private static Dictionary<string, byte> RegisterCodes;
        private static Dictionary<string, int> RegisterSizes;
        private static Dictionary<string, string> Flags;
        public static IAsseblerVirtualModule[] ModuleList;
        
        private static List<byte> Binary;
        private static void LoadRegisterInfo(string path = "registerdefs.arc")
        {
            RegisterCodes = new Dictionary<string, byte>();
            RegisterSizes = new Dictionary<string, int>();
            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                string command = reader.ReadLine();
                string[] args = Commands.GetArguments(command);
                RegisterCodes.Add(command.Substring(0, command.IndexOf(' ')), Convert.ToByte(args[0], 16));
                RegisterSizes.Add(command.Substring(0, command.IndexOf(' ')), Convert.ToInt16(args[1], 10));
            }
        }
        private static Dictionary<string, byte> LoadBaitCodes(string path = "instructiondefs.arc")
        {
            Dictionary<string, byte> result = new Dictionary<string, byte>();
            StreamReader reader = new StreamReader(path);
            int i = 0;
            string instruction;
            while (!reader.EndOfStream)
            {
                instruction = reader.ReadLine();
                Commands.ClearCommand(ref instruction);
                result.Add(instruction, Convert.ToByte(i));
                i++;
            }
            return result;
        }
        private static string[] ReadCode(string path)
        {
            StreamReader reader = new StreamReader(path);
            List<string> result = new List<string>();
            while (!reader.EndOfStream)
                result.Add(reader.ReadLine());
            return result.ToArray();
        }
        private static void InitModules(IAsseblerVirtualModule[] moduleList)
        {
            foreach (IAsseblerVirtualModule module in moduleList)
                module.Init(RegisterCodes, RegisterSizes, BaitCodeList,Flags);
        }
        private static void LoadFlags(string path = "flags.arc")
        {
            Flags = new Dictionary<string, string>();
            StreamReader reader = new StreamReader(path);
            string line, command;
            string[] args;
            while(!reader.EndOfStream)
            {
                line = reader.ReadLine();
                command = line.Substring(0, line.IndexOf(' '));
                args = Commands.GetArguments(line);
                switch(command)
                {
                    case "target_bit_depth": Flags[command] = args[0];break;
                }
            }
        }
        public static void Compile(string path)
        {
            LoadFlags();
            Binary = new List<byte>();
            BaitCodeList = LoadBaitCodes();
            LoadRegisterInfo();
            ModuleList = Connector.GetConnectedModudels();
            InitModules(ModuleList);
            string[] Code = ReadCode(path);
            string Command;
            foreach (string Instruction in Code)
            {
                Command = Instruction.Substring(0, Instruction.IndexOf(' '));
                foreach(IAsseblerVirtualModule module in ModuleList)
                {
                    if (module.IsRealised(Command))
                        Binary.AddRange(module.Compile(Instruction));
                }
            }
            foreach (byte a in Binary)
                Console.WriteLine(a);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            AVC.AVComplier.Compile("testcode.avm");
        }
    }
}