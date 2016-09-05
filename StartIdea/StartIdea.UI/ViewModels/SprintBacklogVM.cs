using StartIdea.Model.ScrumEventos;
using System;
using System.Collections.Generic;

namespace StartIdea.UI.ViewModels
{
    public class SprintBacklogVM
    {
        public IEnumerable<Sprint> Sprints { get; set; }

        public void PreencherSprints()
        {
            List<Sprint> teste = new List<Sprint>();
            Sprint item = new Sprint();
            item.Id = 1;
            item.Objetivo = @"teste 1 crazy";
            item.DataInicio = new DateTime(2015, 10, 2);
            item.DataFim    = new DateTime(2015, 11, 2);

            teste.Add(item);

            Sprint item1 = new Sprint();
            item1.Id = 1;
            item1.Objetivo = @"Teste 2";
            item1.DataInicio = new DateTime(2015, 10, 2);
            item1.DataFim = new DateTime(2015, 11, 2);

            teste.Add(item1);

            Sprint item2 = new Sprint();
            item2.Id = 1;
            item2.Objetivo = @"Teste 3";
            item2.DataInicio = new DateTime(2015, 10, 2);
            item2.DataFim = new DateTime(2015, 11, 2);

            teste.Add(item2);

            Sprint item3 = new Sprint();
            item3.Id = 1;
            item3.Objetivo = @"Teste 4";
            item3.DataInicio = new DateTime(2015, 10, 2);
            item3.DataFim = new DateTime(2015, 11, 2);

            teste.Add(item3);

            Sprint item4 = new Sprint();
            item4.Id = 1;
            item4.Objetivo = @"Teste 5";
            item4.DataInicio = new DateTime(2015, 10, 2);
            item4.DataFim = new DateTime(2015, 11, 2);

            teste.Add(item4);

            Sprint item5 = new Sprint();
            item5.Id = 1;
            item5.Objetivo = @"Teste 6";
            item5.DataInicio = new DateTime(2015, 10, 2);
            item5.DataFim = new DateTime(2015, 11, 2);
            teste.Add(item5);

            Sprint item6 = new Sprint();
            item6.Id = 1;
            item6.Objetivo = @"Teste 7";
            item6.DataInicio = new DateTime(2015, 10, 2);
            item6.DataFim = new DateTime(2015, 11, 2);

            teste.Add(item6);

            Sprint item7 = new Sprint();
            item7.Id = 1;
            item7.Objetivo = @"Teste 8";
            item7.DataInicio = new DateTime(2015, 10, 2);
            item7.DataFim = new DateTime(2015, 11, 2);

            teste.Add(item7);

            Sprints = teste;
        }
    }
}