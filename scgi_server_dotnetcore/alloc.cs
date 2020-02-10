using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace scgi_server_dotnetcore
{
    class Tag
    {
        public int id = 0;
        public string name = "";
        public bool is_array = false;
        public int array_size = 0;
        public int value = 0;
        public bool valid = false;
        public int address = 0;
        public int offset = 0;
        public int size = 0;
        public string scope = "";
        public int type = 0;
        public string description = "";

        // runtime
        public bool read_request = false;
        public bool write_request = false;
        public int timestamp = 0;


        // constructor
        public Tag(string Name)
        {
            name = Name.ToLower();
        }

        public override string ToString()
        {
            string s = $"<Tag> id: {id}, address: {address}, array_size: {array_size}, offset: {offset}, size: {size}, ";
            s += $"scope: {scope}, type: {type}, name: {name}, valid: {valid}, value {value}, description: {description}";
            return s;
        }

    }

    class TagList
    {
        List<Tag> list = new List<Tag>();
        Dictionary<string, Tag> dictByName = new Dictionary<string, Tag>();

        // constructor
        public TagList()
        {
            this.clear();
        }

        public void add(Tag tag)
        {
            list.Add(tag);
            dictByName.Add(tag.name, tag);
        }

        public void clear()
        {
            list.Clear();
            dictByName.Clear();
        }

        public Tag get_by_name(string name)
        {
            name = name.ToLower();
            return dictByName[name];
        }

        public int count()
        {
            return dictByName.Count;
        }
    }

    // Base class for CyBro communication
    class Allocation
    {
        private Regex __alc_pattern = new Regex("^(\\w*)\\s*(\\w*)\\s*(\\w*)\\s*(\\w*)\\s*(\\w*)\\s*(\\w*)\\s*(\\w*)\\s*([\\w\\.]*)\\s*(.*)"); // OPT
        private string __path = "";
        private int __nad = 0;
        public Tag? tags = null;
        public DateTime? file_transfer_timestamp = null;
        
        // constructor
        public Allocation(int nad)
        {
            // get path from configuration
            string alloc_path = ConfigurationManager.AppSettings.Get("AllocationDirectory");

            // make absolute path
            alloc_path = Path.GetFullPath(alloc_path);

            // add slash at the end of the path
            if((alloc_path.Length != 0) && (alloc_path[alloc_path.Length - 1] != Path.DirectorySeparatorChar))
            {
                alloc_path += Path.DirectorySeparatorChar;
            }

            __path = alloc_path;
            __nad = nad;
        }

        private void __parse_alloc_file(string s)
        {
            string[] lines = s.Split(new[] { '\r', '\n' });
            string line;
            
            foreach(string Line in lines)
            {
                line = Regex.Replace(Line, @"\s+", "");
                if(line.Length != 0 && line[0] != ';')
                {
                    MatchCollection m = __alc_pattern.Matches(line);
                }
            }
        }
    }

}
