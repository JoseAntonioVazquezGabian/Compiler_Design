using System;
using System.Collections.Generic;

namespace Falak {
    
    class Parser{

        static readonly ISet<TokenCategory> firstOfDef = new HashSet<TokenCategory>(){
            TokenCategory.VAR,
            TokenCategory.IDENTIFIER
        };

        static readonly ISet<TokenCategory> firstOfStatement = new HashSet<TokenCategory>(){
            TokenCategory.IDENTIFIER,
            TokenCategory.IF,
            TokenCategory.WHILE,
            TokenCategory.DO,
            TokenCategory.BREAK,
            TokenCategory.RETURN,
            TokenCategory.INC,
            TokenCategory.DEC,
            TokenCategory.SEMICOLON
        };

        static readonly ISet<TokenCategory> firstOfComp = new HashSet<TokenCategory>(){
            TokenCategory.COMPARE,
            TokenCategory.DIFFERENT
        };

        static readonly ISet<TokenCategory> firstOfRel = new HashSet<TokenCategory>(){
            TokenCategory.LESS,
            TokenCategory.LESS_EQUAL,
            TokenCategory.MORE,
            TokenCategory.MORE_EQUAL
        };

        static readonly ISet<TokenCategory> firstOfAdd = new HashSet<TokenCategory>(){
            TokenCategory.PLUS,
            TokenCategory.MINUS

        };

        static readonly ISet<TokenCategory> firstOfMul = new HashSet<TokenCategory>(){
            TokenCategory.MUL,
            TokenCategory.DIV,
            TokenCategory.MOD
        };

        static readonly ISet<TokenCategory> firstOfUnary = new HashSet<TokenCategory>(){
            TokenCategory.PLUS,
            TokenCategory.MINUS,
            TokenCategory.NOT

        };

        static readonly ISet<TokenCategory> firstOflit = new HashSet<TokenCategory>(){
            TokenCategory.TRUE,
            TokenCategory.FALSE,
            TokenCategory.CHAR,
            TokenCategory.STRING,
            TokenCategory.INT_LITERAL
        };

        static readonly ISet<TokenCategory> expressions = new HashSet<TokenCategory>(){
            TokenCategory.OR,
            TokenCategory.AND,
            TokenCategory.XOR,
            TokenCategory.COMPARE,
            TokenCategory.DIFFERENT,
            TokenCategory.LESS_EQUAL,
            TokenCategory.LESS,
            TokenCategory.MORE_EQUAL,
            TokenCategory.MORE,
            TokenCategory.PLUS,
            TokenCategory.MINUS,
            TokenCategory.MUL,
            TokenCategory.DIV,
            TokenCategory.MOD,
            TokenCategory.NOT,
            TokenCategory.OPEN_PARENTHESIS,
            TokenCategory.OPEN_SQUARE_BRACKET,
            TokenCategory.TRUE,
            TokenCategory.FALSE,
            TokenCategory.CHAR_LIT,
            TokenCategory.STRING_LIT,
            TokenCategory.INT_LITERAL,
            TokenCategory.IDENTIFIER
        };

        static readonly ISet<TokenCategory> firstOflist = new HashSet<TokenCategory>(){
            TokenCategory.TRUE,
            TokenCategory.FALSE,
            TokenCategory.CHAR,
            TokenCategory.STRING,
            TokenCategory.INT_LITERAL,
            TokenCategory.OPEN_PARENTHESIS,
            TokenCategory.OPEN_BRACKET,
            TokenCategory.IDENTIFIER
        };

        IEnumerator<Token> tokenStream;

        public Parser(IEnumerator<Token> tokenStream) {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        public TokenCategory CurrentToken {
            get { return tokenStream.Current.Category; }
        }

        public Token Expect(TokenCategory category) {
            if (CurrentToken == category) {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            } else {
                throw new SyntaxError(category, tokenStream.Current);
            }
        }

        public void Program(){
            DefList();
            Expect(TokenCategory.EOF);
        }

        public void DefList(){
            while (firstOfDef.Contains(CurrentToken)){
                Def();
            }
        }

        public void Def(){
            switch (CurrentToken){
                case TokenCategory.VAR:
                    VarDef();
                    break;
                case TokenCategory.IDENTIFIER:
                    FunDef();
                    break;
                default:
                    throw new SyntaxError(firstOfDef, tokenStream.Current);
            }
        }

        public void VarDef(){
            Expect(TokenCategory.VAR);
            VarList();
            Expect(TokenCategory.SEMICOLON);
        }

        public void VarList(){
            IdList();
        }

        public void IdList(){
            Expect(TokenCategory.IDENTIFIER);
            while(CurrentToken == TokenCategory.COMA){
                Expect(TokenCategory.COMA);
                Expect(TokenCategory.IDENTIFIER);
            }
        }

        public void FunDef(){
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.OPEN_PARENTHESIS);
            ParamLists();
            Expect(TokenCategory.CLOSE_PARENTHESIS);
            Expect(TokenCategory.OPEN_BRACKET);
            VarDefList();
            StatementList();
            Expect(TokenCategory.CLOSE_BRACKET);
        }

