using System.Collections.Generic;
using System.IO;
using CmdOptions;
using CleanerLogic;
internal class Program
{
	private static int Main(string[] args)
	{
		// Parse the command line options
		Options options = Logic.ParseOptions(args);
		if (options.getExit() < 0) 
			return 1;
		if (options.getExit() == 9)
			return 0;

		// Get which extensions and directories ere required by user	
		string currentDirName = System.IO.Directory.GetCurrentDirectory();
		string[] directories = Logic.GetWorkDirs(currentDirName, options.getExit());
		string[] extensions = options.getExtensions();
		string[] files_to_delete = Logic.GetFilesToDelete(directories, extensions);
		
		// Check if files were found, if not exit
		int file_nums = files_to_delete.Length;
		if (file_nums == 0) {
			Console.WriteLine("No files were found using the followin patterns:");
			foreach (string item in extensions) {
				Console.WriteLine($"{item}");
			}
			return 0;
		}

		// Lists which files were found, ask for confirmation and if Yes delete them
		foreach (string item in files_to_delete) {
			Console.WriteLine("|  \\");
			Console.WriteLine($"|__/ {item}");
		}
		Console.WriteLine($"Number of files found: {file_nums}");
		Console.WriteLine("Do you want to delete them?");
		Console.Write("THERE IS NO TURNING BACK [Y/n]: ");
		if (Console.ReadLine() == "Y"){
			return  Logic.fileDeleter(files_to_delete);
		}
		else
			return 0;
	}
}