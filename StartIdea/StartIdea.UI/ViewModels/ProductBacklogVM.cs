using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StartIdea.UI;
using StartIdea.Model.Scrum.Artefatos;

namespace StartIdea.UI.ViewModels
{
    public class ProductBacklogVM : ProductBacklog
    {
        public IEnumerable<ProductBacklogItem> BackLogItem { get; set; }

        public void PreencheBackLogItem()
        {
            List<ProductBacklogItem> teste = new List<ProductBacklogItem>();
            ProductBacklogItem item = new ProductBacklogItem();
            item.Id = 1;
            item.UserStory = @"Teste da user asflj akjfhafkjhasfjkasfj asjfaj fajf hakfh ajkfh afkjhafkjahfkjahsfjkashf jakf jkashf jahf jkah fkja fjkahs fjashf kjasfkjasfk aksf akjs f7
                afasfaaslfkh afsjk asjf aksjf jaskf askjfh akjsf ajskf hakjshfjkhfjkahsf jaksf hajshfjahsfkajf,alhfsldhçsdghaldgkljdfhg kdj gakjhsjhskl sj jks gj gd";
            item.Tamanho = "PP";
            item.Prioridade = 1;
            item.DataInclusao = new DateTime(2015, 10, 2);

            teste.Add(item);

            BackLogItem = teste;
        }
    }
}