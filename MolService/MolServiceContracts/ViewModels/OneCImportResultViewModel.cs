using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.ViewModels
{
    public class OneCImportResultViewModel
    {
        public int ImportedCount { get; set; }

        public int CreatedCount { get; set; }

        public int UpdatedCount { get; set; }

        public int ErrorCount { get; set; }

        public List<string> Errors { get; set; } = new();
    }
}
