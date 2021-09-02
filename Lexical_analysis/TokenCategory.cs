/*
  Falak compiler - Token categories for the scanner.
  Copyright (C) 2021 José Antonio Vázquez, Daniel Trejo y Jaime Orlando López. ITESM CEM

*/

namespace Falak {

    enum TokenCategory {
        COMMENT,
        MULTILINECOMMENT,
        NEWLINE,
        WHITESPACE,
        AND,
        OR,
        LESS,
        PLUS,
        MUL,
        NEG,
        PARENTHESIS_OPEN,
        PARENTHESIS_CLOSE,
        ASSIGN,
        TRUE,
        FALSE,
        INT_LITERAL,
        BOOL,
        IF,
        ELSEIF,
        RETURN,
        WHILE,
        ELSE,
        BREAK,
        VAR,
        INC,
        DEC,
        DO,
        INT,
        PRINT,
        IDENTIFIER,
        OTHER,
        EOF,
        ILLEGAL_CHAR
        
    }
}
