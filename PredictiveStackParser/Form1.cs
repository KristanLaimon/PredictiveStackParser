using PredictiveStackParser.Automatas;

namespace PredictiveStackParser
{
    public partial class Form1 : Form
    {
        private readonly string TextoPorDefecto = "(X1+B2);\r\n(((VAR2+X1)));\r\n456.78*(12.34*3.56E45)+B2;";
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
