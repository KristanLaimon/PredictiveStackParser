using PredictiveStackParser.Automatas;

namespace PredictiveStackParser
{
    public partial class Form1 : Form
    {
        private readonly string TextoPorDefecto = "(A + B)\r\n(A + B + B) + D\r\n(((A + B)))\r\nA + (C + D)\r\n((A + B) * (C / D) - (D * A)) - D\r\nA + (B + C)";
        private LexicAutomata lexic = new();
        public Form1()
        {
            InitializeComponent();
            var lexicResults = lexic.EscanearTexto(this.TextoPorDefecto);
            lexicResults = lexic.AddSignoDolarAlFinal(lexicResults);
        }
    }
}
