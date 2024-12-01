using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Security.Policy;
using System.Text;

namespace PredictiveStackParser.Automatas
{
    public enum Token
    {
        Delimitadores = 50,
        Operadores = 70,
        CadenaVacia = 99,
        Identificador = 100,
        SignoDolar = 199,
        Constante = 200,
        Regla = 300
    }

    public enum TipoChar
    {
        Letra,
        Digito,
        Delimitador,
        Operador,
        Exponencial,
        EspacioBlanco,
        PuntoFlotante,
        Desconocido
    }

    public record RegistroLexico
    {
        public int LineaNum;
        public string TokenText = "";
        public Token tokenType;
        public int Tipo;
        public int Codigo;
        public int LineaEnDondeAparece;
    }

    public record RegistroDinamico
    {
        public string IdentificadorTexto = "";
        public int Valor;
        public List<int> LineasEnDondeAparece = new();
    }

    public record RegistroConstante
    {
        public string ConstanteTexto = "";
        public int Valor;
        public int LineaEnDondeAparece;
    }
    public record RegistroError
    {
        public int LineaEnDondeAparece;
        public string ErrorTexto = "";
        public int CodigoError;
        public string DescripcionError = "";
    }

    public record AutomataResult
    {
        public List<RegistroLexico> RegistrosLexicos = [];
        public List<RegistroDinamico> RegistrosDinamicos = [];
        public List<RegistroConstante> RegistrosConstantes = [];
        public List<RegistroError> Errores = [];
    }

    internal class LexicAutomata
    {
        private readonly int[,] tablaTransiciones;

        private TipoChar tipoCharActual;
        private int estadoActual;

        // Manejo de constantes
        private const int ValorIdentificadorDefault = 100;
        private const int ValorConstanteDefault = 200;
        private int valorConstanteActual;
        private int valorIdentificadorActual;

        // Mini-Almacenes para almacenar identificadores y constantes
        private StringBuilder bufferIdentificador;
        private StringBuilder bufferConstante;
        private AutomataResult results;

        // Conjuntos y diccionarios
        private readonly (char, int) SignoDolar = ('$', 199);
        private readonly (char, int) CadenaVacia = (' ', 99);
        private readonly Dictionary<char, int> Delimitadores;
        private readonly Dictionary<char, int> Operadores;
        private readonly Dictionary<int, string> ErroresDisponibles;
        private readonly Dictionary<Token, int> Tipos;

        public LexicAutomata()
        {
            tablaTransiciones = new int[,]
             {
                {1, 2, 3, 4, 5, 0, 2, 6 },
                {1, 1, 3, 4, 1, 0, 5, 6 },
                {1, 2, 3, 4, 7, 0, 8, 6 },
                {1, 2, 3, 4, 1, 0, 5, 6 },
                {1, 2, 3, 4, 1, 0, 5, 6 },
                {5, 5, 3, 4, 5, 0, 5, 6 },
                {1, 2, 3, 4, 1, 0, 2, 6 },
                {5, 10,3, 4, 5, 0, 5, 6 },
                {5, 9, 3, 4, 5, 0, 5, 6 },
                {5, 9, 3, 4, 7, 0, 5, 6 },
                {5, 10, 3, 4, 5, 0, 5, 9 }
             };

            estadoActual = 0;
            tipoCharActual = TipoChar.EspacioBlanco; //TipoChar por defecto
            bufferConstante = new StringBuilder();
            bufferIdentificador = new StringBuilder();
            valorConstanteActual = ValorConstanteDefault;
            valorIdentificadorActual = ValorIdentificadorDefault;
            results = new AutomataResult();

            //---------------- Inicializando Diccionarios ------------------
            Tipos = new()
            {
                {Token.Delimitadores, 5 },
                {Token.Operadores,  7},
                {Token.CadenaVacia, 2 },
                {Token.Identificador, 1 },
                {Token.SignoDolar, 4 },
                {Token.Constante, 3 },
                {Token.Regla, 6 }
            };
            ErroresDisponibles = new()
            {
                {100, "Sin errores"},
                {101, "Símbolo desconocido" },
                {102, "Elemento Inválido" }
            };
            Operadores = new()
            {
                { '+', 70 },
                { '-', 71 },
                {  '*', 72 },
                {  '/', 73 }
            };
            Delimitadores = new()
            {
                { '(', 50 },
                { ')', 51 },
                {  ';', 52 }
            };
        }


