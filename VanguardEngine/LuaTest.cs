using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace VanguardEngine
{
    class LuaTest
    {
        public void Testing()
        {
            Script.DefaultOptions.ScriptLoader = new MoonSharp.Interpreter.Loaders.FileSystemScriptLoader();
            Script script = new Script();
            // Register just MyClass, explicitely.
            UserData.RegisterType<ReceiveInt>();

            // create a userdata, again, explicitely.
            DynValue obj = UserData.Create(new ReceiveInt());

            script.Globals.Set("obj", obj);

            DynValue res = script.DoFile("testlua.lua");

            Console.WriteLine(res);
        }
    }

    public class ReceiveInt
    {
        public int GetInt()
        {
            WhereIsInt test = new WhereIsInt();
            return test.GetThatInt();
        }
    }

    class WhereIsInt
    {
        HereIsInt _hereIsInt = new HereIsInt();
        public int GetThatInt()
        {
            return _hereIsInt.TheInt;
        }
    }

    class HereIsInt
    {
        int _theInt = 2;
        public int TheInt
        {
            get => _theInt;
        }
    }

}
