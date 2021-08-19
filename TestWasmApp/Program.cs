using System;
using Wasmtime;

namespace TestWasmApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using var engine = new Engine();

            using var module = Module.FromText(
                engine,
                "hello",
                "(module (func $hello (import \"\" \"hello\")) (func (export \"run\") (call $hello)))"
            );

            using var linker = new Linker(engine);
            using var store = new Store(engine);

            linker.Define(
                "",
                "hello",
                Function.FromCallback(store, () => Console.WriteLine("Hello from C#!"))
            );

            var instance = linker.Instantiate(store, module);
            var run = instance.GetFunction(store, "run");
            run?.Invoke(store);
        }
    }
}