        public AutomataResult EscanearTexto(string allText)
        {
            Resetear();
            string[] lineas = allText.Replace("\r", "").Split('\n');

            //Realiza el análisis
            for (int lineaActualIndex = 0; lineaActualIndex < lineas.Length; lineaActualIndex++)
            {
                foreach (char c in lineas[lineaActualIndex])
                {
                    TipoChar tipoCharAnterior = tipoCharActual;
                    int estadoAnterior = estadoActual;

                    tipoCharActual = CategorizarCaracter(c);
                    estadoActual = tablaTransiciones[estadoActual, (int)tipoCharActual];

                    /// TiposChars (Separación): Lógica de separación de tokens usando delimitadores y operadores
                    if (tipoCharActual == TipoChar.Delimitador || tipoCharActual == TipoChar.Operador || tipoCharActual == TipoChar.EspacioBlanco)
                    {
                        if (elEstadoEsValido(estadoAnterior))
                        {
                            //El texto anterior es una constante válida
                            if (bufferConstante.Length != 0)
                            {
                                AddNuevoConstante(bufferConstante.ToString(), lineaActualIndex + 1);
                            }
                            ///El texto anterior al separador es un identificador válido

                            else if (bufferIdentificador.Length != 0)
                            {
                                AddNuevoIdentificador(bufferIdentificador.ToString(), lineaActualIndex + 1);
                            }

                            //No entraría en ningún if si lo anterior es un operador o delimitador.
                        }
                        else if (estadoAnterior != 0)
                        {
                            string actualErrorText = bufferConstante.Length != 0 ? bufferConstante.ToString() : bufferIdentificador.ToString();

                            results.Errores.Add(new RegistroError
                            {
                                CodigoError = 102,
                                DescripcionError = "Elemento Inválido",
                                ErrorTexto = actualErrorText,
                                LineaEnDondeAparece = lineaActualIndex + 1
                            });
                        }

                        //ResetBuffers
                        bufferConstante.Clear();
                        bufferIdentificador.Clear();
                    }


                    /// ------------------  Handlers individuales para cada TipoChar -------------------
                    switch (tipoCharActual)
                    {
                        case TipoChar.Delimitador:
                            {
                                results.RegistrosLexicos.Add(new RegistroLexico
                                {
                                    LineaNum = lineaActualIndex + 1,
                                    Codigo = Delimitadores[c],
                                    TokenText = c.ToString(),
                                    Tipo = Tipos[Token.Delimitadores],
                                    LineaEnDondeAparece = lineaActualIndex + 1,
                                    tokenType = Token.Delimitadores
                                });
                                break;
                            }
                        case TipoChar.Operador:
                            {
                                results.RegistrosLexicos.Add(new RegistroLexico
                                {
                                    LineaNum = lineaActualIndex + 1,
                                    Codigo = Operadores[c],
                                    TokenText = c.ToString(),
                                    Tipo = Tipos[Token.Operadores],
                                    LineaEnDondeAparece = lineaActualIndex + 1,
                                    tokenType = Token.Operadores
                                });
                                break;
                            }
                        case TipoChar.Letra:
                            {
                                bufferIdentificador.Append(c);
                                break;
                            }
                        case TipoChar.Digito:
                        case TipoChar.Exponencial:
                        case TipoChar.PuntoFlotante:
                            {
                                ///Checa si el buffer identificador tiene chars dentro
                                ///         (Significa que se está analizando un identificador actualmente)
                                ///o de lo contrario significa que se está analizando una constante actualmente  
                                /// ""Solo se usa un buffer a la vez""

                                if (bufferIdentificador.ToString() != string.Empty)
                                    bufferIdentificador.Append(c);
                                else
                                    bufferConstante.Append(c);

                                break;
                            }
                        case TipoChar.Desconocido:
                            {
                                results.Errores.Add(new RegistroError
                                {
                                    LineaEnDondeAparece = lineaActualIndex + 1,
                                    CodigoError = 101,
                                    DescripcionError = "Símbolo desconocido",
                                    ErrorTexto = c.ToString()
                                });
                                break;
                            }
                    }
                } //Aquí acaba el foreach

                /// Checa si se terminó la línea actual en un estado válido (no se dejó
                /// algo cortado, etc, entre otras validaciones al final de la línea
                if (!elEstadoEsValido(estadoActual))
                {
                    string actualErrorText = bufferConstante.Length != 0 ? bufferConstante.ToString() : bufferIdentificador.ToString();
                    results.Errores.Add(new RegistroError
                    {
                        CodigoError = 102,
                        DescripcionError = "Elemento Inválido",
                        ErrorTexto = actualErrorText,
                        LineaEnDondeAparece = lineaActualIndex + 1
                    });

                    bufferConstante.Clear();
                    bufferIdentificador.Clear();
                }
                else
                {
                    //El texto anterior es una constante válida
                    if (bufferConstante.Length != 0)
                        AddNuevoConstante(bufferConstante.ToString(), lineaActualIndex + 1);

                    ///El texto anterior al separador es un identificador válido
                    else if (bufferIdentificador.Length != 0)
                        AddNuevoIdentificador(bufferIdentificador.ToString(), lineaActualIndex + 1);

                    bufferConstante.Clear();
                    bufferIdentificador.Clear();
                }
            }
            return results;
        }

