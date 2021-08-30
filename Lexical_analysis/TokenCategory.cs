/*
  Falak compiler - Token categories for the scanner.
  Copyright (C) 2021 José Antonio Vázquez, Daniel Trejo y Jaime Orlando López. ITESM CEM

*/

namespace Falak {

    enum TokenCategory {
        AND,
        OR,
        ASSIGN,
        BOOL,
        END,
        EOF,
        FALSE,
        IDENTIFIER,
        IF,
        ELSE,
        ELSEIF,
        INT,
        INT_LITERAL,
        LESS,
        BREAK,
        MUL,
        NEG,
        PARENTHESIS_OPEN,
        PARENTHESIS_CLOSE,
        PLUS,
        PRINT,
        TRUE,
        ILLEGAL_CHAR,
        VAR,
        INC,
        DEC,
        DO,
        WHILE,
        RETURN,
        THEN,
        MULTILINECOMMENT,
        COMMENT
    }
}
