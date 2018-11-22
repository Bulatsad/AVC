using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AVI;

namespace AVC
{
    static class Connector
    {
        private static readonly IAsseblerVirtualModule[] ModuleList =
        {
            //Add new modules here
            new MOV(),
            new ADD(),
            new SUB(),
            new MUL(),
            new DIV(),
            new PUSH(),
            new POP()
        };




        public static IAsseblerVirtualModule[] GetConnectedModudels()
        {
            return ModuleList;
        }
    }
}