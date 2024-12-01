using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PredictiveStackParser.Automatas
{
    internal enum BetterTypeError
    {
        MissingOpeningBracket,
        MissingClosingBracket,
        MissingOperand,
        MissingOperator,
        InvalidExpression,
    }

    internal record BetterSintacticError
    {
        public string ExpressionText = "";
        /// <summary>
        /// Error genérico, no debería de mostrarse, ya que se debería de catalogarse correctamente.
        /// </summary>
        public BetterTypeError errorType = BetterTypeError.InvalidExpression;
        public int LineFound;
    }

    internal class BetterSintacticAutomata
    {
        private string[,] tablaSintactica;
        private Stack<string> stack;

        public BetterSintacticAutomata()
        {
            this.stack = new Stack<string>();
            this.tablaSintactica = new string[,]
            {
                /*Op será X*/ /*Or será Z */
                       //(        Op      )    Or     ;    $
                /*P*/ { "(FA;P", "F;P", "\0", "\0", "\0", "λ" },
                /*A*/ { "P)",    "P)",  "\0", "\0", "\0", "\0" },
                /*F*/ { "(FA",   "OG",  "\0", "\0", "\0", "\0" },
                /*G*/ { "\0",    "\0",  "λ",  "RF", "λ",  "λ" },
                /*O*/ { "\0",    "X",   "\0", "\0", "\0", "\0"},
                /*R*/ { "\0",    "\0",  "\0", "Z",  "\0", "\0"},
            };

        }

        private int noProductionToInt(string i)
        {

            Func<string, bool> isOperand = (op) => op.ToArray().Any(letter => !Char.IsLetter(letter) || !Char.IsNumber(letter)); 
            Func<string, bool> isOperator = (or) => or == "+" || or == "-" || or == "*" || or == "/";

            if (i == "(") return 0;
            if (isOperand(i)) return 1;
            if (i == ")") return 2;
            if (isOperator(i)) return 3;
            if (i == ";") return 4;
            if (i == "$") return 5;

            throw new Exception("Esto no debería pasar, error al parsear un noProduction string a un índice para la tabla sintáctica");
        }

        private int productionToInt(string letterProduction)
        {
            return letterProduction switch
            {
                "P" => 0,
                "A" => 1,
                "F" => 2,
                "G" => 3,
                "O" => 4,
                "R" => 5,
                _ => -1,
            };
        }

        private bool isTerminal(string element)
        {
            List<string> terminals = ["P", "A", "F", "G", "O", "R"];
            return !terminals.Contains(element);
        }

        private bool isProduction(string element)
        {
            return element != "\0";
        }

        public List<BetterSintacticError> LL(LexicAutomataResult lexicResults)
        {
            List<BetterSintacticError> toReturn = new List<BetterSintacticError>();
            List<RegistroLexico> fullLexicTable = lexicResults.RegistrosLexicos;

            // Se da por hecho que ya se agregó al final el $ usando el método de objAutomatLexico.AddSignoDolarAlfinal()
            if (fullLexicTable.Last().TokenText != "$")
                throw new Exception("Error al parsear la tabla léxica, no se encontró el signo $ al final de la tabla léxica");

            this.stack.Clear();
            this.stack.Push("$");
            this.stack.Push("P");

            int apuntador = 0;
            string elementoLexico = String.Empty;
            string elementoExtraido = String.Empty;
            RegistroLexico elementoLexicoObj = null;

            do
            {
                elementoLexicoObj = fullLexicTable[apuntador]; //Added by me to get more info about the extracted element!
                elementoExtraido = this.stack.Pop();
                elementoLexico = fullLexicTable[apuntador].TokenText;
                if (isTerminal(elementoExtraido) || elementoExtraido == "$")
                {
                    if (elementoLexico == elementoExtraido)
                        apuntador++;
                    else
                    {
                        toReturn.Add(new BetterSintacticError
                        {
                            ExpressionText = elementoLexico,
                            errorType = BetterTypeError.InvalidExpression, //How can i know the error type?
                            LineFound = elementoLexicoObj.LineaEnDondeAparece
                        });
                    }
                    //else
                    //    throw new Exception("Error sintáctico"); (Se ocupa guardar esto en una lista output)
                }
                else
                {
                    var possibleProductionElement = tablaSintactica[productionToInt(elementoExtraido), noProductionToInt(elementoLexico)];
                    if (isProduction(possibleProductionElement)){
                        if (possibleProductionElement != "λ")
                            stack.Push(possibleProductionElement);
                    }
                    else
                    {
                        //throw new Exception("Error sintáctico"); (Se ocupa guardar esto en una lista output)
                    }
                }
            } while (elementoExtraido == "$");
        }
    }
}