        public void ParamLists(){
            if(CurrentToken == TokenCategory.IDENTIFIER){
                IdList();
            }
        }

        public void VarDefList(){
            while(CurrentToken == TokenCategory.VAR){
                VarDef();
            }
        }

        public void StatementList(){
             while(firstOfStatement.Contains(CurrentToken)){
                Statement();
            }
        }

        public void Statement(){
            switch(CurrentToken){
                case TokenCategory.IDENTIFIER:
                    Expect(TokenCategory.IDENTIFIER);

                    switch(CurrentToken){
                        case TokenCategory.ASSIGN:
                            StatementAssign();
                            break;
                        case TokenCategory.OPEN_PARENTHESIS:
                            StatementFunCall();
                            break;
                        
                        default:
                            throw new SyntaxError(firstOfStatement, tokenStream.Current);

                    }
                break;

                case TokenCategory.INC:
                    StatementInc();
                    break;

                case TokenCategory.DEC:
                    StatementDec();
                    break;

                case TokenCategory.IF:
                    StatementIf();
                    break;

                case TokenCategory.WHILE:
                    StatementWhile();
                    break;

                case TokenCategory.DO:
                    StatementDo();
                    break;

                case TokenCategory.BREAK:
                    Expect(TokenCategory.BREAK);
                    Expect(TokenCategory.SEMICOLON);
                    break;

                case TokenCategory.RETURN:
                    Expect(TokenCategory.RETURN);
                    expr();
                    Expect(TokenCategory.SEMICOLON);
                    break;

                case TokenCategory.SEMICOLON:
                    Expect(TokenCategory.SEMICOLON);
                    break;

            default:
                throw new SyntaxError(firstOfStatement, tokenStream.Current);

            }
        }

        public void StatementAssign(){
            Expect(TokenCategory.ASSIGN);
            expr();
            Expect(TokenCategory.SEMICOLON);
        }

        public void StaInc(){
            Expect(TokenCategory.INC);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.SEMICOLON);
        }

