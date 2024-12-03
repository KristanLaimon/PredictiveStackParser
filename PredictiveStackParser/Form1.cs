using PredictiveStackParser.Automatas;

namespace PredictiveStackParser
{
    public partial class Form1 : Form
    {
        private readonly string TextoPorDefecto = "(A + B)\r\n(A + B + B) + D\r\n(((A + B)))\r\nA + (C + D)\r\n((A + B) * (C / D) - (D * A)) - D\r\nA + (B + C)";
        private LexicAutomata lexic = new();
        private BetterSintacticAutomata sintactic = new();
        public Form1()
        {
            InitializeComponent();
            this.labelMessage.Text = "Sin escanear";
            this.richTextBoxInput.Text = this.TextoPorDefecto;
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            int faseActual = 1;
            int errorCode = 100;
            string errorMsg = "Sin error";
            int errorLine = -1;

            var lexicResults = lexic.EscanearTexto(this.richTextBoxInput.Text);
            if (lexicResults.Errores.Count > 0)
            {
                var first = lexicResults.Errores.First();
                errorCode = first.CodigoError;
                errorMsg = first.DescripcionError;
                errorLine = first.LineaEnDondeAparece;
            }
            else
            {
                lexicResults = lexic.AddSignoDolarAlFinal(lexicResults);
                var sintacticResult = sintactic.LL(lexicResults);
                if (sintacticResult.Count > 0)
                {
                    var first = sintacticResult.First();
                    faseActual = 2;
                    errorCode = first.errorCode;
                    errorMsg = first.errorDescription;
                    errorLine = first.LineFound;
                }
            }

            string finalMsg;
            if (errorCode == 100)
                finalMsg = $"{faseActual}:{errorCode} {errorMsg}";
            else
                finalMsg = $"{faseActual}:{errorCode} Error en línea {errorLine}: {errorMsg}";

            labelMessage.Text = finalMsg;
        }
    }
}
