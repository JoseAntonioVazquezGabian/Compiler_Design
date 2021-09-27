using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Drac {

    class Scanner {

        readonly string input;

        static readonly Regex regex = new Regex(
            @"
                
                (?<Compare>    [=][=]    )
              | (?<Assign>     [=]       )
              | (?<MLComments> ([(][*](.|\s)*?[*][)]) )
              | (?<SLComment>   [-][-].* )
              | (?<LessEqual>  [<][=]    )
              | (?<MoreEqual>  [>][=]    )
              | (?<Different> [<][>]     )
              | (?<LessThan>   [<]       )
              | (?<MoreThan>   [>]       )
              | (?<Plus>       [+]       )
              | (?<Minus>      [-]       )
              | (?<Mul>        [*]       )
              | (?<Div>        [/]       )
              | (?<Mod>        [%]       )
              | (?<Coma>       [,]       )
              | (?<Semicolon>  [;]       )
              | (?<ParLeft>    [(]       )
              | (?<ParRight>   [)]       )
              | (?<SquareBraceLeft> [\[]  )
              | (?<SquareBraceRight> [\]] )
              | (?<CurlyRigth>  [}]      )
              | (?<CurlyLeft>   [{]      )
              | (?<CharLit>    [']([\\]([nrt\'\\""\\]|u[\dA-Fa-f]{6})|[^\\])['] )
              | (?<StringLit>  [""]([^\\""]|[\\].)*?[""] )
              | (?<Unicode>    [\\][u][A-Fa-f\d]{6}  )
              | (?<Newline>    \n        )
              | (?<WhiteSpace> \s        )     # Must go after Newline.
              | (?<Backlash>   [\\]      )
              | (?<IntLiteral> \d+       )
              | (?<Identifier> [a-zA-z_]+[\d]*([a-zA-z_]*[\d]*)* )     # Must go after all keywords
              | (?<Other>      .         )     # Must be last: match any other character.
            ",
            RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                | RegexOptions.Multiline
            );

            static readonly IDictionary<string, TokenCategory> keywords =
            new Dictionary<string, TokenCategory>() {
                {"and", TokenCategory.AND},
                {"break", TokenCategory.BREAK},
                {"dec", TokenCategory.DEC},
                {"do", TokenCategory.DO},
                {"elif", TokenCategory.ELIF},
                {"else", TokenCategory.ELSE},
                {"false", TokenCategory.FALSE},
                {"if", TokenCategory.IF},
                {"inc", TokenCategory.INC},
                {"not", TokenCategory.NOT},
                {"or", TokenCategory.OR},
                {"return", TokenCategory.RETURN},
                {"true", TokenCategory.TRUE},
                {"var", TokenCategory.VAR},
                {"while", TokenCategory.WHILE}
            };

                    static readonly IDictionary<string, TokenCategory> nonKeywords =
            new Dictionary<string, TokenCategory>() {
                {"Coma", TokenCategory.COMA},
                {"Compare", TokenCategory.COMPARE},
                {"Different", TokenCategory.DIFFERENT},
                {"Div", TokenCategory.DIV},
                {"Mod", TokenCategory.MOD},
                {"LessEqual", TokenCategory.LESS_EQUAL},
                {"MoreEqual", TokenCategory.MORE_EQUAL},
                {"MoreThan", TokenCategory.MORE_THAN},
                {"LessThan", TokenCategory.LESS_THAN},
                {"SquareBraceLeft", TokenCategory.OPEN_SQUARE_BRACKET},
                {"SquareBraceRight", TokenCategory.CLOSE_SQUARE_BRACKET},
                {"CurlyLeft", TokenCategory.OPEN_BRACKET},
                {"CurlyRigth", TokenCategory.CLOSE_BRACKET},
                {"Semicolon", TokenCategory.SEMICOLON},
                {"Assign", TokenCategory.ASSIGN},
                {"IntLiteral", TokenCategory.INT_LITERAL},
                {"Mul", TokenCategory.MULTIPLY},
                {"Backlash", TokenCategory.BACKSLASH},
                {"Unicode", TokenCategory.UNICODE},
                {"CharLit", TokenCategory.CHAR_LIT},
                {"StringLit", TokenCategory.STRING_LIT},
                {"ParLeft", TokenCategory.OPEN_PARENTHESIS},
                {"ParRight", TokenCategory.CLOSE_PARENTHESIS},
                {"Plus", TokenCategory.PLUS},
                {"Minus", TokenCategory.MINUS}
            };

            public Scanner(string input) {
            this.input = input;
        }


         public IEnumerable<Token> Start() {

            var row = 1;
            var columnStart = 0;

            Func<Match, TokenCategory, Token> newTok = (m, tc) =>
                new Token(m.Value, tc, row, m.Index - columnStart + 1);

            foreach (Match m in regex.Matches(input)) {

                if (m.Groups["Newline"].Success) {

                    // Found a new line.
                    row++;
                    columnStart = m.Index + m.Length;

                } else if (m.Groups["WhiteSpace"].Success
                    || m.Groups["SLComment"].Success) {

                    // Skip white space and comments.

                } else if (m.Groups ["MLComments"].Success){

                    //Skip multicomments

                    MatchCollection newMatches = Regex.Matches(m.Groups ["MLComments"].Value, "\n", RegexOptions.Multiline);

                    if(newMatches.Count > 0){
                        Match lastMatch = newMatches[newMatches.Count - 1];
                        row += newMatches.Count;
                        columnStart = m.Index + lastMatch.Index + lastMatch.Length;
                    }

                } else if (m.Groups["Identifier"].Success) {

                    if (keywords.ContainsKey(m.Value)) {

                        //Matches keyword
                        yield return newTok(m, keywords[m.Value]);

                    } else {

                        // Plain identifier.
                        yield return newTok(m, TokenCategory.IDENTIFIER);
                    }

                } else if (m.Groups["Other"].Success) {

                    // Illegal character.
                    yield return newTok(m, TokenCategory.ILLEGAL_CHAR);

                } else {

                    // Non keywords.
                    foreach (var name in nonKeywords.Keys) {
                        if (m.Groups[name].Success) {
                            yield return newTok(m, nonKeywords[name]);
                            break;
                        }
                    }
                }
            }

            yield return new Token(null,
                                   TokenCategory.EOF,
                                   row,
                                   input.Length - columnStart + 1);
        }
    }
}
