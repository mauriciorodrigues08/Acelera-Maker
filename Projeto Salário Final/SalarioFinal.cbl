       IDENTIFICATION DIVISION.
           PROGRAM-ID. SALARIOFINAL.

       ENVIRONMENT DIVISION.

       DATA DIVISION.
       WORKING-STORAGE SECTION.
      * FUNCIONARIO 
       01 WS-FUNCIONARIO.
      *    NOME
           05 WS-NOME                 PIC X(30).

      *    SALARIO BASE
           05 WS-SALARIO-BASE         PIC 9(05)V99.

      *    TEMPO DE EMPRESA (EM MESES)
           05 WS-TEMPO-DE-EMPRESA     PIC 9(02).

      * VALORES DE TAXA PARA BONUS
       01 WS-TAXAS.
           05 WS-TAXA-INICIAL         PIC V99 VALUE .05.
           05 WS-TAXA-INTERMEDIARIA   PIC V99 VALUE .10.
           05 WS-TAXA-AVANCADA        PIC V99 VALUE .15.

      * BONUS
       01 WS-BONUS                    PIC 9(05)V99.

      * NOVO SALARIO
       01 WS-NOVO-SALARIO             PIC 9(05)V99.

      * OPCAO PARA INTERAGIR COM O MENU
       01 WS-OPCAO                    PIC 9 VALUE 0.

      * VARIAVEL DE 'RETORNO' DA VERIFICACAO
       01 WS-VALIDACAO                PIC X VALUE 'S'.

      * VARIAVEIS FORMATADAS PARA EXIBICAO DO RESULTADO
       01 WS-RES-NOME                 PIC X(30).
       01 WS-RES-SALARIO-BASE         PIC Z(04)9.99.
       01 WS-RES-BONUS                PIC ZZZZ9.99.
       01 WS-RES-NOVO-SALARIO         PIC ZZZZ9.99.

       PROCEDURE DIVISION.
      * PARAGRAFO PRINCIPAL
       MAIN-PROCEDURE.
           PERFORM UNTIL WS-OPCAO = 2
      *        MOSTRA O MENU E RECEBE A OPCAO
               PERFORM IMPRIME-MENU
               ACCEPT WS-OPCAO
               
      *        VERIFICA A OPCAO INFORMADA
               EVALUATE WS-OPCAO
      *            SE 1 REALIZA A OPERACAO
                   WHEN 1
      *                REINICIALIZA A VALIDACAO
                       MOVE 'S' TO WS-VALIDACAO

      *                RECEBE OS DADOS
                       PERFORM ENTRADA-DADOS

      *                VALIDA OS DADOS PASSADOS
                       PERFORM VALIDA-DADOS

                       IF (WS-VALIDACAO = 'S')
      *                    CALCULA O BONUS
                           PERFORM CALCULA-BONUS
      
      *                    CALCULA O SALARIO
                           PERFORM CALCULA-SALARIO
      
      *                    EXIBE O RESULTADO
                           PERFORM EXIBE-RESULTADO
                       END-IF


      *            SE 2, MOVE O VALOR 2 PARA A VARIAVEL 
                   WHEN 2
                       DISPLAY 'PROGRAMA FINALIZADO!'

      *            SE OUTRO, NOTIFICA ERRO
                   WHEN OTHER
                       DISPLAY 'OPCAO INVALIDA! TENTE NOVAMENTE.'
                       MOVE 0 TO WS-OPCAO
               END-EVALUATE

           END-PERFORM.
           
           STOP RUN.

      * PARAGRAFO DE EXIBICAO DO MENU
       IMPRIME-MENU.
           DISPLAY '============== MENU =============='.
           DISPLAY ' 1. CALCULAR BONUS'.
           DISPLAY ' 2. SAIR'.
           DISPLAY '=================================='.
           DISPLAY 'INFORME SUA OPCAO: ' WITH NO ADVANCING.

      * PARAGRAFO PARA ENTRADA DE DADOS
       ENTRADA-DADOS.
      *    RECEBE O NOME
           DISPLAY 'INFORME O NOME: ' WITH NO ADVANCING.
           ACCEPT WS-NOME.

      *    RECEBE O SALARIO BASE
           DISPLAY 'INFORME O SALARIO BASE: R$' WITH NO ADVANCING.
           ACCEPT WS-SALARIO-BASE.

      *    RECEBE O TEMPO DE EMPRESA
           DISPLAY 'INFORME O TEMPO DE EMPRESA (EM MESES): ' 
           WITH NO ADVANCING.
           ACCEPT WS-TEMPO-DE-EMPRESA.


      * PARAGRAFO PARA VALIDAR OS DADOS
       VALIDA-DADOS.
      *    VALIDA O NOME
           IF (WS-NOME = SPACES OR WS-NOME = LOW-VALUES)
               DISPLAY 'ERRO! NOME INVÁLIDO INFORMADO!'
               MOVE 'N' TO WS-VALIDACAO
           END-IF.

      *    VALIDA O SALARIO
           IF (WS-SALARIO-BASE IS NOT NUMERIC OR WS-SALARIO-BASE<= 0)
               DISPLAY 'ERRO! SALARIO INVALIDO INFORMADO!'
               MOVE 'N' TO WS-VALIDACAO
           END-IF.

      *    VALIDA O TEMPO DE EMPRESA
           IF (WS-TEMPO-DE-EMPRESA IS NOT NUMERIC OR 
           WS-TEMPO-DE-EMPRESA<= 0)
               DISPLAY 'ERRO! TEMPO INVALIDO INFORMADO!'
               MOVE 'N' TO WS-VALIDACAO
           END-IF.

      * PARAGRAFO PARA CALCULAR O BONUS
       CALCULA-BONUS.
      *    VERIFICA O TEMPO DE EMPRESE E CALCULA O BONUS
           EVALUATE TRUE
      *        ATE 1 ANO DE EMPRESA
               WHEN WS-TEMPO-DE-EMPRESA <= 12
                   COMPUTE WS-BONUS = 
                       WS-SALARIO-BASE * WS-TAXA-INICIAL

      *        1 A 5 ANOS
               WHEN WS-TEMPO-DE-EMPRESA <= 60
                   COMPUTE WS-BONUS = 
                       WS-SALARIO-BASE * WS-TAXA-INTERMEDIARIA

      *        MAIS DE 5 ANOS
               WHEN OTHER
                   COMPUTE WS-BONUS = 
                       WS-SALARIO-BASE * WS-TAXA-AVANCADA

           END-EVALUATE.

      * PARAGRAFO PARA CALCULAR O NOVO SALARIO
       CALCULA-SALARIO.
           COMPUTE WS-NOVO-SALARIO = WS-SALARIO-BASE + WS-BONUS.

      * PARAGRAFO PARA EXIBIR O RESULTADO
       EXIBE-RESULTADO.
      *    PASSA OS VALORES OBTIDOS PARA AS VARIAVEIS FORMATADAS
           MOVE WS-NOME TO WS-RES-NOME.
           MOVE WS-SALARIO-BASE TO WS-RES-SALARIO-BASE.
           MOVE WS-BONUS TO WS-RES-BONUS.
           MOVE WS-NOVO-SALARIO TO WS-RES-NOVO-SALARIO.

      *    EXIBE O RESULTADO
           DISPLAY '=========== RESULTADO ============'.
           DISPLAY ' NOME: ' WS-RES-NOME.
           DISPLAY ' SALARIO BASE: R$ ' WS-RES-SALARIO-BASE.
           DISPLAY ' BONUS: R$' WS-RES-BONUS.
           DISPLAY ' SALARIO FINAL: R$' WS-RES-NOVO-SALARIO.
           DISPLAY '=================================='.
