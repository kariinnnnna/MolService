namespace MolServiceContracts.ViewModels
{
    public class OneCImportResultViewModel
    {
        public int ImportedCount { get; set; }

        public int CreatedCount { get; set; }

        public int UpdatedCount { get; set; }

        public int ErrorCount { get; set; }

        public int SkippedCount { get; set; }

        public int TotalInventoryItemsCount { get; set; }

        public int FixedAssetItemsCount { get; set; }

        public int DepartmentFixedAssetItemsCount { get; set; }

        public int DepartmentMolsCount { get; set; }

        public int TotalMaterialStockItemsCount { get; set; }

        public int MaterialStockItemsWithMolCount { get; set; }

        public int DepartmentMaterialStockItemsCount { get; set; }

        public List<string> Messages { get; set; } = new();

        public List<string> Errors { get; set; } = new();
    }
}