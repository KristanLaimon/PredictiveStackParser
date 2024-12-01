using PredictiveStackParser.Automatas;

namespace PredictiveStackParser
{
    public partial class Form1 : Form
    {
        private readonly string TextoPorDefecto = "(a+b);a;";
        private LexicAutomata lexic = new();
        private BetterSintacticAutomata sintactic = new();
        public Form1()
        {
            InitializeComponent();
            var lexicResults = lexic.EscanearTexto(this.TextoPorDefecto);
            lexicResults = lexic.AddSignoDolarAlFinal(lexicResults);
            var help = sintactic.LL(lexicResults);
        }
    }
}
