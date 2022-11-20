using CmdOptions;
using System;
using System.Diagnostics;

namespace CleanerLogic {
	public static class Logic {
		/**
			Return the proper directories to search according to the user options
		*/
		public static string[]GetWorkDirs(string root, int option) {
			string[] get_subdirectories;
			if (option == 1 || option == 3)
				get_subdirectories = Directory.GetDirectories(root,
																	"*",
																	SearchOption.AllDirectories);
			else {
				string[] ret = new string[1];
				ret[0] = root;
				return ret;
			}
			List<string> directories = new List<string>();
			foreach (string directory in get_subdirectories){
					directories.Add(directory);
			}
			directories.Add(root);
			return directories.ToArray();
		}

		public static string[]GetFilesToDelete(string[] directories, string[] extensions)
		{
			List<string> files = new List<string>();
			foreach (string directory in directories)
			{
				foreach (string extension in extensions)
				{
					string[] get_files = System.IO.Directory.GetFiles(directory, extension);
					foreach (string item in get_files)
					{
						files.Add(item);
					}
				}
			}
			return files.ToArray();
		}

		/**
			Print required help or error messages
		*/
		public static int printHelp(int option)
		{
			if (option == -1) {
				Console.WriteLine("Correct use of Cleaner is:");
				Console.WriteLine("cleaner [-a] [-h] [-c] [-ac]");
				return 1;
			}
			else if (option == -2) {
				Console.WriteLine("Improper use of the [-c] flag:");
				Console.WriteLine("You must enter at least one custom patern: *.cpp ");
			}
			else if (option == 9)
			{
				Console.WriteLine("Welcome to Cleaner.");
				Console.WriteLine("Cleaner will delete all .o, .out and files without extensions (take care).");
				Console.WriteLine("Running Cleaner without flags will delete files in current directory.");
				Console.WriteLine("Using the [-a] flag will also delete files in all subdirectories.");
				Console.WriteLine("Using the [-c] will allow custom search pattern to be entered.");
				return 9;
			}
			return (-1);
		}
		
		/**
			ParseOptions checks for flags, and or new search patterns
			Exit codes:
			-2 -> Improper ise of the [-c] flag;
			-1 -> Genereal improper use of flags;
			0 -> No flags inputed;
			1 -> Cleaner called for all sub directories [-a];
			2 -> Custom pattern entered for root;
			3 -> Custom pattern to be searched in all subdirectories;
			9 -> Help called [-h]; 
		*/
		public static Options ParseOptions(string[] args)
		{
			var options = new Options();
			// Default Behaviour, no flags
			if (args.Length == 0) {
				options.setExit(0);
			}
			else if (args.Length >= 1) {
				foreach(string arg in args) {
					if (arg == "-h") {
						options.setExit(printHelp(9));
						return options;
					}
				}
				options = checkFlags(args, options);
				if (options.getCountA() > 1 || options.getCountC() > 1) {
					options.setExit(printHelp(-1));
				}
				else {
					options = enforceOrder(args, options);
					return options;
				}
			}
			return options;
		}

		/**
			Checks which flags were used, and if there are no repeated flags
		*/
		public static Options checkFlags(string[] args, Options options)
		{
				int count_a = 0;
				int count_c = 0;
				foreach (string arg in args) {
					if (arg == "-a") {
						options.setCountA(count_a += 1);
					}
					else if (arg == "-c") {
						options.setCountC(count_c += 1);
					}
					else if (arg == "-ac" || arg == "-ca") {
						options.setCountA(count_a += 1);
						options.setCountC(count_c += 1);
					}
				}
				return options;
		}

		/**
			Checks the order in which the flags are called
		*/
		public static Options enforceOrder(string[] args, Options options)
		{
			if (options.getCountA() == 1 && options.getCountC() == 0) {
				if (args.Length == 1)
					options.setExit(1);
				else
					options.setExit(printHelp(-1));
			}
			else if (options.getCountA() == 0 && options.getCountC() == 1) {
				if (args[0] != "-c")
					options.setExit(printHelp(-1));
				else if (args.Length == 1) {
					options.setExit(printHelp(-2));
				}
				else {
					options.setCommandIndex(1);
					options = customPatter(args, options); 
					options.setExit(2);
				}
			}
			else {
				if ((args[0] == "-ac" || args[0] == "-ca" )&& args.Length > 1) {
					options.setCommandIndex(1);
					options = customPatter(args, options); 
					options.setExit(3);
					return options;
				}
				else if (args.Length == 1)
					options.setExit(printHelp(-2));
				if ((args[0] == "-a" && args[1] == "-c")
					|| (args[0] == "-c" && args[1] == "-a") && args.Length > 2) {
					options.setCommandIndex(2);
					options = customPatter(args, options); 
					options.setExit(3);
				}
				else if (args.Length == 2)
					options.setExit(printHelp(-2));
			}
			return options;
		}

		/**
			Creates the custom pattern
		*/
		public static Options customPatter(string[] args, Options options)
		{
			for (int i = options.getCommandIndex(); i < args.Length; i++) {
				options.addExtension(args[i]);
			}
			options.extesionListToArray();
			return options;
		}

		/**
			If Possible deletes the files, if not throws error
		*/
		public static int fileDeleter(string[] files_to_delete)
		{
			foreach (string item in files_to_delete)
			{
				try
				{
					Console.WriteLine( System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
					if (item == System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName){
						Console.WriteLine("You are about to delete the cleaner.");
						Console.WriteLine("Are you sure you want to proceed? [Y/n]");
						if (Console.ReadLine() == "Y")
							File.Delete(item);
						else {
							continue;
						}

					}
					File.Delete(item);
				}
				catch
				{
					Console.WriteLine($"There was a problem deleting {item}");
					return 1;
				}
			}
			return 0;
		}
	}
}
