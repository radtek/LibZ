﻿using System;
using System.Reflection;
using Linq20;
using ModuleByReference;

namespace ModuleBetween
{
	public class ModuleBetweenCode
	{
		public static void Run()
		{
			Exec(ExecModuleByFull);
			Exec(ExecModuleByPartial);
			Exec(ExecModuleByReference);
		}

		private static void ExecModuleByReference()
		{
			ModuleByReferenceCode.Run();
		}

		private static void ExecModuleByPartial()
		{
			var assembly = Assembly.Load("ModuleByPartial");
			var type = Linq.Single(assembly.GetTypes(), t => t.Name == "ModuleByPartialCode");
			var method = type.GetMethod("Run", BindingFlags.Public | BindingFlags.Static);
			method.Invoke(null, new object[0]);
		}

		private static void ExecModuleByFull()
		{
			// assume version are the same
			var version = Assembly.GetEntryAssembly().GetName().Version;
			var assemblyName = new AssemblyName(
				string.Format("ModuleByFull, Version={0}, Culture=neutral, PublicKeyToken=null", version));
			var assembly = Assembly.Load(assemblyName);
			var type = Linq.Single(assembly.GetTypes(), t => t.Name == "ModuleByFullCode");
			var method = type.GetMethod("Run", BindingFlags.Public | BindingFlags.Static);
			method.Invoke(null, new object[0]);
		}

		private static void Exec(Action action)
		{
			try
			{
				action();
			}
			catch (Exception e)
			{
				Console.WriteLine("FAILURE {0}: {1}", e.GetType().Name, e.Message);
			}
		}
	}
}
