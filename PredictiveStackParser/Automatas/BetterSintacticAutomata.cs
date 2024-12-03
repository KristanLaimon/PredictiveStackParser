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
        public int errorCode;
        public string errorDescription = "";
    }

    internal class BetterSintacticAutomata
    {
        private string[,] tablaSintactica;
        private Stack<string> stack;
        private readonly Dictionary<BetterTypeError, (int, string)> ErroresDisponibles;

        public BetterSintacticAutomata()
        {
            this.stack = new Stack<string>();
            this.tablaSintactica = new string[,]
            {
                /*Op será X*/ /*Or será Z */
                       //(        Op      )    Or     ;    $
                /*P*/ { "(FA;P", "F;P", "\0", "\0", "\0", "λ" },
                /*A*/ { "P)",    "P)",  ")G", "\0", "\0", "\0" },
                /*F*/ { "(FA",   "OG",  "\0", "\0", "\0", "\0" },
                /*G*/ { "\0",    "\0",  "λ",  "RF", "λ",  "λ" },
                /*O*/ { "\0",    "X",   "\0", "\0", "\0", "\0"},
                /*R*/ { "\0",    "\0",  "\0", "Z",  "\0", "\0"},
            };

            ErroresDisponibles = new()
            {
                {BetterTypeError.MissingOpeningBracket, (201, "Falta delimitador") },
                {BetterTypeError.MissingClosingBracket, (202, "Falta delimitador") },
                {BetterTypeError.MissingOperand, (203, "Falta identificador") },
                {BetterTypeError.MissingOperator, (204, "Falta operador") },
                {BetterTypeError.InvalidExpression, (205, "Expresión inválida") } 
            };
        }

        private bool isOperand(string element) => element.All((charsito) => Char.IsLetterOrDigit(charsito) || charsito == '.' || charsito == '_');
        private bool isOperator(string element) => element == "+" || element == "-" || element == "*" || element == "/";


        private int terminalToInt(string i)
        {
            if (i == "(") return 0;
            if (i == "X") return 1;
            if (i == ")") return 2;
            if (i == "Z") return 3;
            if (i == ";") return 4;
            if (i == "$") return 5;
            throw new Exception("Esto no debería pasar, error al parsear un noProduction string a un índice para la tabla sintáctica");
        }

        private int noTerminalToInt(string letterProduction)
        {
            return letterProduction switch
            {
                "P" => 0,
                "A" => 1,
                "F" => 2,
                "G" => 3,
                "O" => 4,
                "R" => 5,
                _ => throw new Exception($"Unexpected non-terminal '{letterProduction}' encountered."),
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

        private BetterTypeError IdentifyErrorType(string elementLexico, string elementExtraido)
        {
            if (elementLexico == "$" && elementExtraido == "F") return BetterTypeError.MissingClosingBracket;
            if (elementLexico == "Z" && elementExtraido == "F") return BetterTypeError.MissingOperand;
            if (elementLexico == "X" && elementExtraido == "G") return BetterTypeError.MissingOperator;
            if (elementLexico == ")")return BetterTypeError.MissingOpeningBracket;
            if (elementExtraido == "A") return BetterTypeError.MissingClosingBracket;

            return BetterTypeError.InvalidExpression;
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

            int actualLine = 1;
            do
            {
                int beforeLine = actualLine;
                actualLine = fullLexicTable[apuntador].LineaEnDondeAparece;
                if (actualLine != beforeLine)
                {
                    //Has changed of line, lets reset this
                    this.stack.Clear();
                    this.stack.Push("$");
                    this.stack.Push("P");
                    elementoLexico = String.Empty;
                    elementoExtraido = String.Empty;
                }

                if (stack.Count == 0)
                {
                    // Error: Stack is empty!!, it means there was a lot of ;;;; or
                    // elementoLexico and elementoExtraido went to "λ" many times that stock out the stack!
                    if (!toReturn.Any(e => e.LineFound == fullLexicTable[apuntador].LineaEnDondeAparece))
                    {
                        BetterTypeError errorTypeFound = IdentifyErrorType(elementoLexico, elementoExtraido);
                        toReturn.Add(new BetterSintacticError
                        {
                            ExpressionText = fullLexicTable[apuntador].TokenText,
                            errorType = errorTypeFound,
                            LineFound = fullLexicTable[apuntador].LineaEnDondeAparece,
                            errorDescription = ErroresDisponibles[errorTypeFound].Item2,
                            errorCode = ErroresDisponibles[errorTypeFound].Item1
                        });
                    }
                    break;
                }

                ///marranada 3000 by kris/tom
                string fullLineStr = string.Join('\0', lexicResults.RegistrosLexicosPorLinea.Where((listaLinea, indice) => (indice + 1) == actualLine).First().Select(registerList => registerList.TokenText));
                int numberOfOpeningBrackets = fullLineStr.Count(c => c == '(');
                int numberOfClosingBrackets = fullLineStr.Count(c => c == ')');
                if (numberOfClosingBrackets != numberOfOpeningBrackets)
                {
                    if (!toReturn.Any(e => e.LineFound == fullLexicTable[apuntador].LineaEnDondeAparece))
                    {
                        BetterTypeError errorTypeFound = BetterTypeError.MissingOpeningBracket;
                        toReturn.Add(new BetterSintacticError
                        {
                            ExpressionText = fullLexicTable[apuntador].TokenText,
                            errorType = errorTypeFound,
                            LineFound = fullLexicTable[apuntador].LineaEnDondeAparece,
                            errorDescription = ErroresDisponibles[errorTypeFound].Item2,
                            errorCode = ErroresDisponibles[errorTypeFound].Item1
                        });
                    }
                }

                elementoExtraido = this.stack.Pop();
                elementoLexico = fullLexicTable[apuntador].TokenText;

                if (isOperand(elementoLexico)) elementoLexico = "X";  //Added by me 
                else if (isOperator(elementoLexico)) elementoLexico = "Z"; //Added by me

                if (isTerminal(elementoExtraido) || elementoExtraido == "$")
                {
                    if (elementoLexico == elementoExtraido)
                        apuntador++;
                    else
                    {
                        // Error: Add once and progress pointer
                        if (!toReturn.Any(e => e.LineFound == fullLexicTable[apuntador].LineaEnDondeAparece))
                        {
                            BetterTypeError errorTypeFound = IdentifyErrorType(elementoLexico, elementoExtraido);
                            toReturn.Add(new BetterSintacticError
                            {
                                ExpressionText = fullLexicTable[apuntador].TokenText,
                                errorType = errorTypeFound,
                                LineFound = fullLexicTable[apuntador].LineaEnDondeAparece,
                                errorDescription = ErroresDisponibles[errorTypeFound].Item2,
                                errorCode = ErroresDisponibles[errorTypeFound].Item1
                            });
                        }
                        apuntador++; // Progress to avoid processing the same error multiple times
                    }
                }
                else
                {
                    var production = tablaSintactica[noTerminalToInt(elementoExtraido), terminalToInt(elementoLexico)];
                    if (isProduction(production))
                    {
                        if (production != "λ")
                        {
                            foreach (char symbol in production.Reverse())
                                this.stack.Push(symbol.ToString());
                        }
                    }
                    else
                    {
                        // Error: Invalid production for the non-terminal and input token
                        if (!toReturn.Any(e => e.LineFound == fullLexicTable[apuntador].LineaEnDondeAparece))
                        {
                            BetterTypeError errorTypeFound = IdentifyErrorType(elementoLexico, elementoExtraido);
                            toReturn.Add(new BetterSintacticError
                            {
                                ExpressionText = fullLexicTable[apuntador].TokenText,
                                errorType = errorTypeFound,
                                LineFound = fullLexicTable[apuntador].LineaEnDondeAparece,
                                errorDescription = ErroresDisponibles[errorTypeFound].Item2,
                                errorCode = ErroresDisponibles[errorTypeFound].Item1
                            });
                        }
                        apuntador++; // Progress to avoid looping on the same issue
                    }
                }
            } while (elementoLexico != "$");

            return toReturn;
        }
    }
}
