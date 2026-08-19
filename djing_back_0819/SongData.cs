using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DJing
{
    public class SongData
    {
        public int SongId { get; set; }
        public string Title { get; set; } = "";
        public string Artist { get; set; } = "";
        public string FilePath { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public int LikeCount { get; set; }
        public string ReleaseDate { get; set; } = "";
    }
}