        public void StatementDec(){
            Expect(TokenCategory.DEC);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.SEMICOLON);
        }

        public void StatementFunCall(){
            FunCall();
            Expect(TokenCategory.SEMICOLON);
        }

        public void FunCall(){
            Expect(TokenCategory.OPEN_PARENTHESIS);
            ExprList();
            Expect(TokenCategory.CLOSE_PARENTHESIS);
        }

        public void ExprList(){
            if(expressions.Contains(CurrentToken)){
                expr();
                while(CurrentToken == TokenCategory.COMA){
                    Expect(TokenCategory.COMA);
                    expr();
                }
            }
        }

        public void StatementIf(){
            Expect(TokenCategory.IF);
            Expect(TokenCategory.OPEN_PARENTHESIS);
            expr();
            Expect(TokenCategory.CLOSE_PARENTHESIS);
            Expect(TokenCategory.OPEN_BRACKET);
            StatementList();
            Expect(TokenCategory.CLOSE_BRACKET);
            ElseIf();
            Else();
        }

        public void ElseIf(){
            while (CurrentToken == TokenCategory.ELIF){
                Expect(TokenCategory.ELIF);
                Expect(TokenCategory.OPEN_PARENTHESIS);
                expr();
                Expect(TokenCategory.CLOSE_PARENTHESIS);
                Expect(TokenCategory.OPEN_BRACKET);
                StatementList();
                Expect(TokenCategory.CLOSE_BRACKET);
            }
        }

        public void Else(){
            if(CurrentToken == TokenCategory.ELSE){
                Expect(TokenCategory.ELSE);
                Expect(TokenCategory.OPEN_BRACKET);
                StatementList();
                Expect(TokenCategory.CLOSE_BRACKET);
            }
        }

        public void StatementWhile(){
            Expect(TokenCategory.WHILE);
            Expect(TokenCategory.OPEN_PARENTHESIS);
            expr();
            Expect(TokenCategory.CLOSE_PARENTHESIS);
            Expect(TokenCategory.OPEN_BRACKET);
            StatementList();
            Expect(TokenCategory.CLOSE_BRACKET);
        }

        
        public void StatementDo(){
            Expect(TokenCategory.DO);
            Expect(TokenCategory.OPEN_BRACKET);
            StatementList();
            Expect(TokenCategory.CLOSE_BRACKET);
            Expect(TokenCategory.WHILE);
            Expect(TokenCategory.OPEN_PARENTHESIS);
            expr();
            Expect(TokenCategory.CLOSE_PARENTHESIS);
            Expect(TokenCategory.SEMICOLON);
        }

        public void expr(){
            ExprOr();
        }

        public void ExprOr(){
            ExprAnd();
            while (CurrentToken == TokenCategory.OR){
                Expect(TokenCategory.OR);
                ExprAnd();
            }
        }

        public void ExprAnd(){
            ExprComp();
            while(CurrentToken == TokenCategory.AND){
                Expect(TokenCategory.AND);
                ExprComp();
            }
        }

        public void ExprComp(){
            ExprRel();
            while(firstOfComp.Contains(CurrentToken)){
                OpComp();
                ExprRel();
            }
        }

        public void OpComp(){
            switch (CurrentToken)
            {
                case TokenCategory.COMPARE:
                    Expect(TokenCategory.COMPARE);
                    break;
                case TokenCategory.DIFFERENT:
                    Expect(TokenCategory.DIFFERENT);
                    break;
                
                default:
                    throw new SyntaxError(firstOfComp, tokenStream.Current);
            }
        }

        public void ExprRel(){
            ExprAdd();
            while(firstOfRel.Contains(CurrentToken)){
                OpRel();
                ExprAdd();
            }
        }

        public void OpRel(){
            switch (CurrentToken)
            {
                case TokenCategory.LESS_THAN:
                    Expect(TokenCategory.LESS_THAN);
                    break;
                case TokenCategory.LESS_EQUAL:
                    Expect(TokenCategory.LESS_EQUAL);
                    break;
                case TokenCategory.MORE_THAN:
                    Expect(TokenCategory.MORE_THAN);
                    break;
                case TokenCategory.MORE_EQUAL:
                    Expect(TokenCategory.MORE_EQUAL);
                    break;
                
                default:
                    throw new SyntaxError(firstOfRel, tokenStream.Current);
            }
        }

        public void ExprAdd(){
            ExprMul();
            while(firstOfAdd.Contains(CurrentToken)){
                OpAdd();
                ExprMul();
            }
        }

        public void OpAdd(){
            switch (CurrentToken)
            {
                case TokenCategory.PLUS:
                    Expect(TokenCategory.PLUS);
                    break;
                case TokenCategory.MINUS:
                    Expect(TokenCategory.MINUS);
                    break;
                
                default:
                    throw new SyntaxError(firstOfAdd, tokenStream.Current);
            }
        }

        public void ExprMul(){
            ExprUnary();
            while(firstOfMul.Contains(CurrentToken)){
                OpMul();
                ExprUnary();
            }
        }

        public void OpMul(){
            switch (CurrentToken){
                case TokenCategory.MULTIPLY:
                    Expect(TokenCategory.MULTIPLY);
                    break;
                case TokenCategory.DIV:
                    Expect(TokenCategory.DIV);
                    break;
                case TokenCategory.MOD:
                    Expect(TokenCategory.MOD);
                    break;
                
                default:
                    throw new SyntaxError(firstOfMul, tokenStream.Current);
            }
        }

        public void ExprUnary(){
            while(firstOfUnary.Contains(CurrentToken)){
                OpUnary();
            }
            ExprPrimary();
        }

        public void OpUnary(){
            switch (CurrentToken)
            {
                case TokenCategory.PLUS:
                    Expect(TokenCategory.PLUS);
                    break;
                case TokenCategory.MINUS:
                    Expect(TokenCategory.MINUS);
                    break;
                case TokenCategory.NOT:
                    Expect(TokenCategory.NOT);
                    break;
                
                default:
                    throw new SyntaxError(firstOfUnary, tokenStream.Current);
            }
        }

        public void ExprPrimary(){
            switch (CurrentToken) 
            {
                case TokenCategory.IDENTIFIER:
                    Expect(TokenCategory.IDENTIFIER);
                    if(CurrentToken == TokenCategory.OPEN_PARENTHESIS){
                        FunCall();
                    }
                    break;
                case TokenCategory.OPEN_SQUARE_BRACKET:
                    Array();
                    break;
                case TokenCategory.TRUE:
                case TokenCategory.FALSE:
                case TokenCategory.INT_LITERAL:
                case TokenCategory.CHAR_LIT:
                case TokenCategory.STRING_LIT:
                    Lit();
                    break;
                
                case TokenCategory.OPEN_PARENTHESIS:
                    Expect(TokenCategory.OPEN_PARENTHESIS);
                    expr();
                    Expect(TokenCategory.CLOSE_PARENTHESIS);
                    break;
                
                default:
                    throw new SyntaxError(firstOflist, tokenStream.Current);
            }
        }

        public void Array(){
            Expect(TokenCategory.OPEN_SQUARE_BRACKET);
            ExprList();
            Expect(TokenCategory.CLOSE_SQUARE_BRACKET);
        }

        public void Lit(){
            switch (CurrentToken)
            {
                case TokenCategory.TRUE:
                    Expect(TokenCategory.TRUE);
                    break;
                case TokenCategory.FALSE:
                    Expect(TokenCategory.FALSE);
                    break;
                case TokenCategory.INT_LITERAL:
                    Expect(TokenCategory.INT_LITERAL);
                    break;
                case TokenCategory.CHAR_LIT:
                    Expect(TokenCategory.CHAR_LIT);
                    break;
                case TokenCategory.STRING_LIT:
                    Expect(TokenCategory.STRING_LIT);
                    break;

                default:
                    throw new SyntaxError(firstOflit, tokenStream.Current);
            }
        }

    }
}