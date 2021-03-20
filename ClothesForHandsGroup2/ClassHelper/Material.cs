using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesForHandsGroup2.EF
{
    public partial class Material
    {
        public string GetTypeAndName { get => $"Тип материала: {TypeMaterial.Name} | Наименование материала: {Name}"; }
        public string GetMinCount { get => $"Минимальное количество: {MinCount} шт."; }
        public string GetCount { get => $"Остаток: {Count} шт."; }



    }
}
