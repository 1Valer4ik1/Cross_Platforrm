using McMaster.Extensions.CommandLineUtils;

namespace Lab4
{
    [Command(Name = "LabRunnerApp", Description = "Console app to run lab projects")]
    [Subcommand(typeof(VersionCommand), typeof(RunCommand), typeof(SetPathCommand))]
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        private void OnExecute()
        {
            Console.WriteLine("Please provide a command. Available commands: version, run, set-path");
        }
    }

    [Command(Name = "version", Description = "Displays application information")]
    class VersionCommand
    {
        public int OnExecute()
        {
            Console.WriteLine("Author: Karvatskyi Volodymyr, Version: 1.0.0");
            return 0;
        }
    }

    [Command(Name = "run", Description = "Executes the specified lab project")]
    class RunCommand
    {
        [Argument(0, Description = "Lab name: lab1, lab2, or lab3")]
        public string LabName { get; }

        [Option("-I|--input", Description = "Direct path to the input file")]
        public string InputPath { get; }

        [Option("-O|--output", Description = "Direct path to the output file")]
        public string OutputPath { get; }

        public int OnExecute()
        {
            try
            {
                string inputPath = Path.Combine(GetDirectoryPath(InputPath, "LAB_PATH"), "input.txt");
                string outputPath = Path.Combine(GetDirectoryPath(OutputPath, "LAB_PATH"), "output.txt");

                if(!File.Exists(inputPath))
                {
                    throw new FileNotFoundException("Required files 'input.txt' not found in any specified path.");
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        private string GetDirectoryPath(string? directPath, string envVariable)
        {
            // Use the direct path if specified and if it contains the required files
            if (!string.IsNullOrEmpty(directPath) && Directory.Exists(directPath))
            {
                return directPath;
            }

            // Check the path from the environment variable if it contains the required files
            string? envPath = Environment.GetEnvironmentVariable(envVariable);
            if (!string.IsNullOrEmpty(envPath) && Directory.Exists(envPath))
            {
                return envPath;
            }

            // Check in the user's home directory if it contains the required files
            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return homePath;
        }
    }

    [Command(Name = "set-path", Description = "Sets the path for input and output files")]
    class SetPathCommand
    {
        [Option("-p|--path", Description = "Path to the directory containing input and output files", ShowInHelpText = true)]
        public string Path { get; }

        public int OnExecute()
        {
            if (Directory.Exists(Path))
            {
                Environment.SetEnvironmentVariable("LAB_PATH", Path);
                Console.WriteLine($"Environment variable 'LAB_PATH' set to {Path}");
                return 0;
            }
            else
            {
                Console.WriteLine("Invalid path. Directory does not exist.");
                return 1;
            }
        }
    }
}