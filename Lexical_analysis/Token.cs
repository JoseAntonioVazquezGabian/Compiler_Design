/*
  Falak compiler - Token class for the scanner.
  Copyright (C) 2021 José Antonio Vázquez, Daniel Trejo y Jaime Orlando López. ITESM CEM
*/

namespace Falak {

    class Token {

        readonly string lexeme;

        readonly TokenCategory category;

        readonly int row;

        readonly int column;

        public string Lexeme {
            get { return lexeme; }
        }

        public TokenCategory Category {
            get { return category; }
        }

        public int Row {
            get { return row; }
        }

        public int Column {
            get { return column; }
        }

        public Token(string lexeme,
                     TokenCategory category,
                     int row,
                     int column) {
            this.lexeme = lexeme;
            this.category = category;
            this.row = row;
            this.column = column;
        }

        public override string ToString() {
            return $"{{{category}, \"{lexeme}\", @({row}, {column})}}";
        }
    }
}
