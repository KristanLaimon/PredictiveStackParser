using PredictiveStackParser.Automatas;

namespace PredictiveStackParser
{
    public partial class Form1 : Form
    {
        private readonly string TextoPorDefecto = "(X1+B2);\r\n(Y1+B3*C4)+D;\r\n(((VAR2+X1)));\r\n(PESO+(CARGO*DIF2));\r\n((X2+45.78)*(CARGO/ABONO)-(PORC*12.55))-INT;\r\n456.78*(12.34*3.56E45)+B2;";
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