        private void Resetear()
        {
            estadoActual = 0;
            valorIdentificadorActual = ValorIdentificadorDefault;
            valorConstanteActual = ValorConstanteDefault;
            bufferConstante.Clear();
            results = new AutomataResult();
        }

        private TipoChar CategorizarCaracter(char c)
        {
            if (c == '(' || c == ')' || c == ';') return TipoChar.Delimitador;
            if (c == '+' || c == '-' || c == '*' || c == '/') return TipoChar.Operador;
            if (c == '.') return TipoChar.PuntoFlotante;
            if (char.IsDigit(c)) return TipoChar.Digito;
            if (char.IsWhiteSpace(c)) return TipoChar.EspacioBlanco;

            if (c == 'e' || c == 'E') return TipoChar.Exponencial;
            if (char.IsLetter(c) || c == '_') return TipoChar.Letra;

            return TipoChar.Desconocido;
        }

        private bool elEstadoEsValido(int state)
        {
            int[] valids = { 1, 2, 3, 4, 8, 9, 10 };
            return valids.Contains(state);
        }

        private void AddNuevoConstante(string constanteText, int noLineaEncontrado)
        {
            //Toca añadirlo en la tabla de constantes
            results.RegistrosConstantes.Add(new RegistroConstante
            {
                ConstanteTexto = constanteText,
                LineaEnDondeAparece = noLineaEncontrado,
                Valor = valorConstanteActual
            });


            //Toca añadirlo a la tabla léxica en general también
            results.RegistrosLexicos.Add(new RegistroLexico
            {
                LineaNum = noLineaEncontrado,
                Codigo = valorConstanteActual,
                Tipo = Tipos[Token.Constante],
                TokenText = constanteText,
                LineaEnDondeAparece = noLineaEncontrado,
                tokenType = Token.Constante
            });

            valorConstanteActual += 1;
        }

        private void AddNuevoIdentificador(string identificadorText, int noLineaEncontrado)
        {
            //------- Lógica para añadirlo solamente a la tabla de Identificadores ------

            ///Checar si no se encuentra ya este identificador
            RegistroDinamico? identificadorExistente = results.RegistrosDinamicos
                .Find(rd => rd.IdentificadorTexto == bufferIdentificador.ToString());

            int valorDeEsteIdentificador;

            //Se encontró que ya existía? Agregar solamente la línea nueva
            if (identificadorExistente != null)
            {
                identificadorExistente.LineasEnDondeAparece.Add(noLineaEncontrado);
                valorDeEsteIdentificador = identificadorExistente.Valor;
            }
            //No? Crear un registro nuevo y la línea en la que se encuentra de paso
            else
            {
                //Toca añadirlo a la tabla de identificadores
                var nuevoIdentificador = new RegistroDinamico
                {
                    IdentificadorTexto = identificadorText,
                    Valor = valorIdentificadorActual
                };

                valorDeEsteIdentificador = valorIdentificadorActual;
                valorIdentificadorActual += 1;
                nuevoIdentificador.LineasEnDondeAparece.Add(noLineaEncontrado);
                results.RegistrosDinamicos.Add(nuevoIdentificador);
            }

            // ---------------- Lógica para añadirlo también en la tabla léxica ---------

            results.RegistrosLexicos.Add(new RegistroLexico()
            {
                LineaNum = noLineaEncontrado,
                Codigo = valorDeEsteIdentificador,
                Tipo = Tipos[Token.Identificador],
                TokenText = identificadorText,
                LineaEnDondeAparece = noLineaEncontrado,
                tokenType = Token.Identificador
            }
            );
        }
    }
}

