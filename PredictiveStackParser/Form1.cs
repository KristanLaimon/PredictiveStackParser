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
            var lexicResults = lexic.EscanearTexto(this.TextoPorDefecto);
            lexicResults = lexic.AddSignoDolarAlFinal(lexicResults);
            var help = sintactic.LL(lexicResults);

        }
        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            dgvErrores.Rows.Clear();
            dgvSintactica.Rows.Clear();

            var error = sintactic.LL(lexicResults);

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            //se crearon por error de deo
        }

        private void label4_Click(object sender, EventArgs e)
        {
            //se crearon por error de deo
        }

        private void label2_Click(object sender, EventArgs e)
        {
            //se crearon por error de deo
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //
        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            //
        }


    }
}
