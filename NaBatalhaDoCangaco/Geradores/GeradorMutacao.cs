using Asteroides.Engine;
using Asteroides.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroides.Geradores
{
    public class GeradorMutacao
    {
        internal void Gerar(IA melhorIA, List<IA> ias)
        {
            ias.ForEach(ia =>
            {
                ia.Cerebro.Synapses.ForEach(syn =>
                {
                    var variacao = 0.10f;
                    var min = melhorIA.Cerebro.Synapses.First(p => p.Id == syn.Id).Weight * (1 - variacao);
                    var max = melhorIA.Cerebro.Synapses.First(p => p.Id == syn.Id).Weight * (1 + variacao);
                    syn.Weight = new MinMax(min, max).Random();
                });
            });
        }
    }
}
