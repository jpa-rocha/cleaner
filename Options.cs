	namespace CmdOptions {

	public class Options {
		private int exitcode;
		private int count_a = 0;
		private int count_c = 0;

		private int command_index;
		private string[] extensions = {};
		private string[] default_extensions  =  { "*.out", "*.o", "*." };
		private List<string> extensions_list = new List<string>();

		public void setExit(int num) {
			exitcode = num;
		}

		public void setCountA(int num) {
			count_a = num;
		}

		public void setCountC(int num) {
			count_c = num;
		}

		public void setCommandIndex(int num) {
			command_index = num;
		}
		public void addExtension(string extension) {
			extensions_list.Add(extension);
		}

		public void extesionListToArray() {
			extensions = extensions_list.ToArray();
		}

		public int getExit() {
			return exitcode;
		}

		public int getCountA() {
			return count_a;
		}

		public int getCountC() {
			return count_c;
		}

		public int getCommandIndex() {
			return command_index;
		}
		public string[]getExtensions() {
			if (exitcode < 2)
				return default_extensions;
			else
				return extensions;
		}		
	}
}
