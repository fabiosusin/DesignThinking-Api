namespace DTO.General.Files.Output
{
    public class GenerateDocOutput
    {
        public GenerateDocOutput(string name, string path) 
        {
            Name = name;
            Path = path;    
        }

        public string Name { get; set; }
        public string Path { get; set; }
    }
}
