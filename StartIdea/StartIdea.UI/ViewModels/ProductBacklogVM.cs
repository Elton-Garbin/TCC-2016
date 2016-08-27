using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StartIdea.UI;

namespace StartIdea.UI.ViewModels
{
    public class ProductBacklogVM
    {
        public IEnumerable<ProductBacklogItem> BackLogItem { get; set; }

        public void PreencheBackLogItem()
        {
            List<ProductBacklogItem> teste = new List<ProductBacklogItem>();
            teste.Add(new ProductBacklogItem(1, "teste 1", "PP", 1));
            teste.Add(new ProductBacklogItem(2, "teste 2", "M" , 2));
            teste.Add(new ProductBacklogItem(3, "teste 3", "G" , 3));
            teste.Add(new ProductBacklogItem(4, "teste 4", "GG", 4));
            teste.Add(new ProductBacklogItem(5, "teste 5", "P" , 5));
            teste.Add(new ProductBacklogItem(6, "teste 6", "M" , 6));

            teste.Add(new ProductBacklogItem(7, "arruma 1", "PP", 7));
            teste.Add(new ProductBacklogItem(8, "arruma 2", "M" , 8));
            teste.Add(new ProductBacklogItem(9, "arruma 3", "G" , 9));
            teste.Add(new ProductBacklogItem(10, "arruma 4", "GG", 10));
            teste.Add(new ProductBacklogItem(11, "arruma 5", "P" , 11));
            teste.Add(new ProductBacklogItem(12, "arruma 6", "M" , 12));

            BackLogItem = teste;
        }
    }

    public class ProductBacklogItem
    {
        public ProductBacklogItem(int _id, string _userStory, string _tamanho, int _prioridade)
        {
            Id = _id;
            UserStory = _userStory;
            Tamanho = _tamanho;
            Prioridade = _prioridade;
        }

        public int Id { get; set; }
        public int ProductBacklogId { get; set; }
        public string UserStory { get; set; }
        public string Tamanho { get; set; }
        public int Prioridade { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataExclusao { get; set; }
    }
}